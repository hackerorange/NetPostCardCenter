using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Response.envelope;
using Hacker.Inko.Net.Response.postCard;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;
using iText.Layout.Element;
using Color = iText.Kernel.Colors.Color;
using Image = iText.Layout.Element.Image;


namespace Hacker.Inko.PostCard.Library.Support
{
    public static class PostCardCollectionSupport
    {
        private class ProgressInternal
        {
            private readonly double _start;
            private readonly double _end;

            public ProgressInternal(double start, double end)
            {
                _start = start;
                _end = end;
            }

            public double GetProcessRealValue(double processValue)
            {
                return (_end - _start) * processValue + _start;
            }
        }

        public static void GeneratePdfFile(this EnvelopeResponse envelopeResponse, string fileFullName, bool urgent, string waterMark, Action<double> processHandler = null, Action<string> onError = null)
        {
            var current = new ProgressInternal(0.0, 0.3);
            // 从服务器获取明信片,并安装份数进行划分
            var postCardResponses = PreparePostCardFromServer(envelopeResponse, value => processHandler?.Invoke(current.GetProcessRealValue(value)), onError);
            // 生成PDF
            GeneratePdfFileByPostCardList(envelopeResponse, postCardResponses, fileFullName, urgent, waterMark, value => processHandler?.Invoke(new ProgressInternal(0.3, 1).GetProcessRealValue(value)), onError);
        }


        private static IEnumerable<PostCardResponse> PreparePostCardFromServer(EnvelopeResponse envelopeResponse, Action<double> processHandler = null, Action<string> onError = null)
        {
            var postCardResponses = new List<PostCardResponse>();
            var isWait = true;
            PostCardItemApi.GetPostCardByEnvelopeId(envelopeResponse.EnvelopeId, postCardList =>
            {
                for (var i1 = 0; i1 < postCardList.Count; i1++)
                {
                    processHandler?.Invoke(i1 / (double) postCardList.Count);
                    var postCard = postCardList[i1];
                    for (var i = 0; i < postCard.Copy; i++) postCardResponses.Add(postCard);
                }

                isWait = false;
            }, message =>
            {
                onError?.Invoke(message);
                isWait = false;
            });
            while (isWait)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            processHandler?.Invoke(1);
            return postCardResponses;
        }


        private static void GeneratePdfFileByPostCardList(EnvelopeResponse envelopeResponse, IEnumerable<PostCardResponse> postCardResponses, string fileFullName, bool urgent, string waterMark, Action<double> processHandler, Action<string> onError = null)
        {
            var fileInfo = new FileInfo(fileFullName);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) fileInfo.Directory.Create();

            if (fileInfo.Exists)
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception e)
                {
                    onError(e.Message);
                    return;
                }

            var tempDirectoryInfo = new DirectoryInfo(fileInfo.DirectoryName + "/" + Guid.NewGuid());
            if (!tempDirectoryInfo.Exists) tempDirectoryInfo.Create();

            var pdfDocument = new PdfDocument(new PdfWriter(new FileStream(fileInfo.FullName, FileMode.CreateNew)));

