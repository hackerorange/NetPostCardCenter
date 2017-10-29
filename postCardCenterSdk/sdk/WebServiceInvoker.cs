using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using postCardCenterSdk.interceptor;
using postCardCenterSdk.Properties;
using postCardCenterSdk.request.order;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.request.system;
using postCardCenterSdk.response;
using postCardCenterSdk.response.envelope;
using postCardCenterSdk.response.file;
using postCardCenterSdk.response.postCard;
using postCardCenterSdk.response.security;
using postCardCenterSdk.response.system;
using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace postCardCenterSdk.sdk
{
    public class WebServiceInvoker
    {
        /// <summary>
        ///     请求失败
        /// </summary>
        /// <param name="message">失败返回的消息</param>
        public delegate void Failure(string message);

        /// <summary>
        ///     请求成功
        /// </summary>
        /// <typeparam name="T">成功返回的结果类型</typeparam>
        /// <param name="backStyleInfos">成功返回的结果</param>
        public delegate void Success<in T>(T backStyleInfos);

        /// <summary>
        ///     请求成功
        /// </summary>
        /// <typeparam name="T">成功列表的结果类型</typeparam>
        public delegate void SuccessList<T>(List<T> resultList);

        public static string Token { get; set; }

        /// <summary>
        ///     提交订单
        /// </summary>
        /// <param name="orderSubmit">要提交的订单集合</param>
        /// <param name="success">成功返回的结果</param>
        /// <param name="failure">失败处理逻辑</param>
        public static void SubmitOrderList(OrderSubmitRequest orderSubmit, Success<string> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            restTemplate.PostForObjectAsync<BodyResponse<object>>(Resources.postCardSubmitUrl, orderSubmit, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }
                if (res.Response.Code > 0)
                    success?.Invoke(res.Response.Message);
                else
                    failure?.Invoke(res.Response.Message);
            });
        }

        /// <summary>
        ///     获取反面模板列表
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public static void GetBackStyleTemplateList(Success<List<BackStyleResponse>> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            restTemplate.GetForObjectAsync<BodyResponse<Page<BackStyleResponse>>>(Resources.backStyleListUrl, response =>
            {
                if (response.Error != null)
                {
                    failure?.Invoke(response.Error.Message);
                }
                else
                {
                    if (response.Response.Code > 0)
                        success?.Invoke(response.Response.Data.Detail);
                    else
                        failure?.Invoke(response.Response.Message);
                }
            });
        }

        /// <summary>
        ///     从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="category">尺寸所属分类</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetSizeInfoFromServer(string category, Success<List<PostCardSizeResponse>> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            restTemplate.GetForObjectAsync<BodyResponse<Page<PostCardSizeResponse>>>(Resources.getAllProductSizeUrl, response =>
            {
                if (response.Error != null)
                {
                    failure?.Invoke(response.Error.Message);
                }
                else
                {
                    if (response.Response.Code > 0)
                        success?.Invoke(response.Response.Data.Detail);
                    else
                        failure?.Invoke(response.Response.Message);
                }
            }, category);
        }

        public static void InsertProductSizeToServer(string category, SizeRequest sizeRequest, Success<PostCardSizeResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            restTemplate.PostForObjectAsync<BodyResponse<PostCardSizeResponse>>(Resources.insertSizeUrl, sizeRequest, response =>
            {
                if (response.Error != null)
                {
                    failure?.Invoke(response.Error.Message);
                }
                else
                {
                    if (response.Response.Code > 0)
                        success?.Invoke(response.Response.Data);
                    else
                        failure?.Invoke(response.Response.Message);
                }
            }, category);
        }

        /// <summary>
        ///     从服务器获取正面板式
        /// </summary>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetFrontStyleTemplateList(Success<List<FrontStyleResponse>> success, Failure failure = null)
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
        public static void DownLoadFile(string url, FileInfo fileInfo, Success<FileInfo> success, Success<int> process, Failure failure = null)
        {
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            ;
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                success?.Invoke(fileInfo);
                return;
            }
            var webClient = new WebClient();
            webClient.Headers.Add("token", Token);
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

        /// <summary>
        ///     根据文件ID，下载到指定路径
        /// </summary>
        /// <param name="file">要下载的文件ID</param>
        /// <param name="path">下载文件后的目标文件夹路径信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string file, DirectoryInfo path, Success<FileInfo> success, Success<int> process = null, Failure failure = null)
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
        public static void DownLoadFileByFileId(string fileId, FileInfo fileInfo, Success<FileInfo> success, Success<int> process = null, Failure failure = null)
        {
            DownLoadFile(Resources.fileDownloadUrl + "/" + fileId, fileInfo, success, process, failure);
        }

        /// <summary>
        ///     询问服务器，判断文件是否为图片文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileId)
        {
            var restTemplate = PrepareRestTemplate();
            var headForHeaders = restTemplate.HeadForHeaders(Resources.fileInfoUrl + "/" + fileId);
            var singleValue = headForHeaders.GetSingleValue("isImage");
            return !string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE");
        }

        /// <summary>
        ///     询问服务器，文件是否存在
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>文件是否已经存在</returns>
        public static void IsFileExistInServer(string fileId, Success<bool> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            restTemplate.HeadForHeadersAsync(Resources.fileInfoUrl + "/" + fileId, resp =>
            {
                if (resp.Error != null)
                {
                    failure?.Invoke(resp.Error.Message);
                }
                else
                {
                    var singleValue = resp.Response.GetSingleValue("isExist");
                    success?.Invoke(!string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE"));
                }
            });
        }


        /// <summary>
        ///     根据文件ID生成文件缩略图
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>文件是否已经存在</returns>
        public static void GetThumbnailFileId(string fileId, Success<FileUploadResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"fileId", fileId}
            };
            restTemplate.GetForObjectAsync<BodyResponse<FileUploadResponse>>(Resources.getFileThumbnailUrl, nameValueCollection, resp =>
            {
                if (resp.Error != null)
                {
                    failure?.Invoke(resp.Error.Message);
                }
                else
                {
                    if (resp.Response.Code < 0)
                        failure?.Invoke(resp.Response.Message);
                    else
                        success?.Invoke(resp.Response.Data);
                }
            });
        }


        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="file">要上传的文件</param>
        /// <param name="success">上传成功的回调函数</param>
        /// <param name="failure">上传失败的回调函数</param>
        public static void Upload(string category, FileInfo file, Success<FileUploadResponse> success, Failure failure)
        {
            var restTemplate = PrepareRestTemplate();
            var dictionary = new Dictionary<string, object>();
            var entity = new HttpEntity(file);
            dictionary.Add("file", entity);
            dictionary.Add("category", Encoding.UTF8.GetBytes(category));
            restTemplate.PostForObjectAsync<BodyResponse<FileUploadResponse>>(Resources.fileUploadUrl, dictionary,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                        return;
                    }
                    if (resp.Response.Code > 0)
                    {
                        success?.Invoke(resp.Response.Data);
                        return;
                    }
                    failure?.Invoke(resp.Response.Message);
                });
        }


        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="file">要上传的文件</param>
        public static FileUploadResponse Upload(string category, FileInfo file)
        {
            var restTemplate = PrepareRestTemplate();
            var dictionary = new Dictionary<string, object>();
            var entity = new HttpEntity(file);
            dictionary.Add("file", entity);
            dictionary.Add("category", Encoding.UTF8.GetBytes(category));
            var postForObject = restTemplate.PostForObject<BodyResponse<FileUploadResponse>>(Resources.fileUploadUrl, dictionary);
            if (postForObject.Code > 0)
                return postForObject.Data;
            throw new Exception(postForObject.Message);
        }


        /// <summary>
        ///     根据订单ID获取所有明信片集合
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">获取成功的响应结果</param>
        /// <param name="failure">获取失败的响应结果</param>
        public static void GetAllEnvelopeByOrderId(string orderId, Success<List<EnvelopeResponse>> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };

            restTemplate.GetForObjectAsync<BodyResponse<Page<EnvelopeResponse>>>(Resources.getAllEnvelopeByOrderIdUrl, nameValueCollection,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                    }
                    else
                    {
                        if (resp.Response.Code > 0)
                            success?.Invoke(resp.Response.Data.Detail);
                        else
                            failure?.Invoke(resp.Response.Message);
                    }
                });
        }

        /// <summary>
        ///     根据明信片集合ID获取明信片集合中的所有明信片
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetPostCardByEnvelopeId(string envelopeId, Success<List<PostCardResponse>> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };

            restTemplate.GetForObjectAsync<BodyResponse<Page<PostCardResponse>>>(
                Resources.getPostCardByEnvelopeIdUrl, nameValueCollection,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                        return;
                    }
                    if (resp.Response.Code < 0)
                    {
                        failure?.Invoke(resp.Response.Message);
                        return;
                    }
                    success?.Invoke(resp.Response.Data.Detail);
                });
        }

        /// <summary>
        ///     根据ID获取明信片集合详细信息
        /// </summary>
        /// <param name="envelopeId">明信片集合ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetEnvelopeInfoById(string envelopeId, Success<EnvelopeResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            var nameValueCollection = new Dictionary<string, object>
            {
                {"envelopeId", envelopeId}
            };
            restTemplate.GetForObjectAsync<BodyResponse<EnvelopeResponse>>(
                Resources.envelopeInfoUrl, nameValueCollection, headCompleted =>
                {
                    if (headCompleted.Error != null)
                    {
                        failure?.Invoke(headCompleted.Error.Message);
                    }
                    else
                    {
                        if (headCompleted.Response.Code < 0)
                        {
                            failure?.Invoke(headCompleted.Response.Message);
                            return;
                        }
                        success?.Invoke(headCompleted.Response.Data);
                    }
                });
        }

        public static void GetOrderDetails(DateTime startDate, DateTime endDate, Success<List<OrderResponse>> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss")},
                {"endDate", endDate.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            restTemplate.PostForObjectAsync<BodyResponse<Page<OrderResponse>>>(Resources.getAllOrderUrl, nameValueCollection, respon =>
            {
                if (respon.Error != null)
                {
                    failure?.Invoke(respon.Error.Message);
                    return;
                }
                if (respon.Response.Code > 0)
                    success?.Invoke(respon.Response.Data.Detail);
                else
                    failure?.Invoke(respon.Response.Message);
            });
        }

        /// <summary>
        ///     修改订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="success">成功回调</param>
        /// <param name="failure">失败回调</param>
        public static void ChangeOrderStatus(string orderId, string orderStatus, Success<OrderResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId},
                {"orderStatus", orderStatus}
            };

            restTemplate.PostForObjectAsync<BodyResponse<OrderResponse>>(Resources.changeOrderStatusUrl, nameValueCollection, response =>
            {
                if (response.Error != null)
                    failure?.Invoke(response.Error.Message);
                else
                    success?.Invoke(response.Response.Data);
            });
        }

        /// <summary>
        ///     我来处理订单当前订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void ChangeOrderProcessor(string orderId, Success<OrderResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };
            restTemplate.PostForObjectAsync<BodyResponse<OrderResponse>>(Resources.changeOrderProcessorUrl, nameValueCollection, response =>
            {
                if (response.Error != null)
                {
                    failure?.Invoke(response.Error.Message);
                    return;
                }
                if (response.Response.Code > 0)
                {
                    success?.Invoke(response.Response.Data);
                    return;
                }
                failure?.Invoke(response.Response.Message);
            });
        }

        public static void GetOrderInfo(string orderId, Success<OrderResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var nameValueCollection = new Dictionary<string, object>
            {
                {"orderId", orderId}
            };

            restTemplate.GetForObjectAsync<BodyResponse<OrderResponse>>(Resources.getOrderInfoUrl, nameValueCollection, respon =>
            {
                if (respon.Error != null)
                    failure?.Invoke(respon.Error.Message);
                else if (respon.Response.Code > 0)
                    success?.Invoke(respon.Response.Data);
                else
                    failure?.Invoke(respon.Response.Message);
            });
        }


        public static void UserLogin(string userName, string password, Success<LoginResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            var nameValueCollection = new NameValueCollection
            {
                {"userName", userName},
                {"password", password}
            };

            var httpHeaders = new HttpHeaders {{"tokenId", "123456"}};

            var headers = new HttpEntity(nameValueCollection, httpHeaders);


            restTemplate.PostForObjectAsync<BodyResponse<LoginResponse>>(Resources.loginUrl, headers, resp =>
            {
                if (resp.Error != null)
                {
                    failure?.Invoke(resp.Error.Message);
                    return;
                }
                if (resp.Response.Code > 0)
                {
                    Token = resp.Response.Data.Token;
                    success?.Invoke(resp.Response.Data);
                    return;
                }
                failure?.Invoke(resp.Response.Message);
            });
        }


        public static void SubmitPostCardCropInfo(CropSubmitRequest request, Success<PostCardResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();

            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            restTemplate.PostForObjectAsync<BodyResponse<PostCardResponse>>(Resources.cropInfoSubmitUrl, request, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }
                if (res.Response.Code > 0)
                    success?.Invoke(res.Response.Data);
                else
                    failure?.Invoke(res.Response.Message);
            });
        }

        public static void ChangePostCardFrontStyle(PostCardInfoPatchRequest frontStyle, Success<PostCardResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            var httpEntity = new HttpEntity(frontStyle);

            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            restTemplate.ExchangeAsync<BodyResponse<PostCardResponse>>(Resources.patchPostCardInfoUrl, new HttpMethod("PATCH"), httpEntity, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }
                if (res.Response.Body.Code > 0)
                    success?.Invoke(res.Response.Body.Data);
                else
                    failure?.Invoke(res.Response.Body.Message);
            });
        }

        /// <summary>
        ///     获取明信片信息
        /// </summary>
        /// <param name="postCardId">明信片ID</param>
        /// <param name="success">成功返回信息</param>
        /// <param name="failure">失败返回信息</param>
        public static void GetPostCardInfo(string postCardId, Success<PostCardResponse> success, Failure failure = null)
        {
            var restTemplate = PrepareRestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());


            var nameValueCollection = new Dictionary<string, object>
            {
                {"postCardId", postCardId}
            };


            restTemplate.GetForObjectAsync<BodyResponse<PostCardResponse>>(Resources.getPostCardInfoUrl, nameValueCollection, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }
                if (res.Response.Code > 0)
                    success?.Invoke(res.Response.Data);
                else
                    failure?.Invoke(res.Response.Message);
            });
        }
        //frontStyle

        public static RestTemplate PrepareRestTemplate()
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            restTemplate.RequestInterceptors.Add(new PerfRequestSyncInterceptor());
            return restTemplate;
        }
    }
}