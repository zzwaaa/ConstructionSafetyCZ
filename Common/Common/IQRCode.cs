using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Common.Common
{
   public interface IQRCode
    {
        Bitmap GetQRCode(string url, int pixel, string title);
    }
}
