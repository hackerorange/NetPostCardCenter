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
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception e)
                {
                    onError(e.Message);
                    return;
                }
            }

            var tempDirectoryInfo = new DirectoryInfo(fileInfo.DirectoryName + "/" + Guid.NewGuid());
            if (!tempDirectoryInfo.Exists) tempDirectoryInfo.Create();

            var pdfDocument = new PdfDocument(new PdfWriter(new FileStream(fileInfo.FullName, FileMode.CreateNew)));

            var document = new Document(pdfDocument);
            try
            {
                pdfDocument.SetDefaultPageSize(new PageSize(envelopeResponse.PaperWidth.MMtoPix(), envelopeResponse.PaperHeight.MMtoPix()));

                var pageContexts = new List<PostCardPageContext>();
                var postCardPageContext = new PostCardPageContext(document, tempDirectoryInfo, envelopeResponse, urgent, waterMark);
                pageContexts.Add(postCardPageContext);

                foreach (var postCard in postCardResponses)
                {
                    if (postCardPageContext.IsFull())
                    {
                        postCardPageContext = new PostCardPageContext(document, tempDirectoryInfo, envelopeResponse, urgent, waterMark);
                        pageContexts.Add(postCardPageContext);
                    }

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

        private Document _document;

        private readonly bool _urgent;
        private readonly string _waterMark;


        public PostCardPageContext(Document document, DirectoryInfo templateDirectoryInfo, EnvelopeResponse envelopeResponse, bool urgent, string waterMark)
        {
            _document = document;
            _document.GetPdfDocument().AddNewPage();
            FrontPageNumber = _document.GetPdfDocument().GetNumberOfPages();
            if (envelopeResponse.DoubleSide)
            {
                _document.GetPdfDocument().AddNewPage();
                BackPageNumber = _document.GetPdfDocument().GetNumberOfPages();
            }

            _templateDirectoryInfo = templateDirectoryInfo;
            _envelopeResponse = envelopeResponse;
            _paperColumn = envelopeResponse.PaperColumn;
            _paperRow = envelopeResponse.PaperColumn;
            _urgent = urgent;
            _waterMark = waterMark;
        }


        public bool IsFull()
        {
            return _postCardResponses.Count() >= _paperColumn * _paperRow;
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
            var leftWhite = (_document.GetPdfDocument().GetDefaultPageSize().GetWidth() - _paperColumn * _envelopeResponse.ProductWidth.MMtoPix()) / 2;
            var topWhite = (_document.GetPdfDocument().GetDefaultPageSize().GetHeight() - _paperRow * _envelopeResponse.ProductHeight.MMtoPix()) / 2;

            // 添加辅助线
            var pdfCanvas = new PdfCanvas(_document.GetPdfDocument(), FrontPageNumber);
            pdfCanvas.SetLineDash(10).SetColor(new DeviceGray(1), true);


            // 画竖线
            for (var i = 0; i <= _paperColumn; i++)
                pdfCanvas
                    .MoveTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), 0)
                    .LineTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), topWhite - 5)
                    .MoveTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), _document.GetPdfDocument().GetDefaultPageSize().GetHeight())
                    .LineTo(leftWhite + i * _envelopeResponse.ProductWidth.MMtoPix(), _document.GetPdfDocument().GetDefaultPageSize().GetHeight() - topWhite + 5);

            for (var j = 0; j <= _paperRow; j++)
                pdfCanvas
                    .MoveTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: 0)
                    .LineTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: leftWhite - 5)
                    .MoveTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: _document.GetPdfDocument().GetDefaultPageSize().GetWidth())
                    .LineTo(y: topWhite + j * _envelopeResponse.ProductHeight.MMtoPix(), x: _document.GetPdfDocument().GetDefaultPageSize().GetWidth() - leftWhite + 5);


            pdfCanvas.Stroke();
            var fontPath = "";
            if (Process.GetCurrentProcess().MainModule is ProcessModule processModule) fontPath = new FileInfo(processModule.FileName).DirectoryName + @"/STKAITI.TTF";

            // 添加水印
            var pdfFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, false); // 列
            var paragraph = new Paragraph(_waterMark);
            paragraph.SetFont(pdfFont);
            paragraph.SetFontSize(8);
            paragraph.SetFontColor(_urgent ? DeviceRgb.RED : DeviceRgb.BLACK);
            paragraph.SetPageNumber(FrontPageNumber);
            paragraph.SetFixedPosition(
                leftWhite + 10,
                _document.GetPdfDocument().GetDefaultPageSize().GetHeight() - topWhite,
                _document.GetPdfDocument().GetDefaultPageSize().GetWidth() - leftWhite - 10);
            _document.Add(paragraph);
            if (_envelopeResponse.DoubleSide)
            {
                var backGroundParagraph = new Paragraph("反面样式为【" + _envelopeResponse.BackStyle + "】");
                backGroundParagraph.SetFont(pdfFont);
                backGroundParagraph.SetFontSize(8);
                backGroundParagraph.SetFontColor(DeviceRgb.BLACK);
                backGroundParagraph.SetPageNumber(BackPageNumber);
                backGroundParagraph.SetFixedPosition(
                    leftWhite + (_paperColumn - 1) * _envelopeResponse.ProductWidth.MMtoPix() + 10,
                    _document.GetPdfDocument().GetDefaultPageSize().GetHeight() - topWhite,
                    _document.GetPdfDocument().GetDefaultPageSize().GetWidth() - leftWhite - 10);
                _document.Add(backGroundParagraph);
            }

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
                    _document.Add(frontImage);
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
                    _document.Add(backImage);
                }
            }
        }
    }
}