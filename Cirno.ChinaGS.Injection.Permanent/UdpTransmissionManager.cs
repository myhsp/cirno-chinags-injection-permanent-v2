using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Cirno.ChinaGS.Injection.Permanent
{
    /// <summary>
    /// UDP 监听管理器
    /// </summary>
    public class UdpTransmissionManager : IDisposable
    {
        private UdpClient client;
        private IPEndPoint localEP;
        private int port;
        private bool listening;

        public UdpTransmissionManager(int localPort)
        {
            this.port = localPort;
            this.localEP = new IPEndPoint(IPAddress.Any, localPort);
            this.client = new UdpClient(this.localEP);
        }

        public bool Listening
        {
            get
            {
                return this.listening;
            }
            set
            {
                this.listening = value;
            }
        }

        /// <summary>
        /// 启动侦听
        /// </summary>
        private void Listen()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (this.listening)
            {
                try
                {
                    byte[] buffer = this.client.Receive(ref remoteEP);
                    string ret = string.Empty;

                    if (buffer.Length > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer);
                        this.UdpTransmissionReceived(new UdpTransmissionEventArgs()
                        {
                            Context = Program.AddonContext,
                            Data = buffer,
                            Message = msg,
                            RemoteEP = remoteEP
                        });
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 启动侦听线程
        /// </summary>
        public void StartListenAsync()
        {
            new Thread(Listen)
            {
                IsBackground = true
            }.Start();
            this.listening = true;
        }

        /// <summary>
        /// 发送 UDP 数据报
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="remoteEP">远程终结点</param>
        /// <param name="remotePort">远程端口</param>
        public void Send(string msg, IPEndPoint remoteEP, int remotePort)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            remoteEP.Port = remotePort;
            this.client.Send(buffer, buffer.Length, remoteEP);
        }

        /// <summary>
        /// 接收到 UDP 消息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void UdpTransmissionReceived(UdpTransmissionEventArgs e)
        {
            UdpTransmissionReceivedEventHandler handler = this.OnUdpTransmissionReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            this.listening = false;
            this.client.Close();
        }

        public event UdpTransmissionReceivedEventHandler OnUdpTransmissionReceived;
    }
    public delegate void UdpTransmissionReceivedEventHandler(object sender, UdpTransmissionEventArgs e);
}
