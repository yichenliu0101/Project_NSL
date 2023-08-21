namespace Nsl_Core.Models.Dtos.Website
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ArticleContent { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Picture { get; set; }

    }
}
