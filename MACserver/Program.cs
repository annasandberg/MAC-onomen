using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MAC_onomen.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using MACserver;

namespace WebSocketServer
{
    class Program
    {
        static TcpClient client;
        
        static List<TcpClient> employees = new List<TcpClient>();
        

        public static void Main()
        {

            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:8000.{0}Waiting for a connection...", Environment.NewLine);

            while (true) // Add exit flag here
            {
                client = server.AcceptTcpClient();
                MyClient c = new MyClient(client, ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
                
                
            }
        }
        
    }

}