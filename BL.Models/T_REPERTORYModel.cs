using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    /// <summary>
    /// 库存表
    /// </summary>
    public class T_REPERTORYModel : UiResponse
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
        /// 批次 T_PURCHASE 中FCODE批次直接用采购入库单号表示
        /// </summary>
        public string FBATCH { get; set; }
        /// <summary>
        /// 供应商 T_SUPPLIER表中FID
        /// </summary>
        public string FSUPPLIERID { get; set; }
        /// <summary>
        /// 仓库 T_WAREHOUSE表中FID 下拉选择 WHERE FCATEGORY=“1”；
        /// </summary>
        public string FWAREHOUSEID { get; set; }
        /// <summary>
        /// 采购人 T_PERSON表中FID
        /// </summary>
        public string FPERSONID { get; set; }
        /// <summary>
        /// 商品编号 T_GOODS表中FID
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
        /// 计量方式 1：称重，2 计数
        /// </summary>
        public string FCALCTYPE { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public decimal? FQUANTITY { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal? FSURPLUS { get; set; }
        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal? FENABLE { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? FPRICE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }
    }
}