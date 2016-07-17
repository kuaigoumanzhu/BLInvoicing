using BL.Framework;
using BL.Models;
using BL.MVC;
using BL.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        public string GetAllGoodsJson()
        {
            var lst = goodsService.GetAllGoodsInfo();
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize = lst.Count() });
        }
        [JsonException]
        public string EditGoods(string json)
        {
            JArray t = (JArray)JsonConvert.DeserializeObject(json);
            T_GOODSModel model = new T_GOODSModel();
            model.FCREATEID = t[0]["FCREATEID"].ToString();
            model.FCREATETIME =DateTime.Now;
            model.FID = t[0]["FID"].ToString();
            model.FNAME = t[0]["FNAME"].ToString();
            model.FSTANDARD = t[0]["FSTANDARD"].ToString();
            model.FUNIT = t[0]["FUNIT"].ToString();
            model.FCALCTYPE = t[0]["FCALCTYPE"].ToString();
            model.FCATEGORY = t[0]["FCATEGORY"].ToString();
            model.FISCONSUMABLES = t[0]["FISCONSUMABLES"].ToString();
            model.FSTATUS = t[0]["FSTATUS"].ToString();
            
            model.FMEMO = t[0]["FMEMO"].ToString();
            int rel = 0;
            if (t[0]["addFlag"]!=null && Convert.ToBoolean(t[0]["addFlag"].ToString()))
            {
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
                    if (t[0]["FENDTIME"].ToString() != "")
                    {
                        model.FENDTIME = Convert.ToDateTime(t[0]["FENDTIME"].ToString());
                    }
                }
                else if (model.FSTATUS == "3")
                {
                    if (t[0]["FSTARTTIME"].ToString() != "")
                    {
                        model.FSTARTTIME = Convert.ToDateTime(t[0]["FSTARTTIME"].ToString());
                    }
                    model.FENDTIME = DateTime.Now;
                }
                model.FGUID = t[0]["FGUID"].ToString();
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
