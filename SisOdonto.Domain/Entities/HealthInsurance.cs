using FluentValidation;
using SisOdonto.Domain.Enums.HealthInsurance;
using System.Collections.Generic;

namespace SisOdonto.Domain.Entities
{
    public class HealthInsurance : Entity<HealthInsurance>
    {
        #region Constructors

        private IList<Patient> _patients;

        public HealthInsurance(string name, ETypeHealthInsurance type)
        {
            Name = name;
            Type = type;
            _patients = new List<Patient>();
        }

        protected HealthInsurance()
        {
            _patients = new List<Patient>();
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }
        public ETypeHealthInsurance Type { get; private set; }
        public IEnumerable<Patient> Patients => _patients;

        #endregion Properties

        #region Methods

        public override bool IsValid()
        {
            ValidateName();
            ValidateType();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        public void Update(string name, ETypeHealthInsurance type)
        {
            Name = name;
            Type = type;
        }

        private void ValidateName()
        {
            RuleFor(d => d.Name)
                .NotEmpty()
                .WithMessage("O nome deve ser preenchido");
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