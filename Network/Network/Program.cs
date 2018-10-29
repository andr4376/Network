using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    class Program
    {

        static Thread listenerThread;
            
        static void Main(string[] args)
        {
            #region readmessageThread

            listenerThread = new Thread(UDPListener.StartListener);

            listenerThread.Start();
            listenerThread.IsBackground = true;
            
            #endregion;
            while (true)
            {
                UDPListener.Send();
               // Console.ReadLine();
            }
        }
    }
}
