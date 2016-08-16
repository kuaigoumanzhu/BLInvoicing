using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class GUIDANCEController : Controller
    {
        //
        // GET: /GUIDANCE/
        GUIDANCEService GUIDANCE = new GUIDANCEService();
        GUIDANCEDETAILSService GUIDANCEDetailsService = new GUIDANCEDETAILSService();
        GoodsService goodsService = new GoodsService();
        CommonService common = new CommonService();
        #region 消耗品入库主表
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult outIndex()
        {
            return View();
        }
        [JsonException]
        public string GetAllGUIDANCEJson(int pageCurrent = 1, int pageSize = 10)
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FDate"]))
            {
                dic["FDate"] = Request.QueryString["FDate"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FCode"]))
            {
                dic["FCode"] = Request.QueryString["FCode"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FPERSONID"]))
            {
                dic["FPERSONID"] = Request.QueryString["FPERSONID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FStatus"]))
            {
                dic["FStatus"] = Request.QueryString["FStatus"];
            }
            else
            {
                dic["FStatus"] = "1";
            }
            int totalPage = 0;
            var lst = GUIDANCE.GetAllGUIDANCEInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

        [JsonException]
        public string EditGUIDANCE(string json)
        {
            int id = 3;
            if (Request.QueryString["FType"] == "1")
            {
                id = 3;
            }
            else
            {
                id = 4;
            }
            var models = JsonHelper.Instance.Deserialize<List<T_GUIDANCEModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            //model.FTYPE = Request.QueryString["FType"];
            int number = 0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            model.FNUMBER = number;
            model.FCREATETIME = DateTime.Now;
            model.FSTATUS = "1";
            model.FAPPLYID = UserContext.CurrentUser.UserName;
            model.FAPPLYTIME = DateTime.Now;
            return JsonHelper.Instance.Serialize(GUIDANCE.AddGUIDANCE(model));

        }
        #endregion

        #region 消耗品入库明细
        public ActionResult GUIDANCEDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_GUIDANCEModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string EditGUIDANCEDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_GUIDANCEDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(GUIDANCEDetailsService.AddFNCBALANCEDETAILS(model));

        }

        [JsonException]
        public string GetAllGUIDANCEDetailJson()
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FPARENTID"]))
            {
                dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            }
            var lst = GUIDANCEDetailsService.GetAllGUIDANCEDETAILSInfo(dic);
            return JsonHelper.Instance.Serialize(new { list = lst });
        }
        #endregion

        #region 消耗品出库明细
        public ActionResult GUIDANCEOutDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_GUIDANCEModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string EditGUIDANCEOutDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_GUIDANCEDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(GUIDANCEDetailsService.AddFNCBALANCEDETAILS(model));

        }
        [JsonException]
        [HttpPost]
        public string DelGUIDANCEDetail(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = GUIDANCEDetailsService.DelGUIDANCEDETAILS(t[0]["FGUID"].ToString());
            if (rel > 0)
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "删除成功" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "删除失败" });
            }
        }
        #endregion

        public string SubmitConsumable(string fguid)
        {
            if (GUIDANCE.submitGUIDANCE(fguid))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public ActionResult selectOutGoods()
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FWAREHOUSEID"]))
            {
                dic["FWAREHOUSEID"] = Request.QueryString["FWAREHOUSEID"];
            }
            var lst = GUIDANCEDetailsService.GetSelectOutGoods(dic);
            ViewBag.list = lst;
            return View();
        }
        public ActionResult selectInGoods()
        {
            IDictionary dic = new Hashtable();
            dic["FISGUIDANCE"] = "1";
            dic["FSTATUS"] = "2";
            var lst = goodsService.GetGoodsInfo(dic);
            return View(lst);
        }

        public ActionResult GUIDANCESearch()
        {
            return View();
        }
        [JsonException]
        public string GetGUIDANCESearchJson(int pageCurrent = 1, int pageSize = 10)
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FType"]))
            {
                dic["FType"] = Request.QueryString["FType"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["startFDate"]))
            {
                dic["startFDate"] = Request.QueryString["startFDate"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["endFDate"]))
            {
                dic["endFDate"] = Request.QueryString["endFDate"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FCode"]))
            {
                dic["FCode"] = Request.QueryString["FCode"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FGoodsName"]))
            {
                dic["FGoodsName"] = Request.QueryString["FGoodsName"];
            }
            dic["FStatus"] = "2";

            int totalPage = 0;
            var lst = GUIDANCE.SearchGUIDANCE(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


        public ActionResult GUIDANCEHourseSearch()
        {
            return View();
        }
        [JsonException]
        public string GetGUIDANCEHourseJson(int pageCurrent = 1, int pageSize = 10)
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FGoodsCode"]))
            {
                dic["FGoodsCode"] = Request.QueryString["FGoodsCode"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FGoodsName"]))
            {
                dic["FGoodsName"] = Request.QueryString["FGoodsName"];
            }

            int totalPage = 0;
            var lst = GUIDANCE.SearchGUIDANCEHourse(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        } 

    }
}
