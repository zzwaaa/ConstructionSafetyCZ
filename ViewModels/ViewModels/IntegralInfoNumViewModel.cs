using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
   public class IntegralInfoNumViewModel
    {
        public int IntegralNums { get; set; }

        public int UnIntegralNums { get; set; }

        public IntegralCount integralCounts { get; set; }
    }

    public class IntegralCount
    {
        public int NoticeCount { get; set; }

        public int ImgTxtCount { get; set; }

        public int VideoCount { get; set; }

        public int GameCount { get; set; }
    }
}
