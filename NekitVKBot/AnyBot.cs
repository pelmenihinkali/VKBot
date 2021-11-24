using System;
using System.Collections.Generic;
using System.Text;
using VkNet;

namespace AnyBotSystem
{
    [Serializable]
    internal class VKSettings
    {
        [NonSerialized] public VkApi API = new VkApi();
        public string Token;
        public ulong IDGroup;
        public int Delay;
        public VKSettings(string token, ulong idgroup, int delay)
        {
            Token = token;
            IDGroup = idgroup;
            Delay = delay;
        }
    }
}