using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public interface Observer
    {
        void stateUpdate(GameState state,int code);
    }
}
