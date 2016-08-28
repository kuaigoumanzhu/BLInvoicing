using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_PURCHASEDETAILSModel : UiResponse
    {
        /// <summary>
        /// FGUID
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建人编号默认当前登录者编号
        /// </summary>
        public string FCREATEID { get; set; }
        /// <summary>
        /// 创建时间默认当前服务器时间
        /// </summary>
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// T_GUIDANCE表中FGUID
        /// </summary>
        public string FPARENTID { get; set; }
        /// <summary>
        /// T_GOODS表中FID 商品编号
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
        /// 计量方式
        /// </summary>
        public string FCALCTYPE { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? FQUANTITY { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? FMONEY { get; set; }
        /// <summary>
        /// 单价= 金额/数量
        /// </summary>
        public decimal? FPRICE { get; set; }
        /// <summary>
        ///  供应商 T_SUPPLIER表中FID
        /// </summary>
        public string FSUPPLIERID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }

    }
}
