using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LYF.FileServer.Web.Services;
using LYF.FileServer.Web.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LYF.FileServer.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileApiController : Controller
    {
        IHostingEnvironment _environment;
        IFileService _fileService;

        public FileApiController(IHostingEnvironment environment,IFileService fileService)
        {
            _environment = environment;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get(string filename)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            FileEntity entity = _fileService.GetFile(filename);
            if (null == entity) return BadRequest();
            string filepath = System.IO.Path.Combine(uploads, entity.file_path, "test.zip");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, entity.file_mimetype);
        }

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
