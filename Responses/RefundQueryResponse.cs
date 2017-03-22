using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Responses
{
    public class RefundQueryResponse:BaseResponse
    {
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public RefundStatus refund_status { get; set; }
        public string refund_accout { get; set; }
        public string refund_recv_accout { get; set; }
    }
}
