using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeChat;
using WeChat.Adapter.Responses;
namespace WeChat.Adapter.Requests
{
    public class RefundQueryRequest : BaseRequest<RefundQueryResponse>
    {
        public string device_info { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public RefundQueryRequest(WeChatPayConfig config) : base(config)
        {
            logger = WeChatLogger.GetLogger();
            if (config == null)
            {
                throw new WeChatException("Wechatconfig cannot be empty");
            }
            url = config.RefundQueryUrl;
        }
        protected override void ParamsVerification()
        {
            if(string.IsNullOrEmpty(transaction_id) && string.IsNullOrEmpty(out_trade_no) 
               && string.IsNullOrEmpty(out_refund_no) && string.IsNullOrEmpty(refund_id))
            {
                WeChatException ex= new WeChatException("微信支付交易号，内部系统支付交易号，内部系统退款支付交易号，微信退款交易号不能同时为空，未必输入一个");
                logger.Error(ex);
                throw ex;
            }
        }

        protected override RefundQueryResponse ParseXML(string xml)
        {
            RefundQueryResponse response = null;
            if (string.IsNullOrEmpty(xml))
            {
                logger.Error("Wechat payment refund API returns nothing.");
                return response;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                response = new RefundQueryResponse();
                ParseBaseResponse(response, doc);
                if (response.return_code == ResultState.SUCCESS)
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

                    XmlNode refund_status= doc.SelectSingleNode("/xml/refund_status");
                    response.refund_status = WeChatRefundStatus.NONE;
                    if (refund_status!=null)
                    {
                        response.refund_status = ResponseHelper.ParseRefundStatus(refund_status.InnerText);
                    }
                    XmlNode refund_accout = doc.SelectSingleNode("/xml/refund_accout");
                    if (refund_accout != null && !string.IsNullOrEmpty(refund_accout.InnerText))
                    {
                        response.refund_accout = refund_accout.InnerText.Trim();
                    }
                    XmlNode refund_recv_accout = doc.SelectSingleNode("/xml/refund_recv_accout");
                    if (refund_recv_accout != null && !string.IsNullOrEmpty(refund_recv_accout.InnerText))
                    {
                        response.refund_recv_accout = refund_recv_accout.InnerText.Trim();
                    }
                }
            }
            catch (WeChatException wex)
            {
                logger.Error(wex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
            return response;
        }
    }
}
