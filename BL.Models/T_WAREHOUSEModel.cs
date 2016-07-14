using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_WAREHOUSEModel
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
        /// 编号
        /// </summary>
        public string FID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 仓库类别
        /// </summary>
        public string FCATEGORY { get; set; }
        /// <summary>
        /// 上级编码
        /// </summary>
        public string FPARENTID { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string FPROVINCE { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime FSTARTTIME { get; set; }
        /// <summary>
        /// 禁用时间
        /// </summary>
        public DateTime FENDTIME { get; set; }
    }
}
