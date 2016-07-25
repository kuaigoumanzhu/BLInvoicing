using BL.Framework;
using BL.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Service;
using BL.Models;
using System.Data;

namespace BL.Web.Controllers
{
    /// <summary>
    /// 商品回库
    /// </summary>
    public class GoodsBackController : Controller
    {
        GoodsBackService goodsBack = new GoodsBackService();
        CommonService common = new CommonService();
        #region 商品回库主表
        public ActionResult GoodsBackList()
        {
            return View();
        }
        //获取所有列表
        [JsonException]
        public string GetGoodsBackList(int pageCurrent = 1, int pageSize = 10)
        {
            var total = 0;
            var lst = goodsBack.GetAllGoodsBackInfo(pageCurrent,pageSize,out total);
            return JsonHelper.Instance.Serialize(new { list=lst,pageSize= pageSize, pageCurrent = pageCurrent, total = total });
        }
        /// <summary>
        /// 商品回库主表只增加
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [JsonException]
        public string EditGoodsBackJson(string json)
        {
            var id = 1;
            var models = JsonHelper.Instance.Deserialize<List<ViewGOODSBACKModel>>(json);
            var model = models[0];
            var now = DateTime.Now;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FCREATETIME = now;
            model.FDATE = now;
            model.FAPPLYID = UserContext.CurrentUser.UserName;
            model.FAPPLYTIME = now;
            int number=0;
            model.FCODE = common.GetNumberAndCodeById(id, out number);
            var result=goodsBack.AddGoodsBack(model, id, number, common);
            return JsonHelper.Instance.Serialize(result);
        }
        #endregion
        /// <summary>
        /// 商品回库明细
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodBackDetails(string rowData,string outWare,string inWare)
        {
            var model = JsonHelper.Instance.Deserialize<ViewGOODSBACKModel>(rowData);
            ViewBag.outWare = outWare;
            ViewBag.inWare = inWare;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string GetGoodsBackDetailsList(string parentId)
        {
            var lst = goodsBack.GetAllGoodsBackDetailsInfo(parentId);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = lst.Count() });
        }
    }
}
