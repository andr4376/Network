using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientViewDraw
{
    public partial class ViewForm : Form
    {
        private static readonly object key = new object();
        Graphics graphics;
        Pen pen;
        private static TcpClient slaveClient;
        private static StreamReader sReader;
        private static StreamWriter sWriter; //Maybe if we want to make a game

        private static bool connected;
        private static int portNumber = 25565;
        private static string iPAddress = "127.0.0.1";
        private static List<Point[]> drawCordinates = new List<Point[]>();
        Thread viewThread;

        int oldX = -1;
        int oldY = -1;

        int newX = -1;
        int newY = -1;

        private static List<Point[]> DrawCordinates
        {
            get
            {
                lock (key)
                {
                    return drawCordinates;
                }
            }
            set
            {
                lock (key)
                {
                    drawCordinates = value;
                }
            }
        }

        public ViewForm()
        {
            InitializeComponent();
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            viewThread = new Thread(ClientSetUp);
            viewThread.Start();
            viewThread.IsBackground = true;
            graphics = panel1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            pen = new Pen(Color.Black, 6f);

            //Draws smoother lines
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;


        }


        private void ClientSetUp()
        {
            slaveClient = new TcpClient();
            slaveClient.Connect(IPAddress.Parse(iPAddress), portNumber);//Lets the client connect.

            GetDrawing();
        }
        private void GetDrawing()
        {
            NetworkStream getStream = slaveClient.GetStream();//Gets info from client
            sReader = new StreamReader(getStream, Encoding.ASCII);
            sWriter = new StreamWriter(getStream, Encoding.ASCII);
            sWriter.WriteLine("View");
            sWriter.Flush();
            connected = true;
            string sData = null;

            while (connected)
            {
                try
                {
                    sData = sReader.ReadLine();
                    Recieve(sData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    connected = false;
                }
                Thread.Sleep(17);
            }

        }

        private void Recieve(string sData)
        {
            if (sData != null && sData != string.Empty)
            {
                ConvertCoordinates(sData);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }


        private void Draw()
        {
            lock (key)
            {
                foreach (Point[] point in DrawCordinates)
                {
                    graphics.DrawLine(pen, point[0], point[1]);
                }

                DrawCordinates.Clear();
            }
        }


        /// <summary>
        /// Converts the string message containing coordinates, and updates the new x&y
        /// </summary>
        /// <param name="message"></param>
        private void ConvertCoordinates(string message)
        {

            //creates string array that contains the elements on each side of ',' char

            string[] coordinates = message.Split(',');

            Point[] cords = new Point[2];



            //X
            if (coordinates[0].All(char.IsDigit))
            {
                //Updates new x position
                Int32.TryParse(coordinates[0], out oldX);

            }

            //y
            if (coordinates[1].All(char.IsDigit))
            {
                //Updates new y position
                Int32.TryParse(coordinates[1], out oldY);
            }

            //newx
            if (coordinates[2].All(char.IsDigit))
            {
                //Updates new y position
                Int32.TryParse(coordinates[2], out newX);
            }   //X
            if (coordinates[3].All(char.IsDigit))
            {
                //Updates new x position
                Int32.TryParse(coordinates[3], out newY);

            }

            cords[0] = new Point(oldX, oldY);
            cords[1] = new Point(newX, newY);


            DrawCordinates.Add(cords);
        }
    }
}
