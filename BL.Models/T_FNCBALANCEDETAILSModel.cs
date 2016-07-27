using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_FNCBALANCEDETAILSModel:UiResponse
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
        /// 交易仓库
        /// </summary>
        public string FWAREHOUSEID { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public string FMARKETMONEY { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public string FBACKTMONEY { get; set; }
        /// <summary>
        /// 差异金额
        /// </summary>
        public string FDIFFERMONEY { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }
    }
}
