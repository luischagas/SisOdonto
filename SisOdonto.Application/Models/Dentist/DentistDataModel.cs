using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SisOdonto.Domain.Enums.Dentist;

namespace SisOdonto.Application.Models.Dentist
{
    public class DentistDataModel
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

        [DisplayName("CRO")]
        [Required(ErrorMessage = "O CRO é requerido.")]
        public string Cro { get; set; }

        [DisplayName("Bairro")]
        [Required]
        public string District { get; set; }

        [DisplayName("E-mail")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Especialidade")]
        [Required]
        public EExpertise Expertise { get; set; }

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

        #endregion Properties
    }
}