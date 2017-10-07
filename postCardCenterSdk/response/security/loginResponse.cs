using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.response.security
{
    public class LoginResponse
    {
        /// <summary>
        /// TOKEN
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

      
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

    }
}
