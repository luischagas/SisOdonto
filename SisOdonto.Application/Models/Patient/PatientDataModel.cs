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

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
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
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string District { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }

        [DisplayName("Estado Civil")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public EMaritalStatus MaritalStatus { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [DisplayName("Número")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Number { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string State { get; set; }

        [DisplayName("Logradouro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Street { get; set; }

        [DisplayName("Criar Usuário?")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool CreateUser { get; set; }

        [DisplayName("Profissão")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Occupation { get; set; }

        [DisplayName("Telefone")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Telephone { get; set; }

        [DisplayName("Celular")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Cellular { get; set; }

        [DisplayName("Convênio")]
        public Guid? HealthInsuranceId { get; set; }

        public HealthInsuranceDataModel HealthInsurance { get; set; }

        public IEnumerable<HealthInsuranceDataModel> HealthInsurances { get; set; }

        #endregion Properties
    }
}