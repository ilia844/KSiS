using System;
using System.Collections.Generic;

namespace InteractionTools
{
    [Serializable]
    public class DialogInfo
    {
        public bool IsActive;
        public string Name;
        public int Id;
        public List<ChatMessage> MessagesHistory;
        public int UnreadMessCount;

        public DialogInfo(string name, int id)
        {
            IsActive = true;
            Name = name;
            MessagesHistory = new List<ChatMessage>();
            UnreadMessCount = 0;
            Id = id;
        }

        public DialogInfo()
        { 
        
        }
    }
}
