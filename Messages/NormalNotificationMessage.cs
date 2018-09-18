using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Messages
{
    public class NormalNotificationMessage : BaseMsg
    {
        public NormalNotificationMessage(WeChatPayConfig _config, AccessToken token) : base(_config, token)
        {
            template_id = "RsP_3nVu7U1HLF1I01rTdIjFk8HAFJbuJSt4Jsbcs3k";
        }
    }
}
