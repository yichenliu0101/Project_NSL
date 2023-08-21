using Nsl_Core.Models.EFModels;

namespace Nsl_Core.Models.Dtos.Website.Index
{
    public class IndexDto
    {
        public int Member { get; set; }
        public int Teacher { get; set; }
        public int FeedBack { get; set; }
        public List<IndexMemberCommentDto> Comments { get; set; }
        public List<Articles> Articles { get; set; }
    }
}
