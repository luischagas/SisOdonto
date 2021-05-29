using Microsoft.AspNetCore.Identity;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Models.HealthInsurance;
using SisOdonto.Domain.Entities;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisOdonto.Application.Services
{
    public class HealthInsuranceService : AppService, IHealthInsuranceService
    {
        #region Fields

        private readonly IHealthInsuranceRepository _healthInsuranceRepository;
        private readonly IPatientRepository _patientRepository;

        #endregion Fields

        #region Constructors

        public HealthInsuranceService(IUnitOfWork unitOfWork,
            INotifier notifier,
            UserManager<IdentityUser> userManager,
            IHealthInsuranceRepository dentistRepository, 
            IPatientRepository patientRepository)
            : base(unitOfWork, notifier, userManager)
        {
            _healthInsuranceRepository = dentistRepository;
            _patientRepository = patientRepository;
        }

        public async Task Create(HealthInsuranceDataModel request)
        {
            var hasHealthInsurance = await _healthInsuranceRepository.GetAsync(request.Name);

            if (hasHealthInsurance is not null)
            {
                Notify("Já existe um convênio cadastrado com este nome informado.");
                return;
            }

            var healthInsurance = new HealthInsurance(request.Name, request.Type);

            if (healthInsurance.IsValid())
                await _healthInsuranceRepository.AddAsync(healthInsurance);
            else
            {
                Notify(healthInsurance.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task Delete(Guid id)
        {
            var healthInsurance = await _healthInsuranceRepository.GetAsync(id);

            if (healthInsurance is null)
            {
                Notify("Dados do Convênio não encontrado.");
                return;
            }

            var patient = await _patientRepository.GetByHealthInsuranceAsync(id);

            if (patient is not null)
            {
                Notify("Não é possível excluir o convênio, pois existem pacientes associado a ele.");
                return;
            }

            healthInsurance.Delete();

            if (healthInsurance.IsValid())
                _healthInsuranceRepository.Update(healthInsurance);
            else
            {
                Notify(healthInsurance.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }

        public async Task<HealthInsuranceDataModel> Get(Guid id)
        {
            HealthInsuranceDataModel healthInsuranceModel = null;

            var healthInsurance = await _healthInsuranceRepository.GetAsync(id);

            if (healthInsurance is null)
                Notify("Dados do Convênio não encontrado.");
            else
            {
                healthInsuranceModel = new HealthInsuranceDataModel()
                {
                    Id = healthInsurance.Id,
                    Name = healthInsurance.Name,
                    Type = healthInsurance.Type
                };
            }

            return healthInsuranceModel;
        }

        public async Task<IEnumerable<HealthInsuranceDataModel>> GetAll()
        {
            var healthInsurances = await _healthInsuranceRepository.GetAllAsync();

            var healthInsurancesModel = new List<HealthInsuranceDataModel>();

            foreach (var healthInsurance in healthInsurances)
            {
                healthInsurancesModel.Add(new HealthInsuranceDataModel()
                {
                    Id = healthInsurance.Id,
                    Name = healthInsurance.Name,
                    Type = healthInsurance.Type
                });
            }

            return healthInsurancesModel;
        }
        public async Task Update(HealthInsuranceDataModel request)
        {
            var healthInsurance = await _healthInsuranceRepository.GetAsync(request.Id);

            if (healthInsurance is null)
            {
                Notify("Dados do Convênio não encontrado.");
                return;
            }

            healthInsurance.Update(request.Name, request.Type);

            if (healthInsurance.IsValid())
                _healthInsuranceRepository.Update(healthInsurance);
            else
            {
                Notify(healthInsurance.ValidationResult);
                return;
            }

            if (await CommitAsync() is false)
                Notify("Erro ao salvar dados.");
        }
        #endregion Constructors
    }
}