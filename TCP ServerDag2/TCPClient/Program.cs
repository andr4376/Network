using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                string message = Console.ReadLine();

                if (message.Length > 0)
                {

                    MyTcpClient.Connect("127.0.0.1", message);
                }
            }
        }
    }
}
