using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace LYF.FileServer.Web.DBAccess
{
    public class DapperAccess : IDisposable
    {
        private IDbConnection _conn;
        private ILogger<DapperAccess> _log;
        private bool disposed = false;

        public DapperAccess()
        {
            /*
            _log = log;
            switch (dbType)
            {
                case DBType.PostgreSql:
                    _conn = new NpgsqlConnection(sqlConnectStr);
                    break;
                case DBType.SqlServer:
                    _conn = new SqlConnection(sqlConnectStr);
                    break;
                default:
                    break;
            }
            **/
            _conn = null;
        }

        public int ExcuteSQL(string sql, object param)
        {
            try
            {
                return _conn.Execute(sql, param: param);
            }
            catch (Exception ex)
            {
                _log.LogError("ExcuteSQL错误：" + ex.ToString());
            }
            return 0;
        }

        public IEnumerable<T> Query<T>(string sql, object param)
        {
            try
            {
                return _conn.Query<T>(sql, param: param);
            }
            catch (Exception ex)
            {
                _log.LogError("ExcuteSQL错误：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary>
        ~DapperAccess()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                _log = null;
            }
            // 清理非托管资源
            _conn.Dispose();
            _conn = null;
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}