﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeChat;
using WeChat.Adapter.Responses;
namespace WeChat.Adapter.Requests
{
    public class TranfserRequest:BaseRequest<TranfserResponse>
    {
        public string device_info { get; set; }
        public string partner_trade_no { get; set; }
        public string openid { get; set; }
        public string check_name { get; set; }
        public string re_user_name { get; set; }
        public int amount { get; set; }
        public string desc { get; set; }
        public string spbill_create_ip { get; set; }
        public TranfserRequest(WeChatPayConfig config):base(config)
        {
            //必须强制验证收款方姓名
            check_name = "FORCE_CHECK";
        }

        protected override void ParamsVerification()
        {
            if (string.IsNullOrEmpty(url))
            {
                WeChatException ex = new WeChatException("Refund url cannot be empty");
                logger.Error(ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(partner_trade_no) && string.IsNullOrEmpty(partner_trade_no))
            {
                WeChatException ex = new WeChatException("partner_trade_no cannot be empty");
                logger.Error(ex);
                throw ex;
            }
            if (string.IsNullOrEmpty(openid))
            {
                WeChatException ex = new WeChatException("openid cannot be empty");
                logger.Error(ex);
                throw ex;
            }

            if (string.IsNullOrEmpty(re_user_name))
            {
                WeChatException ex = new WeChatException("re_user_name cannot be empty");
                logger.Error(ex);
                throw ex;
            }

            if (string.IsNullOrEmpty(desc))
            {
                WeChatException ex = new WeChatException("desc cannot be empty");
                logger.Error(ex);
                throw ex;
            }

            if (string.IsNullOrEmpty(spbill_create_ip))
            {
                WeChatException ex = new WeChatException("spbill_create_ip cannot be empty");
                logger.Error(ex);
                throw ex;
            }

            if(amount<=0)
            {

                WeChatException ex = new WeChatException("amount cannot be 0");
                logger.Error(ex);
                throw ex;
            }
        }

        protected override TranfserResponse ParseXML(string xml)
        {
            TranfserResponse response = null;
            if (string.IsNullOrEmpty(xml))
            {
                logger.Error("Wechat payment refund API returns nothing.");
                return response;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                response = new TranfserResponse();
                ParseBaseResponse(response, doc);
                if (response.return_code == ResultState.SUCCESS)
                {
                    XmlNode mch_appid = doc.SelectSingleNode("/xml/mch_appid");
                    if (mch_appid != null && !string.IsNullOrEmpty(mch_appid.InnerText))
                    {
                        response.mch_appid = mch_appid.InnerText.Trim();
                    }
                    XmlNode mchid = doc.SelectSingleNode("/xml/mchid");
                    if (mchid != null && !string.IsNullOrEmpty(mchid.InnerText))
                    {
                        response.mchid = mchid.InnerText.Trim();
                    }
                    XmlNode partner_trade_no = doc.SelectSingleNode("/xml/partner_trade_no");
                    if (partner_trade_no != null && !string.IsNullOrEmpty(partner_trade_no.InnerText))
                    {
                        response.partner_trade_no = partner_trade_no.InnerText.Trim();
                    }
                    XmlNode payment_no = doc.SelectSingleNode("/xml/payment_no");
                    if (payment_no != null && !string.IsNullOrEmpty(payment_no.InnerText))
                    {
                        response.payment_no = payment_no.InnerText.Trim();
                    }
                    XmlNode payment_time = doc.SelectSingleNode("/xml/payment_time");
                    if (payment_time != null && !string.IsNullOrEmpty(payment_time.InnerText))
                    {
                        response.payment_time = payment_time.InnerText.Trim();
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
