using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.HealthInsurance;
using SisOdonto.Application.Models.Patient;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Interfaces.Services;
using SisOdonto.Domain.Shared;
using SisOdonto.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SisOdonto.Application.Services
{
    public class PatientService : AppService, IPatientService
    {
        #region Fields

        private readonly IEmailService _emailService;
        private readonly IPatientRepository _patientRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHealthInsuranceService _healthInsuranceService;
        private readonly IHealthInsuranceRepository _healthInsuranceRepository;
        private readonly ISchedulingRepository _schedulingRepository;

        #endregion Fields

        #region Constructors

        public PatientService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IPatientRepository patientRepository,
            IEmailService emailService,
            IHealthInsuranceService healthInsuranceService,
            IHealthInsuranceRepository healthInsuranceRepository,
            ISchedulingRepository schedulingRepository)
            : base(unitOfWork, notifier, userManager)
        {
            _userManager = userManager;
            _patientRepository = patientRepository;
            _emailService = emailService;
            _healthInsuranceService = healthInsuranceService;
            _healthInsuranceRepository = healthInsuranceRepository;
            _schedulingRepository = schedulingRepository;
        }

        public async Task Create(PatientDataModel request)
        {
            var hasPatient = await _patientRepository.GetAsync(request.Cpf);

            if (hasPatient is not null)
            {
                Notify("Já existe um paciente cadastrado com este cpf informado.");
                return;
            }

            if (request.BirthDate == DateTime.MinValue)
            {
                Notify("Data inválida");
                return;
            }

            var userId = Guid.NewGuid();

            if (request.CreateUser)
            {
                var user = new IdentityUser { UserName = request.Email, Email = request.Email };

                userId = Guid.Parse(user.Id);

                var options = _userManager.Options.Password;

                var password = Utils.GeneratePassword(options.RequiredLength, options.RequireNonAlphanumeric, options.RequireDigit, options.RequireLowercase, options.RequireUppercase);

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    await _userManager.ConfirmEmailAsync(user, code);

                    await AddClaims(userId);

                    _emailService.SendEmail(user.Email, "Credenciais de Acesso - SisOdonto", "Credenciais", $"Usuário: {user.Email} <br> Senha: {password}");
                }
                else
                {
                    Notify("Erro ao criar usuário.");
                    return;
                }
            }

            var patient = new Patient(userId, request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.HealthInsuranceId, request.Street, request.MaritalStatus, request.Gender, request.Occupation, request.Telephone.Replace("(", "").Replace(")", "").Replace("-", "").Trim(), request.Cellular.Replace("(", "").Replace(")", "").Replace("-", "").Trim());

            if (patient.IsValid())
                await _patientRepository.AddAsync(patient);
            else
            {
                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);

                Notify(patient.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
            {
                Notify("Erro ao salvar dados.");

                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            }
        }

        public async Task Delete(Guid id)
        {
            var patient = await _patientRepository.GetAsync(id);

            if (patient is null)
            {
                Notify("Dados do Paciente não encontrado.");
                return;
            }

            var scheduling = await _schedulingRepository.GetAllByPatientAsync(id);

            if (scheduling.Any())
            {
                Notify("Não é possível excluir o paciente, pois existem agendamentos cadastrados para ele.");
                return;
            }

            patient.Delete();

            _patientRepository.Update(patient);

            if (await CommitAsync())
            {
                var userManager = await _userManager.FindByIdAsync(id.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            }
            else
                Notify("Erro ao salvar dados.");
        }

        public async Task<PatientDataModel> Get(Guid id)
        {
            PatientDataModel patientModel = null;

            var patient = await _patientRepository.GetAsync(id);

            if (patient is null)
                Notify("Dados do Paciente não encontrado.");
            else
            {
                patientModel = new PatientDataModel
                {
                    Id = patient.Id,
                    BirthDate = patient.BirthDate,
                    Cep = patient.Cep,
                    City = patient.City,
                    Complement = patient.Complement,
                    Cpf = patient.Cpf,
                    Telephone = patient.Telephone,
                    Cellular = patient.Cellular,
                    Street = patient.Street,
                    Number = patient.Number,
                    State = patient.State,
                    District = patient.District,
                    Email = patient.Email,
                    Gender = patient.Gender,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    HealthInsuranceId = patient.HealthInsuranceId,
                    Name = patient.Name,
                    HealthInsurances = await _healthInsuranceService.GetAll(),
                };

                if (patient.HealthInsurance is not null)
                {
                    patientModel.HealthInsurance = new HealthInsuranceDataModel()
                    {
                        Id = patient.HealthInsurance.Id,
                        Name = patient.HealthInsurance.Name,
                        Type = patient.HealthInsurance.Type
                    };
                }
            }

            return patientModel;
        }

        public async Task<PatientDataModel> GetByCpf(string cpf)
        {
            PatientDataModel patientModel = null;

            var patient = await _patientRepository.GetAsync(cpf);

            if (patient is null)
                Notify("Dados do Paciente não encontrado.");
            else
            {
                patientModel = new PatientDataModel
                {
                    Id = patient.Id,
                    BirthDate = patient.BirthDate,
                    Cep = patient.Cep,
                    City = patient.City,
                    Complement = patient.Complement,
                    Cpf = patient.Cpf,
                    Telephone = patient.Telephone,
                    Cellular = patient.Cellular,
                    Street = patient.Street,
                    Number = patient.Number,
                    State = patient.State,
                    District = patient.District,
                    Email = patient.Email,
                    Gender = patient.Gender,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    HealthInsuranceId = patient.HealthInsuranceId,
                    Name = patient.Name,
                    HealthInsurances = await _healthInsuranceService.GetAll(),
                };
            }

            return patientModel;
        }

        public async Task<IEnumerable<PatientDataModel>> GetAll()
        {
            var patients = await _patientRepository.GetAllAsync();

            var patientsModel = new List<PatientDataModel>();

            foreach (var patient in patients)
            {
                patientsModel.Add(new PatientDataModel()
                {
                    Id = patient.Id,
                    BirthDate = patient.BirthDate,
                    Cep = patient.Cep,
                    City = patient.City,
                    Complement = patient.Complement,
                    Cpf = patient.Cpf,
                    Telephone = patient.Telephone,
                    Cellular = patient.Cellular,
                    Street = patient.Street,
                    Number = patient.Number,
                    State = patient.State,
                    District = patient.District,
                    Email = patient.Email,
                    Gender = patient.Gender,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    HealthInsuranceId = patient.HealthInsuranceId,
                    Name = patient.Name
                });
            }

            return patientsModel;
        }

        public async Task<IEnumerable<PatientDataModel>> GetAllToReport()
        {
            var patients = await _patientRepository.GetAllAsync();

            var patientsModel = new List<PatientDataModel>();

            foreach (var patient in patients)
            {
                var patientModel = new PatientDataModel()
                {
                    Id = patient.Id,
                    BirthDate = patient.BirthDate,
                    Cep = patient.Cep,
                    City = patient.City,
                    Complement = patient.Complement,
                    Cpf = patient.Cpf,
                    Telephone = patient.Telephone,
                    Cellular = patient.Cellular,
                    Street = patient.Street,
                    Number = patient.Number,
                    State = patient.State,
                    District = patient.District,
                    Email = patient.Email,
                    Gender = patient.Gender,
                    MaritalStatus = patient.MaritalStatus,
                    Occupation = patient.Occupation,
                    HealthInsuranceId = patient.HealthInsuranceId,
                    Name = patient.Name
                };

                if (patient.HealthInsurance is not null)
                {
                    patientModel.HealthInsurance = new HealthInsuranceDataModel()
                    {
                        Id = patient.HealthInsurance.Id,
                        Name = patient.HealthInsurance.Name,
                        Type = patient.HealthInsurance.Type
                    };
                }

                patientsModel.Add(patientModel);
            }

            return patientsModel;
        }

        public async Task Update(PatientDataModel request)
        {
            var patient = await _patientRepository.GetAsync(request.Id);

            if (patient is null)
            {
                Notify("Dados do Paciente não encontrado.");
                return;
            }

            if (request.BirthDate == DateTime.MinValue)
            {
                Notify("Data inválida");
                return;
            }

            patient.Update(request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.MaritalStatus, request.Gender, request.Occupation, request.Telephone.Replace("(", "").Replace(")", "").Replace("-", "").Trim(), request.Cellular.Replace("(", "").Replace(")", "").Replace("-", "").Trim());

            if (patient.HealthInsurance?.Id != request.HealthInsuranceId && request.HealthInsuranceId is not null)
            {
                var newHealthInsurance = await _healthInsuranceRepository.GetAsync((Guid)request.HealthInsuranceId);

                patient.SetHealthInsurance(newHealthInsurance);
            }

            if (patient.IsValid())
                _patientRepository.Update(patient);
            else
            {
                Notify(patient.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        private async Task AddClaims(Guid userId)
        {
            await AddClaimAsync(new Claim("Scheduling", "Search"), userId);
            await AddClaimAsync(new Claim("Scheduling", "Details"), userId);
        }

        #endregion Constructors
    }
}