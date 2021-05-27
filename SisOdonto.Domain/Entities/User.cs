using FluentValidation;
using FluentValidation.Results;
using System;
using SisOdonto.Domain.Utils;

namespace SisOdonto.Domain.Entities
{
    public abstract class User : Entity<User>
    {
        #region Properties

        public DateTimeOffset BirthDate { get; protected set; }
        public string Cep { get; protected set; }
        public string City { get; protected set; }
        public string Complement { get; protected set; }
        public string Cpf { get; protected set; }
        public string District { get; protected set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public string Number { get; protected set; }
        public string State { get; protected set; }
        public string Street { get; protected set; }
        #endregion Properties

        #region Methods

        public override bool IsValid()
        {
            ValidateName();
            ValidateBirthDate();
            ValidateStreet();
            ValidateNumber();
            ValidateDistrict();
            ValidateState();
            ValidateCity();
            ValidateCep();
            ValidateCpf();
            ValidateEmail();

            AddErrors(Validate(this));

            return ValidationResult.IsValid;
        }

        private void ValidateBirthDate()
        {
            RuleFor(d => d.BirthDate)
                .LessThan(p => DateTime.Now)
                .WithMessage("A data deve estar no passado");
        }

        private void ValidateCep()
        {
            RuleFor(d => d.Cep)
                .NotEmpty()
                .WithMessage("O CEP deve ser preenchido");
        }

        private void ValidateCity()
        {
            RuleFor(d => d.City)
                .Length(3, 100)
                .WithMessage("A cidade deve ter entre 3 e 100 caracteres.");
        }

        private void ValidateCpf()
        {
            RuleFor(d => d.Cpf)
                .NotEmpty()
                .WithMessage("O CPF deve ser preenchido")
                .When(c => string.IsNullOrEmpty(c.Cpf) is false);
            RuleFor(f => DocValidation.Validate(f.Cpf)).Equal(true)
                .WithMessage("O CPF fornecido é inválido.");
        }

        private void ValidateDistrict()
        {
            RuleFor(d => d.District)
                .Length(3, 100)
                .WithMessage("O bairro deve ter entre 3 e 100 caracteres.");
        }

        private void ValidateEmail()
        {
            RuleFor(d => d.Email)
                .NotEmpty()
                .WithMessage("O Email deve ser preenchido")
                .When(c => string.IsNullOrEmpty(c.Email) is false);
        }

        private void ValidateName()
        {
            RuleFor(d => d.Name)
                .Length(3, 100)
                .WithMessage("O nome deve ter entre 3 e 100 caracteres.");
        }

        private void ValidateNumber()
        {
            RuleFor(d => d.Number)
                .Length(1, 100)
                .WithMessage("O número deve ter entre 1 e 100 caracteres.");
        }

        private void ValidateState()
        {
            RuleFor(d => d.State)
                .Length(1, 100)
                .WithMessage("O estado deve ter entre 1 e 100 caracteres.");
        }

        private void ValidateStreet()
        {
            RuleFor(d => d.Street)
                .Length(3, 100)
                .WithMessage("O endereço deve ter entre 3 e 100 caracteres.");
        }

        #endregion Methods
    }
}