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

            if (!string.IsNullOrEmpty(Request.QueryString["FWAREHOUSEID"]))
            {
                dic["FWAREHOUSEID"] = Request.QueryString["FWAREHOUSEID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                dic["startDate"] = Request.QueryString["startDate"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                dic["endDate"] = Request.QueryString["endDate"];
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
