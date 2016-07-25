using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Models
{
    public class T_NUMBERModel
    {
        /// <summary>
        /// 流水号类别
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 编号常量
        /// </summary>
        public string FConstant { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int FNUMBER { get; set; }
        /// <summary>
        /// 自增序号位数
        /// </summary>
        public int FPadLeft { get; set; }

        public string FMEMO { get; set; }
    }
}
