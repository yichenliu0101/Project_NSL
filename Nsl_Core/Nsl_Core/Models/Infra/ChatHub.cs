using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.EFModels;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Text.Json;

namespace Nsl_Core.Models.Infra
{
    public class ChatHub : Hub
    {
        private readonly NSL_DBContext _db;
        public static List<ChatNumber> _connIDList = new List<ChatNumber>();

        public ChatHub(NSL_DBContext db)
        {
            _db = db;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string? token = httpContext.Request.Cookies.Get<string>("Login");
            if (string.IsNullOrEmpty(token)) return;

            var loginDto = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(token));
            _connIDList = _db.ChatNumber.ToList();

            var dbNumber = _db.ChatNumber.Where(x => x.MemberId == loginDto.Id).FirstOrDefault();
            //更新資料庫連線
            if (dbNumber == null)
            {
                var addNumber = new ChatNumber()
                {
                    MemberId = loginDto.Id,
                    RandomNumber = Context.ConnectionId
                };
                _db.ChatNumber.Add(addNumber);
                _connIDList.Add(addNumber);
            }
            else
            {
                dbNumber.RandomNumber = Context.ConnectionId;
                _connIDList.Add(dbNumber);
            }
            _db.SaveChanges();
            await base.OnConnectedAsync();
        }

        //聊天
        public async Task SendMessage(string message, string sendToID)
        {
            try
            {
				if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(sendToID)) return;
				var httpContext = Context.GetHttpContext();
				string? token = httpContext.Request.Cookies.Get<string>("Login");
				if (string.IsNullOrEmpty(token)) return;

				else
				{
					var loginDto = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(token));
					var dbMess = new Message()
					{
						SenderId = loginDto.Id,
						RecipientId = Convert.ToInt32(sendToID),
						MessageText = message,
						CreateTime = DateTime.Now,
					};
					_db.Message.Add(dbMess);
					_db.SaveChanges();

					// 接收人
					var isnum = _db.ChatNumber.Where(x => x.MemberId == Convert.ToInt32(sendToID)).FirstOrDefault();
					if (isnum != null)
					{
						var messnum = _db.ChatNumber.Find(Convert.ToInt32(sendToID)).RandomNumber;
						await Clients.Client(messnum).SendAsync("UpdContent", message, loginDto.Id);
					}
				}
			}
            catch
            {
				await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "無法傳送訊息，請稍後再試",0);
			}
            
        }
    }
}