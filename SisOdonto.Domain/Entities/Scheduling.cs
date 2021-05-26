using FluentValidation;
using System;

namespace SisOdonto.Domain.Entities
{
    public class Scheduling : Entity<Scheduling>
    {
        #region Properties

        public DateTimeOffset Datetime { get; private set; }
        public Dentist Dentist { get; private set; }
        public Guid DentistId { get; private set; }
        public string Obs { get; private set; }
        public Patient Patient { get; private set; }
        public Guid PatientId { get; private set; }

        #endregion Properties

        #region Methods

        public override bool IsValid()
        {
            ValidatePatientId();
            ValidateDentistId();
            ValidateDatetime();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        private void ValidateDatetime()
        {
            RuleFor(d => d.Datetime)
                .LessThan(p => DateTime.Now)
                .WithMessage("A data deve estar no passado");
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

        #endregion Methods
    }
}