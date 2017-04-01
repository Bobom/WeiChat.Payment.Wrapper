using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter
{
    public enum ResultState {
        SUCCESS,
        FAIL,
        NONE
    }
    public enum TradeState {
        SUCCESS,
        REFUND,
        NOTPAY,
        CLOSED,
        REVOKED,
        USERPAYING,
        PAYERROR,
        NONE
    }
    public enum TradeType
    {
        JSAPI,
        NATIVE,
        APP,
        MICROPAY,
        NONE
    }
    public enum WeChatRefundStatus
    {
        SUCCESS,
        REFUNDCLOSE,
        NOTSURE,
        PROCESSING,
        CHANGE,
        NONE
    }

    public enum TransStatus
    {
        SUCCESS,
        FAILED,       
        PROCESSING,
        NONE
    }
}
