using System;
using System.Collections.Generic;
using InteractionTools;

namespace ChatClient
{
    public class CommunityData
    {
        public Dictionary<int, DialogInfo> Dialogs;

        public CommunityData(Dictionary<int, DialogInfo> dialogs)
        {
            Dialogs = dialogs;
        }
    }
}
