using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class ProjectUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string CompanyProId { get; set; }

        /// <summary>
        /// 项目单位
        /// </summary>
        public string ProjectCompany { get; set; }
        
        /// <summary>
        /// 单位负责人手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 单位负责人
        /// </summary>
        public string ProjPerson { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int? DeleteMark { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonId { get; set; }

    }
}
