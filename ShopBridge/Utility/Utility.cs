using ShopBridge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using System.Text;

namespace ShopBridge
{
    public static class Utility
    {
        public static async Task<ResJsonOutput> PostDataAsync<T>(string ApiPath, object obj, List<KeyValue> Headers = null)
        {
            ResJsonOutput result = new ResJsonOutput();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiPath);
                    client.DefaultRequestHeaders.Accept.Clear();
                    if (Headers != null)
                    {
                        foreach (var item in Headers.Where(c => c.Value.IsNullString() != string.Empty))
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                        }
                    }
                    response = await client.PostAsync(ApiPath, new StringContent(ConvertObjectToJson(obj),Encoding.UTF8, "application/json"));//new StringContent(ConvertObjectToJson(obj))
                    result = response.Content.ReadAsJsonAsync<ResJsonOutput>().Result;
                }
            }
            catch (Exception ex)
            {
                result.Status.Message = ex.Message;
            }
            return result;
        }

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
