using SisOdonto.Domain.Enums.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SisOdonto.Application.Models.HealthInsurance;

namespace SisOdonto.Application.Models.Patient
{
    public class PatientDataModel
    {
        #region Properties

        public Guid Id { get; set; }

        [Required]
        [DisplayName("Data de Nascimento")]
        public DateTimeOffset BirthDate { get; set; }

        [Required]
        [DisplayName("CEP")]
        public string Cep { get; set; }

        [Required]
        [DisplayName("Cidade")]
        public string City { get; set; }

        [DisplayName("Complemento")]
        public string Complement { get; set; }

        [DisplayName("CPF")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Cpf { get; set; }

        [DisplayName("Sexo")]
        [Required(ErrorMessage = "O Sexo é requerido.")]
        public EGender Gender { get; set; }

        [DisplayName("Bairro")]
        [Required]
        public string District { get; set; }

        [DisplayName("E-mail")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Estado Civil")]
        [Required]
        public EMaritalStatus MaritalStatus { get; set; }

        [DisplayName("Nome")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Número")]
        [Required]
        public string Number { get; set; }

        [DisplayName("Estado")]
        [Required]
        public string State { get; set; }

        [DisplayName("Logradouro")]
        [Required]
        public string Street { get; set; }

        [DisplayName("Criar Usuário?")]
        [Required]
        public bool CreateUser { get; set; }

        [DisplayName("Profissão")]
        [Required]
        public string Occupation { get; set; }

        [DisplayName("Telefone")]
        [Required]
        public string Telephone { get; set; }

        [DisplayName("Celular")]
        [Required]
        public string Cellular { get; set; }

        [Required]
        [DisplayName("Convênio")]
        public Guid HealthInsuranceId { get; set; }

        public HealthInsuranceDataModel HealthInsurance { get; set; }

        public IEnumerable<HealthInsuranceDataModel> HealthInsurances { get; set; }

        #endregion Properties
    }
}