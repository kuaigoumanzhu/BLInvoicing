using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class PURCHASEBACKController : Controller
    {
        //
        // GET: /PURCHASEBACK/
        PURCHASEBACKService PURCHASEBACKService = new PURCHASEBACKService();
        PURCHASEBACKDETAILSService PURCHASEBACKDetailsService = new PURCHASEBACKDETAILSService();
        GoodsService goodsService = new GoodsService();
        CommonService common = new CommonService();

        DATADICTService dictService = new DATADICTService();
        #region 采购退货主表
        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllPURCHASEBACKJson(int pageCurrent = 1, int pageSize = 10)
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
            if (!string.IsNullOrEmpty(Request.QueryString["FStatus"]))
            {
                dic["FStatus"] = Request.QueryString["FStatus"];
            }
            else
            {
                dic["FStatus"] = "1";
            }
            int totalPage = 0;
            var lst = PURCHASEBACKService.GetAllPURCHASEBACKInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }
        [JsonException]
        public string EditPURCHASEBACK(string json)
        {

            int id = 8;

            var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEBACKModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            int number = 0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            model.FNUMBER = number;
            model.FCREATETIME = DateTime.Now;
            model.FSTATUS = "1";
            model.FAPPLYID = UserContext.CurrentUser.UserName;
            model.FAPPLYTIME = DateTime.Now;
            return JsonHelper.Instance.Serialize(PURCHASEBACKService.AddPURCHASEBACK(model,id,number,common));

        }
        #endregion

        #region 采购退货明细
        public ActionResult PURCHASEBACKDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_PURCHASEBACKModel>(rowData);
            IDictionary dicpurchaseBack = new Hashtable();
            dicpurchaseBack["FGUID"] = model.FGUID;
            var purchaseBackModes = PURCHASEBACKService.GetPurchaseBackModes(dicpurchaseBack).ToList();
            if (purchaseBackModes.Count() > 0)
            {
                model = purchaseBackModes.First();
            }
            ViewBag.outWare = outWare;
            ViewBag.userName = common.GetNameById(model.FCREATEID);
            return View(model);
        }
        [JsonException]
        public string EditPURCHASEBACKInDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEBACKDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(PURCHASEBACKDetailsService.AddPURCHASEBACKDETAILS(model));

        }

        [JsonException]
        public string GetAllPURCHASEBACKDetailJson()
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FPARENTID"]))
            {
                dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            }
            var lst = PURCHASEBACKDetailsService.GetAllPURCHASEBACKDETAILSInfo(dic);
            return JsonHelper.Instance.Serialize(new { list = lst });
        }
        #endregion

        #region 消耗品出库明细
        public ActionResult PURCHASEBACKOutDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_PURCHASEBACKModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = model.FCREATEID;
            return View(model);
        }
        //[JsonException]
        //public string EditPURCHASEBACKOutDetail(string json)
        //{
        //    var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEBACKDETAILSModel>>(json);
        //    var model = models[0];
        //    model.FCREATEID = UserContext.CurrentUser.UserName;
        //    model.FGUID = Guid.NewGuid().ToString();
        //    model.FCREATETIME = DateTime.Now;
        //    model.FPARENTID = Request.QueryString["FPARENTID"];
        //    if (PURCHASEBACKDetailsService.AddPURCHASEBACKDETAILS(models))
        //    {
        //        return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "添加成功" });
        //    }
        //    else
        //    {
        //        return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "添加失败" });
        //    }

        //}
        [JsonException]
        [HttpPost]
        public string DelPURCHASEBACKDetail(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = PURCHASEBACKDetailsService.DelPURCHASEBACKDETAILS(t[0]["FGUID"].ToString());
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
            if (PURCHASEBACKService.submitPURCHASEBACK(fguid))
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
            var lst = PURCHASEBACKDetailsService.GetSelectOutGoods(dic);
            ViewBag.list = lst;
            return View();
        }
        public ActionResult selectInGoods()
        {
            IDictionary dic = new Hashtable();
            dic["FISPURCHASEBACK"] = "1";
            dic["FSTATUS"] = "2";
            var lst = goodsService.GetGoodsInfo(dic);
            return View(lst);
        }

        public ActionResult PURCHASEBACKSearch()
        {
            return View();
        }
        [JsonException]
        public string GetPURCHASEBACKSearchJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = PURCHASEBACKService.SearchPURCHASEBACK(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


        public ActionResult PURCHASEBACKHourseSearch()
        {
            return View();
        }
        [JsonException]
        public string GetPURCHASEBACKHourseJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = PURCHASEBACKService.SearchPURCHASEBACKHourse(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

        public string ExportInfoIn()
        {
            IDictionary dic = new Hashtable();
            dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            var lst = PURCHASEBACKDetailsService.GetAllPURCHASEBACKDETAILSInfo(dic);
            DataTable dt = new DataTable();
            dt.Columns.Add("序号");
            dt.Columns.Add("商品编号");
            dt.Columns.Add("商品名称");
            dt.Columns.Add("计量单位");
            dt.Columns.Add("数量");
            dt.Columns.Add("金额");
            dt.Columns.Add("单价");
            dt.Columns.Add("供应商");
            dt.Columns.Add("备注");
            int i = 0;
            foreach (var item in lst)
            {
                DataRow dr = dt.NewRow();
                //计量单位
                IDictionary dicjldw = new Hashtable();
                dicjldw["FCATEGORY"] = "Unit";
                dicjldw["FID"] = item.FUNIT;
                var jldw = dictService.GetAllDictInfo(dicjldw);
                string funit = "";
                if (jldw.Count() > 0)
                {
                    funit = jldw.First().FNAME;
                }
                //供应商
                IDictionary dicgys = new Hashtable();
                dicgys["FCATEGORY"] = "Unit";
                dicgys["FID"] = item.FSUPPLIERID;
                var gys = dictService.GetAllDictInfo(dicgys);
                string fsupplierid = "";
                if (gys.Count() > 0)
                {
                    fsupplierid = gys.First().FNAME;
                }
                dr["序号"] = (i + 1).ToString();
                dr["商品编号"] = item.FGOODSID;
                dr["商品名称"] = item.FGOODSNAME;
                dr["计量单位"] = funit;
                dr["数量"] = item.FQUANTITY;
                dr["金额"] = item.FMONEY;
                dr["单价"] = item.FPRICE;
                dr["供应商"] = fsupplierid;
                dr["备注"] = item.FMEMO;
                dt.Rows.Add(dr);
            }
            NPOIHelper.ExportByWeb(dt, "消耗品入库单", "消耗品入库单.xls");
            return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "导出成功" });
        }
        public string ExportInfoOut()
        {
            IDictionary dic = new Hashtable();
            dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            var lst = PURCHASEBACKDetailsService.GetAllPURCHASEBACKDETAILSInfo(dic);
            DataTable dt = new DataTable();
            dt.Columns.Add("序号");
            dt.Columns.Add("商品编号");
            dt.Columns.Add("商品名称");
            dt.Columns.Add("计量单位");
            dt.Columns.Add("数量");
            dt.Columns.Add("金额");
            dt.Columns.Add("单价");
            dt.Columns.Add("备注");
            int i = 0;
            foreach (var item in lst)
            {
                DataRow dr = dt.NewRow();
                //计量单位
                IDictionary dicjldw = new Hashtable();
                dicjldw["FCATEGORY"] = "Unit";
                dicjldw["FID"] = item.FUNIT;
                var jldw = dictService.GetAllDictInfo(dicjldw);
                string funit = "";
                if (jldw.Count() > 0)
                {
                    funit = jldw.First().FNAME;
                }
                dr["序号"] = (i + 1).ToString();
                dr["商品编号"] = item.FGOODSID;
                dr["商品名称"] = item.FGOODSNAME;
                dr["计量单位"] = funit;
                dr["数量"] = item.FQUANTITY;
                dr["金额"] = item.FMONEY;
                dr["单价"] = item.FPRICE;
                dr["备注"] = item.FMEMO;
                dt.Rows.Add(dr);
            }
            NPOIHelper.ExportByWeb(dt, "消耗品出库单", "消耗品出库单.xls");
            return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "导出成功" });
        }
    }
}
