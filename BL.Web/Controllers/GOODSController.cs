using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BL.Web.Controllers
{
    public class GOODSController : Controller
    {
        //
        // GET: /GOODS/
        GoodsService goodsService = new GoodsService();

        public ActionResult Index()
        {
            return View();
        }
        [JsonException]
        public string GetAllGoodsJson(int pageCurrent=1, int pageSize=1,string FID="",string FNAME="")
        {
            IDictionary dic = new Hashtable();
            int totalPage=0;
            var lst = goodsService.GetAllGoodsInfo(dic,ref totalPage,pageCurrent,pageSize);
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize =pageSize,pageCurrent=pageCurrent,total=totalPage });
        }
        [JsonException]
        public string EditGoods(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_GOODSModel>>(json);
            var model = models[0];
            model.FCREATETIME =DateTime.Now;
            int rel = 0;
            if (string.IsNullOrEmpty(model.FGUID))
            {
                model.FGUID = Guid.NewGuid().ToString();
                if (model.FSTATUS == "2")
                {
                    model.FSTARTTIME = DateTime.Now;
                    model.FENDTIME = null;
                }
                else if (model.FSTATUS == "3")
                {
                    model.FENDTIME = DateTime.Now;
                    model.FSTARTTIME = null;
                }
                model.FGUID = Guid.NewGuid().ToString();
                rel=goodsService.AddGoods(model);
            }
            else
            {
                if (model.FSTATUS == "2")
                {
                    model.FSTARTTIME = DateTime.Now;                    
                }
                else if (model.FSTATUS == "3")
                {
                    model.FENDTIME = DateTime.Now;
                }
                rel=goodsService.EditGoods(model);
            }
            if (rel > 0)
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "保存成功" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "保存失败" });
            }
        }
        [JsonException]
        [HttpPost]
        public string DelGoods(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            int rel = goodsService.DelGoods(t[0]["FGUID"].ToString());
            if (rel > 0)
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "200", message = "删除成功" });
            }
            else
            {
                return JsonHelper.Instance.Serialize(new { statusCode = "300", message = "删除失败" });
            }
        }

        /// <summary>
        /// 设置商品状态
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FSTATUS">1未启用，2已启用，3禁用</param>
        /// <returns></returns>
        [JsonException]
        public string SetGoodsStatus(string FGUID, string FSTATUS)
        {
            var time = DateTime.Now;
            return JsonHelper.Instance.Serialize(new { result = goodsService.SetGoodsStatusByGuid(FGUID, FSTATUS, time), data = FSTATUS, time = time });
        }
    }
}
