using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    /// <summary>
    /// 其他出库明细单
    /// </summary>
    public class T_OTHEROUTDETAILSModel
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
        /// 主表FGUID:T_PURCHASEBACK(采购退货)表中FGUID
        /// </summary>
        public string FPARENTID { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string FBATCH { get; set; }
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
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }
    }
}
