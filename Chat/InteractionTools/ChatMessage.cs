using System;

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
