using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSL_html.Models.Infra
{
    public static class ImageProcess
    {
        public static string HashImageName(this IFormFile photo)
        {
            return Path.GetFileNameWithoutExtension(photo.FileName) + Guid.NewGuid().GetHashCode() + Path.GetExtension(photo.FileName);
		}

		public static string UploadResult(this IFormFile photo, IWebHostEnvironment env, string photoName)
		{
			try
			{
				string result = Path.Combine(env.WebRootPath, "uploads", photoName);
				photo.CopyToLocal(result);
				return "Success";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		private static string CopyToLocal(this IFormFile photo, string photoPath)
		{
			try
			{
				using (var fileStream = new FileStream(photoPath, FileMode.Create))
				{
					photo.CopyTo(fileStream);
				}
				return $"Success! UploadPath is {photoPath}";
			}
			catch(Exception ex)
			{
				throw new Exception($"Fail! Reason is {ex.Message}");
			}
		}
	}
}
