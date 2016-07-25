using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    /// <summary>
    /// 分仓库存表
    /// </summary>
    public class T_REPERTORYCHILDModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        public string FCREATEID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime FCREATETIME { get; set; }
        /// <summary>
        /// 批次:T_PURCHASE(采购入库主表)中FCODE批次直接用采购入库单号表示
        /// </summary>
        public string FBATCH { get; set; }
        /// <summary>
        /// 调出仓库T_WAREHOUSE表中FID 下拉选择 where FCATEGORY=“1”；
        /// </summary>
        public string FOUTWAREHOUSEID{get;set;}
        /// <summary>
        /// 调入仓库T_WAREHOUSE表中FID 下拉选择 where FCATEGORY=“2”
        /// </summary>
        public string FINWAREHOUSEID { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string FBARCODE { get; set; }
        /// <summary>
        /// 商品编号:T_GOODS(商品表)表中FID
        /// </summary>
        public string FGOODSID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string FUNIT { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public float FQUANTITY { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public float FSURPLUS { get; set;}
        /// <summary>
        /// 可用数量
        /// </summary>
        public float FENABLE { get; set;}
        /// <summary>
        /// 成本单价
        /// </summary>
        public float FPRICE { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public float FMARKETPRICE { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
    }
}
