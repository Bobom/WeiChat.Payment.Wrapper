using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter
{
    public enum RefoundAccount
    {
        REFUND_SOURCE_UNSETTLED_FUNDS,
        REFUND_SOURCE_RECHARGE_FUNDS
    }
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

    public enum AppType
    {
        OPEN_PLATFORM = 1,
        PUBLICK_SVR = 2,
        MINI_APP = 3,
        APP = 4
    }
}
