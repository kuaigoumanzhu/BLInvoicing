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
            //model.FAPPLYID = UserContext.CurrentUser.UserName;
            //model.FAPPLYTIME = now;
            int number = 0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            var result = repertoryCheck.AddRepertoryCheck(model, id, number, common);
            return JsonHelper.Instance.Serialize(result);
        }
    }
}
