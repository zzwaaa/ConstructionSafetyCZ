using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Models
{
   public class LifeInUserInfo
    {
        public int ID { get; set; }

        public string UserInfoId { get; set; }

        //public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
      //  public string Avatar { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(11)]
        public string Telephone { get; set; }

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

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }


    }
}
