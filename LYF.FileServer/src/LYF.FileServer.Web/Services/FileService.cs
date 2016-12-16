using LYF.FileServer.Web.DBAccess;
using LYF.FileServer.Web.Model;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LYF.FileServer.Web.Services
{
    public interface IFileService
    {
        List<FileEntity> GetFiles(string fileName="");
        FileEntity GetFile(string filename);
        FileEntity AddFile(FileEntity entity);
        int DelFile(string id);
    }
    public class FileService : IFileService
    {
        DapperAccess _dal;
        IHostingEnvironment _environment;

        public FileService(DapperAccess dal, IHostingEnvironment environment)
        {
            _environment = environment;
            _dal = dal;
        }

        public FileEntity AddFile(FileEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.file_id))
                entity.file_id = Guid.NewGuid().ToString("N");
            _dal.ExcuteSQL("insert into file(file_id, file_name, file_path, file_md5, file_ext, file_mimitype, file_downcnt, file_length, file_createtime, file_creator, file_updatetime,file_updator, file_isdel) values (file_id, file_name, file_path, file_md5, file_ext, file_mimitype, file_downcnt, file_length, file_createtime, file_creator, file_updatetime,file_updator, file_isdel)",entity);
            return entity;
        }

        public int DelFile(string id)
        {
            return _dal.ExcuteSQL("delete from file where file_id=@file_id", new
            {
                file_id = id
            });
        }

        public FileEntity GetFile(string filename)
        {
            IEnumerable<FileEntity> entities = _dal.Query<FileEntity>("SELECT file_id,file_name,file_path,file_md5,file_ext,file_mimitype,file_downcnt,file_length,file_createtime,file_creator,file_updatetime,file_updator,file_isdel FROM file where file_name=@file_name",
                new
                {
                    file_name = filename
                });
            return entities.FirstOrDefault();
        }

        public List<FileEntity> GetFiles(string fileName = "")
        {
            IEnumerable<FileEntity> entities = _dal.Query<FileEntity>("SELECT file_id,file_name,file_path,file_md5,file_ext,file_mimitype,file_downcnt,file_length,file_createtime,file_creator,file_updatetime,file_updator,file_isdel FROM file where file_name like %@file_name%",
                new
                {
                    file_name = fileName
                });
            return entities.ToList();
        }
    }
}
