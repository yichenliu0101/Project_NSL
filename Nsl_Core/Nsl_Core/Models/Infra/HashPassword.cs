using Nsl_Core.Models.EFModels;
using System.Security.Cryptography;
using System.Text;

namespace Nsl_Core.Models.Infra
{
	public class HashPassword
	{
		private readonly NSL_DBContext _db;
		public HashPassword(NSL_DBContext db)
		{
			_db = db;
		}
		private string GenerateSalt()
		{
			byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
			return Convert.ToBase64String(salt);
		}

		public string GenerateHashPassword(string password, out string salt)
		{
			salt = GenerateSalt();
			using(var hashAlgorithm = SHA256.Create())
			{
				var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
				var hash = hashAlgorithm.ComputeHash(passwordBytes);
				return Convert.ToBase64String(hash);
			}
		}
		public string GenerateHashPassword(string password, string salt)
		{
			using (var hashAlgorithm = SHA256.Create())
			{
				var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
				var hash = hashAlgorithm.ComputeHash(passwordBytes);
				return Convert.ToBase64String(hash);
			}
		}
		/// <summary>
		/// 驗證密碼是否正確，回傳bool
		/// </summary>
		/// <param name="password"></param>
		/// <param name="memberId"></param>
		/// <returns></returns>
		public bool VerifyPassword(string password, string email)
		{
			var saltAndPassword = _db.Members.Where(x => x.Email == email)
								.Select(x => new { x.Salt, x.EncryptedPassword }).FirstOrDefault();
			return (GenerateHashPassword(password,saltAndPassword.Salt) == saltAndPassword.EncryptedPassword)? true : false;
		}
	}
}
