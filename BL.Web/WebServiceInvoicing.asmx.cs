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
        VIPINFOService vipInfoService = new VIPINFOService();
        WareHoseService wareHouseService = new WareHoseService();
        GoodsService goodService = new GoodsService();
        SALEDAYBOOKService saledayBookService = new SALEDAYBOOKService();

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
        /// 获取所有分仓场所信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<T_WAREHOUSEModel> GetPlaceInfo()
        {
            return wareHouseService.GetAllWareHoseInfo().ToList();
        }
        ///// <summary>
        ///// 录入商品信息
        ///// </summary>
        ///// <returns></returns>
        //[WebMethod]
        //public bool AddCommodity(T_GOODSModel model)
        //{
        //    if (goodService.IsExistsFID(model.FGUID,model.FID))
        //    {
        //        return goodService.EditGoodsInfo(model);
        //    }

        //    else
        //    {
        //        return goodService.AddGoodsInfo(model);
        //    }

        //}
        /// <summary>
        /// 录入会员信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool AddMember(T_VIPINFOModel model)
        {

            if (vipInfoService.IsExistsFID(model.FGUID,model.FID))
            {
                return vipInfoService.EditVIPINFO(model);
            }

            else
            {
                return vipInfoService.AddVIPINFO(model);
            }
        }
        /// <summary>
        /// 录入交易信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int AddDeal(List<T_SALEDAYBOOKModel> listModel)
        {
            int result = 0;
            for (int i = 0; i < listModel.Count; i++)
            {
                IDictionary dic = new Hashtable();
                dic["FBARCODE"] = listModel[i].FOUTWAREHOUSEID.ToString()+" "+listModel[i].FGOODSID.ToString()+" "+ listModel[i].FPRICE.ToString()+" "+ listModel[i].FQUANTITY.ToString();
                IEnumerable<T_REPERTORYCHILDModel> rcList = saledayBookService.SearchREPERTORYCHILD(dic);
                if(rcList.Count()>0)
                {
                    listModel[i].FPARENTID = rcList.First().FGUID;
                    listModel[i].FBATCH = rcList.First().FBATCH;
                    listModel[i].FGUID = Guid.NewGuid().ToString();
                    listModel[i].FCREATETIME = DateTime.Now;
                    listModel[i].FINWAREHOUSEID = rcList.First().FINWAREHOUSEID;
                    listModel[i].FOUTWAREHOUSEID = rcList.First().FOUTWAREHOUSEID;
                    listModel[i].FDATE = DateTime.Now;
                    listModel[i].FGOODSID = rcList.First().FGOODSID;
                    listModel[i].FGOODSNAME = rcList.First().FGOODSNAME;
                    listModel[i].FUNIT = rcList.First().FUNIT;
                    listModel[i].FPRICE = rcList.First().FPRICE;
                    listModel[i].FMONEY = rcList.First().FQUANTITY* rcList.First().FPRICE;
                    listModel[i].FMARKETPRICE = rcList.First().FMARKETPRICE;
                    listModel[i].FMARKETMONEY = rcList.First().FQUANTITY * rcList.First().FMARKETPRICE;
                    listModel[i].FPARENTID = rcList.First().FGUID;

                }
                result += saledayBookService.AddSALEDAYBOOKInfo(listModel[i]);
            }

            return result;
        }
    }
}


