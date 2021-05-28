using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Domain.Enums.Scheduling
{
    public enum ETypeScheduling
    {
        [Display(Name = "Particular")]
        Particular = 1,

        [Display(Name = "Convênio")]
        HealthInsurance = 2,
    }
}