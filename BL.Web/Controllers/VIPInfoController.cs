using BL.Framework;
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
    public class VIPInfoController : Controller
    {
        //
        // GET: /VIPInfo/

        VIPINFOService vipInfoService = new VIPINFOService();
        public ActionResult Index()
        {
            return View();
        }

        [JsonException]
        public string GetAllVIPInfoJson(int pageCurrent = 1, int pageSize = 10)
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
            int totalPage = 0;
            var lst = vipInfoService.GetAllVIPINFO(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }
    }
}
