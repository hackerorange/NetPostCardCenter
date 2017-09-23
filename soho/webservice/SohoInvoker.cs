using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using soho.domain;
using soho.helper;
using soho.security;
using soho.web;
using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Json;
using Spring.Rest.Client;

namespace soho.webservice
{
    public static class SohoInvoker
    {
        public delegate void SuccessDownload(FileInfo file);

        public delegate void SuccessUpload(FileInfo file);

        public delegate void HttpRequestFailure(string message);

        public delegate void SuccessGetBackStyles(List<BackStyleInfo> backStyleInfos);

        public delegate void SuccessDo(bool result);


        /// <summary>
        /// 向服务器上传文件
        /// </summary>
        /// <param name="file">要上传到服务器的文件</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>服务器返回的文件ID</returns>
        public static void Upload(FileInfo file, SuccessDo success, HttpRequestFailure failure = null)
        {
            var md5 = file.getMd5();
            IsFileExistInServer(md5, isFileExist =>
            {
                if (isFileExist)
                {
                    success(true);
                }
                else
                {
                    var restTemplate = new RestTemplate();
                    restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
                    var dictionary = new Dictionary<string, object>();
                    var entity = new HttpEntity(file);
                    dictionary.Add("file", entity);
                    restTemplate.PostForObjectAsync<StandardResponse>(RequestUtils.GetUrl("fileUploadUrl"), dictionary,
                        resp =>
                        {
                            if (resp.Error != null)
                            {
                                if (failure != null)
                                {
                                    failure(resp.Error.Message);
                                }
                            }
                            else
                            {
                                success(resp.Response.code == 200);
                            }
                        });
                }
            },failure: result =>
            {
                var restTemplate = new RestTemplate();
                restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
                var dictionary = new Dictionary<string, object>();
                var entity = new HttpEntity(file);
                dictionary.Add("file", entity);
                restTemplate.PostForObjectAsync<StandardResponse>(RequestUtils.GetUrl("fileUploadUrl"), dictionary,
                    resp =>
                    {
                        if (resp.Error != null)
                        {
                            if (failure != null)
                            {
                                failure(resp.Error.Message);
                            }
                        }
                        else
                        {
                            success(resp.Response.code == 200);
                        }
                    });
            });
        }

        public static void DownLoadFile(string fileId, bool original, SuccessDownload success)
        {
            var ff = new DirectoryInfo("D:/postCardTmpFile/");
            if (!ff.Exists)
            {
                ff.Create();
            }
            var fileInfo = new FileInfo("D:/postCardTmpFile/" + fileId + ".jpg");
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                success(fileInfo);
            }
            var webClient = new WebClient();
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                }
                else
                {
                    success(fileInfo);
                }
            };
            webClient.DownloadFileAsync(
                new Uri(RequestUtils.GetUrl("fileDownloadUrl") + "/" + fileId + "?isOriginal=" +
                        (original ? "true" : "false")),
                fileInfo.FullName, fileInfo.Name);
        }


        /// <summary>
        /// 询问服务器，文件是否存在
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>文件是否已经存在</returns>
        public static void IsFileExistInServer(string fileId, SuccessDo success, HttpRequestFailure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            restTemplate.HeadForHeadersAsync(RequestUtils.GetUrl("fileInfoUrl") + "/" + fileId, resp =>
            {
                if (resp.Error != null)
                {
                    if (failure != null)
                    {
                        failure(resp.Error.Message);
                    }
                }
                else
                {
                    var singleValue = resp.Response.GetSingleValue("isExist");
                    success(!string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE"));
                }
            });
        }

        /// <summary>
        /// 询问服务器，判断文件是否为图片文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileId)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var headForHeaders = restTemplate.HeadForHeaders(RequestUtils.GetUrl("fileInfoUrl") + "/" + fileId);
            var singleValue = headForHeaders.GetSingleValue("isImage");
            return !string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE");
        }

        public static List<PostSize> GetProductSizeTemplateList()
        {
            var list = new List<PostSize>
            {
                new PostSize
                {
                    Name = "标准尺寸",
                    Height = 100,
                    Width = 148                    
                },
                new PostSize
                {
                    Name = "大尺寸",
                    Height = 105,
                    Width = 160
                }
            };
            return list;
        }

        public static List<string> GetFrontStyleTemplateList()
        {
            return new List<string>
            {
                "A",
                "B",
                "C",
            };
        }

        public static void GetBackStyleTemplateList(SuccessGetBackStyles success, HttpRequestFailure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var url = RequestUtils.GetUrl("backStyleListUrl");
            restTemplate.GetForObjectAsync<PageResponse<BackStyleInfo>>(url, response =>
            {
                if (response.Error != null)
                {
                    if (failure != null)
                    {
                        failure(response.Error.Message);
                    }
                }
                else
                {
                    if (response.Response.code == 200 && success != null)
                    {
                        success(response.Response.page);
                    }
                    else
                    {
                        failure(response.Response.message);
                    }
                }
            });
        }

        public static void SubmitPostCardList(Order postCards, SuccessDo success, HttpRequestFailure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            if (Security.AccountSessionInfo == null)
            {
                if (failure != null) failure("此操作需要登录，当前用户没有登录");
            }

            var httpHeaders = new HttpHeaders {{"tokenId", Security.TokenId}};

            var headers = new HttpEntity(postCards, httpHeaders);

            restTemplate.PostForObjectAsync<StandardResponse>(RequestUtils.GetUrl("submitOrderUrl"), headers, res =>
            {
                if (res.Error != null)
                {
                    if (failure != null)
                    {
                        failure(res.Error.Message);
                    }
                }
                else
                {
                    if (success != null)
                    {
                        success(res.Response.code == 200);
                    }
                }
            });
        }
    }
}