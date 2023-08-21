namespace Nsl_Core.Models.Dtos.Member.Login
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public int citys { get; set; }
        public int Area { get; set; }
        public string Email { get; set; }
        public int EmailToken { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
