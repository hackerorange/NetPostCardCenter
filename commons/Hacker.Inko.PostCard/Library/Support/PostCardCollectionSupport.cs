using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Api.Collection;
using Hacker.Inko.Net.Response.envelope;
using Hacker.Inko.Net.Response.postCard;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


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

        public static void GeneratePdfFile(this EnvelopeResponse envelopeResponse, string fileFullName,
            Action<double> processHandler = null, Action<string> onError = null)
        {
            var current = new ProgressInternal(0.0, 0.3);
            // 从服务器获取明信片,并安装份数进行划分
            var postCardResponses = PreparePostCardFromServer(envelopeResponse, value => processHandler?.Invoke(current.GetProcessRealValue(value)), onError);
            // 生成PDF
            GeneratePdfFileByPostCardList(envelopeResponse, postCardResponses, fileFullName, processHandler: value => processHandler?.Invoke(new ProgressInternal(0.3, 1).GetProcessRealValue(value)));
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
                    for (var i = 0; i < postCard.Copy; i++)
                    {
                        postCardResponses.Add(postCard);
                    }
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


        private static void GeneratePdfFileByPostCardList(EnvelopeResponse envelopeResponse, IEnumerable<PostCardResponse> postCardResponses, string fileFullName, Action<double> processHandler)
        {
            var fileInfo = new FileInfo(fileFullName);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            var tempDirectoryInfo = new DirectoryInfo(fileInfo.DirectoryName + "/" + Guid.NewGuid());
            if (!tempDirectoryInfo.Exists)
            {
                tempDirectoryInfo.Create();
            }

            var pdfDocument = new PdfDocument(new PdfWriter(new FileStream(fileInfo.FullName, FileMode.CreateNew)));

            var document = new Document(pdfDocument);
            try
            {
                pdfDocument.SetDefaultPageSize(new PageSize(envelopeResponse.PaperWidth.MMtoPix(), envelopeResponse.PaperHeight.MMtoPix()));

                var pageContexts = new List<PostCardPageContext>();

                pdfDocument.AddNewPage();
                var postCardPageContext = new PostCardPageContext(tempDirectoryInfo, envelopeResponse)
                {
                    FrontPageNumber = pdfDocument.GetNumberOfPages(),
                    BackPageNumber = pdfDocument.GetNumberOfPages() + 1,
                    Document = document
                };
                pdfDocument.AddNewPage();

                pageContexts.Add(postCardPageContext);

                foreach (var postCard in postCardResponses)
                {
                    if (!postCardPageContext.AddPostCardResponse(postCard))
                    {
                        pdfDocument.AddNewPage();
                        postCardPageContext = new PostCardPageContext(tempDirectoryInfo, envelopeResponse)
                        {
                            FrontPageNumber = pdfDocument.GetNumberOfPages(),
                            BackPageNumber = pdfDocument.GetNumberOfPages() + 1,
                            Document = document
                        };
                        pdfDocument.AddNewPage();
                        pageContexts.Add(postCardPageContext);
                    }
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
                tempDirectoryInfo.Delete(true);
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


        public PostCardPageContext(DirectoryInfo templateDirectoryInfo, EnvelopeResponse envelopeResponse)
        {
            _templateDirectoryInfo = templateDirectoryInfo;
            _envelopeResponse = envelopeResponse;
            _paperColumn = envelopeResponse.PaperColumn;
            _paperRow = envelopeResponse.PaperColumn;
        }

        public bool AddPostCardResponse(PostCardResponse postCardResponse)
        {
            if (_postCardResponses.Count() >= _paperColumn * _paperRow)
            {
                return false;
            }

            _postCardResponses.Add(postCardResponse);
            return true;
        }


        public void PreparePdfPage()
        {
            ProcessFrontPage();
            ProcessBackPage();
        }

        /// <summary>
        /// 处理正面
        /// </summary>
        private void ProcessFrontPage()
        {
            // 创建新的一页

            var leftWhite = (Document.GetPdfDocument().GetDefaultPageSize().GetWidth() - _paperColumn * _envelopeResponse.ProductWidth.MMtoPix()) / 2;
            var topWhite = (Document.GetPdfDocument().GetDefaultPageSize().GetHeight() - _paperRow * _envelopeResponse.ProductHeight.MMtoPix()) / 2;

            // 正面
            // 行
            for (var i = 0; i < _paperRow; i++)
            {
                // 列
                for (var j = 0; j < _paperColumn; j++)
                {
                    var currentIndex = i * _paperRow + j;
                    if (currentIndex >= _postCardResponses.Count)
                    {
                        continue;
                    }

                    var postCard = _postCardResponses[currentIndex];
                    var fileId = postCard.ProductFileId;
                    var templateFileInfo = new FileInfo(_templateDirectoryInfo.FullName + "/" + fileId);
                    if (!templateFileInfo.Exists)
                    {
                        var bytes = FileApi.DownloadBytesByFileId(fileId);

                        var fileStream = new BufferedStream(new FileStream(templateFileInfo.FullName, FileMode.CreateNew));
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }

                    // 正面
                    var image = new Image(ImageDataFactory.Create(templateFileInfo.FullName));
                    // 转化尺寸
                    image.ScaleAbsolute(_envelopeResponse.ProductWidth.MMtoPix(), _envelopeResponse.ProductHeight.MMtoPix());
                    image.SetPageNumber(FrontPageNumber);

                    // 坐标po
                    image.SetFixedPosition(
                        leftWhite + j * _envelopeResponse.ProductWidth.MMtoPix(),
                        topWhite + i * _envelopeResponse.ProductHeight.MMtoPix());
                    Document.Add(image);
                }
            }
        }

        /// <summary>
        /// 处理反面
        /// </summary>
        private void ProcessBackPage()
        {
        }
    }
}