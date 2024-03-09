using GS.Unitive.Framework.Core;
using System;
using System.Net;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class UdpTransmissionEventArgs : EventArgs
    {
        public IAddonContext Context { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
        public EndPoint RemoteEP { get; set; }
    }
}
