using System;
using System.Collections.Generic;

namespace InteractionTools
{
    [Serializable]
    public class ChatMessage
    {
        public int SenderId;
        public string SenderName;
        public string Content;
        public List<int> AttachedFiles;//
        public DateTime Time;

        public ChatMessage(int senderId, string senderName, string content, DateTime time)
        {
            SenderId = senderId;
            SenderName = senderName;
            Content = content;
            AttachedFiles = new List<int>();
            Time = time;
        }
//
        public ChatMessage(int senderId, string senderName, string content, List<int> files, DateTime time)
        {
            SenderId = senderId;
            SenderName = senderName;
            Content = content;
            AttachedFiles = files;
            Time = time;
        }
//
        public ChatMessage()
        {

        }
    }
}
