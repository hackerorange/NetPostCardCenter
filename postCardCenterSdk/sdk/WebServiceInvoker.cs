using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using postCardCenterSdk.constant;
using postCardCenterSdk.helper;
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
        public void SubmitOrderList(OrderSubmitRequest orderSubmit, Action<string> success, Action<string> failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<string>>("/order/submit", orderSubmit, resp => resp.prepareResult(success, failure));
        }

        /// <summary>
        ///     获取反面模板列表
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void GetBackStyleTemplateList(Action<List<BackStyleResponse>> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<List<BackStyleResponse>>>("/backStyle", resp => resp.prepareResult(success, failure));


        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetSizeInfoFromServer(string category, Action<List<PostCardSizeResponse>> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<List<PostCardSizeResponse>>>("/size/{category}", new Dictionary<string, object>
            {
                {"category", category}
            }, resp => resp.prepareResult(success, failure));

        /// <summary>
        /// 向服务器插入成品尺寸信息
        /// </summary>
        /// <param name="category"></param>
        /// <param name="sizeRequest"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void InsertProductSizeToServer(string category, SizeRequest sizeRequest, Action<PostCardSizeResponse> success, Action<string> failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<PostCardSizeResponse>>("/size/{category}", sizeRequest, resp => resp.prepareResult(success, failure), category);
        }

        /// <summary>
        ///     从服务器获取正面板式
        /// </summary>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetFrontStyleTemplateList(Action<List<FrontStyleResponse>> success, Action<string> failure = null)
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
        public void DownLoadFile(string url, FileInfo fileInfo, Action<FileInfo> success, Action<int> process, Action<string> failure = null)
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
            webClient.Headers.Add("token", GlobalApiContext.Token);
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

        public void DownLoadBytesAsync(string url, Action<byte[]> success, Action<int> process, Action<string> failure = null)
        {
            var webClient = new WebClient
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable)
            };
            webClient.Headers.Add("token", GlobalApiContext.Token);
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
        public void DownLoadFileByFileId(string file, DirectoryInfo path, Action<FileInfo> success, Action<int> process = null, Action<string> failure = null)
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
        public void DownLoadFileByFileId(string fileId, FileInfo fileInfo, Action<FileInfo> success, Action<int> process = null, Action<string> failure = null)
        {
            DownLoadFile(Resources.fileDownloadUrl + "/" + fileId, fileInfo, success, process, failure);
        }


        /// <summary>
        ///     根据订单ID获取所有明信片集合
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">获取成功的响应结果</param>
        /// <param name="failure">获取失败的响应结果</param>
        public void GetAllEnvelopeByOrderId(string orderId, Action<List<EnvelopeResponse>> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<Page<EnvelopeResponse>>>("/bill/{billId}/collection/list", new Dictionary<string, object>
            {
                { "billId", orderId }
            }, resp => resp.prepareResult(kk => success?.Invoke(kk.Detail), failure));

        /// <summary>
        ///     根据明信片集合ID获取明信片集合中的所有明信片
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetPostCardByEnvelopeId(string envelopeId, Action<List<PostCardResponse>> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<Page<PostCardResponse>>>("/collection/{collectionId}/postCard/list", new Dictionary<string, object>
            {
                {"collectionId", envelopeId}
            }, resp => resp.prepareResult(kk => success?.Invoke(kk.Detail), failure));

        /// <summary>
        ///     根据ID获取明信片集合详细信息
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public void GetEnvelopeInfoById(string envelopeId, Action<EnvelopeResponse> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<EnvelopeResponse>>("/envelope/{envelopeId}/info", new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            }, resp => resp.prepareResult(success, failure));

        public void GetOrderDetails(DateTime startDate, DateTime endDate, Action<List<OrderResponse>> success, Action<string> failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<Page<OrderResponse>>>("/order/getAll", new GetAllOrderRequest
            {
                StarDateTime = startDate,
                EndDateTime = endDate
            }, postCompleted: respon => respon.prepareResult(result => success?.Invoke(result.Detail), failure));
        }

        /// <summary>
        ///     修改订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="success">成功回调</param>
        /// <param name="failure">失败回调</param>
        public void ChangeOrderStatus(string orderId, string orderStatus, Action<OrderResponse> success, Action<string> failure = null)
        {

            _restTemplate.PostForObjectAsync<BodyResponse<OrderResponse>>("/order/changeStatus", new HttpEntity(new Dictionary<string, object>
            {
                {"orderId", orderId},
                {"orderStatus", orderStatus}
            }), postCompleted: respon => respon.prepareResult(success, failure));
            //PostForObjectAsync<OrderResponse>(Resources.changeOrderStatusUrl, nameValueCollection, response => { success?.Invoke(response); }, failure);
        }

        /// <summary>
        ///     我来处理订单当前订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public OrderResponse ChangeOrderProcessor(string orderId)
        {
            try
            {
                var result = _restTemplate.PostForObject<BodyResponse<OrderResponse>>(@"/order/{orderId}/updateProcessor", null, uriVariables: orderId);

                if (result.Code >= 0)
                {
                    return result.Data;
                }
            }
            catch (Exception e)
            {
                var a = e.Message;
            }

            return null;

        }

        public void GetOrderInfo(string orderId, Action<OrderResponse> success, Action<string> failure = null) =>
            _restTemplate.GetForObjectAsync<BodyResponse<OrderResponse>>("/bill/{billId}/info", new Dictionary<string, object>
            {
                {"orderId", orderId}
            }, resp => resp.prepareResult(success, failure));

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="success">成功后的回调函数</param>
        /// <param name="failure">失败后的回调函数</param>
        public void UserLogin(string userName, string password, Action<LoginResponse> success, Action<string> failure) =>
            _restTemplate.PostForObjectAsync<BodyResponse<LoginResponse>>("/security/login", new HttpEntity(new UserLoginRequest
            {
                UserName = userName,
                Password = password
            }), result => result.prepareResult(success, failure));


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="success">成功后的回调函数</param>
        /// <param name="failure">失败后的回调函数</param>
        public void RefreshToken(string refreshToken, Action<LoginResponse> success, Action<string> failure)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<LoginResponse>>("/security/refreshToken", new HttpEntity(new HttpHeaders
            {
                { "refresh-token", refreshToken }
            }), result => result.prepareResult(success, failure));
        }

        public void SubmitPostCardProductFile(string postCardId, string productFileId, Action<bool> success = null, Action<string> failure = null) =>
            _restTemplate.PostForObjectAsync<BodyResponse<bool>>("/postCard/submitProduct", new PostCardProductFileIdSubmitRequest
            {
                PostCardId = postCardId,
                ProductFileId = productFileId
            }, result => result.prepareResult(success, failure));

        public void UpdatePostCardProcessStatus(string postCardId, string processStatus, Action<bool> success = null, Action<string> failure = null) =>
            _restTemplate.PostForObjectAsync<BodyResponse<bool>>("/postCard/{postCardId}/updateProcessStatus/{processStatusCode} ", null, new Dictionary<string, object>
            {
                { "postCardId" , postCardId },
                { "processStatusCode" , processStatus }
            }, result => result.prepareResult(success, failure));

        /// <summary>
        /// 改变明信片的正面样式
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        /// <param name="frontStyle">正面样式：A、B、C、D</param>
        /// <param name="success">成功后的回调函数</param>
        /// <param name="failure">失败了的回调函数</param>

        public void ChangePostCardFrontStyle(string postCardId, string frontStyle, Action<PostCardResponse> success, Action<string> failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<PostCardResponse>>("/postCard/{postCardId}/changeFrontStyle/{fontStyleCode}", null, postCompleted: respon => respon.prepareResult(success, failure), postCardId, frontStyle);
        }

        /// <summary>
        ///     获取明信片信息
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        /// <param name="success">成功返回信息</param>
        /// <param name="failure">失败返回信息</param>
        public void GetPostCardInfo(string postCardId, Action<PostCardResponse> success, Action<string> failure = null)
        {
            _restTemplate.GetForObjectAsync<BodyResponse<PostCardResponse>>("/postCard/{postCardId}/info", new Dictionary<string, object>
            {
                {"postCardId", postCardId}
            }, resp => resp.prepareResult(success, failure));
        }

        /// <summary>
        ///     获取明信片信息
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        //public PostCardResponse GetPostCardInfo(string postCardId)
        //{
        //    try
        //    {
        //        var result = _restTemplate.GetForObject<BodyResponse<PostCardResponse>>("/postCard/{postCardId}/info", new Dictionary<string, object>
        //    {
        //        {"postCardId", postCardId}
        //    });

        //        return result.Data;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        //frontStyle
    }
}