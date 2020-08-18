using Newtonsoft.Json;

namespace ShopBridgeAPI.Utility
{
    public static class Utility
    {
        public static string IsNullString(this object str)
        {
            try
            {
                return str.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ConvertObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T ConvertJsonToObject<T>(object obj)
        {
            return (T)JsonConvert.DeserializeObject(obj.IsNullString(), typeof(T));
        }
    }
}
