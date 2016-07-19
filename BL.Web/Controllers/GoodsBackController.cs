using BL.Framework;
using BL.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Service;

namespace BL.Web.Controllers
{
    /// <summary>
    /// 商品回库
    /// </summary>
    public class GoodsBackController : Controller
    {
        GoodsBackService goodsBack = new GoodsBackService();
        public ActionResult GoodsBackList()
        {
            return View();
        }
        [JsonException]
        public string GetGoodsBackList()
        {
            var lst = goodsBack.GetAllGoodsBackInfo();
            return JsonHelper.Instance.Serialize(new { list=lst,pageSize=lst.Count()});
        }

    }
}
