using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.MVC
{
    /// <summary>
    /// hpf 前端框架ajax错误返回
    /// </summary>
    public class UiResponse
    {
        public string statusCode
        {
            get; set;

        }

        public string message
        {
            get; set;

        }

        public string tabid
        {
            get; set;

        }

        public bool closeCurrent
        {
            get; set;

        }
        public string forward { get; set; }
        public string forwardConfirm { get; set; }
        public UiResponse()
        {
            statusCode = "200";
            message = "操作成功";
            tabid = "";
            closeCurrent = false;
            forward = "";
            forwardConfirm = "";
        }
    }
}
