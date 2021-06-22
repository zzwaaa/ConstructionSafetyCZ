using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Common.Common
{
    //public class RaffQRCode:IQRCode
    //{
        //public Bitmap GetQRCode(string url, int pixel, string title)
        //{
        //    QRCodeGenerator generator = new QRCodeGenerator();
        //    QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
        //    QRCode qrcode = new QRCode(codeData);

        //    Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, true);

        //    int width = qrImage.Width;
        //    int height = qrImage.Height;

        //    float dpiX = qrImage.HorizontalResolution;
        //    float dpiY = qrImage.VerticalResolution;

        //    Bitmap bitmapResult = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        //    bitmapResult.SetResolution(dpiX, dpiY);

        //    Graphics Grp = Graphics.FromImage(bitmapResult);
        //    Rectangle Rec = new Rectangle(0, 0, width, height);
        //    Grp.DrawImage(qrImage, 0, 0, Rec, GraphicsUnit.Pixel);

        //    var count = Encoding.UTF8.GetByteCount(title);
        //    //平移Graphics对象
        //    Grp.TranslateTransform(width / 2 - count * 14 / 2, 10);
        //    //设置文字填充颜色
        //    Brush brush = Brushes.Black;
        //    Grp.DrawString(title, new Font("微软雅黑", 28, GraphicsUnit.Pixel), brush, 0, 0);
        //    Grp.ResetTransform();
        //    Grp.Dispose();
        //    GC.Collect();
        //    return bitmapResult;
        //}
    //}
}
