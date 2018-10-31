using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

namespace serverThreadingDay3
{
    class Program
    {

        private static int Port = 13000;
        private static Object laas = new Object();
        public static string beskedTilAlle = "Faelles info";
        private static TcpListener _server;
        private static Boolean _isRunning;

        private static List<TcpClient> clients = new List<TcpClient>();

        private static TcpClient lastClientToSendMessage = null;


        static object listKey = new object();
        static void Main(string[] args)
        {

            Console.WriteLine("Multi-Threaded TCP Server Demo");

            TcpServer(Port);
        }

        public static void TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();
            _isRunning = true;

            Thread readMessagesThread = new Thread(ReadServerMessages);
            readMessagesThread.IsBackground = true;
            readMessagesThread.Start();

            LoopClients();
        }

        private static void ReadServerMessages()
        {
            while (true)
            {
                lock (listKey)
                {

                    if (beskedTilAlle != string.Empty && lastClientToSendMessage != null)
                    {
                        int index = 0;
                        for (int i = 0; i < clients.Count; i++)
                        {
                            if (clients[i] == lastClientToSendMessage)
                            {
                                index = i;
                            }
                        }

                        for (int i = 0; i < clients.Count; i++)
                        {

                            if (clients[i] != lastClientToSendMessage || HighNLow.isNumber == true)
                            {

                                StreamWriter sWriter = new StreamWriter(clients[i].GetStream(), Encoding.ASCII);


                                sWriter.WriteLine("Client(" + index + ")   <<" + DateTime.Now.ToShortTimeString() + ">>" + beskedTilAlle);
                                sWriter.Flush();
                                //beskedTilAlle = string.Empty;

                            }


                        }
                        lastClientToSendMessage = null;
                        beskedTilAlle = string.Empty;
                    }

                }


            }

        }

        public static void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
                // client found.

                clients.Add(newClient);

                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public static void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;
            // sets two streams
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested
            Boolean bClientConnected = true;
            String sData = null;
            IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IPEndPoint localPoint = (IPEndPoint)client.Client.LocalEndPoint;

            while (bClientConnected)
            {
                // reads from stream
                try
                {
                    sData = sReader.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(endPoint.Port.ToString() + " " + localPoint.Port.ToString() + " lukkede forbindelsen");
                    lock (listKey)
                    {
                        clients.Remove(client);
                    }
                    Thread.CurrentThread.Abort();
                }

                Console.WriteLine(DateTime.Now.ToShortTimeString() + "  :  " + EvaluateData(sData, client));


                //Console.WriteLine("Remote host port: " + endPoint.Port.ToString() + " Local socket port: " + localPoint.Port.ToString());
                // Thread.Sleep(5000);

            }
        }

        private static string EvaluateData(string sData, TcpClient client)
        {
            string result = HighNLow.EvaluateReply(sData);

            beskedTilAlle = result;
            lastClientToSendMessage = client;

            return result;
        }
    }
}
