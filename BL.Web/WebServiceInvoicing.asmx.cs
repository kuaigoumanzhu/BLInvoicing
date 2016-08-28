using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BL.Framework;
using BL.MVC;
using BL.Service;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using BL.Models;
using BL.Framework.Orm;


namespace BL.Web
{
    /// <summary>
    /// WebServiceInvoicing 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceInvoicing : System.Web.Services.WebService
    {
        GoodsService goodsService = new GoodsService();
        VIPINFOOService vipInfoService = new VIPINFOOService();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<T_GOODSModel> GetCommodityInfo()
        {
            IDictionary dic = new Hashtable();
            dic["FISCONSUMABLES"] = "1";//非消耗品
            dic["FSTATUS"] = "2";//启用
            return goodsService.GetGoodsInfo(dic).ToList();
        }
        /// <summary>
        /// 获取所有会员信息
        /// </summary>
        /// <returns></returns>s
        [WebMethod]
        public List<T_VIPINFOModel> GetMemberInfo()
        {
            IDictionary dic = new Hashtable();
            return vipInfoService.GetVIPINFOInfo(dic).ToList();
        }
        /// <summary>
        /// 获取所有场所信息
        /// </summary>
        /// <returns></returns>
        //[WebMethod]
        //public List<Maticsoft.Model.PlaceInfo> GetPlaceInfo()
        //{
        //    return place.GetModelList("");
        //}
        ///// <summary>
        ///// 录入商品信息
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod]
        //public bool AddCommodity(Maticsoft.Model.CommodityInfo model)
        //{
        //    if (commodity.Exists(model.commodityNumber))
        //    {
        //        return commodity.Update(model);
        //    }

        //    else
        //    {
        //        return commodity.Add(model) > 0;
        //    }

        //}
        ///// <summary>
        ///// 录入会员信息
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod]
        //public bool AddMember(Maticsoft.Model.MemberInfo model)
        //{

        //    if (member.Exists(model.cardNumber))
        //    {
        //        return member.Update(model);
        //    }

        //    else
        //    {
        //        return member.Add(model) > 0;
        //    }
        //}
        ///// <summary>
        ///// 更新会员信息
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod]
        //public bool UpdateMember(Maticsoft.Model.MemberInfo model)
        //{

        //    return member.Update(model);

        //}
        ///// <summary>
        ///// 录入交易信息
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod]
        //public int AddDeal(List<Maticsoft.Model.DealInfo> listModel)
        //{
        //    int result = 0;
        //    for (int i = 0; i < listModel.Count; i++)
        //    {
        //        result += deal.Add(listModel[i]);
        //    }

        //    return result;
        //}
    }
}


