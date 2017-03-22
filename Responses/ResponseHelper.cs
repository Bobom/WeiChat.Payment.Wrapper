using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Responses
{
    public static class ResponseHelper
    {
        public static ResultState ParseResultState(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return ResultState.NONE;
            }
            if (text.Trim().ToUpper() == "SUCCESS")
            {
                return ResultState.SUCCESS;
            }
            else if (text.Trim().ToUpper() == "FAIL")
            {
                return ResultState.FAIL;
            }
            return ResultState.NONE;
        }
        public static TradeType ParseTradeType(string type)
        {
            TradeType tradeType = TradeType.NONE;
            if (string.IsNullOrEmpty(type))
            {
                return tradeType;
            }
            if (type.Trim().ToUpper() == "MICROPAY")
            {
                tradeType = TradeType.MICROPAY;
            }
            else if (type.Trim().ToUpper() == "JSAPI")
            {
                tradeType = TradeType.JSAPI;
            }
            else if (type.Trim().ToUpper() == "NATIVE")
            {
                tradeType = TradeType.NATIVE;
            }
            else if (type.Trim() == "APP")
            {
                tradeType = TradeType.APP;
            }
            return tradeType;
        }
        public static TradeState ParseTraseState(string state)
        {
            TradeState tradeState = TradeState.NONE;
            if (string.IsNullOrEmpty(state))
            {
                return tradeState;
            }
            if (!string.IsNullOrEmpty(state))
            {
                switch (state.Trim().ToUpper())
                {
                    case "SUCCESS":
                        tradeState = TradeState.SUCCESS;
                        break;
                    case "REFUND":
                        tradeState = TradeState.REFUND;
                        break;
                    case "NOTPAY":
                        tradeState = TradeState.NOTPAY;
                        break;
                    case "CLOSED":
                        tradeState = TradeState.CLOSED;
                        break;
                    case "REVOKED":
                        tradeState = TradeState.REVOKED;
                        break;
                    case "USERPAYING":
                        tradeState = TradeState.USERPAYING;
                        break;
                    case "PAYERROR":
                        tradeState = TradeState.PAYERROR;
                        break;
                }
            }
            return tradeState;
        }
        public static RefundStatus ParseRefundStatus(string status)
        {
            if(string.IsNullOrEmpty(status))
            {
                return RefundStatus.NONE;
            }
            RefundStatus state = RefundStatus.NONE;
            switch (status)
            {
                case "SUCCESS":
                    state = RefundStatus.SUCCESS;
                    break;
                case "REFUNDCLOSE":
                    state = RefundStatus.REFUNDCLOSE;
                    break;
                case "NOTSURE":
                    state = RefundStatus.NOTSURE;
                    break;
                case "PROCESSING":
                    state = RefundStatus.PROCESSING;
                    break;
                case "CHANGE":
                    state = RefundStatus.CHANGE;
                    break;
            }

            return state;
        }
    }
}
