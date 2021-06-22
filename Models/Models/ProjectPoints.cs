using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class ProjectPoint
    {
        public int ID { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectInfoId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string  ProjectName { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 总排名
        /// </summary>
        public string AllRanks { get; set; }

        /// <summary>
        /// 项目排名
        /// </summary>
        public string ProjectRanks { get; set; }


    }
}
