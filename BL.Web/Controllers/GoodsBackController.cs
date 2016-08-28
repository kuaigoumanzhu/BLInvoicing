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
        /// <summary>
        /// 商品回库明细列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="wareHouse">总仓</param>
        /// <returns></returns>
        [JsonException]
        public string GetGoodsBackDetailsList(string parentId, string inWareHouse,string wareHouse)
        {
            var lst = goodsBack.GetAllGoodsBackDetailsInfo(parentId,inWareHouse, wareHouse);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = lst.Count() });
        }

        [JsonException]
        public string EditGoodsBackDetailsJson(string json,string parentId,string outWare)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_GOODSBACKDETAILSModel>>(json);
            string userId = UserContext.CurrentUser.UserName;
            var lst=goodsBack.AddGoodsBackDetailUpdateChild(models, userId, parentId,outWare);
            return JsonHelper.Instance.Serialize(new { list = lst, statusCode = "200", message = "保存成功！" });
        }

        public string DelGoodsBackDetailJson(string json)
        {
            var model = JsonHelper.Instance.Deserialize<List<T_GOODSBACKDETAILSModel>>(json)[0];
            if(!goodsBack.GetGoodsBackByParentId(model.FPARENTID))
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { statusCode = "300", closeCurrent = true, message = "该商品已提交，无法删除！" });
            }
            if(goodsBack.DelGoodsBackDetail(model.FGUID))
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { closeCurrent = true });
            }else
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { statusCode = "300", closeCurrent = true, message = "删除失败！" });
            }
        }
        public string ApplayGoodsBackDetailJson(string json,string parentId, string outWare)
        {
            var status = goodsBack.GetGoodsBackStatusByParentId(parentId);
            if (status== "2")
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { statusCode = "300", closeCurrent = true, message = "已提交！" });
            }
            if (status == "1")
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { statusCode = "300", closeCurrent = true, message = "请保存后再提交！" });
            }
            
            var models = JsonHelper.Instance.Deserialize<List<T_GOODSBACKDETAILSModel>>(json);
            var bol= goodsBack.ApplayGoodsBackDetail(models, parentId, outWare);
            if (bol)
            {
                return JsonHelper.Instance.Serialize(new BL.Models.UiResponse { message = "提交成功！", statusCode = "200" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new BL.Models.UiResponse { message = "提交失败！", statusCode = "300" });
            }

        }
    }
}
