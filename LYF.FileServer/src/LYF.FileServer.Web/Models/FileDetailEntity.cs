using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYF.FileServer.Web.Model
{
    public class FileDetailEntity
    {
        /// <summary>
		/// 
		/// </summary>
		public string fdetail_id
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string fdetail_fileid
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int fdetail_type
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime fdetail_createtime
        {
            get; set;
        } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string fdetail_creator
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime fdetail_updatetime
        {
            get; set;
        } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public string fdetail_updator
        {
            get;set;
        }
    }
}
