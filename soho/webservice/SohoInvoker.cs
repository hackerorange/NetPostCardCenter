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
        

        private static DataContractJsonHttpMessageConverter dataContractJsonHttpMessageConverter { get; set; }

        static SohoInvoker()
        {
            dataContractJsonHttpMessageConverter = new DataContractJsonHttpMessageConverter();
        }

        /// <summary>
        /// 向服务器上传文件
        /// </summary>
        /// <param name="file">要上传到服务器的文件</param>
        /// <returns>服务器返回的文件ID</returns>
        public static void Upload(FileInfo file, SuccessDo orange, HttpRequestFailure failure = null)
        {
            var md5 = file.getMd5();
            IsFileExistInServer(md5, isFileExist =>
            {
                if (isFileExist)
                {
                    orange(true);
                }
                else
                {
                    var restTemplate = new RestTemplate("http://localhost:8089");
                    restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
                    var dictionary = new Dictionary<string, object>();
                    var entity = new HttpEntity(file);
                    dictionary.Add("file", entity);
                    restTemplate.PostForObjectAsync<StandardResponse>("file/upload", dictionary, resp =>
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
                            orange(resp.Response.code == 200);
                        }
                    });
                }
            });
        }

        public static void downLoadFile(string fileId, bool original, SuccessDownload aa)
        {
            var fileInfo = new FileInfo("D:/postCardTmpFile/" + fileId + ".jpg");
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                aa(fileInfo);
            }
            var webClient = new WebClient();
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                }
                else
                {
                    aa(fileInfo);
                }
            };
            webClient.DownloadFileAsync(
                new Uri("http://localhost:8089/file/" + fileId + "?isOriginal=" + (original ? "true" : "false")),
                fileInfo.FullName, fileInfo.Name);
        }


        /// <summary>
        /// 询问服务器，文件是否存在
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <returns>文件是否已经存在</returns>
        public static void IsFileExistInServer(string fileId, SuccessDo success, HttpRequestFailure failure = null)
        {
            var restTemplate = new RestTemplate("http://localhost:8089");
            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
            restTemplate.HeadForHeadersAsync("file/" + fileId, resp =>
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
            var restTemplate = new RestTemplate("http://localhost:8089");
            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
            var headForHeaders = restTemplate.HeadForHeaders("file/" + fileId);
            var singleValue = headForHeaders.GetSingleValue("isImage");
            return !string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE");
        }

        public static List<ProductSize> getProductSizeTemplateList()
        {
            var list = new List<ProductSize>
            {
                new ProductSize
                {
                    name = "标准尺寸",
                    productHeight = 100,
                    productWidth = 148
                },
                new ProductSize
                {
                    name = "大尺寸",
                    productHeight = 105,
                    productWidth = 160
                }
            };
            return list;
        }

        public static List<String> GetFrontStyleTemplateList()
        {
            return new List<string>
            {
                "A",
                "B",
                "C",
            };
        }

        public static void GetBackStyleTemplateList(SuccessGetBackStyles success,HttpRequestFailure failure=null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
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

        public static void SubmitPostCardList(Order postCards, SuccessDo success,HttpRequestFailure failure=null)
        {
            var restTemplate = new RestTemplate("http://localhost:8083");
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var httpHeaders = new HttpHeaders {{"tokenId", "123456"}};

            var headers = new HttpEntity(postCards, httpHeaders);
            restTemplate.PostForObjectAsync<StandardResponse>("rest/orderController", headers, res =>
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

        /// <summary>
        /// 获取请求URL
        /// </summary>
        /// <param name="category">请求分类</param>
        /// <param name="key">请求Key</param>
        /// <returns>URL</returns>
        public static string getInvokeUrl(string category, string key)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
            IDictionary<string, object> dictionary = new Dictionary<string, object>(2);
            dictionary.Add("category", Encoding.UTF8.GetBytes(category));

            var postForObject =
                restTemplate.PostForObject<BodyResponse<string>>("http://localhost:8080/invokeUrl", dictionary);
            if (postForObject.code == 200)
            {
                return postForObject.body;
            }
            throw new Exception("获取URL错误");
        }
    }
}