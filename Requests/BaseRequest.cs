using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.Adapter;
using WeChat.Adapter.Responses;
using log4net;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml;

namespace WeChat.Adapter.Requests
{
   
    public class BaseRequest<T> where T:BaseResponse
    {
        protected string url = null;        
        protected string secret = null;
        protected string shop_secret = null;
        protected bool needCert = false;
        public string appid { get; protected set; }        
        public string mch_id { get; protected set; }
        public string nonce_str { get; protected set; }
        public string sign_type { get; set; }
        public string body { get; set; }
        protected ILog logger = null;
        protected WeChatPayConfig config;
        public BaseRequest(WeChatPayConfig config)
        {
            this.config = config;
            logger = WeChatLogger.GetLogger();
            this.appid = config.APPID;
            this.secret = config.Secret;
            this.shop_secret = config.ShopSecret;
            this.mch_id = config.ShopID;
            this.sign_type = config.SignType;
            this.nonce_str = Guid.NewGuid().ToString().Replace("-","");
            if(nonce_str.Length>32)
            {
                nonce_str = nonce_str.Substring(0,32);
            }
            ConfigVerification();
        }
        protected virtual void ParamsVerification()
        {

        }
        public virtual BaseResponse Execute()
        {
            logger.Info("Executing...");
            ParamsVerification();
            BaseResponse response = null;
            SortedDictionary<string, string> paras = new SortedDictionary<string, string>();
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (properties != null)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    string key = properties[i].Name;
                    object value = properties[i].GetValue(this);
                  
                    paras.Add(key, (value!=null?value.ToString():""));
                }
            }
           
            string sign = null;
            sign = HashWrapper.MD5_Hash(paras, this.shop_secret); //sign.ToUpper();
            logger.Info("sign:" + sign);
            paras.Add("sign", sign);
            NameValueCollection col = new NameValueCollection();
            foreach (KeyValuePair<string, string> param in paras)
            {
                if (param.Value != null && !string.IsNullOrEmpty(param.Value.ToString()))
                {
                    col.Add(param.Key, param.Value);
                }
            }
            string str = null;
            if (needCert)
            {
                str = HttpSercice.PostHttpRequest(this.url, col, RequestType.POST, "text/xml",true,config);
            }
            else
            {
                str = HttpSercice.PostHttpRequest(this.url, col, RequestType.POST, "text/xml");
            }
            logger.Info("response:" + str);
            response = ParseXML(str);
            logger.Info("Done.");
            return response;
        }
        protected virtual T ParseXML(string resStr)
        {
            throw new NotImplementedException("Drivered class must implement this method");
        }
        private void ConfigVerification()
        {
            if(string.IsNullOrEmpty(appid))
            {
                throw new WeChatException("AppId cannot be empty");
            }
            if (string.IsNullOrEmpty(mch_id))
            {
                throw new WeChatException("mch_id cannot be empty");
            }
            if (string.IsNullOrEmpty(sign_type))
            {
                throw new WeChatException("signType cannot be empty,it should be MD5 or sha");
            }
        }      
        protected void ParseBaseResponse(BaseResponse res,XmlDocument doc)
        {
            try
            {
                XmlNode return_code = doc.SelectSingleNode("/xml/return_code");
                if (return_code != null && !string.IsNullOrEmpty(return_code.InnerText))
                {
                    res.return_code = ResponseHelper.ParseResultState(return_code.InnerText);
                }

                //If fail
                if (res.return_code == ResultState.FAIL)
                {
                    XmlNode err_code = doc.SelectSingleNode("/xml/err_code");
                    if (err_code != null && !string.IsNullOrEmpty(err_code.InnerText))
                    {
                        res.err_code = err_code.InnerText.Trim();
                    }
                    XmlNode err_code_des = doc.SelectSingleNode("/xml/err_code_des");
                    if (err_code_des != null && !string.IsNullOrEmpty(err_code_des.InnerText))
                    {
                        res.err_code_des = err_code_des.InnerText.Trim();
                    }
                }

                XmlNode return_msg = doc.SelectSingleNode("/xml/return_msg");
                if (return_msg != null && !string.IsNullOrEmpty(return_msg.InnerText))
                {
                    res.return_msg = return_msg.InnerText.Trim();
                }

                XmlNode appid = doc.SelectSingleNode("/xml/appid");
                if (appid != null && !string.IsNullOrEmpty(appid.InnerText))
                {
                    res.appid = appid.InnerText.Trim();
                }

                XmlNode mch_id = doc.SelectSingleNode("/xml/mch_id");
                if (mch_id != null && !string.IsNullOrEmpty(mch_id.InnerText))
                {
                    res.mch_id = mch_id.InnerText.Trim();
                }

                XmlNode nonce_str = doc.SelectSingleNode("/xml/nonce_str");
                if (nonce_str != null && !string.IsNullOrEmpty(nonce_str.InnerText))
                {
                    res.nonce_str = nonce_str.InnerText.Trim();
                }

                XmlNode sign = doc.SelectSingleNode("/xml/sign");
                if (sign != null && !string.IsNullOrEmpty(sign.InnerText))
                {
                    res.sign = sign.InnerText.Trim();
                }
                XmlNode result_code = doc.SelectSingleNode("/xml/result_code");
                if (result_code != null && !string.IsNullOrEmpty(result_code.InnerText))
                {
                    res.result_code = result_code.InnerText.Trim();
                }
            }
            catch(Exception ex)
            {
                throw new WeChatException(ex);
            }            
        }
    }
}
