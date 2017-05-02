using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Adapter.Messages
{
    public class CropCalculateMessage: BaseMsg
    {
        public string first { get; set; }
        public string remark { get; set; }
        public string keyword1 { get; set; }
        public string keyword2 { get; set; }
        public string keyword3 { get; set; }
        public string keyword4 { get; set; }
        public string keyword5 { get; set; }
        public CropCalculateMessage(WeChatPayConfig _config, AccessToken token) : base(_config, token)
        {
            template_id = "vKkNNORiSJ7jg1WusrJiZZ3iAKyvqc93C6WZ49kS9hQ";
        }
    }
}
