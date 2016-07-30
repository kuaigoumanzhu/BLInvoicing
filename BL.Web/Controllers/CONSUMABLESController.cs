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
        #endregion

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
    }
}
