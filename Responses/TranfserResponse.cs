using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Responses
{
    public class TranfserResponse:BaseResponse
    {
        public string mch_appid { get; set; }
        public string mchid { get; set; }
        public string payment_no { get; set; }
        public string partner_trade_no { get; set; }
        public string payment_time { get; set; }
    }
}
