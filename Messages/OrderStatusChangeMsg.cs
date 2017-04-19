using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Messages
{
    public class OrderStatusChangeMsg:BaseMsg
    {
        public string first { get; set; }
        public string remark { get; set; }
        public string OrderSn { get; set; }
        public string OrderStatus { get; set; }
        public OrderStatusChangeMsg(WeChatPayConfig _config, AccessToken token) : base(_config, token)
        {
            template_id = "RsP_3nVu7U1HLF1I01rTdIjFk8HAFJbuJSt4Jsbcs3k";
        }
    }
}
