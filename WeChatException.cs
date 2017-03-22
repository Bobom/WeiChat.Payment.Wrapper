using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter
{
    public class WeChatException:Exception
    {
        public WeChatException(string message):base(message)
        {

        }
        public WeChatException(Exception ex) : base(ex.Message,ex)
        {

        }
    }
}
