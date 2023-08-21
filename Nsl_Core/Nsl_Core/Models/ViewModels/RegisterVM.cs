using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nsl_Core.Models.ViewModels
{
    public class RegisterVM
    {
        [DisplayName("姓名")]
        [Required(ErrorMessage = "{0}必填")]
        public string Name { get; set; }
        [DisplayName("性別")]
        [Required(ErrorMessage = "{0}必填")]
        public bool? Gender { get; set; }
        [DisplayName("生日")]
        [Required(ErrorMessage = "{0}必填")]
        public DateTime? Birthday { get; set; }
        [DisplayName("電話")]
        [Required(ErrorMessage = "{0}必填")]
        public string? Phone { get; set; }
        [DisplayName("縣市")]
        [Required(ErrorMessage = "{0}必選")]
        public int? City { get; set; }
        [DisplayName("區域")]
        [Required(ErrorMessage = "{0}必選")]
        public int? Area { get; set; }

        [DisplayName("電子郵件")]
        [MaxLength(50, ErrorMessage = "{0}長度不可大於{1}")]
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "帳號格式錯誤")]
        public string? Email { get; set; }

        [DisplayName("密碼")]
        [MaxLength(20, ErrorMessage = "{0}長度不可大於{1}")]
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression("[a-zA-Z0-9]{8,}", ErrorMessage = "輸入格式錯誤,請輸入8至20字元[英文]或[數字]")]
        public string? Password { get; set; }

        public string? ImageName { get; set; }
    }
}
