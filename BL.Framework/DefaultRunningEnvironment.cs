using BL.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BL.Framework
{
    public class DefaultRunningEnvironment : IRunningEnvironment
    {
        private const string strConfig = "~/web.config";
        private const string strBin = "~/bin";
        private const string strRefresh = "~/refresh.html";

        /// <summary>
        /// 尝试修改web.config最后更新时间
        /// <remarks>是应用程序自动重新加载</remarks>
        /// </summary>
        /// <returns>修改成功返回true，否则返回false</returns>
        private bool TryWriteWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(WebUtility.GetPhysicalFilePath(strConfig), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 尝试引起bin文件夹得改动
        /// <remarks>目的是应用程序自动重新加载</remarks>
        /// </summary>
        /// <returns>成功写入返回true，否则返回false</returns>
        private bool TryWriteBinFolder()
        {
            try
            {
                string physicaFilePath = WebUtility.GetPhysicalFilePath(strBin);
                Directory.CreateDirectory(physicaFilePath);
                using (StreamWriter writer = File.CreateText(Path.Combine(physicaFilePath, "log.txt")))
                {
                    writer.WriteLine("Restart on {0}", DateTime.UtcNow);
                    writer.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 重新启动AppDomain
        /// </summary>
        public void RestartAppDomain()
        {
            if (this.IsFullTrust)
            {
                HttpRuntime.UnloadAppDomain();
            }
            else if (!(this.TryWriteBinFolder() || this.TryWriteWebConfig()))
            {
                throw new ApplicationException(string.Format("需要启动站点，在非FullTrust环境下必须给\"{0}\"或者\"~/{1}\"写入的权限", "~/bin", "~/web.config"));
            }
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                if (current.Request.RequestType == "GET")
                {
                    current.Response.Redirect(current.Request.RawUrl, true);
                }
                else
                {
                    current.Response.ContentType = "text/html";
                    current.Response.WriteFile(strRefresh);
                    current.Response.End();
                }
            }
        }
        /// <summary>
        /// 是否完全信任运行环境
        /// </summary>
        /// <returns></returns>
        public bool IsFullTrust
        {
            get
            {
                return (AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted);
            }
        }
    }
}
