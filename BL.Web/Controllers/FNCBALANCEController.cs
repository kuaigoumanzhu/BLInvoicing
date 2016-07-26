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
    public class FNCBALANCEController : Controller
    {
        //
        // GET: /FNCBALANCE/
        FNCBALANCEService fncbalanceService = new FNCBALANCEService();
        FNCBALANCEDETAILSService fncbalancedetailsService = new FNCBALANCEDETAILSService();

        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllFNCBALANCEJson(int pageCurrent = 1, int pageSize = 10, string FID = "", string FNAME = "")
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
            var lst = fncbalanceService.GetAllFNCBALANCEInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


        public ActionResult FncbalanceDetail()
        {
            //IDictionary dic = new Hashtable();

            //if (!string.IsNullOrEmpty(Request.QueryString["FID"]))
            //{
            //    dic["FID"] = Request.QueryString["FID"];
            //}
            //if (!string.IsNullOrEmpty(Request.QueryString["FNAME"]))
            //{
            //    dic["FNAME"] = Request.QueryString["FNAME"];
            //}
            //var lst = fncbalancedetailsService.GetAllFNCBALANCEInfo(dic);
            //return JsonHelper.Instance.Serialize(new { list = lst });
            return View();
        }

        [JsonException]
        public string getSaleDayBook()
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FINWAREHOUSEID"]))
            {
                dic["FINWAREHOUSEID"] = Request.QueryString["FINWAREHOUSEID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Fdate"]))
            {
                dic["Fdate"] = Request.QueryString["Fdate"];
            }
            var lst = fncbalancedetailsService.GetSaleDayBook(dic);
            return JsonHelper.Instance.Serialize(new { list = lst });
        }
    }
}
