using FluentValidation;
using SisOdonto.Domain.Enums.Scheduling;
using System;
using SisOdonto.Domain.Enums.Dentist;

namespace SisOdonto.Domain.Entities
{
    public class Scheduling : Entity<Scheduling>
    {
        #region Constructors

        public Scheduling(DateTime datetime, Guid dentistId, string obs, Guid patientId, ETypeScheduling type, EExpertise typeExpertise)
        {
            Datetime = datetime;
            DentistId = dentistId;
            Obs = obs;
            PatientId = patientId;
            Type = type;
            TypeExpertise = typeExpertise;
        }

        protected Scheduling(EExpertise typeExpertise)
        {
            TypeExpertise = typeExpertise;
        }

        #endregion Constructors

        #region Properties

        public DateTime Datetime { get; private set; }
        public Dentist Dentist { get; private set; }
        public Guid DentistId { get; private set; }
        public string Obs { get; private set; }
        public Patient Patient { get; private set; }
        public Guid PatientId { get; private set; }
        public ETypeScheduling Type { get; private set; }

        public EExpertise TypeExpertise { get; private set; }

        #endregion Properties

        #region Methods

        public void SetDentist(Dentist dentist)
        {
            if (dentist.IsValid())
                Dentist = dentist;

            AddErrors(dentist.ValidationResult);
        }

        public void SetPatient(Patient patient)
        {
            if (patient.IsValid())
                Patient = patient;

            AddErrors(patient.ValidationResult);
        }

        public void Update(DateTime datetime, string obs, ETypeScheduling type, EExpertise typeExpertise)
        {
            Datetime = datetime;
            Obs = obs;
            Type = type;
            TypeExpertise = typeExpertise;
        }

        public override bool IsValid()
        {
            ValidatePatientId();
            ValidateDentistId();
            ValidateDatetime();
            ValidateType();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        private void ValidateDatetime()
        {
            RuleFor(d => d.Datetime)
                .GreaterThan(p => DateTime.Now)
                .WithMessage("A data deve estar no futuro");
        }

        private void ValidateDentistId()
        {
            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("O id do dentista é requerido")
                .NotEqual(Guid.Empty).WithMessage("O id do dentista deve ser válido");
        }

        private void ValidatePatientId()
        {
            RuleFor(a => a.PatientId)
                .NotEmpty().WithMessage("O id do paciente é requerido")
                .NotEqual(Guid.Empty).WithMessage("O id do paciente deve ser válido");
        }

        private void ValidateType()
        {
            RuleFor(d => d.Type)
                .IsInEnum()
                .WithMessage("O tipo deve ter um valor válido");
        }

        #endregion Methods
    }
}