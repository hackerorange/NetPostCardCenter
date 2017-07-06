using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using soho.domain.orderCenter;
using soho.web;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

namespace soho.webservice
{
    public static class OrderCenterInvoker
    {
        public static List<EnvelopeInfo> GetOrderDetails(DateTime startDate, DateTime endDate)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());

            var nameValueCollection = new Dictionary<string, object>();


            nameValueCollection.Add("startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            nameValueCollection.Add("endDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var postForObject =
                restTemplate.GetForObject<PageResponse<EnvelopeInfo>>("http://localhost:8083/rest/orderController?createDate={startDate}&endDate={endDate}", nameValueCollection);

            var a=restTemplate.GetForMessage<String>("http://localhost:8083/rest/orderController?createDate={startDate}&endDate={endDate}", nameValueCollection);
            Console.WriteLine(a.Body);

             if (postForObject.code == 200)
                return postForObject.page;
            throw new Exception("获取URL错误");
        }
    }
}