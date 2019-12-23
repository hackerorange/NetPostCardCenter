using iText.IO.Image;
using iText.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using postCardCenterSdk.helper;
using postCardCenterSdk.response.envelope;
using postCardCenterSdk.response.postCard;
using postCardCenterSdk.sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Layout.Element;

namespace Hacker.Inko.PostCard.Support
{
    public static class PostCardCollectionSupport
    {

        private class ProgressInternal
        {
            double _start;
            double _end;

            public ProgressInternal(double start, double end)
            {
                this._start = start;
                this._end = end;
            }

            public double GetProcessRealValue(double processValue)
            {
                return (_end - _start) * processValue + _start;
            }

        }

        public static void GeneratePdfFile(this EnvelopeResponse envelopeResponse, string fileFullName, Action<double> processHandler = null, Action<string> onError = null)
        {
            var current = new ProgressInternal(0.0, 0.3);
            // 从服务器获取明信片,并安装份数进行划分
            var postCardResponses = preparePostCardFromServer(envelopeResponse, value => processHandler?.Invoke(current.GetProcessRealValue(value)), onError);
            // 生成PDF
            GeneratePdfFileByPostCardList(envelopeResponse, postCardResponses, fileFullName, value => processHandler?.Invoke(new ProgressInternal(0.3, 1).GetProcessRealValue(value)));
        }


        private static List<PostCardResponse> preparePostCardFromServer(EnvelopeResponse envelopeResponse, Action<double> processHandler = null, Action<string> onError = null)
        {
            var postCardResponses = new List<PostCardResponse>();
            bool isWait = true;
            WebServiceInvoker.GetInstance().GetPostCardByEnvelopeId(envelopeResponse.EnvelopeId, postCardList =>
            {
                for (int i1 = 0; i1 < postCardList.Count; i1++)
                {
                    processHandler?.Invoke(i1 / (double)postCardList.Count);
                    PostCardResponse postCard = postCardList[i1];
                    for (var i = 0; i < postCard.Copy; i++)
                    {
                        postCardResponses.Add(postCard);
                    }
                }
                isWait = false;

            }, failure: message =>
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



        private static void GeneratePdfFileByPostCardList(EnvelopeResponse envelopeResponse, List<PostCardResponse> postCardResponses, string fileFullName, Action<double> processHandler)
        {

            try
            {
                var fileInfo = new FileInfo(fileFullName);

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                var pdfDocument = new PdfDocument(new PdfWriter(new FileStream(fileInfo.FullName, FileMode.CreateNew)));

                var document = new Document(pdfDocument);
                try
                {
                    pdfDocument.SetDefaultPageSize(new PageSize(464.MMtoPix(), 320.MMtoPix()));

                    var pageContexts = new List<PostCardPageContext>();

                    pdfDocument.AddNewPage();
                    var postCardPageContext = new PostCardPageContext(envelopeResponse)
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
                            postCardPageContext = new PostCardPageContext(envelopeResponse)
                            {
                                FrontPageNumber = pdfDocument.GetNumberOfPages(),
                                BackPageNumber = pdfDocument.GetNumberOfPages() + 1,
                                Document = document
                            };
                            pdfDocument.AddNewPage();
                            pageContexts.Add(postCardPageContext);
                        }
                    }

                    for (int i = 0; i < pageContexts.Count; i++)
                    {
                        processHandler?.Invoke(i / (double)pageContexts.Count);
                        PostCardPageContext page = pageContexts[i];
                        page.PreparePdfPage();
                    }
                    processHandler?.Invoke(1);
                }
                finally
                {
                    document.Flush();
                    document.Close();
                }
            }
            //catch (Exception e)
            //{
            //    return null;
            //}
            finally
            {
            }
        }
    }

    class PostCardPageContext
    {
        private readonly int _paperColumn;
        private readonly int _paperRow;
        private readonly List<PostCardResponse> _postCardResponses = new List<PostCardResponse>();
        private readonly EnvelopeResponse _envelopeResponse;
        private static readonly DirectoryInfo templateDir = new DirectoryInfo(@"D:\postCard\tmpFile\generatePdf\");
        public int FrontPageNumber { get; set; }
        public int BackPageNumber { get; set; }

        public Document Document { get; set; }


        public PostCardPageContext(EnvelopeResponse _envelopeResponse)
        {
            this._envelopeResponse = _envelopeResponse;
            _paperColumn = _envelopeResponse.PaperColumn;
            _paperRow = _envelopeResponse.PaperColumn;

            if (!templateDir.Exists)
            {
                templateDir.Create();
            }
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
        /// <param name="pdfPage"></param>
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
                    var bytes = WebServiceInvoker.GetInstance().DownloadBytesByFileId(fileId);
                    // 正面
                    var image = new Image(ImageDataFactory.Create(bytes));
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
        /// <param name="document"></param>
        private void ProcessBackPage()
        {
        }
    }
}
