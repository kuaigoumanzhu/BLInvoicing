using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_VIPINFOModel:UiResponse
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
        /// 编号（卡号）
        /// </summary>
        public string FID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string FNAME { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string FSEX { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string FAGE { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string FTEL { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string FMOBILE { get; set; }
        /// <summary>
        /// 办卡地址
        /// </summary>
        public string FPLACE { get; set; }
        /// <summary>
        /// 消费次数
        /// </summary>
        public int FCONSUMPTION { get; set; }
        /// <summary>
        /// 累计积分
        /// </summary>
        public float FINTEGRAL { get; set; }
    }
}
