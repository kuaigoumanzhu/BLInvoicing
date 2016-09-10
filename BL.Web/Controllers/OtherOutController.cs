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
    public class OtherOutController : Controller
    {
        OtherOutService otherOut = new OtherOutService();
        CommonService common = new CommonService();
        public ActionResult OtherOutList()
        {
            return View();
        }
        [JsonException]
        public string GetOtherOutList(int pageCurrent = 1, int pageSize = 10)
        {
            var total = 0;
            var lst = otherOut.GetAllOtherOutInfo(pageCurrent, pageSize, out total);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = pageSize, pageCurrent = pageCurrent, total = total });
        }
        /// <summary>
        /// 保存其他出库
        /// </summary>
        /// <returns></returns>
        [JsonException]
        public string EditOtherOutJson(string json)
        {
            var id = 9;
            var models = JsonHelper.Instance.Deserialize<List<ViewOTHEROUTModel>>(json);
            string userId = UserContext.CurrentUser.UserName;
            var model = models[0];
            var now = DateTime.Now;
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATEID = userId;
            model.FCREATETIME = now;
            model.FDATE = now;
            model.FAPPLYID = userId;
            model.FAPPLYTIME = now;
            int number = 0;
            model.FCODE= common.GetNumberAndCodeById(id, out number);
            var result = otherOut.AddOtherOut(model, id, number, common);
            return JsonHelper.Instance.Serialize(result);
        }

        public ActionResult OtherOutDetails(string rowData, string ware,string wareId)
        {
            var model = JsonHelper.Instance.Deserialize<ViewOTHEROUTModel>(rowData);
            ViewBag.ware = ware;
            ViewBag.wareId = wareId;
            ViewBag.userName = UserContext.CurrentUser.TrueName;
            return View(model);
        }
        [JsonException]
        public string GetAllOtherOutDetailsJson(string parentId)
        {
            var lst=otherOut.GetAllOtherOutDetailsByParentId(parentId);
            return JsonHelper.Instance.Serialize(new {list=lst, pageSize=lst.Count() });
        }
        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsId">商品编号</param>
        /// <param name="batch">批次号</param>
        /// <param name="ware">仓库号</param>
        /// <returns></returns>
        [JsonException]
        public string GetGoodsInfoByIdAndBatchWareJson(string goodsId,string batch,string wareId)
        {
            return JsonHelper.Instance.Serialize(otherOut.GetGoodsInfoByIdAndBatchWare(goodsId, batch, wareId));
        }

        public ActionResult SelOtherOutGoods(string wareId)
        {
            return View(otherOut.GetGoodsInfoByWare(wareId));
        }


        [JsonException]
        public string EditOtherOutDetailsJson(string json,string wareId)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_OTHEROUTDETAILSModel>>(json);
            var model = models[0];
            model.FGUID = Guid.NewGuid().ToString();
            model.FCREATEID = UserContext.CurrentUser.UserName;
            model.FCREATETIME = DateTime.Now;
            var res= otherOut.AddOtherOutDetailInfo(model,wareId);
            return JsonHelper.Instance.Serialize(res);
        }
    }
}
