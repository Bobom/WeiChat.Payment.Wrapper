using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace WeChat.Adapter
{
    public class HashWrapper
    {
        public static string SHA1_Hash(SortedDictionary<string, string> paras,string key=null)
        {
            if (paras != null)
            {
                string signStr = string.Empty;
                foreach (KeyValuePair<string, string> parameter in paras)
                {
                    if (signStr == string.Empty)
                    {
                        signStr = parameter.Key + "=" + parameter.Value != null ? parameter.Value : "";
                    }
                    else
                    {
                        signStr += "&" + parameter.Key + "=" + parameter.Value != null ? parameter.Value : "";
                    }
                }
                if(!string.IsNullOrEmpty(key))
                {
                    signStr += "&key="+ key;
                }
                return SHA1_Hash(signStr);
            }
            return null;
        }
        public static string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }
        public static string MD5_Hash(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        public static string MD5_Hash(SortedDictionary<string, string> paras,string key=null)
        {
            if (paras != null)
            {
                string signStr = string.Empty;
                foreach (KeyValuePair<string, string> parameter in paras)
                {
                    if (signStr == string.Empty)
                    {
                        signStr = parameter.Key + "=" + parameter.Value != null ? parameter.Value : "";
                    }
                    else
                    {
                        signStr += "&" + parameter.Key + "=" + parameter.Value != null ? parameter.Value : "";
                    }
                }
                if(!string.IsNullOrEmpty(key))
                {
                    signStr += "&key=" + key;
                }                
                return MD5_Hash(signStr);
            }
            return null;
        }
    }
}
