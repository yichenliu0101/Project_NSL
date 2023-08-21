using Microsoft.EntityFrameworkCore.Query.Internal;
using Nsl_Api.Models.EFModels;
using System.Net;

namespace Nsl_Api.Models.DTOs
{
    public class MessageListDto
    {
        public string Name { get; set; }
        public int LastMessageIndex { get; set; }
        public int ChatMemberId { get; set; }
        public List<spMessageResult> List { get; set; }
    }
}
