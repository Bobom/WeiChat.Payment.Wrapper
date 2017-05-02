using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;
using WeChat.Adapter.Requests;
namespace WeChat.Adapter.Messages
{
    public class BaseMsg
    {
        private WeChatPayConfig config;
        private AccessToken token;
        private ILog logger = WeChatLogger.GetLogger();
        public string touser;
        public string topcolor;
        public string template_id;
        public string url;
        protected string ToJson()
        {
            logger.Info("To Json...");        
            if (string.IsNullOrEmpty(touser))
            {
                throw new WeChatException("touser cannot be empty");
            }
            if (string.IsNullOrEmpty(template_id))
            {
                throw new WeChatException("template_id cannot be empty");
            }
            if (string.IsNullOrEmpty(topcolor))
            {
                topcolor = "#3DD471";
            }
            
            StringBuilder jsonBuilder = new StringBuilder("{");
            Type type = this.GetType();
            FieldInfo[] fields= type.GetFields();
            if(fields!=null && fields.Length>0)
            {
                for(int i=0;i<fields.Length;i++)
                {
                    FieldInfo field = fields[i];
                    if (field.IsPrivate)
                    {
                        continue;
                    }
                    if (field.GetValue(this) == null)
                    {
                        logger.Info(string.Format("Value of field {0} is null",field.Name));
                        continue;
                    }
                    jsonBuilder.Append("\"" + field.Name + "\":");
                    jsonBuilder.Append("\"" + (field.GetValue(this)!=null?field.GetValue(this).ToString():"") + "\",");
                }
            }
            else
            {
                logger.Info("No fields");
            }
            jsonBuilder.Append("\"data\":{");
            PropertyInfo[] properties = type.GetProperties();
            if(properties!=null && properties.Length > 0)
            {
                for(int i=0;i< properties.Length;i++)
                {
                    PropertyInfo property = properties[i];
                    string proName = property.Name;
                    if (property.GetValue(this) == null)
                    {
                        logger.Info(string.Format("Value of property {0} is null",proName));
                        continue;
                    }
                    string value = property.GetValue(this).ToString();
                    string color = "#173177";
                    if (value!=null && value.IndexOf("@") > -1)
                    {
                        color = value.Split('@')[1];
                        value= value.Split('@')[0];
                    }
                    jsonBuilder.Append("\"" + proName + "\":{");
                    jsonBuilder.Append("\"value\":" + "\"" + value + "\",");
                    jsonBuilder.Append("\"color\":" + "\"" + color + "\"");
                    jsonBuilder.Append("}");
                    if (i <properties.Length-1)
                    {
                        jsonBuilder.Append(",");
                    }
                }
            }
            else
            {
                logger.Info("No properties");
            }

            jsonBuilder.Append("}}");
            logger.Info("To Json done.");
            return jsonBuilder.ToString();
        }

        public BaseMsg(WeChatPayConfig _config,AccessToken token)
        {
            if(_config==null || string.IsNullOrEmpty(_config.TemplateMessageUrl))
            {
                throw new WeChatException("Template url cannot be empty");
            }
            this.config = _config;
            if(token==null || string.IsNullOrEmpty(token.Access_Token))
            {
                throw new WeChatException("Access token cannot be empty");
            }
            this.token = token;
        }
        public virtual void Send()
        {
            try
            {
                logger.Info("Template Id:" + template_id);
                logger.Info("Access token:" + token.Access_Token);
                string reqUrl = config.TemplateMessageUrl + token.Access_Token;
                string toJson = ToJson();
                if (string.IsNullOrEmpty(toJson))
                {
                    logger.Error("message cannot be null");
                    throw new WeChatException("message cannot be null");
                }
                logger.Info(string.Format("Json string {0} has been sent to wechat", toJson));
                string res = HttpSercice.PostHttpRequest(reqUrl, toJson, null, RequestType.POST, false, null);
                if (string.IsNullOrEmpty(res))
                {
                    logger.Error("no response from wechat");
                }
                else
                {
                    logger.Info(res);
                }
            }
            catch(WeChatException wex)
            {
                logger.Error(wex);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
            }
        }
    }   
}
