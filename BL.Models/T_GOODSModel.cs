using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_GOODSModel
    {
        /// <summary>
        /// FGUID主键
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
        /// 编号
        /// </summary>
        public string FID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string FSTANDARD { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string FUNIT { get; set; }
        /// <summary>
        /// 计量方式
        /// </summary>
        public string FCALCTYPE { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string FCATEGORY { get; set; }
        /// <summary>
        /// 是否消耗品
        /// </summary>
        public string FISCONSUMABLES { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime? FSTARTTIME { get; set; }
        /// <summary>
        /// 禁用时间
        /// </summary>
        public DateTime? FENDTIME { get; set; }
        public string FMEMO { get; set; }
    }
}
