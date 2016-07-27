using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    /// <summary>
    /// 商品回库明细表
    /// </summary>
    public class T_GOODSBACKDETAILSModel:UiResponse
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
        /// 主表FGUID
        /// </summary>
        public string FPARENTID { get; set; }
        /// <summary>
        /// 商品编号:T_GOODS(商品表)表中FID
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
        /// 批次:T_PURCHASE(采购入库主表)中FCODE批次直接用采购入库单号表示
        /// </summary>
        public string FBATCH { get; set; }
        /// <summary>
        /// 账存数量
        /// </summary>
        public float FQUANTITY { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        public float FACTUALQUANTITY { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public float FPRICE { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public float FMARKETPRICE { get; set; }
        /// <summary>
        /// 差异数量
        /// </summary>
        public float FDIFFERQUANTITY { get; set; }
        /// <summary>
        /// 差异金额
        /// </summary>
        public float FDIFFERMONEY { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string FBARCODE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }
    }
}
