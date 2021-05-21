using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Minerals.Infrastructure
{
    public static class SessionExtensions
    {
        /// <summary> Sets session variables as JSON</summary>
        public static void SetJson(this ISession session, string key, object value)
        {
            string valueasJson = JsonConvert.SerializeObject(value,
                             new JsonSerializerSettings
                             {
                                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                             });

            session.SetString(key, valueasJson);

        }

        /// <summary> Retusrns JSON session variables as Object </summary>
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null
                ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
