using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
   public static class MyTcpClient
    {
       public static void Connect(String server, String message)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent to server: {0}", message);
                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received from server: {0}", responseData);
                Console.WriteLine();
                stream.Close();
                client.Close();
            }

            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException:{0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException:{0}", e);
            }
            
            
        }
    }
}

