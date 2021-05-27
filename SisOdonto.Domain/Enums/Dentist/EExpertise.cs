using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Domain.Enums.Dentist
{
    public enum EExpertise
    {
        [Display(Name = "Clínico Geral")]
        GeneralPractitioner = 1,

        [Display(Name = "Ortodondia")]
        Orthodontics = 2,

        [Display(Name = "Implantodontia")]
        Implantology = 3,

        [Display(Name = "Endodontia")]
        Endodontics = 4
    }
}