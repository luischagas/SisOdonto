using System.ComponentModel.DataAnnotations;

namespace SisOdonto.Application.Models.Common
{
    public class UpdatePasswordModel
    {
        #region Properties

        [DataType(DataType.Password)]
        [Display(Name = "Confirme Sua Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação precisam ser iguais.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        #endregion Properties
    }
}