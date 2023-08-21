using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSL_html.Models
{
	public static class SqlReaderExtMethods
	{
		public static int GetInt(this SqlDataReader reader, string columnName)
		{
			try
			{
				int columnIndex = reader.GetOrdinal(columnName);
				if (reader.IsDBNull(columnIndex))
				{
					return 0;
				}
				return reader.GetInt32(columnIndex);
			}
			catch (Exception ex)
			{
				throw new Exception("查無此欄位，請確定輸入字串是否正確");
			}
		}
		public static string GetString(this SqlDataReader reader, string columnName)
		{
			try
			{
				int columnIndex = reader.GetOrdinal(columnName);
				if (reader.IsDBNull(columnIndex))
				{
					return null;
				}
				return reader.GetString(columnIndex);
			}
			catch (Exception ex)
			{
				throw new Exception("查無此欄位，請確定輸入字串是否正確");
			}
		}

		public static bool GetBool(this SqlDataReader reader, string columnName)
		{
			try
			{
				int columnIndex = reader.GetOrdinal(columnName);
				if (reader.IsDBNull(columnIndex))
				{
					return false;
				}
				return reader.GetBoolean(columnIndex);
			}
			catch (Exception ex)
			{
				throw new Exception("查無此欄位，請確定輸入字串是否正確");
			}
		}

        public static bool? GetNullableBool(this SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (reader.IsDBNull(columnIndex))
                {
                    return null;
                }
                return reader.GetBoolean(columnIndex);
            }
            catch (Exception ex)
            {
                throw new Exception("查無此欄位，請確定輸入字串是否正確");
            }
        }

        public static DateTime GetDateTime(this SqlDataReader reader, string columnName)
		{
			try
			{
				int columnIndex = reader.GetOrdinal(columnName);
				if (reader.IsDBNull(columnIndex))
				{
					throw (new Exception("找不到修改日期"));
				}
				return reader.GetDateTime(columnIndex);
			}
			catch (Exception ex)
			{
				throw new Exception("查無此欄位，請確定輸入字串是否正確");
			}
		}

        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                if (reader.IsDBNull(columnIndex))
                {
					return null;
                }
                return reader.GetDateTime(columnIndex);
            }
            catch (Exception ex)
            {
                throw new Exception("查無此欄位，請確定輸入字串是否正確");
            }
        }

		public static decimal GetDecimal(this SqlDataReader reader, string columnName)
		{
			try
			{
				int columnIndex = reader.GetOrdinal(columnName);
				if (reader.IsDBNull(columnIndex))
				{
					return 0;
				}
				return reader.GetDecimal(columnIndex);
			}
			catch (Exception ex)
			{
				throw new Exception("查無此欄位，請確定輸入字串是否正確");
			}
		}
	}
}
