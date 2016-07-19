using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_PERSONModel:UiResponse
    {
        /// <summary>
        /// FGUID
        /// </summary>
        public string FGUID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string FCREATETIME { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string FID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public string FCOMPANYID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string FCOMPANY { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public string FDEPTID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string FDEPT { get; set; }
        /// <summary>
        /// 岗位编号
        /// </summary>
        public string FPOSTID { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string FPOST { get; set; }
        /// <summary>
        /// 状态1：未启用，2已启用，3已禁用
        /// </summary>
        public string FSTATUS { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public string FSTARTTIME { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string FENDTIME { get; set; }
        /// <summary>
        /// 功能URL用英文逗号隔开存储。
        /// </summary>
        public string FFUNCURL { get; set; }
    }
}
