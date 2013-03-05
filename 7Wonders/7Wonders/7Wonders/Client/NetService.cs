using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    public interface NetService
    {
        int joinHost(bool local);//discovers and joins a host
        int disconnect();//disconnects from the server
        int sendMessage(String message, int type);//send a message to the server
        //note: need to create an enum to track different message types
    }
}
