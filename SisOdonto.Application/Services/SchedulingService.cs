using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.Dentist;
using SisOdonto.Application.Models.Patient;
using SisOdonto.Application.Models.Scheduling;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisOdonto.Domain.Enums.Dentist;

namespace SisOdonto.Application.Services
{
    public class SchedulingService : AppService, ISchedulingService
    {
        #region Fields

        private readonly IDentistRepository _dentistRepository;
        private readonly IDentistService _dentistService;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientService _patientService;
        private readonly ISchedulingRepository _schedulingRepository;

        #endregion Fields

        #region Constructors

        public SchedulingService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            ISchedulingRepository schedulingRepository,
            IDentistService dentistService,
            IPatientService patientService,
            IDentistRepository dentistRepository,
            IPatientRepository patientRepository)
            : base(unitOfWork, notifier, userManager)
        {
            _schedulingRepository = schedulingRepository;
            _dentistService = dentistService;
            _patientService = patientService;
            _dentistRepository = dentistRepository;
            _patientRepository = patientRepository;
        }

        public async Task Create(SchedulingDataModel request)
        {
            var dentist = await _dentistService.Get(request.DentistId);

            if (dentist is null)
            {
                Notify("Dentista não encontrado");
                return;
            }

            var patient = await _patientService.Get(request.Patient.Id);

            if (patient is null)
            {
                Notify("Paciente não encontrado");
                return;
            }

            if (request.DateTime == DateTime.MinValue)
            {
                Notify("Data inválida");
                return;
            }

            var scheduling = new Scheduling(request.DateTime, dentist.Id, request.Obs, patient.Id, request.Type, dentist.Expertise);

            if (scheduling.IsValid())
                await _schedulingRepository.AddAsync(scheduling);
            else
            {
                Notify(scheduling.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task Delete(Guid id)
        {
            var scheduling = await _schedulingRepository.GetAsync(id);

            if (scheduling is null)
            {
                Notify("Dados do Agendamento não encontrado.");
                return;
            }

            scheduling.Delete();

            if (scheduling.IsValid())
                _schedulingRepository.Update(scheduling);
            else
            {
                Notify(scheduling.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task<SchedulingDataModel> Get(Guid id)
        {
            SchedulingDataModel schedulingModel = null;

            var scheduling = await _schedulingRepository.GetAsync(id);

            if (scheduling is null)
                Notify("Dados do Agendamento não encontrado.");
            else
            {
                schedulingModel = new SchedulingDataModel()
                {
                    Id = scheduling.Id,
                    Patient = new PatientDataModel()
                    {
                        Cpf = scheduling.Patient.Cpf,
                        Name = scheduling.Patient.Name,
                        BirthDate = scheduling.Patient.BirthDate,
                        Street = scheduling.Patient.Street,
                        Number = scheduling.Patient.Number,
                        Complement = scheduling.Patient.Complement,
                        District = scheduling.Patient.District,
                        State = scheduling.Patient.State,
                        City = scheduling.Patient.City,
                    },
                    PatientId = scheduling.Patient.Id,
                    Dentist = new DentistDataModel()
                    {
                        Id = scheduling.Dentist.Id,
                        Name = scheduling.Dentist.Name
                    },
                    DentistId = scheduling.Dentist.Id,
                    TypeExpertise = scheduling.Dentist.Expertise,
                    DateTime = scheduling.Datetime,
                    Type = scheduling.Type,
                    Obs = scheduling.Obs,
                    Dentists = await _dentistService.GetAll()
                };
            }

            return schedulingModel;
        }

        public async Task<IEnumerable<SchedulingDataModel>> GetAll(string typeUser)
        {
            var schedulings = await _schedulingRepository.GetAllAsync();

            var schedulingsModel = new List<SchedulingDataModel>();

            foreach (var scheduling in schedulings)
            {
                schedulingsModel.Add(new SchedulingDataModel()
                {
                    Id = scheduling.Id,
                    Patient = new PatientDataModel()
                    {
                        Name = scheduling.Patient.Name,
                        BirthDate = scheduling.Patient.BirthDate,
                        Street = scheduling.Patient.Street,
                        Number = scheduling.Patient.Number,
                        Complement = scheduling.Patient.Complement,
                        District = scheduling.Patient.District,
                        State = scheduling.Patient.State,
                        City = scheduling.Patient.City,
                    },
                    Dentist = new DentistDataModel()
                    {
                        Id = scheduling.Dentist.Id,
                        Name = scheduling.Dentist.Name,
                        Expertise = scheduling.Dentist.Expertise
                    },
                    DateTime = scheduling.Datetime,
                    Type = scheduling.Type,
                    Obs = scheduling.Obs
                });
            }

            return schedulingsModel;
        }

        public async Task Update(SchedulingDataModel request)
        {
            var scheduling = await _schedulingRepository.GetAsync(request.Id);

            if (scheduling is null)
            {
                Notify("Dados do Agendamento não encontrado.");
                return;
            }

            var dentist = await _dentistService.Get(request.DentistId);

            if (dentist is null)
            {
                Notify("Dentista não encontrado");
                return;
            }

            if (request.DateTime == DateTime.MinValue)
            {
                Notify("Data inválida");
                return;
            }

            scheduling.Update(request.DateTime, request.Obs, request.Type, dentist.Expertise);

            if (scheduling.Dentist.Id != request.DentistId && request.DentistId != Guid.Empty)
            {
                var newDentist = await _dentistRepository.GetAsync(request.DentistId);

                scheduling.SetDentist(newDentist);
            }

            if (scheduling.Patient.Id != request.Patient.Id && request.Patient.Id != Guid.Empty)
            {
                var newPatient = await _patientRepository.GetAsync(request.Patient.Id);

                scheduling.SetPatient(newPatient);
            }

            if (scheduling.IsValid())
                _schedulingRepository.Update(scheduling);
            else
            {
                Notify(scheduling.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        #endregion Constructors
    }
}