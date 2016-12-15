using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYF.FileServer.Web
{
    public enum FileOperateType
    {
        Upload = 0,
        Download = 1,
        Remove = 2
    }
    public enum DBType
    {
        SqlServer,
        MySql,
        PostgreSql
    }
}
