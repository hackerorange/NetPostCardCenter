using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
        public static string Upload(FileInfo file)
        {
            try
            {
                var md5 = file.getMd5();
                if (!IsFileExistInServer(md5))
                {
                    var restTemplate = new RestTemplate("http://localhost:8089");
                    restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
                    var dictionary = new Dictionary<string, object>();
                    var entity = new HttpEntity(file);
                    dictionary.Add("file", entity);
                    var httpResponseMessage = restTemplate.PostForObject<StandardResponse>("file/upload", dictionary);
                    if (httpResponseMessage.code != 200) return "服务器发生异常";
                    restTemplate.HeadForHeaders("file/" + md5);
                }

                return md5;
            }
            catch (Exception)
            {
                throw new IOException("网络连接异常");
            }
        }

        public static FileInfo downLoadFile(string fileId, bool original = false)
        {
            var fileInfo = new FileInfo("D:/postCardTmpFile/" + fileId + ".jpg");
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                return fileInfo;
            }
            var httpDownload =
                FileDownload.HttpDownload(
                    "http://localhost:8089/file/" + fileId + "?isOriginal=" + (original ? "true" : "false"),
                    fileInfo.FullName);
            Console.WriteLine(httpDownload == true ? @"下载成功" : @"下载失败");
            return fileInfo;
//            IDictionary<string,object> orange=new Dictionary<string, object>();
//            orange.Add("fileId",fileId);
//
//            var restTemplate = new RestTemplate("http://localhost:8089");
//            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
//            var fileStream = restTemplate.GetForObject<Stream>("file/{fileId}", orange);
        }

        /// <summary>
        /// 询问服务器，文件是否存在
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <returns>文件是否已经存在</returns>
        public static bool IsFileExistInServer(string fileId)
        {
            var restTemplate = new RestTemplate("http://localhost:8089");
            restTemplate.MessageConverters.Add(dataContractJsonHttpMessageConverter);
            var headForHeaders = restTemplate.HeadForHeaders("file/" + fileId);
            var singleValue = headForHeaders.GetSingleValue("isExist");
            return !string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE");
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

        public static List<BackStyleInfo> GetBackStyleTemplateList()
        {
            return new List<BackStyleInfo>
            {
                new BackStyleInfo {fileId = "fileId_A", name = "A"},
                new BackStyleInfo {fileId = "fileId_B", name = "B"},
                new BackStyleInfo {fileId = "fileId_C", name = "C"},
                new BackStyleInfo {fileId = "fileId_D", name = "D"},
                new BackStyleInfo {fileId = "fileId_E", name = "E"},
                new BackStyleInfo {fileId = "fileId_F", name = "F"},
                new BackStyleInfo {fileId = "fileId_G", name = "G"},
                new BackStyleInfo {fileId = "fileId_H", name = "H"},
                new BackStyleInfo {fileId = "fileId_I", name = "I"},
                new BackStyleInfo {fileId = "fileId_J", name = "J"},
                new BackStyleInfo {fileId = "fileId_K", name = "K"},
                new BackStyleInfo {fileId = "fileId_L", name = "L"},
                new BackStyleInfo {fileId = "fileId_M", name = "M"},
                new BackStyleInfo {fileId = "fileId_N", name = "N"},
                new BackStyleInfo {fileId = "fileId_O", name = "O"},
                new BackStyleInfo {fileId = "fileId_P", name = "P"},
                new BackStyleInfo {fileId = "fileId_Q", name = "Q"},
                new BackStyleInfo {fileId = "fileId_R", name = "R"},
                new BackStyleInfo {fileId = "fileId_S", name = "S"},
                new BackStyleInfo {fileId = "fileId_T", name = "T"},
                new BackStyleInfo {fileId = "fileId_U", name = "U"},
                new BackStyleInfo {fileId = "fileId_V", name = "V"},
                new BackStyleInfo {fileId = "fileId_W", name = "W"},
                new BackStyleInfo {fileId = "fileId_X", name = "X"},
                new BackStyleInfo {fileId = "fileId_Y", name = "Y"},
                new BackStyleInfo {fileId = "fileId_Z", name = "Z"},
            };
        }

        public static bool SubmitPostCardList(List<Order> postCards)
        {
            try
            {
                var restTemplate = new RestTemplate("http://localhost:8083");
                restTemplate.MessageConverters.Add(new DataContractJsonHttpMessageConverter());

                var httpHeaders = new HttpHeaders {{"tokenId", "123456"}};

                var headers = new HttpEntity(postCards, httpHeaders);
                var postForObject = restTemplate.PostForObject<StandardResponse>("rest/orderController", headers);
                Console.WriteLine(postForObject.message);
                return postForObject.code == 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"无法连接到服务器");
                return false;
            }
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