using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.EFModels;

namespace Nsl_Core.Models.Infra
{
    public class SqlDb
    {
        public static string GetConnStr(string connName)
        {
            try
            {
                string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ToString();
                return ConnStr;
            }
            catch (Exception)
            {
                throw new Exception($"找不到名為{connName}的連線字串，請重新確認後再輸入");
            }
        }

        #region SqlConnection
        public static SqlConnection GetConnection()
        {
            return GetConnection("default");
        }

        public static SqlConnection GetConnection(string stringName)
        {
            string connStr = GetConnStr(stringName);
            return new SqlConnection(connStr);
        }
        #endregion

        public static int[] GetAreas(int cityId)
        {
            var db = new NSL_DBContext();
            return (from a in db.Areas
                    join c in db.Citys on a.CityId equals c.Id
                    where c.Id == cityId
                    select a.Id).ToArray();
        }
        public static T Get<T>(Func<SqlConnection> connGetter, string sql,
                                Func<SqlDataReader, T> assembly, params SqlParameter[] parameters)
        {
            using (var conn = connGetter())
            {
                conn.Open();
                //Todo                
                using (var cmd = conn.CreateCommand())
                {
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.CommandText = sql;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    return reader.Read() ? assembly(reader) : default;//default(Class) = null
                }

            }
        }

        public static IEnumerable<T> Search<T>(Func<SqlConnection> connGetter, string sql,
                                Func<SqlDataReader, T> assembly, params SqlParameter[] parameters)
        {
            using (var conn = connGetter())
            {
                conn.Open();
                //Todo                
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    //如果有參數
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        #region Assemble
                        //int id = reader.GetInt("Id");
                        //string name = reader.GetString("Name");
                        //int displayOrder = reader.GetInt("DisplayOrder");


                        //var dto = new CategoryDto
                        //{
                        //	Id = id,
                        //	Name = name,
                        //	DisplayOrder = displayOrder
                        //};
                        #endregion
                        yield return assembly(reader);
                    }
                }

            }
        }

        public static int Create(Func<SqlConnection> connGetter, string sql, params SqlParameter[] parameters)
        {
            using (var conn = connGetter())
            {
                conn.Open();
                //Todo                
                using (var cmd = conn.CreateCommand())
                {
                    sql += ";\r\nSET @newId = SCOPE_IDENTITY()";
                    cmd.CommandText = sql;

                    SqlParameter parameter = new SqlParameter("@newId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parameter);

                    //如果有參數
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();

                    return (int)cmd.Parameters["@newId"].Value;
                }

            }
        }

        public static int UpdateOrDelete(Func<SqlConnection> connGetter, string sql, params SqlParameter[] parameters)
        {
            using (var conn = connGetter())
            {
                conn.Open();
                //Todo                
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    //如果有參數
                    if (parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("FK_"))
                        {
                            throw new Exception("此類別之主鍵有其他外部鍵參考，因此無法異動");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}
