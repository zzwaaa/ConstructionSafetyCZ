using System;

namespace Models.Models
{
    public class FileImg
    {
        public int ID { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImgUuId { get; set; }

        public string ImgUrl { get; set; }

        public string VideoUrl { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
