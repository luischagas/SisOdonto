using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Domain.Enums.Patient
{
    public enum EMaritalStatus
    {
        [Display(Name = "Solteiro")]
        Single = 1,

        [Display(Name = "Casado")]
        Married = 2,

        [Display(Name = "Divorciado")]
        Divorced = 3,

        [Display(Name = "Viúvo")]
        Widower = 4,

        [Display(Name = "União Estável")]
        UnionStable = 5
    }
}