using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
   public class NoticeInfoViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public List<string> ImgUrl { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 带标签的正文
        /// </summary>
        public string ContentLable { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        public string ProjectInfoId { get; set; }
    }

    public class PersonNewViewModel 
    {
        public NoticeInfoViewModel Notices { get; set; }

        public string MID { get; set; }
    }
}
