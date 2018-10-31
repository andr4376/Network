using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientThreadedServer
{
    namespace Client
    {
        class Program
        {
            static void Main(string[] args)
            {

                string ipAdresse = "";
                ClientDemo client;
                if (File.Exists("IP.txt"))
                {
                    ipAdresse = File.ReadAllText("IP.txt");

                }

                if (ipAdresse != string.Empty && File.Exists("IP.txt"))
                {
                    client = new ClientDemo(ipAdresse, 13000);

                }
                else
                {
                    client = new ClientDemo("127.0.0.1", 13000);

                }

            }
        }
    }
}
