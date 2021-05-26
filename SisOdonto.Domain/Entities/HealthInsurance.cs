using FluentValidation;
using SisOdonto.Domain.Enums.HealthInsurance;
using System;

namespace SisOdonto.Domain.Entities
{
    public class HealthInsurance : Entity<HealthInsurance>
    {
        #region Constructors

        public HealthInsurance(string name, ETypeHealthInsurance type)
        {
            Name = name;
            Type = type;
        }

        protected HealthInsurance()
        {
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }
        public ETypeHealthInsurance Type { get; private set; }

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