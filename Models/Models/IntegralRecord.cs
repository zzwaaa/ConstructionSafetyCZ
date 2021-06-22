using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class IntegralRecord
    {
        public int ID { get; set; }

        /// <summary>
        /// 积分类型 0:看通知 ，1:看图文 ,2:看视频 ，3:玩游戏
        /// </summary>
        public string IntegralType { get; set; }

        /// <summary>
        /// 所看内容所得的积分
        /// </summary>
        public int IntegralNums { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectInfoId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 项目所发布的通知 如果某人在某项目已读过该则消息 若要已获取积分 就把此条通知添加到积分记录表中以该人id和所在项目id以及获取积分消息id存储
        /// </summary>
        public string NoticeId { get; set; }
    }
}
