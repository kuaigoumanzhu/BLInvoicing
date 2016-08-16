using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_PURCHASEBACKModel : UiResponse
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
        public DateTime? FDATE { get; set; }
        /// <summary>
        /// 流水号 用于计算自增序号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 调出仓库 T_WAREHOUSE表中FID
        /// </summary>
        public string FOUTWAREHOUSEID { get; set; }
        /// <summary>
        /// 调入仓库 T_WAREHOUSE表中FID
        /// </summary>
        public string FINWAREHOUSEID { get; set; }
        /// <summary>
        /// 摘要 T_GOODS表中FID
        /// </summary>
        public string FMEMO { get; set; }
        /// <summary>
        /// 状态 1：未提交，2已提交。
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 提交人 T_PERSON表中FID
        /// </summary>
        public string FAPPLYID { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? FAPPLYTIME { get; set; }

    }
}
