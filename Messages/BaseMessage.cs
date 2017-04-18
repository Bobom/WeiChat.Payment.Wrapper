using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat;
using WeChat.Adapter.Responses;
using WeChat.Adapter;
using WeChat.Adapter.Requests;
using log4net;
namespace WeChat.Adapter.Messages
{
    public class BaseTemplateMessage
    {
        protected ILog logger;
        public string TemplateId { get; protected set; }
        public WeChatPayConfig Config { get; set; }
        public AccessToken Token { get; set; }
        private string url { get; set; }
        public BaseTemplateMessage(WeChatPayConfig _config)
        {
            logger = WeChatLogger.GetLogger();
            Config = _config;
            url = Config.TemplateMessageUrl;
        }

        public virtual void Execute(BaseMsg message)
        {
            if(Token==null)
            {
                logger.Error("token is null");
                throw new WeChatException("Access token cannot be null");
            }
            logger.Info("Access token:"+Token.Access_Token);
            if (message == null)
            {
                logger.Error("message cannot be null");
                throw new WeChatException("message cannot be null");
            }
            this.url = this.url + Token.Access_Token;
            string toJson = message.ToJson();
            if (string.IsNullOrEmpty(toJson))
            {
                logger.Error("message cannot be null");
                throw new WeChatException("message cannot be null");
            }
            logger.Info(string.Format("Json string {0} has been sent to wechat",toJson));
            string res=HttpSercice.PostHttpRequest(this.url, null, RequestType.POST, null, false, null);
            if (string.IsNullOrEmpty(res))
            {
                logger.Error("no response from wechat");
            }
            else
            {
                logger.Error(res);
            }
        }
    }
}
