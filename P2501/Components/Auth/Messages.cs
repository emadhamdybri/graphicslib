using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Auth
{
    public class RequestAdd : AuthMessage
    {
        public string email = string.Empty;
        public string password = string.Empty;
        public string callsign = string.Empty;

        public RequestAdd()
        {
            Name = AuthMessage.RequestAdd;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(email);
            buffer.Write(password);
            buffer.Write(callsign);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            email = buffer.ReadString();
            password = buffer.ReadString();
            callsign = buffer.ReadString();
            return true;
        }
    }
}
