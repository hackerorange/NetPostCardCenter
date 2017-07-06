using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using soho.domain;
using soho.domain.orderCenter;
using soho.web;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;
using System.Collections.Specialized;

namespace soho.webservice
{
    public static class PostCardInvoker
    {
        

        public static PostCard SubmitPostCardCropInfo(string postCardId,CropInfo cropInfo)
        {
            try
            {
                var restTemplate = new RestTemplate();
                restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());


                var nameValueCollection = new NameValueCollection()
                {
                    {"postCardId", postCardId},
                    {"cropLeft", cropInfo.leftScale.ToString()},
                    {"cropTop", cropInfo.topScale.ToString()},
                    {"cropHeight", cropInfo.heightScale.ToString()},
                    {"cropWidth", cropInfo.widthScale.ToString()},
                    {"rotation",cropInfo.rotation.ToString()}
                };
                var postForObject =restTemplate.PostForObject<BodyResponse<PostCard>>("http://localhost:8083/rest/PostCardController/submitPostCardCropInfo", nameValueCollection);

                if (postForObject.code == 200)
                {
                    return postForObject.body;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("获取明信片集合错误");
            }
        }
    }
}