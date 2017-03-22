using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeChat.Adapter;
using WeChat;
using WeChat.Adapter.Responses;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml;

namespace WeChat.Adapter.Requests
{
    public class RefundApplyRequest: BaseRequest<RefundApplyResponse>
    {
        public string device_info { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public int total_fee { get; set; }
        public int refund_fee { get; set; }
        public string refund_fee_type { get; set; }
        public string op_user_id { get; set; }
        public string refund_account { get; set; }
        public RefundApplyRequest(WeChatPayConfig config) : base(config)
        {
            logger = WeChatLogger.GetLogger();
            if (config==null)
            {
                throw new WeChatException("Wechatconfig cannot be empty");
            }
            op_user_id = config.ShopID;
            url = config.RefundUrl;
            refund_fee_type = "CNY";
        }
        protected override void ParamsVerification()
        {
            if(string.IsNullOrEmpty(url))
            {
                WeChatException ex = new WeChatException("Refund url cannot be empty");
                logger.Error(ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(transaction_id) && string.IsNullOrEmpty(out_trade_no))
            {
                WeChatException ex = new WeChatException("transaction_id and out_trade_no cannot both be empty");
                logger.Error(ex);
                throw ex;
            }
            if(string.IsNullOrEmpty(out_refund_no))
            {
                WeChatException ex = new WeChatException("out_refund_no cannot be empty");
                logger.Error(ex);
                throw ex;
            }
            if (out_refund_no.Length > 32)
            {
                WeChatException ex = new WeChatException("max lenght of out_refund_no is 32");
                logger.Error(ex);
                throw ex;
            }
            if(total_fee<=0)
            {
                WeChatException ex = new WeChatException("total_fee cannot be 0");
                logger.Error(ex);
                throw ex;
            }
            if (refund_fee <= 0)
            {
                WeChatException ex = new WeChatException("refund_fee cannot be 0");
                logger.Error(ex);
                throw ex;
            }
            if(refund_fee>total_fee)
            {
                WeChatException ex = new WeChatException("refund_fee cannot larger than total_fee");
                logger.Error(ex);
                throw ex;
            }
        }
        protected override RefundApplyResponse ParseXML(string xml)
        {
            RefundApplyResponse response = null;
            if(string.IsNullOrEmpty(xml))
            {
                logger.Error("Wechat payment refund API returns nothing.");
                return response;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                response = new RefundApplyResponse();
                ParseBaseResponse(response,doc);
                if(response.return_code== ResultState.SUCCESS)
                {
                    XmlNode transaction_id = doc.SelectSingleNode("/xml/transaction_id");
                    if (transaction_id != null && !string.IsNullOrEmpty(transaction_id.InnerText))
                    {
                        response.transaction_id = transaction_id.InnerText.Trim();
                    }
                    XmlNode out_trade_no = doc.SelectSingleNode("/xml/out_trade_no");
                    if (out_trade_no != null && !string.IsNullOrEmpty(out_trade_no.InnerText))
                    {
                        response.out_trade_no = out_trade_no.InnerText.Trim();
                    }
                    XmlNode out_refund_no = doc.SelectSingleNode("/xml/out_refund_no");
                    if (out_refund_no != null && !string.IsNullOrEmpty(out_refund_no.InnerText))
                    {
                        response.out_refund_no = out_refund_no.InnerText.Trim();
                    }
                    XmlNode refund_id = doc.SelectSingleNode("/xml/refund_id");
                    if (refund_id != null && !string.IsNullOrEmpty(refund_id.InnerText))
                    {
                        response.refund_id = refund_id.InnerText.Trim();
                    }
                    XmlNode total_fee = doc.SelectSingleNode("/xml/total_fee");
                    if (total_fee != null && !string.IsNullOrEmpty(total_fee.InnerText))
                    {
                        int totalFee = 0;
                        int.TryParse(total_fee.InnerText.ToString().Trim(), out totalFee);
                        response.total_fee = totalFee;                        
                    }
                    XmlNode refund_fee = doc.SelectSingleNode("/xml/total_fee");
                    if (refund_fee != null && !string.IsNullOrEmpty(refund_fee.InnerText))
                    {
                        int refundFee = 0;
                        int.TryParse(refund_fee.InnerText.ToString().Trim(), out refundFee);
                        response.total_fee = refundFee;
                    }
                }
            }
            catch(WeChatException wex)
            {
                logger.Error(wex);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
            }
            return response;
        }
    }
}
