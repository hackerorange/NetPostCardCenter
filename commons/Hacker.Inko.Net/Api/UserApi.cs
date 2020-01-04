using System;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using Hacker.Inko.Net.Request;
using Hacker.Inko.Net.Response;
using Spring.Http;

namespace Hacker.Inko.Net.Api
{
    public static class UserApi
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="success">成功后的回调函数</param>
        /// <param name="failure">失败后的回调函数</param>
        public static void UserLogin(string userName, string password, Action<LoginResponse> success, Action<string> failure)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<LoginResponse>>(
                "/security/login",
                new UserLoginRequest
                {
                    UserName = userName,
                    Password = password
                },
                result => result.PrepareResponse(success, failure));
        }


        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public static void RefreshToken(string refreshToken, Action<LoginResponse> success, Action<string> failure)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<LoginResponse>>(
                "/security/refreshToken",
                new HttpEntity(new HttpHeaders
                {
                    {"refresh-token", refreshToken}
                }),
                result => result.PrepareResponse(success, failure));
        }
    }
}