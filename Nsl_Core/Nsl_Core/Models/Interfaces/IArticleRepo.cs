using Nsl_Core.Models.Dtos.Website;

namespace Nsl_Core.Models.Interfaces
{
    public interface IArticleRepo
    {
        int Create(ArticleDto dto, IFormFile Picture);
        int Update(ArticleDto dto, IFormFile Picture);
        ArticleDto GetArticle(int id);
    }
}
