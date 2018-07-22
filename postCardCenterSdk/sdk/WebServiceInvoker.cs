using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using postCardCenterSdk.Properties;
using postCardCenterSdk.request.order;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.request.security;
using postCardCenterSdk.request.system;
using postCardCenterSdk.response;
using postCardCenterSdk.response.envelope;
using postCardCenterSdk.response.postCard;
using postCardCenterSdk.response.security;
using postCardCenterSdk.response.system;
using Spring.Http;
using Spring.Rest.Client;

namespace postCardCenterSdk.sdk
{
    public class WebServiceInvoker : BaseApi
    {
        private static WebServiceInvoker _webServiceInvoker;

        public static WebServiceInvoker GetInstance()
        {
            return _webServiceInvoker ?? (_webServiceInvoker = new WebServiceInvoker());
        }

        private WebServiceInvoker()
        {

        }

        /// <summary>
        ///     提交订单
        /// </summary>
        /// <param name="orderSubmit">要提交的订单集合</param>
        /// <param name="success">成功返回的结果</param>
        /// <param name="failure">失败处理逻辑</param>
        public void SubmitOrderList(OrderSubmitRequest orderSubmit, Success<string> success, Failure failure = null)
        {
            PostForObjectAsync(Resources.postCardSubmitUrl, orderSubmit, success, failure);
        }

        /// <summary>
        ///     获取反面模板列表
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void GetBackStyleTemplateList(Success<List<BackStyleResponse>> success, Failure failure = null)
        {
            GetForObjectAsync(Resources.backStyleListUrl, null, success, failure);
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetSizeInfoFromServer(string category, Success<List<PostCardSizeResponse>> success, Failure failure = null)
        {
            var request = new Dictionary<string, object>
            {
                {"category", category}
            };

            GetForObjectAsync<Page<PostCardSizeResponse>>(Resources.getAllProductSizeUrl, request, result => { success(result.Detail); }, failure);
        }

        /// <summary>
        /// 向服务器插入成品尺寸信息
        /// </summary>
        /// <param name="category"></param>
        /// <param name="sizeRequest"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void InsertProductSizeToServer(string category, SizeRequest sizeRequest, Success<PostCardSizeResponse> success, Failure failure = null)
        {
            var url = Resources.insertSizeUrl.Replace("{category}", category);
            PostForObjectAsync(url, sizeRequest, success, failure);
        }

        /// <summary>
        ///     从服务器获取正面板式
        /// </summary>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetFrontStyleTemplateList(Success<List<FrontStyleResponse>> success, Failure failure = null)
        {
            var a = new List<FrontStyleResponse>
            {
                new FrontStyleResponse {Name = "A"},
                new FrontStyleResponse {Name = "B"},
                new FrontStyleResponse {Name = "C"}
            };
            success?.Invoke(a);
        }

        /// <summary>
        ///     异步下载文件
        /// </summary>
        /// <param name="url">文件所在路径URL</param>
        /// <param name="fileInfo">文件下载路径信息</param>
        /// <param name="success">下载成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">下载失败响应信息</param>
        public void DownLoadFile(string url, FileInfo fileInfo, Success<FileInfo> success, Success<int> process, Failure failure = null)
        {
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                success?.Invoke(fileInfo);
                return;
            }

            var webClient = new WebClient();
            webClient.Headers.Add("token", SecurityInfo.Token);
            //下载完成
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    failure?.Invoke(e.Error.Message);
                else
                    success?.Invoke(fileInfo);
            };

            // 进度条
            webClient.DownloadProgressChanged += (sender, e) => { process?.Invoke(e.ProgressPercentage); };
            // 异步下载文件
            webClient.DownloadFileAsync(new Uri(url), fileInfo.FullName, fileInfo.Name);
        }

