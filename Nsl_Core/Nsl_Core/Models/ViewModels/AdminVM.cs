namespace Nsl_Core.Models.ViewModels
{
	public class AdminVM
	{
		private readonly int _role = 3;
		private readonly bool _emailCheck = true;
        public string Name { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
        public string? ImageName { get; set; }
		public int Role
		{
			get
			{
				return _role;
			}
		}
		public bool EmailCheck
		{
			get
			{
				return _emailCheck;
			}
		}
    }
}
