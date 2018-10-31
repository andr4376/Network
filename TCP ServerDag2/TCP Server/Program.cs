using System;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    class MyTcpListener
    {
        public static void Main()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                Console.WriteLine(localAddr.ToString());
                server = new TcpListener(localAddr, port);
                server.Start();
                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {


                    Console.Write("Waiting message...");
                    TcpClient client = server.AcceptTcpClient();//herstopperkoden
                    Console.WriteLine("Connected!");
                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received from client:{0}", data);

                        EvaluateReply(data, stream);

                        //data = data.ToUpper();
                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent to client:{0}", data);
                        //Console.WriteLine();
                    }
                    client.Close();

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();


        }
        public static void EvaluateReply(string message, NetworkStream stream)
        {
            string reply = HighNLow.EvaluateReply(message);



            if (reply.Length > 0)
            {
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(reply);
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent to client:{0}", reply);
                Console.WriteLine();
            }
        }
    }
}