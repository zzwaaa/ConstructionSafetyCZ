using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class IntegralInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 积分数
        /// </summary>
        public int IntegralNums { get; set; }

        /// <summary>
        /// 已兑换积分数
        /// </summary>
        public int UnIntegralNums { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectInfoId { get; set; }

        /// <summary>
        /// 积分排序
        /// </summary>
        //public int IntegralRank { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
