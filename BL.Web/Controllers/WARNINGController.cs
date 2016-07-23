using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class WARNINGController : Controller
    {
        //
        // GET: /WARNING/

        WARNINGService warningService = new WARNINGService();
        public ActionResult Index()
        {
            T_WARNINGModel model = new T_WARNINGModel();
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            warningService.InserWarning(model);
            return View();
        }

        [JsonException]
        public string GetWarningJson(int pageCurrent = 1, int pageSize = 10, string FID = "", string FNAME = "",bool isDel=false)
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FID"]))
            {
                dic["FID"] = Request.QueryString["FID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FNAME"]))
            {
                dic["FNAME"] = Request.QueryString["FNAME"];
            }
            if (isDel)
            {
                T_WARNINGModel model = new T_WARNINGModel();
                model.FCREATEID = UserContext.CurrentUser.UserName;
                model.FGUID = Guid.NewGuid().ToString();
                model.FCREATETIME = DateTime.Now;
                warningService.InserWarning(model);
            }
            int totalPage = 0;
            var lst = warningService.GetWarningInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

    }
}
