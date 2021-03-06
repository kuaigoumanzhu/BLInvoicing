﻿using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        CommonService common = new CommonService();
        #region 财务结算主表
        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllFNCBALANCEJson(int pageCurrent = 1, int pageSize = 10, string FID = "", string FNAME = "")
        {
            IDictionary dic = new Hashtable();

            if (!string.IsNullOrEmpty(Request.QueryString["Fdate"]))
            {
                dic["Fdate"] = Request.QueryString["Fdate"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FCode"]))
            {
                dic["FCode"] = Request.QueryString["FCode"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FState"]))
            {
                dic["FState"] = Request.QueryString["FState"];
            }
            else
            {
                dic["FState"] ="1";
            }
            int totalPage = 0;
            var lst = fncbalanceService.GetAllFNCBALANCEInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }

        [JsonException]
        public string EditFncbalance(string json)
        {
            int id = 2;
            var models = JsonHelper.Instance.Deserialize<List<T_FNCBALANCEModel>>(json);
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
            return JsonHelper.Instance.Serialize(fncbalanceService.AddFncbalance(model,id,number,common));

        }
        #endregion

        public ActionResult FncbalanceDetail(string rowData, string outWare)
        {
            var model = JsonHelper.Instance.Deserialize<T_FNCBALANCEModel>(rowData);
            IDictionary dicFncbalance = new Hashtable();
            dicFncbalance["FGUID"] = model.FGUID;
            var FncbalanceModes = fncbalanceService.GetFncbalanceModes(dicFncbalance).ToList();
            if (FncbalanceModes.Count() > 0)
            {
                model = FncbalanceModes.First();
            }
            ViewBag.outWare = outWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }

        [JsonException]
        public string EditFncbalanceDetail(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_FNCBALANCEDETAILSModel>>(json);
            var model = models[0];
            //if (string.IsNullOrEmpty(model.FGUID))
            //{
                model.FCREATEID = UserContext.CurrentUser.UserName;
                model.FGUID = Guid.NewGuid().ToString();
                model.FCREATETIME = DateTime.Now;
                model.FPARENTID = Request.QueryString["FPARENTID"];
                if (fncbalancedetailsService.AddFNCBALANCEDETAILS(models))
                {
                    return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "添加成功" });
                }
                else {
                    return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "添加失败" });
                }
            //}
            //else
            //{
            //    return JsonHelper.Instance.Serialize(fncbalancedetailsService.EditFNCBALANCEDETAILS(model));
            //}

        }

        [JsonException]
        public string getSaleDayBook()
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FPARENTID"]))
            {
                dic["FPARENTID"] = Request.QueryString["FPARENTID"];
                var lst = fncbalancedetailsService.GetAllFNCBALANCEInfo(dic);
                if (lst.Count() > 0)
                {
                    return JsonHelper.Instance.Serialize(new { list = lst });
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["FINWAREHOUSEID"]))
            {
                dic["FINWAREHOUSEID"] = Request.QueryString["FINWAREHOUSEID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Fdate"]))
            {
                dic["Fdate"] = Request.QueryString["Fdate"];
            }
            var lstSale = fncbalancedetailsService.GetSaleDayBook(dic);
            return JsonHelper.Instance.Serialize(new { list = lstSale });

        }
        public string SubmitFNCBALANCE(string fguid)
        {
            if (fncbalanceService.submitFNCBALANCE(fguid))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public string ExportInfo()
        {
            WareHoseService warehourseService = new WareHoseService();
            IDictionary dic = new Hashtable();
            dic["FPARENTID"] = Request.QueryString["FPARENTID"];
            var lst = fncbalancedetailsService.GetAllFNCBALANCEInfo(dic);
            DataTable dt = new DataTable();
            dt.Columns.Add("序号");
            dt.Columns.Add("销售仓库");
            dt.Columns.Add("销售金额");
            dt.Columns.Add("回款金额");
            dt.Columns.Add("差异金额");
            dt.Columns.Add("备注");
            int i = 0;
            foreach (var item in lst)
            {
                DataRow dr = dt.NewRow();
                IDictionary dicware = new Hashtable();
                dicware["FID"] = item.FWAREHOUSEID;
                var warehouse = warehourseService.GetAllWareHoseInfo(dicware);
                string ck = "";
                if (warehouse.Count() > 0)
                {
                    ck = warehouse.First().FNAME;
                }
                dr["序号"] = (i + 1).ToString();
                dr["销售仓库"] = ck;
                dr["销售金额"] = item.FMARKETMONEY;
                dr["回款金额"] = item.FBACKTMONEY;
                dr["差异金额"] = item.FDIFFERMONEY;
                dr["备注"] = item.FMEMO;
                dt.Rows.Add(dr);
            }
            NPOIHelper.ExportByWeb(dt, "财务结算单", "财务结算单.xls");
            return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "导出成功" });
        }
    }
}
