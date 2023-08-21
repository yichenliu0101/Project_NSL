namespace Nsl_Core.Models.Dtos.Member.Login
{
    public class LineLoginDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageName { get; set; }
        public bool EmailCheck { get; set; }
        public int Role { get; set; }

        public string? LineId { get; set; }
    }
}
