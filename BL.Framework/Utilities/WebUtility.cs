using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace BL.Framework.Utilities
{
    /// <summary>
    /// 提供与Web请求时可使用的工具类，包括Url解析、Url/Html编码、获取IP地址、返回http状态码
    /// </summary>
    public static class WebUtility
    {
        public static readonly string HtmlNewLine = "<br />";
        /// <summary>
        /// 把content中的虚拟路径转化成完整的url
        /// </summary>
        /// </remarks>
        /// <param name="content">content</param>
        /// <returns></returns>
        public static string FormatCompleteUrl(string content)
        {
            string pattern = "src=[\"']\\s*(/[^\"']*)\\s*[\"']";
            string str2 = "href=[\"']\\s*(/[^\"']*)\\s*[\"']";
            string str3 = HostPath(HttpContext.Current.Request.Url);
            content = Regex.Replace(content, pattern, "src=\"" + str3 + "$1\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, str2, "href=\"" + str3 + "$1\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIP()
        {
            return GetIP(HttpContext.Current);
        }
        /// <summary>
        /// 透过代理获取真实IP
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIP(HttpContext httpContext)
        {
            string userHostAddress = string.Empty;
            if (httpContext != null)
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = HttpContext.Current.Request.UserHostAddress;
                }
            }
            return userHostAddress;
        }
        /// <summary>
        /// 获取物理文件路径
        /// </summary>
        /// <param name="filePath">    <remarks>
        ///         <para>filePath支持以下格式：</para>
        ///        <list type="bullet">
        ///        <item>~/abc/</item>
        ///        <item>c:\abc\</item>
        ///        <item>\\192.168.0.1\abc\</item>
        ///       </list>
        ///   </remarks></param>
        /// <returns></returns>
        public static string GetPhysicalFilePath(string filePath)
        {
            if ((filePath.IndexOf(@":\") == -1) && (filePath.IndexOf(@"\\") == -1))
            {
                if (HostingEnvironment.IsHosted)
                {
                    return HostingEnvironment.MapPath(filePath);
                }
                filePath = filePath.Replace('/', Path.DirectorySeparatorChar).Replace("~", "");
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }
            return filePath;
        }
        /// <summary>
        /// 获取根域名
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="domainRules">域名规则</param>
        /// <returns></returns>
        public static string GetServerDomain(Uri uri, string[] domainRules)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str4 = uri.Host.ToString().ToLower();
            if (str4.IndexOf('.') <= 0)
            {
                return str4;
            }
            string[] strArray2 = str4.Split(new char[] { '.' });
            string s = strArray2.GetValue((int)(strArray2.Length - 1)).ToString();
            int result = -1;
            if (int.TryParse(s, out result))
            {
                return str4;
            }
            string oldValue = string.Empty;
            string str = string.Empty;
            string str3 = string.Empty;
            for (int i = 0; i < domainRules.Length; i++)
            {
                if (str4.EndsWith(domainRules[i].ToLower()))
                {
                    oldValue = domainRules[i].ToLower();
                    str = str4.Replace(oldValue, "");
                    if (str.IndexOf('.') > 0)
                    {
                        string[] strArray = str.Split(new char[] { '.' });
                        return (strArray.GetValue((int)(strArray.Length - 1)).ToString() + oldValue);
                    }
                    return (str + oldValue);
                }
                str3 = str4;
            }
            return str3;
        }
        /// <summary>
        /// 获取带传输协议的完整的主机地址
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns></returns>
        public static string HostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = uri.IsDefaultPort ? string.Empty : (":" + Convert.ToString(uri.Port, CultureInfo.InvariantCulture));
            return (uri.Scheme + Uri.SchemeDelimiter + uri.Host + str);
        }
        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="rawContent">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlDecode(string rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
            {
                return rawContent;
            }
            return HttpUtility.HtmlDecode(rawContent);
        }
        /// <summary>
        /// html编码
        /// </summary>
        /// <remarks>
        ///    <para>调用HttpUtility.HtmlEncode()，当前已知仅作如下转换：</para>
        ///    <list type="bullet">
        ///        <item>&lt; = &amp;lt;</item>
        ///        <item>&gt; = &amp;gt;</item>
        ///        <item>&amp; = &amp;amp;</item>
        ///        <item>&quot; = &amp;quot;</item>
        ///    </list>
        ///    </remarks>
        /// <param name="rawContent">待编码的字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
            {
                return rawContent;
            }
            return HttpUtility.HtmlEncode(rawContent);
        }
        /// <summary>
        /// 将URL转换为在请求客户端可用的 URL（转换 ~/ 为绝对路径）
        /// </summary>
        /// <param name="relativeUrl">相对url</param>
        /// <returns>返回绝对路径</returns>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
            {
                return relativeUrl;
            }
            if (!relativeUrl.StartsWith("~/"))
            {
                return relativeUrl;
            }
            string[] strArray = relativeUrl.Split(new char[] { '?' });
            string str = VirtualPathUtility.ToAbsolute(strArray[0]);
            if (strArray.Length > 1)
            {
                str = str + "?" + strArray[1];
            }
            return str;
        }
        /// <summary>
        ///   返回 StatusCode 304
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="endResponse">是否终止HttpResponse</param>
        public static void Return304(HttpContext httpContext, bool endResponse = true)
        {
            HttpStatusCode(httpContext, 304, "304 Not Modified", endResponse);
        }

        public static void Return403(HttpContext httpContext)
        {
            HttpStatusCode(httpContext, 403, null, false);
            if (httpContext != null)
            {
                httpContext.Response.SuppressContent = true;
                httpContext.Response.End();
            }
        }

        public static void Return404(HttpContext httpContext)
        {
            HttpStatusCode(httpContext, 404, null, false);
            if (httpContext != null)
            {
                httpContext.Response.SuppressContent = true;
                httpContext.Response.End();
            }
        }

        public static void SetStatusCodeForError(HttpResponseBase response)
        {
            response.StatusCode = 300;
        }
        /// <summary>
        /// 返回http状态码
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="status">状态码</param>
        /// <param name="info">状态描述字符串</param>
        /// <param name="endResponse">是否终止HttpResponse</param>
        private static void HttpStatusCode(HttpContext httpContext, int status, string info, bool endResponse)
        {
            if (httpContext != null)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = status;
                if (!string.IsNullOrEmpty(info))
                {
                    httpContext.Response.Status = info;
                }
                if (endResponse)
                {
                    httpContext.Response.End();
                }
            }
        }
        /// <summary>
        ///   Url解码
        /// </summary>
        /// <param name="urlToDecode">待解码的字符串</param>
        /// <returns></returns>
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode);
        }
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="urlToEncode">待编码的url字符串</param>
        /// <returns></returns>
        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode).Replace("'", "%27");
        }
    }
}
