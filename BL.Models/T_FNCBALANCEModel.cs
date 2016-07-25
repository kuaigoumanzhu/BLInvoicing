using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_FNCBALANCEModel:UiResponse
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
        public string FCREATETIME { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? FDATE { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string FWAREHOUSEID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string FAPPLYID { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? FAPPLYTIME { get; set; }
    }
}