        public void DownLoadBytesAsync(string url, Success<byte[]> success, Success<int> process, Failure failure = null)
        {
            var webClient = new WebClient
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable)
            };
            webClient.Headers.Add("token", SecurityInfo.Token);
            //下载完成
            webClient.DownloadDataCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    failure?.Invoke(e.Error.Message);
                else
                    success?.Invoke(e.Result);
            };


            // 进度条
            webClient.DownloadProgressChanged += (sender, e) => { process?.Invoke(e.ProgressPercentage); };
            // 异步下载文件
            webClient.DownloadDataAsync(new Uri(url));
        }

        /// <summary>
        ///     根据文件ID，下载到指定路径
        /// </summary>
        /// <param name="file">要下载的文件ID</param>
        /// <param name="path">下载文件后的目标文件夹路径信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public void DownLoadFileByFileId(string file, DirectoryInfo path, Success<FileInfo> success, Success<int> process = null, Failure failure = null)
        {
            DownLoadFileByFileId(file, new FileInfo(path.FullName + "/" + file), success, process, failure);
        }

        /// <summary>
        ///     根据文件ID，下载到指定文件路径
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileInfo">下载文件后的目标文件信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public void DownLoadFileByFileId(string fileId, FileInfo fileInfo, Success<FileInfo> success, Success<int> process = null, Failure failure = null)
        {
            DownLoadFile(Resources.fileDownloadUrl + "/" + fileId, fileInfo, success, process, failure);
        }

       

        /// <summary>
        ///     根据订单ID获取所有明信片集合
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">获取成功的响应结果</param>
        /// <param name="failure">获取失败的响应结果</param>
        public void GetAllEnvelopeByOrderId(string orderId, Success<List<EnvelopeResponse>> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            GetForObjectAsync<Page<EnvelopeResponse>>(Resources.getAllEnvelopeByOrderIdUrl, nameValueCollection, result => { success?.Invoke(result.Detail); }, failure);
        }

        /// <summary>
        ///     根据明信片集合ID获取明信片集合中的所有明信片
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetPostCardByEnvelopeId(string envelopeId, Success<List<PostCardResponse>> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };
            GetForObjectAsync<Page<PostCardResponse>>(Resources.getPostCardByEnvelopeIdUrl, nameValueCollection, resp => { success?.Invoke(resp.Detail); }, failure);
        }

        /// <summary>
        ///     根据ID获取明信片集合详细信息
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetEnvelopeInfoById(string envelopeId, Success<EnvelopeResponse> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };
            GetForObjectAsync<EnvelopeResponse>(Resources.envelopeInfoUrl, nameValueCollection, headCompleted => { success?.Invoke(headCompleted); }, failure);
        }

        public void GetOrderDetails(DateTime startDate, DateTime endDate, Success<List<OrderResponse>> success, Failure failure = null)
        {

            var request = new GetAllOrderRequest
            {
                StarDateTime = startDate,
                EndDateTime = endDate
            };

            PostForObjectAsync<Page<OrderResponse>>(Resources.getAllOrderUrl, request, respon => { success?.Invoke(respon.Detail); }, failure);
        }

        /// <summary>
        ///     修改订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="success">成功回调</param>
        /// <param name="failure">失败回调</param>
        public void ChangeOrderStatus(string orderId, string orderStatus, Success<OrderResponse> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId},
                {"orderStatus", orderStatus}
            };

            PostForObjectAsync<OrderResponse>(Resources.changeOrderStatusUrl, nameValueCollection, response => { success?.Invoke(response); }, failure);
        }

        /// <summary>
        ///     我来处理订单当前订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void ChangeOrderProcessor(string orderId, Success<OrderResponse> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            PostForObjectAsync<OrderResponse>(Resources.changeOrderProcessorUrl, nameValueCollection, response => { success?.Invoke(response); }, failure);
        }

        public void GetOrderInfo(string orderId, Success<OrderResponse> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            GetForObjectAsync<OrderResponse>(Resources.getOrderInfoUrl, nameValueCollection, respon => { success?.Invoke(respon); }, failure);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="success">成功后的回调函数</param>
        /// <param name="failure">失败后的回调函数</param>
        public void UserLogin(string userName, string password, Success<LoginResponse> success, Failure failure = null)
        {
            var userLoginRequest = new UserLoginRequest
            {
                UserName = userName,
                Password = password
            };

            var httpHeaders = new HttpHeaders {{"tokenId", "123456"}};

            var headers = new HttpEntity(userLoginRequest, httpHeaders);

            PostForObjectAsync<LoginResponse>(Resources.loginUrl, headers, resp => { success?.Invoke(resp); }, failure);
        }

        public RestOperationCanceler SubmitPostCardProductFile(string postCardId, string productFile, Success success=null, Failure failure=null)
        {
            var request = new PostCardProductFileIdSubmitRequest
            {
                PostCardId = postCardId,
                ProductFileId = productFile
            };
            return PostForObjectAsync(Resources.productFileSubmitUrl, request, () => { success?.Invoke(); }, failure);
        }

        public void ChangePostCardFrontStyle(string postCardId, string frontStyle, Success<PostCardResponse> success, Failure failure = null)
        {
            var url = Resources.changePostCardFrontStyle;
            var postCardFrontStyleUpdate = new PostCardFrontStyleUpdateRequest
            {
                FrontStyle = frontStyle,
                PostCardId = postCardId
            };
            url = url.Replace("{postcardId}", postCardId);
            PostForObjectAsync<PostCardResponse>(url, postCardFrontStyleUpdate, (pos) => { success?.Invoke(pos); }, failure);
        }

        /// <summary>
        ///     获取明信片信息
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        /// <param name="success">成功返回信息</param>
        /// <param name="failure">失败返回信息</param>
        public void GetPostCardInfo(string postCardId, Success<PostCardResponse> success, Failure failure = null)
        {
            var nameValueCollection = new Dictionary<string, object>
            {
                {"fileId", postCardId}
            };
            GetForObjectAsync<PostCardResponse>(Resources.getPostCardInfoUrl, nameValueCollection, res => { success?.Invoke(res); }, failure);
        }


        /// <summary>
        ///     获取明信片信息
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        public PostCardResponse GetPostCardInfo(string postCardId)
        {
            PostCardResponse postCardResponse = null;
            var nameValueCollection = new Dictionary<string, object>
            {
                {"fileId", postCardId}
            };
            GetForObject<PostCardResponse>(Resources.getPostCardInfoUrl, nameValueCollection, res =>
            {
                postCardResponse = res;
                
            });
            return postCardResponse;
        }
        //frontStyle
    }
}