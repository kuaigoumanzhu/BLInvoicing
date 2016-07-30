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
    public class CONSUMABLESController : Controller
    {
        //
        // GET: /CONSUMABLES/
        CONSUMABLESService consumablesService = new CONSUMABLESService();
        CONSUMABLESDETAILSService consumablesDetailsService = new CONSUMABLESDETAILSService();
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
        public string GetAllCONSUMABLESJson(int pageCurrent = 1, int pageSize = 10)
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FType"]))
            {
                dic["FType"] = Request.QueryString["FType"];
            }
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
            var lst = consumablesService.GetAllCONSUMABLESInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

        [JsonException]
        public string EditCONSUMABLES(string json)
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
            var models = JsonHelper.Instance.Deserialize<List<T_CONSUMABLESModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FTYPE = Request.QueryString["FType"];
            int number = 0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            model.FNUMBER = number;
            model.FCREATETIME = DateTime.Now;
            model.FSTATUS = "1";
            model.FAPPLYID = UserContext.CurrentUser.UserName;
            model.FAPPLYTIME = DateTime.Now;
            return JsonHelper.Instance.Serialize(consumablesService.AddCONSUMABLES(model));

        }
        #endregion

        #region 消耗品入库明细
        public ActionResult ConsumablesInDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_CONSUMABLESModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string EditConsumablesInDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_CONSUMABLESDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(consumablesDetailsService.AddFNCBALANCEDETAILS(model));

        }

        [JsonException]
        public string GetAllCONSUMABLESDetailJson()
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FPARENTID"]))
            {
                dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            }
            var lst = consumablesDetailsService.GetAllCONSUMABLESDETAILSInfo(dic);
            return JsonHelper.Instance.Serialize(new { list = lst});
        }
        #endregion

        #region 消耗品出库明细
        public ActionResult ConsumablesOutDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_CONSUMABLESModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string EditConsumablesOutDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_CONSUMABLESDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(consumablesDetailsService.AddFNCBALANCEDETAILS(model));

        }
        [JsonException]
        [HttpPost]
        public string DelConsumablesDetail(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = consumablesDetailsService.DelCONSUMABLESDETAILS(t[0]["FGUID"].ToString());
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
            if (consumablesService.submitConsumables(fguid))
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
            var lst = consumablesDetailsService.GetSelectOutGoods(dic);
            ViewBag.list = lst;
            return View();
        }
        public ActionResult selectInGoods()
        {
            IDictionary dic = new Hashtable();
            dic["FISCONSUMABLES"] = "1";
            dic["FSTATUS"] = "2";
            var lst = goodsService.GetGoodsInfo(dic);
            return View(lst);
        }

        public ActionResult CONSUMABLESSearch()
        {
            return View();
        }
        [JsonException]
        public string GetCONSUMABLESSearchJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = consumablesService.SearchCONSUMABLES(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


        public ActionResult CONSUMABLESHourseSearch()
        {
            return View();
        }
        [JsonException]
        public string GetCONSUMABLESHourseJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = consumablesService.SearchCONSUMABLESHourse(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        } 
    }
}
