using SisOdonto.Domain.Enums.HealthInsurance;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Application.Models.HealthInsurance
{
    public class HealthInsuranceDataModel
    {
        #region Properties

        public Guid Id { get; set; }

        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Tipo")]
        [Required]
        public ETypeHealthInsurance Type { get; set; }

        #endregion Properties
    }
}