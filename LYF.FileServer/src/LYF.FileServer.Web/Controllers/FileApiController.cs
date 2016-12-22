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
    [Route("api/files")]
    public class FileApiController : Controller
    {
        [HttpGet("{filename}")]
        public ActionResult Get(string filename)
        {
            FileEntity fileEntity = GetFileInfo(filename);
            string fileFullPath = System.IO.Path.Combine(@"C:\", fileEntity.file_path, filename);
            if (fileEntity == null)
                return NotFound();
            Response.ContentType = fileEntity.file_mimetype;
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileEntity.file_name);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            Response.ContentLength = fileEntity.file_length;
            Response.Headers[HeaderNames.AcceptRanges] = "bytes";
            var reqRangeHeader = Request.Headers[HeaderNames.Range].FirstOrDefault();
            FileStream fileStream = System.IO.File.Open(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            long s = fileStream.Length;
            if (reqRangeHeader != null)
            {
                string strStartIndex = reqRangeHeader.Split('-')[0];
                string strEndIndex = reqRangeHeader.Split('-')[1];
                //断点续传处理，多range情景没有处理
                long startByteIndex = string.IsNullOrWhiteSpace(strStartIndex) ? 0 : long.Parse(strStartIndex);
                long endByteIndex = string.IsNullOrWhiteSpace(strEndIndex) ? fileEntity.file_length-1 : long.Parse(strEndIndex);
                Response.StatusCode = StatusCodes.Status206PartialContent;
                var contentRange = new ContentRangeHeaderValue(startByteIndex, endByteIndex);
                Response.Headers[HeaderNames.ContentRange] = contentRange.ToString();
                return new FileStreamResult(new PartialContentFileStream(fileStream, startByteIndex, endByteIndex), fileEntity.file_mimetype);
            }
            else
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return new FileStreamResult(fileStream, fileEntity.file_mimetype);
            }
        }

        private FileEntity GetFileInfo(string filename)
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