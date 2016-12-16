using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYF.FileServer.Web
{
    public static class FileExt
    {
        /// <summary>
        /// 文件的MD5值
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string MD5(this FileInfo fileInfo)
        {
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] retVal = md5.ComputeHash(fs);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
        }
    }
}
