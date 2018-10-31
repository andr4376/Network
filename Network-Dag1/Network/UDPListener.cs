using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Network
{
    class UDPListener
    {
        private const int listenPort = 11000; //

        public static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            while (true)
            {

                try
                {
                    Console.WriteLine("Waiting for Broadcast...");
                    byte[] bytes = listener.Receive(ref groupEP); //while loop untill it receives message


                    //decodes the ASCII byte array
                    Console.WriteLine("Received broadcast from {0} : \n{1}\n",
                        groupEP.ToString(), Encoding.ASCII.GetString 
                        (bytes, 0, bytes.Length));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());

                }
                //finally
                //{
                //    listener.Close();
                //}
            }

        }

        public static void Send()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //stefan ip :  10.131.69.236
            //min ip: 10.131.68.224

            /*for at finde IP - åben command prompt og skriv "ipconfig" */

            IPAddress serverAddr = IPAddress.Parse("10.131.69.236"); // the IP to send message to


            //the endpoint for the message to send (ip + port)
            IPEndPoint endPoint = new IPEndPoint(serverAddr, listenPort); 

            string text = Console.ReadLine(); //message to write

            byte[] send_buffer = Encoding.ASCII.GetBytes(text); // encode message to ASCII
            
            sock.SendTo(send_buffer, endPoint); //Send Ascii message to 
        }

        private void WriteAsciiCode(byte[] bytes)
        {
            foreach (byte bte in bytes)
            {
                Console.Write(bte);
            }
        }
    }

    
}
