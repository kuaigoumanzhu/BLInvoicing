﻿using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class SUPPLIERController : Controller
    {
        SUPPLIERService supplierService = new SUPPLIERService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SUPPLIERList()
        {
            return View();
        }
        [JsonException]
        public string GetAllSupplierJson(int pageCurrent = 1, int pageSize = 10)
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
            if (!string.IsNullOrEmpty(Request.QueryString["FCATEGORY"]))
            {
                dic["FCATEGORY"] = Request.QueryString["FCATEGORY"];
            }
            int totalPage = 0;
            var lst = supplierService.GetAllSUPPLIERInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }
        [JsonException]
        public string EditSupplier(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_SUPPLIERModel>>(json);
            var model = models[0];
            if (supplierService.IsExistsFID(model.FGUID, model.FID))
            {
                return JsonHelper.Instance.Serialize(new { statusCode = 300, message = "该编号已存在！" });
            }
            else
            {
                if (string.IsNullOrEmpty(model.FGUID))
                {
                    model.FCREATEID = UserContext.CurrentUser.UserName;
                    model.FGUID = Guid.NewGuid().ToString();
                    model.FCREATETIME = DateTime.Now;
                    model.FSTATUS = "2";
                    if (model.FSTATUS == "2")
                    {
                        model.FSTARTTIME = DateTime.Now;
                        model.FENDTIME = null;
                    }
                    else if (model.FSTATUS == "3")
                    {
                        model.FENDTIME = DateTime.Now;
                        model.FSTARTTIME = null;
                    }
                    model.FGUID = Guid.NewGuid().ToString();
                    return JsonHelper.Instance.Serialize(supplierService.AddSUPPLIER(model));
                }
                else
                {
                    if (model.FSTATUS == "2")
                    {
                        model.FSTARTTIME = DateTime.Now;
                    }
                    else if (model.FSTATUS == "3")
                    {
                        model.FENDTIME = DateTime.Now;
                    }
                    return JsonHelper.Instance.Serialize(supplierService.EditSUPPLIER(model));
                }
            }
        }
        [JsonException]
        [HttpPost]
        public string DelSupplier(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = supplierService.DelSUPPLIER(t[0]["FGUID"].ToString());
            if (rel > 0)
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "删除成功" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "删除失败" });
            }
        }

        /// <summary>
        /// 设置商品状态
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FSTATUS">1未启用，2已启用，3禁用</param>
        /// <returns></returns>
        [JsonException]
        public string SetSupplierStatus(string FGUID, string FSTATUS)
        {
            var time = DateTime.Now;
            return JsonHelper.Instance.Serialize(new { result = supplierService.SetSUPPLIERStatusByGuid(FGUID, FSTATUS, time), data = FSTATUS, time = time });
        }

        [JsonException]
        public string GetSupplierJson()
        {
            IDictionary dic = new Hashtable();
            dic["FSTATUS"] = 2;
            var lst = supplierService.GetSupplierInfo(dic);
            string result = "[]";
            if (lst.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (T_SUPPLIERModel item in lst)
                {
                    sb.Append("{\"" + item.FID + "\":\"" + item.FNAME + "\"},");
                }
                result = sb.ToString().Substring(0, sb.ToString().Length - 1) + "]";
            }
            return result;
        }
    }
}
