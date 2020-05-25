using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionTools
{
    [Serializable]
    public class ChatMessage
    {
        public int SenderId;
        public string SenderName;
        public string Content;
        public DateTime Time;

        public ChatMessage(int senderId, string senderName, string content, DateTime time)
        {
            SenderId = senderId;
            SenderName = senderName;
            Content = content;
            Time = time;
        }

        public ChatMessage()
        {

        }
    }
}
