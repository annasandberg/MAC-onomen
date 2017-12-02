using MAConomen.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MACserver
{
    public class SocketHelper
    {
        public static List<TcpClient> employees = new List<TcpClient>();
        public static List<TcpClient> customers = new List<TcpClient>();
        public static List<TcpClient> screens = new List<TcpClient>();

        public static void ToEmployees(ServiceTypeViewModel order)
        {
            var s = string.Format("{0},{1}", order.Regnumber, order.ServiceTypes.ToString());
            var message = Encoding.UTF8.GetBytes(s);
            var send = new byte[message.Length + 2];
            send[0] = 0x81;
            send[1] = (byte)(message.Length);
            for (var i = 0; i < message.Length; i++)
            {
                send[i + 2] = (byte)message[i];
            }

            foreach (var conn in employees)
            {
                if (conn.Client.Connected)
                {
                    conn.Client.Send(send);
                }
                
            }
        }

        public static void ToScreens(string data)
        {
            
            var message = Encoding.UTF8.GetBytes(data);
            var send = new byte[message.Length + 2];
            send[0] = 0x81;
            send[1] = (byte)(message.Length);
            for (var i = 0; i < message.Length; i++)
            {
                send[i + 2] = (byte)message[i];
            }

            foreach (var conn in screens)
            {
                if (conn.Client.Connected)
                {
                    conn.Client.Send(send);
                }
            }
        }

        public static void UpdateWaitingList(string data)
        {
            var message = Encoding.UTF8.GetBytes(data);
            var send = new byte[message.Length + 2];
            send[0] = 0x81;
            send[1] = (byte)(message.Length);
            for (var i = 0; i < message.Length; i++)
            {
                send[i + 2] = (byte)message[i];
            }

            foreach (var conn in employees)
            {
                if (conn.Client.Connected)
                {
                    conn.Client.Send(send);
                }

            }
        }
    }
}
