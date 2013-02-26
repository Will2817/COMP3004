using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatThingy
{
    class Message
    {
        private String senderName;
        private String contents;

        public Message(String senderName, String contents)
        {
            this.senderName = senderName;
            this.contents = contents;
        }

        public Message(String str)
        {
            String[] vals = str.Split(new string[] { ": " }, StringSplitOptions.None);
            this.senderName = vals[0];
            this.contents = vals[1];
        }

        public String getSenderName()
        {
            return senderName;
        }

        public String getContents()
        {
            return contents;
        }

        public override string ToString()
        {
            return getSenderName() + ": " + getContents();
        }
    }
}
