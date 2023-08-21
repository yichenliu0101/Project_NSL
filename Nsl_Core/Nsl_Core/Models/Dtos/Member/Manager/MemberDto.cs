using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        [DisplayName("密碼")]
        [MaxLength(20, ErrorMessage = "{0}長度不可大於{1}")]
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression("[a-zA-Z0-9]{8,}", ErrorMessage = "輸入格式錯誤,請輸入8至20字元[英文]或[數字]")]
        public string? Password { get; set; }
        public bool? EmailCheck { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }
        public string? ImageName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int Role { get; set; }
        public bool? Gender { get; set; }
        public string DoAction { get; set; }
        public string Msg { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
