using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;
using System.Diagnostics.Metrics;
using System;
using NuGet.Protocol.Plugins;
using Humanizer;

namespace Nsl_Api.Models.Infra.Repositories.DapperRepositories
{
    public class ChatsRepo
    {
        private readonly SqlConnection _conn;
        public ChatsRepo(IConfiguration configuration)
        {
            _conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
        }
        public async Task<List<MessageListDto>> GetMessage(NSL_DBContext db,int memberId)
        {
            var messageListDto = new List<MessageListDto>();
            var chatIdList = GetChatsId(memberId);
            foreach (var chatId in chatIdList)
            {
                var dto = new NSL_DBContextProcedures(db).spMessageAsync(memberId, chatId);
                var message = new MessageListDto() { List = await dto, Name = db.Members.Find(chatId).Name, LastMessageIndex = (await dto).Count-1, ChatMemberId = chatId };
                messageListDto.Add(message);
            }
            return messageListDto;
        }

        public Task<List<spMessageDetailResult>> GetMessageDetail(NSL_DBContext db,int memberId, int chatMemberId)
        {
            var dto = new NSL_DBContextProcedures(db).spMessageDetailAsync(memberId, chatMemberId);

            return dto;
        }


        public HashSet<int> GetChatsId(int senderid)
        {
            string sql = @"SELECT m.Id AS SenderId,m2.Id AS RecipientId
                            FROM Member.Members m
                            JOIN Chat.Message me ON me.SenderId = m.Id
                            JOIN Member.Members m2 ON m2.Id = me.RecipientId
                            WHERE me.SenderId = @SenderId OR me.RecipientId = @SenderId";
            var dto = _conn.Query<ChatsIdDto>(sql, new { SenderId = senderid });

            HashSet<int> Ids = new HashSet<int>();

            foreach (var item in dto)
            {
                Ids.Add(item.SenderId);
                Ids.Add(item.RecipientId);
            }
            Ids.Remove(senderid);

            return Ids;
        }

    }
}
