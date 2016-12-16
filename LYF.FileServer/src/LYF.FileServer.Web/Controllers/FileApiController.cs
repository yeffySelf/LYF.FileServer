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
using System.Net.Http;
using Microsoft.AspNetCore.Http;


namespace LYF.FileServer.Web.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        [HttpGet("{filename}")]
        public ActionResult Get(string filename)
        {
            FileEntity fileEntity = GetFileInfo(filename);
            if (fileEntity == null)
                return NotFound();
            FileStream fileStream = System.IO.File.Open(System.IO.Path.Combine(@"C:\", fileEntity.file_path, filename), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStreamResult result = new FileStreamResult(fileStream,fileEntity.file_mimetype);
            Response.ContentLength = fileEntity.file_length;
            return result;
        }

        public FileEntity GetFileInfo(string filename)
        {
            string fileFullPath = System.IO.Path.Combine(@"C:\", "uploads", filename);
            FileInfo fi = new FileInfo(fileFullPath);
            return new FileEntity()
            {
                file_name = filename,
                file_id = Guid.NewGuid().ToString("N"),
                file_ext = fi.Extension,
                file_mimetype = "application/octet-stream",
                file_path = "uploads",
                file_length = fi.Length,
                file_md5 = fi.MD5()
            };
        }
    }
}
