using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_CONSUMABLESDETAILSModel:UiResponse
    {
        /// <summary>
        /// FGUID
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        public string FCREATEID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// 主表FGUID
        /// </summary>
        public string FPARENTID { get; set; }
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
        /// 数量
        /// </summary>
        public float FQUANTITY { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public float FPRICE { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public float FMONEY { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string FSUPPLIERID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
    }
}
