using System;
using System.Collections.Generic;
using System.Text;

namespace Router
{
    public enum State
    {
        Creating,
        Failed,
        Running,
        Closing,
        Closed
    }
}
