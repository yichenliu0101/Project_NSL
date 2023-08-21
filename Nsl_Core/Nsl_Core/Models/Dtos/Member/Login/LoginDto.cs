namespace Nsl_Core.Models.Dtos.Member.Login
{
    public class LoginDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageName { get; set; }
        public int Role { get; set; }
        public bool EmailCheck { get; set; }
	}
}
