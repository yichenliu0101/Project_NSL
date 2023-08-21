using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Security.Cryptography;
using MemoryCache = System.Runtime.Caching.MemoryCache;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos.Member.Manager;
using Dapper;

namespace Nsl_Core.Models.Infra
{
	public class Ecpay
	{
		private readonly SqlConnection _conn;
		public Ecpay(IConfiguration configuration)
		{
			_conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
		}

		public MemberOrderDataDto GetOrderData(string orderCode)
		{
			using (_conn)
			{
				var sql = @"SELECT SUM(mcrd.CurrentPrice*mcrd.Count) AS Price,ConsumeTime,OrderCode
							FROM Member.MembersConsumptionRecords mcr
							JOIN Member.MembersConsumptionRecordDetails mcrd
							ON mcr.Id=mcrd.MembersConsumptionRecordId
							WHERE OrderCode=@OrderCode
							GROUP BY OrderCode,ConsumeTime";
				var result = _conn.Query<MemberOrderDataDto>(sql, new { OrderCode = orderCode }).FirstOrDefault();
				return result;
			}
		}
		public Dictionary<string, string> GetProductOrder(MemberOrderDataDto dto)
		{
			var website = $"https://localhost:7217";
			var order = new Dictionary<string, string>
			{
            //特店交易編號
            { "MerchantTradeNo",  dto.OrderCode},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  dto.ConsumeTime.ToString("yyyy/MM/dd HH:mm:ss")},

            //交易金額
            { "TotalAmount",  dto.Price.ToString("0")},

            //交易描述
            { "TradeDesc",  "無"},

            //商品名稱
            { "ItemName",  "NSL 教師教學課程 一式"},

            //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
            { "ExpireDate",  "3"},

            //自訂名稱欄位1
            { "CustomField1",  ""},

            //自訂名稱欄位2
            { "CustomField2",  ""},

            //自訂名稱欄位3
            { "CustomField3",  ""},

            //自訂名稱欄位4
            { "CustomField4",  ""},

            //綠界回傳付款資訊的至 此URL
            { "ReturnURL",  $"{website}/Ecpay/AddPayInfo"},

            //使用者於綠界 付款完成後，綠界將會轉址至 此URL
            { "ClientBackURL", $"{website}/Cart/OrderListUpdate?orderCode={dto.OrderCode}"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
            //{ "PaymentInfoURL",  $"{website}/Cart/AddAccountInfo"},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
            //{ "ClientRedirectURL",  $"{website}/Cart/PurchaseDetails/{dto.OrderCode}"},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID",  "2000132"},//2000132

            //忽略付款方式
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

            //交易類型 固定填入 aio
            { "PaymentType",  "aio"},

            //選擇預設付款方式 固定填入 ALL
            { "ChoosePayment",  "ALL"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"},
			};

			//檢查碼
			order["CheckMacValue"] = GetCheckMacValue(order);

			return order;
		}

		public Dictionary<string, string> GetPayInfo(string id)
		{
			var cache = MemoryCache.Default;
			var cacheData = cache.Get(id);
			var dataStr = JsonSerializer.Serialize(cacheData);
			var data = JsonSerializer.Deserialize<Dictionary<string, string>>(dataStr);

			return data;
		}

		private string GetSHA256(string value)
		{
			var result = new StringBuilder();
			var sha256 = SHA256Managed.Create();
			var bts = Encoding.UTF8.GetBytes(value);
			var hash = sha256.ComputeHash(bts);

			for (int i = 0; i < hash.Length; i++)
			{
				result.Append(hash[i].ToString("X2"));
			}
			return result.ToString();
		}

		private string GetCheckMacValue(Dictionary<string, string> order)
		{
			var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

			var checkValue = string.Join("&", param);

			//測試用的 HashKey
			var hashKey = "5294y06JbISpM5x9";//5294y06JbISpM5x9

			//測試用的 HashIV
			var HashIV = "v77hoKGq4kWxNNIS";//v77hoKGq4kWxNNIS

			checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

			checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

			checkValue = GetSHA256(checkValue);

			return checkValue.ToUpper();
		}
	}
}