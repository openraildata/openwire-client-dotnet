using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalNetworkRailOpenDataClient
{
    public class OpenRailMessage
    {
        protected OpenRailMessage()
        {
        }
    }

    public class OpenRailTextMessage : OpenRailMessage
    {
        private string? msText = null;

        public OpenRailTextMessage(string sText)
        {
            msText = sText;
        }

        public string? Text { get { return msText; } }
    }
}
