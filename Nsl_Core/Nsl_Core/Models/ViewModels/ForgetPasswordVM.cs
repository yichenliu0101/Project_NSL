using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nsl_Core.Models.ViewModels
{
    public class ForgetPasswordVM
    {
        [DisplayName("電子郵件")]
        [MaxLength(50, ErrorMessage = "{0}長度不可大於{1}")]
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "帳號格式錯誤")]
        public string? Email { get; set; }
    }
}
