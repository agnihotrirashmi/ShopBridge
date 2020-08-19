using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    public class FileHandlingController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public FileHandlingController(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }

        [HttpPost]
        public async Task<ResJsonOutput> UploadFile(IFormFile file)
        {
            ResJsonOutput result = new ResJsonOutput();
            if (file == null || file.Length == 0)
            {
                result.Status.Message = "file not selected";
            }
            else
            {
                string folderName = "Products";
                string strpath = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                Utility.CheckDir(strpath);
                string FileName = Utility.EpochTime() + "_" + file.FileName;
                strpath = strpath + "\\" + FileName;

                using (var stream = new FileStream(strpath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                result.Data = folderName+ "\\"+ FileName;
                result.Status.IsSuccess = true;
            }
            return result;
        }

    }
}