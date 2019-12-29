using System;
using System.IO;
using Photoshop;
using PostCardProcessor.model;


namespace PostCardProcessor
{
    public static class Processor
    {
        /**
         * 处理明信片信息
         */
        public static FileInfo Process(this FileInfo sourceFileInfo, PostCardProcessCropInfo postCardProcessCropInfo, string frontStyle, int productWidth, int productHeight)
        {
            if (sourceFileInfo.Directory != null && !sourceFileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(sourceFileInfo.Directory.FullName);
            }

            try //尝试打开，如果打不开进行标记
            {
                var photoShopOperation = new PhotoShopOperation(sourceFileInfo);

                var myDoc = photoShopOperation.OpenDocument();

                //如果色彩模式不是CMYK、RGB、灰度的话，转化为CMYK模式
                if ((myDoc.Mode != PsDocumentMode.psCMYK) && (myDoc.Mode != PsDocumentMode.psRGB) && (myDoc.Mode != PsDocumentMode.psGrayscale))
                {
                    myDoc.ChangeMode(PsChangeMode.psConvertToCMYK);
                }

                myDoc.RotateCanvas(postCardProcessCropInfo.Rotation);

                myDoc.ResizeImage(null, null, 300);
                //切换到点
                photoShopOperation.SwitchRuler(PsUnits.psPoints);

                var pictureSize = new Size(productWidth, productHeight);
                switch (frontStyle)
                {
                    case "A":
                        pictureSize.Width -= 10;
                        pictureSize.Height -= 10;
                        break;
                    case "C":
                        pictureSize.Width = pictureSize.Height = Math.Min(pictureSize.Height, pictureSize.Width) - 10;
                        break;
                    default:
                        pictureSize.Width = pictureSize.Width;
                        pictureSize.Height = pictureSize.Height;
                        //默认不进行处理
                        break;
                }

                var cutRight = myDoc.Width * (+postCardProcessCropInfo.CropWidth + postCardProcessCropInfo.CropLeft);
                var cutWidth = myDoc.Width * postCardProcessCropInfo.CropWidth;

                var cutHeight = myDoc.Height * postCardProcessCropInfo.CropHeight;

                var cutBottom = myDoc.Height * (postCardProcessCropInfo.CropTop + postCardProcessCropInfo.CropHeight);

                myDoc.ResizeCanvas(cutRight, cutBottom, PsAnchorPosition.psTopLeft);
                myDoc.ResizeCanvas(cutWidth, cutHeight, PsAnchorPosition.psBottomRight);

                //切换到厘米
                photoShopOperation.SwitchRuler(PsUnits.psMM);

                myDoc.ResizeImage(pictureSize.Width, pictureSize.Height);

                //photoShopOperation.Cut(cutLeft, cutTop, cutRight, cutBottom, pictureSize.Width, pictureSize.Height, 300);
                if (frontStyle == "C")
                {
                    //增加白边到成品尺寸的A版
                    myDoc.ResizeCanvas(productWidth - 10, productHeight - 10, PsAnchorPosition.psTopLeft);
                }

                //将文件大小修改为成品尺寸,图像居中
                myDoc.ResizeCanvas(productWidth, productHeight);

                //切换到厘米
                photoShopOperation.SwitchRuler(PsUnits.psPoints);
                myDoc.Selection.SelectAll();


                var solidColor = new SolidColor
                {
                    CMYK = new CMYKColor
                    {
                        Cyan = 0,
                        Magenta = 0,
                        Yellow = 0,
                        Black = 40
                    }
                };

                myDoc.Selection.Stroke(solidColor, 1, Opacity: 100);

                //切换到厘米
                photoShopOperation.SwitchRuler(PsUnits.psMM);

                var myJpegSaveOption = new JPEGSaveOptions
                {
                    Quality = 12,
                    Matte = PsMatteType.psNoMatte,
                    FormatOptions = PsFormatOptionsType.psStandardBaseline
                };

                try
                {
                    photoShopOperation.Application.DoJavaScript("executeAction(charIDToTypeID( \"FltI\" ), undefined, DialogModes.NO );");
                }
                catch
                {
                    // ignored
                }

                foreach (Channel orange in myDoc.Channels)
                {
                    if (orange.Visible == false)
                    {
                        orange.Delete();
                    }
                }


                var fileInfo = new FileInfo(sourceFileInfo.FullName + ".tmp.jpg");
                myDoc.SaveAs(fileInfo.FullName, myJpegSaveOption);
                photoShopOperation.ResetRuler();
                myDoc.Close();
                return fileInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}