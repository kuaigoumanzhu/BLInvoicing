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
    public class PURCHASEController : Controller
    {
        //
        // GET: /PURCHASE/
        PURCHASEService PURCHASEService = new PURCHASEService();
        PURCHASEDETAILSService PURCHASEDetailsService = new PURCHASEDETAILSService();
        GoodsService goodsService = new GoodsService();
        CommonService common = new CommonService();

        DATADICTService dictService = new DATADICTService();
        #region 采购入库主表
        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllPURCHASEJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = PURCHASEService.GetAllPURCHASEInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }
        [JsonException]
        public string EditPURCHASE(string json)
        {

            int  id = 6;

            var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEModel>>(json);
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
            return JsonHelper.Instance.Serialize(PURCHASEService.AddPURCHASE(model));

        }
        #endregion

        #region 采购入库明细
        public ActionResult PURCHASEInDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_PURCHASEModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = common.GetNameById(model.FCREATEID);
            return View(model);
        }
        [JsonException]
        public string EditPURCHASEInDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEDETAILSModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATETIME = DateTime.Now;
            model.FPARENTID = Request.QueryString["FPARENTID"];
            return JsonHelper.Instance.Serialize(PURCHASEDetailsService.AddPURCHASEDETAILS(model));

        }

        [JsonException]
        public string GetAllPURCHASEDetailJson()
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["FPARENTID"]))
            {
                dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            }
            var lst = PURCHASEDetailsService.GetAllPURCHASEDETAILSInfo(dic);
            return JsonHelper.Instance.Serialize(new { list = lst });
        }
        #endregion

        #region 消耗品出库明细
        public ActionResult PURCHASEOutDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_PURCHASEModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.userName = model.FCREATEID;
            return View(model);
        }
        //[JsonException]
        //public string EditPURCHASEOutDetail(string json)
        //{
        //    var models = JsonHelper.Instance.Deserialize<List<T_PURCHASEDETAILSModel>>(json);
        //    var model = models[0];
        //    model.FCREATEID = UserContext.CurrentUser.UserName;
        //    model.FGUID = Guid.NewGuid().ToString();
        //    model.FCREATETIME = DateTime.Now;
        //    model.FPARENTID = Request.QueryString["FPARENTID"];
        //    if (PURCHASEDetailsService.AddPURCHASEDETAILS(models))
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
        public string DelPURCHASEDetail(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = PURCHASEDetailsService.DelPURCHASEDETAILS(t[0]["FGUID"].ToString());
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
            if (PURCHASEService.submitPURCHASE(fguid))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public ActionResult selectGoods()
        {
            IDictionary dic = new Hashtable();
            var lst = PURCHASEDetailsService.GetSelectGoods(dic);
            ViewBag.list = lst;
            return View();
        }
        public ActionResult selectInGoods()
        {
            IDictionary dic = new Hashtable();
            dic["FISPURCHASE"] = "1";
            dic["FSTATUS"] = "2";
            var lst = goodsService.GetGoodsInfo(dic);
            return View(lst);
        }

        public ActionResult PURCHASESearch()
        {
            return View();
        }
        [JsonException]
        public string GetPURCHASESearchJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = PURCHASEService.SearchPURCHASE(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }


        public ActionResult PURCHASEHourseSearch()
        {
            return View();
        }
        [JsonException]
        public string GetPURCHASEHourseJson(int pageCurrent = 1, int pageSize = 10)
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
            var lst = PURCHASEService.SearchPURCHASEHourse(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

        public string ExportInfoIn()
        {
            IDictionary dic = new Hashtable();
            dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            var lst = PURCHASEDetailsService.GetAllPURCHASEDETAILSInfo(dic);
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
            var lst = PURCHASEDetailsService.GetAllPURCHASEDETAILSInfo(dic);
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
