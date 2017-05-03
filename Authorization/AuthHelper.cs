using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Adapter.Responses;
using WeChat.Adapter.Requests;
using WeChat.Adapter;
namespace WeChat.Adapter.Authorization
{
    public class AuthHelper
    {
        public static AccessToken GetAccessToken(WeChatPayConfig config, string code)
        {
            if(string.IsNullOrEmpty(code))
            {
                throw new Exception("Authentication code must be provided");
            }
            AccessToken token = null;
            AuthAccessTokenRequest authRequest = new AuthAccessTokenRequest(config);
            authRequest.code = code;
            BaseResponse res = authRequest.Execute();
            if(res!=null)
            {
                token = ((AccessTokenResponse)res).Access_Token;
            }
            return token;
        }

        public static WeChatUserInfo GetUserInfo(WeChatPayConfig config, AccessToken token)
        {
            if (token == null || string.IsNullOrEmpty(token.Access_Token) || string.IsNullOrEmpty(token.OpenId))
            {
                throw new Exception("Access token and open id must be provided");
            }
            UserInfoRequest userInfoRequest = new UserInfoRequest(config);
            userInfoRequest.AccessToken = token.Access_Token;
            userInfoRequest.OpenId = token.OpenId;
            BaseResponse res = userInfoRequest.Execute();
            UserInfoResponse response = null;
            if (res != null)
            {
                response = (UserInfoResponse)res;
            }
            return response.UserInfo;
        }

        /// <summary>
        /// Used by Wechat Mini program to decode userinfo
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Json str contains openid and session_key</returns>
        public static string GetSeeionKey(WeChatPayConfig config,string code)
        {
            string result = null;
            if (config == null) {
                throw new WeChatException("Configuration cannot be empty.");
            }
            if (string.IsNullOrEmpty(config.MiniAppId)) {
                throw new WeChatException("Appid cannot be empty of wechat mini app");
            }
            if (string.IsNullOrEmpty(config.MiniAppSecret))
            {
                throw new WeChatException("AppSecret cannot be empty of wechat mini app");
            }
            if (string.IsNullOrEmpty(config.GetSeeionKeyApi))
            {
                throw new WeChatException("Get seeion key api url cannot be empty of wechat mini app");
            }

            string reqUrl = config.GetSeeionKeyApi;
            if (!reqUrl.EndsWith("?"))
            {
                reqUrl +="?";
            }
            reqUrl += "appid="+config.MiniAppId+"&secret="+config.MiniAppSecret+"&js_code="+code+ "&grant_type=authorization_code";
            result=HttpSercice.PostHttpRequest(reqUrl, null, null, RequestType.GET, false, null);
            return result;
        }
    }
}
