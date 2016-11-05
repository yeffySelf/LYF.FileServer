using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LYF.FileServer.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileApiController : Controller
    {
        IHostingEnvironment _environment;

        public FileApiController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// 下载某个文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        [HttpGet]
        public FileResult Get(string id)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            string filepath = System.IO.Path.Combine(uploads,"test.zip");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload");
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public IActionResult Post(string id)
        {
            var files = HttpContext.Request.Form.Files;
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    Stream fileStream = file.OpenReadStream();
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(uploads,"test.zip"), bytes);
                }
            }
            return Ok();
        }
    }
}
