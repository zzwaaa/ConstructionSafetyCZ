using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
   public class FileUpload
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 通知文档ID
        /// </summary>
        public string NoticeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 通知文档类型
        /// </summary>
        public string NoticeType { get; set; }

        ///// <summary>
        ///// 图片
        ///// </summary>
        //public string ImgUrl { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImgUuId { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }

        ///// <summary>
        ///// 标签
        ///// </summary>
        //public string Lable { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 带标签的正文
        /// </summary>
        public string ContentLable { get; set; }

        ///// <summary>
        ///// 是否发布 0：草稿状态  1：发布
        ///// </summary>
        //public string IsPublish { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeleteMark { get; set; }

        ///// <summary>
        /////  阅读数
        ///// </summary>
        //public string SelectCount { get; set; }

        /// <summary>
        ///  项目ID 该内容发布到该项目上 
        /// </summary>
        public string ProjectInfoId { get; set; }

        ///// <summary>
        ///// 消息标识 0:未读，1：已读
        ///// </summary>
        //public string MID { get; set; }
    }
}
