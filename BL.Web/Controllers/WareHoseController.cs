using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.MVC;
using BL.Service;
using BL.Models;
using BL.Framework;

namespace BL.Web.Controllers
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class WareHoseController : Controller
    {
        WareHoseService wareHose = new WareHoseService();
        public ActionResult LoadWareHose()
        {
            return View();
        }
        [JsonException]
        public string GetAllWareHoseJson()
        {
            var lst= wareHose.GetAllWareHoseInfo();
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize=lst.Count() });
        }

        [JsonException]
        public string EditWareHoseJson(string json)
        {
            var model= JsonHelper.Instance.Deserialize<T_WAREHOUSEModel>(json);
            return "";
        }
    }
}
