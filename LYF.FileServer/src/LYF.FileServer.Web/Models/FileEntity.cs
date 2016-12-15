using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYF.FileServer.Web.Model
{
    /// <summary>
    /// 文件实体类
    /// </summary>
    public class FileEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string file_id
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_name
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_path
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_md5
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_ext
        {
            get;set;
        }
        public string file_mimetype
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public long file_downCnt
        {
            get; set;
        } = 0;
        /// <summary>
        /// 
        /// </summary>
        public long file_length
        {
            get;set;
        } = 0;
        /// <summary>
        /// 
        /// </summary>
        public DateTime file_createtime
        {
            get;set;
        } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string file_creator
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime file_updatetime
        {
            get; set;
        } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string file_updator
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool file_isDel
        {
            get; set;
        } = false;
    }
}
