using BL.Framework;
using BL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class RepertoryCheckController : Controller
    {
        RepertoryCheckService repertoryCheck = new RepertoryCheckService();
        public ActionResult RepertoryCheckList()
        {
            return View();
        }

        public string GetRepertoryCheckList(int pageCurrent = 1, int pageSize = 10)
        {
            var total = 0;
            var lst = repertoryCheck.GetAllRepertoryCheckInfo(pageCurrent, pageSize, out total);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = total });
        }
    }
}
