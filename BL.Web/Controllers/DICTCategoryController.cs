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
    public class DICTCategoryController : Controller
    {
        DATADICTService datadictService = new DATADICTService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DICTCategoryList()
        {
            return View();
        }
        [JsonException]
        public string GetAllDATADICTJson(int pageCurrent = 1, int pageSize = 10)
        {
            IDictionary dic = new Hashtable();
            dic["FCATEGORY"] = "数据字典类别";

            if (!string.IsNullOrEmpty(Request.QueryString["FNAME"]))
            {
                dic["FNAME"] = Request.QueryString["FNAME"];
            }
            int totalPage = 0;
            var lst = datadictService.GetAllDictCategoryInfo(dic, ref totalPage, pageCurrent, pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = totalPage });
        }
        [JsonException]
        public string EditDATADICT(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_DATADICTModel>>(json);
            var model = models[0];
            if (datadictService.IsExistsCategory(model.FGUID, model.FID))
            {
                return JsonHelper.Instance.Serialize(new { statusCode = 300, message = "该编号已存在！" });
            }
            else
            {
                if (string.IsNullOrEmpty(model.FGUID))
                {
                    model.FCATEGORY = "数据字典类别";
                    model.FCREATEID = UserContext.CurrentUser.UserName;
                    model.FGUID = Guid.NewGuid().ToString();
                    model.FCREATETIME = DateTime.Now;
                    model.FSTATUS = "2";
                    model.FSTARTTIME = DateTime.Now;
                    model.FGUID = Guid.NewGuid().ToString();
                    return JsonHelper.Instance.Serialize(datadictService.AddDATADICT(model));
                }
                else
                {
                    return JsonHelper.Instance.Serialize(datadictService.EditDATADICT(model));
                }
            }
        }
        [JsonException]
        [HttpPost]
        public string DelDATADICT(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = datadictService.DelDATADICT(t[0]["FGUID"].ToString());
            if (rel > 0)
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "删除成功" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "删除失败" });
            }
        }


    }
}
