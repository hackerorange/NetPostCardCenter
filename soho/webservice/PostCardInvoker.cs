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
using System.Globalization;
using soho.helper;

namespace soho.webservice
{
    public static class PostCardInvoker
    {
        public delegate void Failure(string message);

        public delegate void SuccessGetObject(PostCard backStyleInfos);

        public static void SubmitPostCardCropInfo(string postCardId, CropInfo cropInfo, SuccessGetObject success,
            Failure failure = null)
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
                {"rotation", cropInfo.rotation.ToString()}
            };
            restTemplate.PostForObjectAsync<BodyResponse<PostCard>>(
                RequestUtils.GetUrl("cropInfoSubmitUrl"),
                nameValueCollection,
                response =>
                {
                    if (response.Error != null)
                    {
                        if (failure != null) failure(response.Error.Message);
                    }
                    else
                    {
                        if (response.Response.code == 200)
                        {
                            if (success != null) success(response.Response.body);
                        }
                        else
                        {
                            if (failure != null) failure(response.Response.message);
                        }
                    }
                });
        }
    }
}