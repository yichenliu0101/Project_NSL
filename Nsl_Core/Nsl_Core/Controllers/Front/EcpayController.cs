using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http;

namespace Nsl_Core.Controllers.Front
{
    public class EcpayController : ApiController
    {
        /// <summary>
        /// 綠界回傳 付款資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPayInfo")]
        public HttpResponseMessage AddPayInfo(object info)
        {
            try
            {
                //var order = info.ToString();
                //var cache = MemoryCache.Default;
                
                //cache.Set(order.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }

        /// <summary>
        /// 綠界回傳 虛擬帳號資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAccountInfo(JObject info)
        {
            try
            {
                var cache = MemoryCache.Default;
                cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }

        /// <summary>
        /// 回傳給 綠界 失敗
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseError()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("0|Error");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        /// <summary>
        /// 回傳給 綠界 成功
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseOK()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("1|OK");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

    }
}
