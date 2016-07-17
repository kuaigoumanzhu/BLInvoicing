using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_DATADICTModel:UiResponse
    {
        /// <summary>
        /// FGUID 主键
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建人编号 默认当前登录者编号
        /// </summary>
        public string FCREATEID { get; set; }
        /// <summary>
        /// 创建时间 默认当前服务器时间
        /// </summary>
        public DateTime? FCREATETIME { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string FID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 类别如果是类别默认直接是“数据字典类别”，否则是关联的类别编码
        /// </summary>
        public string FCATEGORY { get; set; }
        /// <summary>
        /// 上级编码
        /// </summary>
        public string FPARENTID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int? FORDER { get; set; }
        /// <summary>
        /// 状态1：未启用，2已启用，3已禁用
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime? FSTARTTIME { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? FENDTIME { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FMEMO { get; set; }
    }
}
