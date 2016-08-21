using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_GUIDANCEModel : UiResponse
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
        /// 日期
        /// </summary>
        public DateTime FDATE { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 单据编号字段只读，保存时自动生成，XHPRK-YYYY-MM-DD-00
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 采购人
        /// </summary>
        public string FPERSONID { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string FWAREHOUSEID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
        /// <summary>
        /// 状态1：未提交，2已提交
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 提交人T_PERSON表中FID
        /// </summary>
        public string FAPPLYID { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? FAPPLYTIME { get; set; }
    }
}
