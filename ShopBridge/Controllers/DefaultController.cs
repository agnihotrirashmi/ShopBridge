using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using ShopBridge.Models;

namespace ShopBridge.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IConfiguration _config;

        public DefaultController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ResJsonOutput> PostDataAsync(string ApiPath, object obj = null)
        {
            try
            {
                return await Utility.PostDataAsync<ResJsonOutput>(_config.GetSection("APIURL").Value + ApiPath, obj);
            }
            catch (Exception)
            {
                ResJsonOutput result = new ResJsonOutput();
                result.Status.IsSuccess = false;
                result.Status.StatusCode = "GNLERR";
                result.Status.Message = "Error while procedding your request.";
                return result;
            }
        }

        protected string GetModelStateErrors(ModelStateDictionary ModelState)
        {
            return string.Join("<br />", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)); ;
        }
    }
}