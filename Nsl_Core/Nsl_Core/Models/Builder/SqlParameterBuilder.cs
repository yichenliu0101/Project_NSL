using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NSL_html.Models.Builder
{
	public class SqlParameterBuilder
	{
		private List<SqlParameter> _parameters = new List<SqlParameter>();

		public SqlParameterBuilder AddInt(string parameterName, int value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.Int) { Value = value };
			_parameters.Add(parameter);
			return this;
		}
		public SqlParameterBuilder AddNullableInt(string parameterName, int? value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.Int);
			if (value.HasValue)
			{
				parameter.Value = value.Value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}
			_parameters.Add(parameter);
			return this;
		}

		public SqlParameterBuilder AddDateTime(string parameterName, DateTime value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.DateTime) { Value = value };
			_parameters.Add(parameter);
			return this;
		}


		public SqlParameterBuilder AddNullableDateTIme(string parameterName, DateTime? value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.DateTime);
			if (value.HasValue)
			{
				parameter.Value = value.Value;
			}
			else
			{
				parameter.Value = DBNull.Value;
			}
			_parameters.Add(parameter);
			return this;
		}

		public SqlParameterBuilder AddNVarChar(string parameterName, int length, string value)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("長度不可為負數");
			}

			var parameter = new SqlParameter(parameterName, SqlDbType.NVarChar, length) { Value = value };
			_parameters.Add(parameter);
			return this;
		}

		public SqlParameterBuilder AddBool(string parameterName, bool value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.Bit) { Value = value };
			_parameters.Add(parameter);
			return this;
		}

        public SqlParameterBuilder AddNullableBool(string parameterName, bool? value)
        {
            var parameter = new SqlParameter(parameterName, SqlDbType.Bit);
            if (value.HasValue)
            {
                parameter.Value = value.Value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
            _parameters.Add(parameter);
            return this;
        }

        public SqlParameterBuilder AddVarchar(string parameterName, int length, string value)
		{

			var parameter = new SqlParameter(parameterName, SqlDbType.VarChar, length) { Value = value };
			_parameters.Add(parameter);
			return this;
		}

		public SqlParameterBuilder AddDecimal(string parameterName, decimal value)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.Money) { Value = value };
			_parameters.Add(parameter);
			return this;
		}

		public SqlParameterBuilder GetIntOutput(string parameterName)
		{
			var parameter = new SqlParameter(parameterName, SqlDbType.Int) { Direction = ParameterDirection.Output };
			_parameters.Add(parameter);
			return this;
		}
        public SqlParameter[] Build()
		{
			return _parameters.ToArray();
		}
	}
}
