﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer
{
    using System.Net.Sockets;
    using System.Net;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Security.Cryptography;
    using System.Net.WebSockets;

    class Program
    {
        static NetworkStream stream;
        static Task task = new Task(() => FromClient());
        static TcpClient client;

        public static void Main()
        {

            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:8000.{0}Waiting for a connection...", Environment.NewLine);
            client = server.AcceptTcpClient();
            Console.WriteLine("A client connected.");
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

                    stream.Write(response, 0, response.Length); //Avsluta handskakningen
                                                                // task.Start();
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

                //beroende på om klient är kund el anställd så ska olika saker göras
                ToClient(data);
            }
            stream.Close();
            client.Close();
        }

        static void ToClient(string input)
        {
            var s = "Echo:" + input;
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