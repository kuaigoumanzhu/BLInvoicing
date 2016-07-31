using BL.Framework;
using BL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class OtherOutController : Controller
    {
        OtherOutService otherOut = new OtherOutService();
        public ActionResult OtherOutList()
        {
            return View();
        }

        public string GetOtherOutList(int pageCurrent = 1, int pageSize = 10)
        {
            var total = 0;
            var lst = otherOut.GetAllOtherOutInfo(pageCurrent, pageSize, out total);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = total });
        }
    }
}