            var document = new Document(pdfDocument);
            try
            {
                pdfDocument.SetDefaultPageSize(new PageSize(envelopeResponse.PaperWidth.MMtoPix(), envelopeResponse.PaperHeight.MMtoPix()));

                var pageContexts = new List<PostCardPageContext>();
                var postCardPageContext = new PostCardPageContext(tempDirectoryInfo, envelopeResponse, urgent, waterMark)
                {
                    Document = document
                };

                pdfDocument.AddNewPage(); // 添加一页
                postCardPageContext.FrontPageNumber = pdfDocument.GetNumberOfPages(); // 正面为当前最后一页
                // 如果是正反面，再添加一页
                if (envelopeResponse.DoubleSide)
                {
                    pdfDocument.AddNewPage(); // 添加一页
                    postCardPageContext.BackPageNumber = pdfDocument.GetNumberOfPages(); // 反面为最后一页
                }

                pageContexts.Add(postCardPageContext);

                foreach (var postCard in postCardResponses)
                    if (!postCardPageContext.AddPostCardResponse(postCard))
                    {
                        pdfDocument.AddNewPage();
                        postCardPageContext = new PostCardPageContext(tempDirectoryInfo, envelopeResponse, urgent, waterMark)
                        {
                            FrontPageNumber = pdfDocument.GetNumberOfPages(),
                            BackPageNumber = pdfDocument.GetNumberOfPages() + 1,
                            Document = document
                        };
                        pdfDocument.AddNewPage();
                        pageContexts.Add(postCardPageContext);
                        postCardPageContext.AddPostCardResponse(postCard);
                    }

                for (var i = 0; i < pageContexts.Count; i++)
                {
                    processHandler?.Invoke(i / (double) pageContexts.Count);
                    var page = pageContexts[i];
                    page.PreparePdfPage();
                }

                processHandler?.Invoke(1);
            }
            finally
            {
                try
                {
                    tempDirectoryInfo.Delete(true);
                }
                catch
                {
                }

                document.Flush();
                document.Close();
            }
        }
    }

    internal class PostCardPageContext
    {
        private readonly int _paperColumn;
        private readonly int _paperRow;
        private readonly DirectoryInfo _templateDirectoryInfo;
        private readonly List<PostCardResponse> _postCardResponses = new List<PostCardResponse>();
        private readonly EnvelopeResponse _envelopeResponse;

        public int FrontPageNumber { get; set; }
        public int BackPageNumber { get; set; }

        public Document Document { get; set; }

        private readonly bool _urgent;
        private readonly string _waterMark;


        public PostCardPageContext(DirectoryInfo templateDirectoryInfo, EnvelopeResponse envelopeResponse, bool urgent, string waterMark)
        {
            _templateDirectoryInfo = templateDirectoryInfo;
            _envelopeResponse = envelopeResponse;
            _paperColumn = envelopeResponse.PaperColumn;
            _paperRow = envelopeResponse.PaperColumn;
            _urgent = urgent;
            _waterMark = waterMark;
        }

        public bool AddPostCardResponse(PostCardResponse postCardResponse)
        {
            if (_postCardResponses.Count() >= _paperColumn * _paperRow) return false;

            _postCardResponses.Add(postCardResponse);
            return true;
        }


        public void PreparePdfPage()
        {
            // 创建新的一页
            var leftWhite = (Document.GetPdfDocument().GetDefaultPageSize().GetWidth() - _paperColumn * _envelopeResponse.ProductWidth.MMtoPix()) / 2;
            var topWhite = (Document.GetPdfDocument().GetDefaultPageSize().GetHeight() - _paperRow * _envelopeResponse.ProductHeight.MMtoPix()) / 2;

            // 添加辅助线
            var pdfCanvas = new PdfCanvas(Document.GetPdfDocument(), FrontPageNumber);
            pdfCanvas.SetLineDash(10).SetColor(new DeviceGray(1), true);


            // 画竖线
            for (var i = 0; i <= _paperColumn; i++)
                pdfCanvas
                    .MoveTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), 0)
                    .LineTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), topWhite - 5)
                    .MoveTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), Document.GetPdfDocument().GetDefaultPageSize().GetHeight())
                    .LineTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), Document.GetPdfDocument().GetDefaultPageSize().GetHeight() - topWhite + 5);

            for (var j = 0; j <= _paperRow; j++)
                pdfCanvas
                    .MoveTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: 0)
                    .LineTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: leftWhite - 5)
                    .MoveTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: Document.GetPdfDocument().GetDefaultPageSize().GetWidth())
                    .LineTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: Document.GetPdfDocument().GetDefaultPageSize().GetWidth() - leftWhite + 5);


            pdfCanvas.Stroke();
            var fontPath = "";
            if (Process.GetCurrentProcess().MainModule is ProcessModule processModule) fontPath = new FileInfo(processModule.FileName).DirectoryName + @"/STKAITI.TTF";

            // 添加水印
            var pdfFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, false); // 列
            var paragraph = new Paragraph(_waterMark);
            paragraph.SetFont(pdfFont);
            paragraph.SetFontSize(8);
            paragraph.SetFontColor(_urgent ? DeviceRgb.RED : DeviceRgb.BLACK);
            paragraph.SetFixedPosition(
                leftWhite + 10,
                Document.GetPdfDocument().GetDefaultPageSize().GetHeight() - topWhite,
                Document.GetPdfDocument().GetDefaultPageSize().GetWidth() - leftWhite - 10);
            Document.Add(paragraph);
            // 正面
            // 行
            for (var i = 0; i < _paperRow; i++)
                // 列
            for (var j = 0; j < _paperColumn; j++)
            {
                var currentIndex = i * _paperRow + j;
                if (currentIndex >= _postCardResponses.Count) continue;

                var postCard = _postCardResponses[currentIndex];


                // 正面
                var fileId = postCard.ProductFileId;
                if (!string.IsNullOrEmpty(fileId))
                {
                    var frontFile = new FileInfo(_templateDirectoryInfo.FullName + "/" + fileId);
                    if (!frontFile.Exists) frontFile = FileApi.DownloadFileByFileId(fileId, frontFile);

                    var frontImage = new Image(ImageDataFactory.Create(frontFile.FullName));
                    // 转化尺寸
                    frontImage.ScaleAbsolute(_envelopeResponse.ProductWidth.MMtoPix(), _envelopeResponse.ProductHeight.MMtoPix());
                    frontImage.SetPageNumber(FrontPageNumber);

                    // 坐标po
                    frontImage.SetFixedPosition(
                        leftWhite + j * _envelopeResponse.ProductWidth.MMtoPix(),
                        topWhite + i * _envelopeResponse.ProductHeight.MMtoPix());
                    Document.Add(frontImage);
                }

                // 反面
                var backFileId = postCard.BackProductFileId;
                if (!string.IsNullOrEmpty(backFileId))
                {
                    var backFileInfo = new FileInfo(_templateDirectoryInfo.FullName + "/" + backFileId);
                    if (!backFileInfo.Exists) FileApi.DownloadFileByFileId(backFileId, backFileInfo);

                    var backImage = new Image(ImageDataFactory.Create(backFileInfo.FullName));
                    // 转化尺寸
                    backImage.ScaleAbsolute(_envelopeResponse.ProductWidth.MMtoPix(), _envelopeResponse.ProductHeight.MMtoPix());
                    backImage.SetPageNumber(BackPageNumber);

                    // 坐标po
                    backImage.SetFixedPosition(
                        leftWhite + (_paperColumn - j - 1) * _envelopeResponse.ProductWidth.MMtoPix(),
                        topWhite + i * _envelopeResponse.ProductHeight.MMtoPix());
                    Document.Add(backImage);
                }
            }
        }
    }
}