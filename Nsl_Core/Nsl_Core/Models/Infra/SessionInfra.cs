using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Nsl_Core.Models.Infra
{
    public static class SessionInfra
    {
        public static void SetKey<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetKey<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
			return (value == null) ? default : JsonSerializer.Deserialize<T>(value);
		}
    }
}
