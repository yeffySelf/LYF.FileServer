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
using Microsoft.Extensions.Primitives;

namespace LYF.FileServer.Web.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        [HttpGet("{filename}")]
        public ActionResult Get(string filename)
        {
            FileEntity fileEntity = GetFileInfo(filename);
            string fileFullPath = System.IO.Path.Combine(@"C:\", fileEntity.file_path, filename);
            if (fileEntity == null)
                return NotFound();
            FileStream fileStream = System.IO.File.Open(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            long startByteIndex = 0;
            long endByteIndex = fileEntity.file_length-1;
            var reqTypedHeader = Request.GetTypedHeaders();
            if (reqTypedHeader.Range!=null && reqTypedHeader.Range.Ranges.Count>0)
            {
                //断点续传处理，多range情景没有处理
                startByteIndex = reqTypedHeader.Range.Ranges.First().From ?? 0;
                endByteIndex = reqTypedHeader.Range.Ranges.First().To ?? fileEntity.file_length - 1;
                Response.StatusCode = StatusCodes.Status206PartialContent;
                var contentRange = new ContentRangeHeaderValue(startByteIndex, endByteIndex);
                Response.Headers[HeaderNames.ContentRange] = contentRange.ToString();
            }
            Response.ContentType = fileEntity.file_mimetype;
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileEntity.file_name);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            Response.ContentLength = fileStream.Length;
            Response.Headers[HeaderNames.AcceptRanges] = "bytes";
            return new FileStreamResult(fileStream,fileEntity.file_mimetype);
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
                file_length = fi.Length
            };
        }
    }
}
