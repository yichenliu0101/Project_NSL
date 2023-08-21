using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.Repositories.ADORepositories;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Models.Services
{
    public class ArticleService
    {
        private readonly IWebHostEnvironment _host;
        private readonly NSL_DBContext _db;
        public ArticleService(IWebHostEnvironment host, NSL_DBContext db)
        {
            _host = host;
            _db = db;
        }
        public int Create(ArticleDto dto, IFormFile Picture)
        {
            try
            {
                if (dto.Title.Length >= 50) throw new Exception("標題請小於50字元");

                //建立一筆新紀錄
                IArticleRepo repo = new ArticleRepositoriesEF(_host, _db);
                int newId = repo.Create(dto, Picture);

                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int Update(ArticleDto dto, IFormFile Picture)
        {
            try
            {
                if (dto.Title.Length >= 50) throw new Exception("標題請小於50字元");

                IArticleRepo repo = new ArticleRepositoriesEF(_host, _db);
                int newId = repo.Update(dto, Picture);

                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
