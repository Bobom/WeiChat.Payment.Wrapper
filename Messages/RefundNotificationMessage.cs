using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Messages
{
    public class RefundNotificationMessage:BaseMsg
    {
        public string first { get; set; }
        public string remark { get; set; }
        public string reason { get; set; }
        public string refund { get; set; }
        public RefundNotificationMessage(WeChatPayConfig _config, AccessToken token) : base(_config, token)
        {
            template_id = "4xUtbva04CAlG-IjLg4ag_VIm51I7NGGjFJj7Qs_9Ak";
        }
    }
}
