using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace WeChat.Adapter.Messages
{
    public class BaseMsg
    {
        public string touser;
        public string topcolor;
        public string template_id;
        public string url;
        public string ToJson()
        {
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
                    jsonBuilder.Append("\"" + field.Name + "\":");
                    jsonBuilder.Append("\"" + field.GetValue(this).ToString() + "\",");
                }
            }
            jsonBuilder.Append("\"data\":{");
            PropertyInfo[] properties = type.GetProperties();
            if(properties!=null && properties.Length > 0)
            {
                for(int i=0;i< properties.Length;i++)
                {
                    PropertyInfo property = properties[i];
                    string proName = property.Name;
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

            jsonBuilder.Append("}}");
            return jsonBuilder.ToString();
        }
    }
    public class OrderStatusChangeMsg:BaseMsg
    {
        public string first { get; set; }
        public string remark { get; set; }
        public string OrderSn { get; set; }
        public string OrderStatus { get; set; }
        public OrderStatusChangeMsg()
        {
            template_id = "RsP_3nVu7U1HLF1I01rTdIjFk8HAFJbuJSt4Jsbcs3k";
        }
    }
}
