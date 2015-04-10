using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kent_Hack_Enough
{
    class SocketRequest
    {
            public SocketRequest()
            {
                nsp = null;
                cmd = null;
                data = null;
            }

            public SocketRequest(string nspTmp, string cmdTmp, string dataTmp)
            {
                nsp = nspTmp;
                cmd = cmdTmp;
                data = dataTmp;
            }

            public string getNsp()
            {
                return nsp;
            }

            public string getCmd()
            {
                return cmd;
            }

            public string getData()
            {
                return data;
            }

            private string nsp;
            private string cmd;
            private string data;
        }
}
