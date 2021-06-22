using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
  public  class FileImgViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImgUuId { get; set; }

        public List<string> ImgUrl { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
