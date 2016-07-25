using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    /// <summary>
    /// 商品回库主表
    /// </summary>
    public class T_GOODSBACKModel:UiResponse
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
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? FDATE { get; set; }
        /// <summary>
        /// 流水号:用于计算自增序号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 调出仓库:T_WAREHOUSE表中FID
        /// </summary>
        public string FOUTWAREHOUSEID { get; set; }
        /// <summary>
        /// 调回仓库:T_WAREHOUSE表中FID
        /// </summary>
        public string FINWAREHOUSEID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
        /// <summary>
        /// 状态:1：未提交，2已提交(默认1)
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 提交人:T_PERSON表中FID
        /// </summary>
        public string FAPPLYID { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? FAPPLYTIME { get; set; }
    }
    public class ViewGOODSBACKModel : UiResponse
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
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? FDATE { get; set; }
        /// <summary>
        /// 流水号:用于计算自增序号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 调出仓库:T_WAREHOUSE表中FID
        /// </summary>
        public string FOUTWAREHOUSEID { get; set; }
        /// <summary>
        /// 调回仓库:T_WAREHOUSE表中FID
        /// </summary>
        public string FINWAREHOUSEID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FMEMO { get; set; }
        /// <summary>
        /// 状态:1：未提交，2已提交(默认1)
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 提交人:T_PERSON表中FID
        /// </summary>
        public string FAPPLYID { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string FAPPLYName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? FAPPLYTIME { get; set; }
    }
}
