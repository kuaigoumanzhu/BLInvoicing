using BL.Framework;
using BL.MVC;
using BL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Models;

namespace BL.Web.Controllers
{
    public class RepertoryCheckController : Controller
    {
        RepertoryCheckService repertoryCheck = new RepertoryCheckService();
        CommonService common = new CommonService();
        public ActionResult RepertoryCheckList()
        {
            return View();
        }
        [JsonException]
        public string GetRepertoryCheckList(int pageCurrent = 1, int pageSize = 10)
        {
            var total = 0;
            var lst = repertoryCheck.GetAllRepertoryCheckInfo(pageCurrent, pageSize, out total);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = total });
        }
        /// <summary>
        /// 库存盘点主表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [JsonException]
        public string EditRepertoryCheckJson(string json)
        {
            var id = 1;
            var models = JsonHelper.Instance.Deserialize<List<ViewREPERTORYCHECK>>(json);
            var model = models[0];
            var now = DateTime.Now;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FCREATETIME = now;
            model.FDATE = now;
            model.FAPPLYID = UserContext.CurrentUser.UserName;
            model.FAPPLYTIME = now;
            model.FSTATUS = "1";
            int number = 0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            var result = repertoryCheck.AddRepertoryCheck(model, id, number, common);
            return JsonHelper.Instance.Serialize(result);
        }

        public ActionResult RepertoryCheckDetails(string rowData,string ware)
        {
            var model = JsonHelper.Instance.Deserialize<ViewREPERTORYCHECK>(rowData);
            ViewBag.ware = ware;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }

        [JsonException]
        public string GetRepertoryCheckDetailsList(string parentId, string ware)
        {
            var lst = repertoryCheck.GetAllRepertoryCheckDetailsInfo(parentId, ware);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = lst.Count() });
        }
        /// <summary>
        /// 保存库存盘点明细
        /// </summary>
        /// <param name="details"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [JsonException]
        public string AddRepertoryCheckDetailsJson(string details,string parentId,string wareId)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_REPERTORYCHECKDETAILSModel>>(details);
            var res = repertoryCheck.AddRepertoryCheckDetails(models, parentId, UserContext.CurrentUser.UserName, wareId);
            return JsonHelper.Instance.Serialize(res);
        }
    }
}
