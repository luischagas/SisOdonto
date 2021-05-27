using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Domain.Enums.Patient
{
    public enum EGender
    {
        [Display(Name = "Masculino")]
        Male = 1,

        [Display(Name = "Feminino")]
        Female = 2,
            
        [Display(Name = "Outro")]
        Other = 3
    }
}