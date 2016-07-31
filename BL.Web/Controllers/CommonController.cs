using BL.Framework;
using BL.MVC;
using BL.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class CommonController : Controller
    {
        CommonService commonService = new CommonService();
        [JsonException]
        public string GetWareHoseJson()
        {
            IDictionary dic = new Hashtable();
            if (!string.IsNullOrEmpty(Request.QueryString["FCATEGORY"]))
            {
                dic["FCATEGORY"] = Request.QueryString["FCATEGORY"];
            }
            var result = commonService.GetWareHouseSelect(dic);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in result)
            {
                sb.Append("{\"" + item.FID + "\":\"" + item.FNAME + "\"},");
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1) + "]";
        }
        /// <summary>
        /// 分仓库存表数据带回
        /// </summary>
        /// <returns></returns>
        public ActionResult LookUpRepertoryChild(string inWareHouse)
        {
            var lst=commonService.GetRepertoryChildByInWareHouse(inWareHouse);
            return View(lst);
        }
    }
}
