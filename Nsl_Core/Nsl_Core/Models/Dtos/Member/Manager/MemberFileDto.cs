namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberFileDto
    {
        public int MemberId { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        //public string GenderString { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        public string? CityName { get; set; }
        public string? AreaName { get; set; }
        public string? Password { get; set; }
        public string? ImageName { get; set; }
        public string? Phone { get; set; }
    }
}
