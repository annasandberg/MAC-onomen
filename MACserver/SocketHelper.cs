using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MACserver
{
    public class SocketHelper
    {
        public static List<TcpClient> connections = new List<TcpClient>();

        public static void ToClient(byte[] message)
        {

            foreach (var conn in connections)
            {
                conn.Client.Send(message);
            }
            //var s = string.Format("Inkommen beställning: {0} för bil med regnr {1}", order.ServiceTypes.ToString(), order.Regnumber);
            //var message = Encoding.UTF8.GetBytes(s);
            //var send = new byte[message.Length + 2];
            //send[0] = 0x81;
            //send[1] = (byte)(message.Length);
            //for (var i = 0; i < message.Length; i++)
            //{
            //    send[i + 2] = (byte)message[i];
            //}

            //stream.Write(send, 0, send.Length);
        }
    }
}
