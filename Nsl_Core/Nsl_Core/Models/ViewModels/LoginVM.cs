using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nsl_Core.Models.ViewModels
{
    public class LoginVM
    {
        //[DisplayName("帳號")]
        //[MaxLength(50, ErrorMessage = "{0}長度不可大於{1}")]
        //[Required(ErrorMessage = "{0}必填")]
        //[EmailAddress(ErrorMessage ="帳號格式錯誤")]
        public string Account { get; set; }

        //[DisplayName("密碼")]
        //[MaxLength(20, ErrorMessage = "{0}長度不可大於{1}")]
        //[Required(ErrorMessage = "{0}必填")]
        //[RegularExpression("[a-zA-Z]{8,}",ErrorMessage ="輸入格式錯誤,請輸入8至20字元[英文]或[數字]")]
        public string Password { get; set; }
    }
}
