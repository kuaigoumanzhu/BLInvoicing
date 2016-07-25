using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_WARNINGModel
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
        /// 销售仓库
        /// </summary>
        public string FWAREHOUSEID { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string FGOODSID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string FGOODSNAME { get; set; }
        /// <summary>
        /// 销售完成时间
        /// </summary>
        public DateTime? FENDTIME { get; set; }
    }
}
