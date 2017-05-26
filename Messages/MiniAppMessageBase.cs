using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeChat.Adapter.Requests;

namespace WeChat.Adapter.Messages
{
    public class MiniAppMessageBase:BaseMsg
    {
        public string page=string.Empty;
        public string color=string.Empty;
        public string form_id=string.Empty;
        public List<MiniMessageEntity> Data { get; set; }
        public MiniAppMessageBase(WeChatPayConfig _config, AccessToken token):base(_config,token)
        {
            this.apiUrl = config.MiniAppTemplateMessageUrl;
        }

        protected override string ToJson()
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
                color = "#3DD471";
            }

            StringBuilder jsonBuilder = new StringBuilder("{");
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields();
            if (fields != null && fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = fields[i];
                    if (field.IsPublic)
                    {
                        if (field.GetValue(this) == null)
                        {
                            logger.Info(string.Format("Value of field {0} is null", field.Name));
                            continue;
                        }
                        jsonBuilder.Append("\"" + field.Name + "\":");
                        jsonBuilder.Append("\"" + (field.GetValue(this) != null ? field.GetValue(this).ToString() : "") + "\",");
                    }
                }
            }
            else
            {
                logger.Info("No fields");
            }
            jsonBuilder.Append("\"data\":{");

            if(Data==null || Data.Count <= 0)
            {
                throw new WeChatException("Data list cannot be empty");
            }
            for(int i=0;i<Data.Count;i++)
            {
                MiniMessageEntity data = Data[i];
                if(string.IsNullOrEmpty(data.Key) || string.IsNullOrEmpty(data.Value))
                {
                    continue;
                }
                string value = data.Value;
                string color = "#173177";
                if (!string.IsNullOrEmpty(data.Color))
                {
                    color = data.Color;
                }
                jsonBuilder.Append("\"" + data.Key + "\":{");
                jsonBuilder.Append("\"value\":" + "\"" + value + "\",");
                jsonBuilder.Append("\"color\":" + "\"" + color + "\"");
                jsonBuilder.Append("}");
                if (i < Data.Count - 1)
                {
                    jsonBuilder.Append(",");
                }
            }

            jsonBuilder.Append("}}");
            logger.Info("To Json done.");
            return jsonBuilder.ToString();
        }
    }
}
