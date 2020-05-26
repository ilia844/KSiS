using System;
using System.Collections.Generic;

namespace InteractionTools
{
    public static class DialogInfoMethods
    {
        public static void AddMessage(List<ChatMessage> MessagesHistory, ref int UnreadMessCount, ChatMessage message)
        {
            MessagesHistory.Add(message);
            UnreadMessCount++;
        }

        public static List<DialogInfo> DictionaryIntoList(Dictionary<int, DialogInfo> dialogs)
        {
            List<DialogInfo> result = new List<DialogInfo>();
            foreach (DialogInfo dialog in dialogs.Values)
            {
                result.Add(dialog);
            }
            return result;
        }

        public static Dictionary<int, DialogInfo> ListIntoDictionary(List<DialogInfo> dialogs)
        {
            Dictionary<int, DialogInfo> result = new Dictionary<int, DialogInfo>();
            foreach (DialogInfo dialog in dialogs)
            {
                result.Add(dialog.Id, dialog);
            }
            return result;
        }
    }
}
