using MAConomen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MACserver
{
    public class MyClient
    {
        static TcpClient _client;
        string clientType;
      
        public MyClient(TcpClient client, string clientType)
        {
            _client = client;
            this.clientType = clientType;
            Console.WriteLine("A client connected: " + clientType);
            var childSocketTask = new Task(() => HandleClient(client));
            childSocketTask.Start();   
        }

        private static void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();

            while (true)
            {
                while (!stream.DataAvailable) ;
                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);

                String data = Encoding.UTF8.GetString(bytes);

                if (new Regex("^GET").IsMatch(data))
                {
                    Console.WriteLine("OK");
                    if (data.Contains("employee"))
                    {
                        SocketHelper.employees.Add(client);
                    }
                    if (data.Contains("customer"))
                    {
                        SocketHelper.customers.Add(client);
                    }
                    if (data.Contains("screen"))
                    {
                        SocketHelper.screens.Add(client);
                    }

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

                    FromClient(stream, data);
                }
                else
                {

                }

            }

            
        }

        static void FromClient(NetworkStream stream, string input)
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

                                      
                    if (input.Contains("customer"))
                    {
                        ServiceTypeViewModel order = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceTypeViewModel>(data);
                        Console.WriteLine(order.ServiceTypes.ToString());
                        Console.WriteLine(order.Regnumber);

                        SocketHelper.ToEmployees(order); 
                    }
                    if (input.Contains("employee"))
                    {
                        Console.WriteLine(data);
                        SocketHelper.ToScreens(data);
                    }

                }
            }
            stream.Close();
            _client.Close();

            //if (SocketHelper.customers.Contains(_client))
            //{
            //    SocketHelper.customers.Remove(_client);
            //}
            //if (SocketHelper.employees.Contains(_client))
            //{
            //    SocketHelper.employees.Remove(_client);
            //}
            //if (SocketHelper.screens.Contains(_client))
            //{
            //    SocketHelper.screens.Remove(_client);
            //}

        }

    }
}
