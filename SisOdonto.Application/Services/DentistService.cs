using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Dentist;
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
    public class DentistService : AppService, IDentistService
    {
        #region Fields

        private readonly IDentistRepository _dentistRepository;
        private readonly ISchedulingRepository _schedulingRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        #endregion Fields

        #region Constructors

        public DentistService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IDentistRepository dentistRepository,
            IEmailService emailService, ISchedulingRepository schedulingRepository)
            : base(unitOfWork, notifier, userManager)
        {
            _userManager = userManager;
            _dentistRepository = dentistRepository;
            _emailService = emailService;
            _schedulingRepository = schedulingRepository;
        }

        public async Task<DentistDataModel> Get(Guid id)
        {
            DentistDataModel dentistModel = null;

            var dentist = await _dentistRepository.GetAsync(id);

            if (dentist is null)
                Notify("Dados do Dentista não encontrado.");
            else
            {
                dentistModel = new DentistDataModel()
                {
                    Id = dentist.Id,
                    BirthDate = dentist.BirthDate,
                    Cep = dentist.Cep,
                    City = dentist.City,
                    Complement = dentist.Complement,
                    Cpf = dentist.Cpf,
                    Cro = dentist.Cro,
                    Street = dentist.Street,
                    Number = dentist.Number,
                    State = dentist.State,
                    District = dentist.District,
                    Email = dentist.Email,
                    Expertise = dentist.Expertise,
                    Name = dentist.Name
                };
            }

            return dentistModel;
        }

        public async Task<IEnumerable<DentistDataModel>> GetAll()
        {
            var dentists = await _dentistRepository.GetAllAsync();

            var dentistsModel = new List<DentistDataModel>();

            foreach (var dentist in dentists)
            {
                dentistsModel.Add(new DentistDataModel()
                {
                    Id = dentist.Id,
                    BirthDate = dentist.BirthDate,
                    Cep = dentist.Cep,
                    City = dentist.City,
                    Complement = dentist.Complement,
                    Cpf = dentist.Cpf,
                    Cro = dentist.Cro,
                    Street = dentist.Street,
                    Number = dentist.Number,
                    State = dentist.State,
                    District = dentist.District,
                    Email = dentist.Email,
                    Expertise = dentist.Expertise,
                    Name = dentist.Name
                });
            }

            return dentistsModel;
        }

        public async Task Create(DentistDataModel request)
        {
            var hasDentist = await _dentistRepository.GetAsync(request.Cpf);

            if (hasDentist is not null)
            {
                Notify("Já existe um dentista cadastrado com este cpf informado.");
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

            var dentist = new Dentist(userId, request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Cro, request.Expertise);

            if (dentist.IsValid())
                await _dentistRepository.AddAsync(dentist);
            else
            {
                var userManager = await _userManager.FindByIdAsync(userId.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);

                Notify(dentist.ValidationResult);
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

        public async Task Update(DentistDataModel request)
        {
            var dentist = await _dentistRepository.GetAsync(request.Id);

            if (dentist is null)
            {
                Notify("Dados do Dentista não encontrado.");
                return;
            }

            if (request.BirthDate == DateTime.MinValue)
            {
                Notify("Data inválida");
                return;
            }

            dentist.Update(request.BirthDate, request.Cep, request.City, request.Complement, request.Cpf.Replace(".", "").Replace("-", ""), request.District, request.Email, request.Name, request.Number, request.State, request.Street, request.Cro, request.Expertise);

            if (dentist.IsValid())
                _dentistRepository.Update(dentist);
            else
            {
                Notify(dentist.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task Delete(Guid id)
        {
            var dentist = await _dentistRepository.GetAsync(id);

            if (dentist is null)
            {
                Notify("Dados do Dentista não encontrado.");
                return;
            }

            var scheduling = await _schedulingRepository.GetAllByDentistAsync(id);

            if (scheduling.Any())
            {
                Notify("Não é possível excluir o dentista, pois existem agendamentos cadastrados para ele.");
                return;
            }

            dentist.Delete();

            _dentistRepository.Update(dentist);

            if (await CommitAsync())
            {
                var userManager = await _userManager.FindByIdAsync(id.ToString());

                if (userManager is not null)
                    await _userManager.DeleteAsync(userManager);
            }
            else
                Notify("Erro ao salvar dados.");
        }

        private async Task AddClaims(Guid userId)
        {
            await AddClaimAsync(new Claim("Patient", "Search"), userId);
            await AddClaimAsync(new Claim("Patient", "Details"), userId);

            await AddClaimAsync(new Claim("Scheduling", "Search"), userId);
            await AddClaimAsync(new Claim("Scheduling", "Details"), userId);
        }

        #endregion Constructors
    }
}