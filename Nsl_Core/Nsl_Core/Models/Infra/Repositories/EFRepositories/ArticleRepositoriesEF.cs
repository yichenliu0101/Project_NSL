using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class ArticleRepositoriesEF:IArticleRepo
    {
        private readonly IWebHostEnvironment _host;
        private readonly NSL_DBContext _db;
        public ArticleRepositoriesEF(IWebHostEnvironment host,NSL_DBContext db)
        {
            _host = host;
            _db = db;
        }
        public ArticleDto GetArticle(int id)
        {
            var article = _db.Articles.Find(id);
            var dto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                ArticleContent = article.ArticleContent,
                Picture = article.Picture,
                CreatedTime = article.CreatedTime,
                ModifiedTime = article.ModifiedTime,
            };

            return dto;
        }
        public int Create(ArticleDto dto, IFormFile Picture)
        {
            try
            {
				var article = new Articles()
				{
					Title = dto.Title,
					ArticleContent = dto.ArticleContent,
					ModifiedTime = dto.ModifiedTime,
				};

				if (Picture != null)
				{
					article.Picture = Picture.FileName;
					string path = Path.Combine(_host.WebRootPath, "uploads", Picture.FileName);
					using (var filestream = new FileStream(path, FileMode.Create))
					{
						Picture.CopyTo(filestream);
					}
				}
				_db.Articles.Add(article);
				_db.SaveChanges();
			}
            catch (Exception ex)
            {
				throw new Exception("新增失敗，請聯絡管理員");
			}

            return dto.Id;
        }
        public int Update(ArticleDto dto, IFormFile Picture)
        {
            try
            {
				var article = _db.Articles.Find(dto.Id);

				article.Title = dto.Title;
				article.ArticleContent = dto.ArticleContent;
				article.ModifiedTime = DateTime.Now;
				if (Picture != null)
				{
					article.Picture = Picture.FileName;
					string path = Path.Combine(_host.WebRootPath, "uploads", Picture.FileName);
					using (var filestream = new FileStream(path, FileMode.Create))
					{
						Picture.CopyTo(filestream);
					}
				}
				_db.SaveChanges();
			}
            catch
            {
				throw new Exception("更新失敗，請聯絡管理員");
			}

            return dto.Id;
        }
    }
}
