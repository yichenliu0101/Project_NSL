using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nsl_Core.Models.ViewModels
{
    public class VerifyPasswordVM
    {
        [DisplayName("密碼")]
        [MaxLength(20, ErrorMessage = "{0}長度不可大於{1}")]
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression("[a-zA-Z0-9]{8,}", ErrorMessage = "輸入格式錯誤,請輸入8至20字元[英文]或[數字]")]
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
