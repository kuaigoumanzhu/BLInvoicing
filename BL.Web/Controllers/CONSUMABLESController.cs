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
    public class CONSUMABLESController : Controller
    {
        //
        // GET: /CONSUMABLES/
        CONSUMABLESService consumablesService = new CONSUMABLESService();

        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllCONSUMABLESJson(int pageCurrent = 1, int pageSize = 10, string FID = "", string FNAME = "")
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
            var lst = consumablesService.GetAllCONSUMABLESInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


    }
}
