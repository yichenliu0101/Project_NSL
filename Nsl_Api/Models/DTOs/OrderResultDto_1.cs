using System.Security.Policy;

namespace Nsl_Api.Models.DTOs
{
    public class OrderResultDto
    {
        public bool isSuccess { get; set; }
        public string errMsg { get; set; }
    }
}
