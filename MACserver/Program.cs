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

namespace WebSocketServer
{
    class Program
    {
        static NetworkStream stream;
        static Task task = new Task(() => FromClient());
        static TcpClient client;
        static List<TcpClient> customers = new List<TcpClient>();
        static List<TcpClient> employees = new List<TcpClient>();

        public static void Main()
        {

            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:8000.{0}Waiting for a connection...", Environment.NewLine);

            while (true) // Add exit flag here
            {
                client = server.AcceptTcpClient();
                Console.WriteLine("A client connected.");
                var childSocketThread = new Task(() => HandleClient(client));
                childSocketThread.Start();
            }
        }
        
    

        private static void HandleClient(TcpClient obj)
        {
            string clientIPAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            var clientPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            //lägg till i en lista?
            employees.Add(client);
            customers.Add(client);

            ReadStream();

        }

        static void ReadStream()
        {
            stream = client.GetStream();

            while (true)
            {
                while (!stream.DataAvailable) ;
                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);

                String data = Encoding.UTF8.GetString(bytes);

                if (new Regex("^GET").IsMatch(data))
                {
                    Console.WriteLine("OK");
                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                        + "Connection: Upgrade" + Environment.NewLine
                        + "Upgrade: websocket" + Environment.NewLine
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + Environment.NewLine
                        + Environment.NewLine);

                    stream.Write(response, 0, response.Length);

                    //ta emot data från kunder
                    FromClient();
                }
                else
                {

                }

            }
        }

        static void FromClient()
        {
            
            while (true)
            {
                var bytes = new Byte[1024];
                int rec = stream.Read(bytes, 0, 1024);  //Blocking
                var length = bytes[1] - 128; //message length

                if (length > 0)
                {
                    Byte[] key = new Byte[4];
                    Array.Copy(bytes, 2, key, 0, key.Length);
                    byte[] encoded = new Byte[length];
                    byte[] decoded = new Byte[length];
                    Array.Copy(bytes, 6, encoded, 0, encoded.Length);
                    for (int i = 0; i < encoded.Length; i++)
                    {
                        decoded[i] = (Byte)(encoded[i] ^ key[i % 4]);
                    }
                    var data = Encoding.UTF8.GetString(decoded);

                    if (data == "exit") break;

                    //data är en beställning - skicka vidare till en el flera anställda
                    if (data.Contains("serviceTypes"))
                    {
                        ServiceTypeViewModel order = JsonConvert.DeserializeObject<ServiceTypeViewModel>(data);
                        Console.WriteLine(order.ServiceTypes.ToString());
                        Console.WriteLine(order.Regnumber);

                        //till varje anställd: skicka order
                        foreach (var employee in employees)
                        {
                            employee.Client.Send(decoded);
                        }
                        //ToClient(order);
                    }
                    
                }

                //skicka data till anställda - använd javascript på den sidan för att fylla i info.
                //ToClient(null);
            }
            stream.Close();
            client.Close();
            employees.Remove(client);
            customers.Remove(client);
            //ta bort client från listan
        }

        static void ToClient(ServiceTypeViewModel order)
        {
            var s = string.Format("Inkommen beställning: {0} för bil med regnr {1}", order.ServiceTypes.ToString(), order.Regnumber);
            var message = Encoding.UTF8.GetBytes(s);
            var send = new byte[message.Length + 2];
            send[0] = 0x81;
            send[1] = (byte)(message.Length);
            for (var i = 0; i < message.Length; i++)
            {
                send[i + 2] = (byte)message[i];
            }

            stream.Write(send, 0, send.Length);

            
        }

    }

}