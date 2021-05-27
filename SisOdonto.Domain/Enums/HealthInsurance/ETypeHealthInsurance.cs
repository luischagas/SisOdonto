using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Domain.Enums.HealthInsurance
{
    public enum ETypeHealthInsurance
    {
        [Display(Name = "Individual")]
        Individual = 1,

        [Display(Name = "Empresarial")]
        Business = 2
    }
}