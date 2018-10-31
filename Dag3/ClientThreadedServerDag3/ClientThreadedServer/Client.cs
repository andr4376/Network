using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ClientThreadedServer
{
    class ClientDemo
    {
        private TcpClient _client;

        private StreamReader _sReader;
        private StreamWriter _sWriter;

        private Boolean _isConnected;

        private string name;

        public ClientDemo(string ipAddress, int portNum)
        {
            _client = new TcpClient();
            _client.Connect(IPAddress.Parse(ipAddress), portNum);

            //Console.WriteLine("Name please");
            //name = Console.ReadLine();

            Thread receiveMessageThread = new Thread(ReceiveMessages);

            receiveMessageThread.IsBackground = true;

            receiveMessageThread.Start();


            HandleCommunication();



        }

        public void HandleCommunication()
        {
            NetworkStream ns = _client.GetStream();
            _sReader = new StreamReader(ns, Encoding.UTF8);
            _sWriter = new StreamWriter(ns, Encoding.UTF8);
            String sData = null;
            _isConnected = true;

            while (_isConnected)
            {
              
                sData = /* name + ":>"+ */Console.ReadLine();
                // write data and make sure to flush, or the buffer will continue to 
                // grow, and your data might not be sent when you want it, and will
                // only be sent once the buffer is filled.
                _sWriter.WriteLine(sData);
                try
                {
                    _sWriter.Flush();

                    // if you want to receive anything

                }
                catch (IOException e)
                {
                    Console.WriteLine("Serveren er lukket ned");
                    Console.ReadLine();
                    Thread.CurrentThread.Abort();
                }

            }
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {


                    String sDataIncomming = string.Empty;

                    if (_sReader != null)
                    {
                    sDataIncomming = _sReader.ReadLine();

                    }
                    if (sDataIncomming != string.Empty)


                    Console.WriteLine(sDataIncomming);

                    Thread.Sleep(30);



                }
                catch (Exception e)
                {
                    

                }
               
            }
        }
    }




}
