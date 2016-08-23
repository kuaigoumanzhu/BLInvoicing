using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_SALEDAYBOOKModel : UiResponse
    {

        /// <summary>
        /// FGUID
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string FCREATEID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// 批次 按商品编码和仓库 计数从1开始
        /// </summary>
        public string FBATCH { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? FDATE { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string FVIPCARD { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string FVIPNAME { get; set; }
        /// <summary>
        /// 消费次数
        /// </summary>
        public int FEXPENDNUMBER { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string FGOODSID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string FGOODSNAME { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string FUNIT { get; set; }
        /// <summary>
        /// 批次 T_PURCHASE 中FCODE
        /// </summary>
        public string FBATCH2 { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? FQUANTITY { get; set; }
        /// <summary>
        /// 成本单价
        /// </summary>
        public decimal? FPRICE { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public decimal? FMONEY { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal? FMARKETPRICE { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal? FMARKETMONEY { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? FINTEGRAL { get; set; }
        /// <summary>
        /// 利润
        /// </summary>
        public decimal? FPROFIT { get; set; }
        /// <summary>
        /// 利润率
        /// </summary>
        public string FPROFITRATE { get; set; }
    }
}