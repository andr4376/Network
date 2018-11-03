using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ClientDraw
{
    public partial class ClientForm : Form
    {

        //network
        private TcpClient _client;

        private StreamReader _sReader;
        private StreamWriter _sWriter;

        private Boolean _isConnected;
        //10.131.69.236"
        private string ip = "127.0.0.1";
        int port = 25565;
        //

        Graphics graphics;

        Pen pen;

        //current coordinates
        #region Prototype fields
        // coordinates out of screen
        int x = -1;
        int y = -1;
        #endregion;

        //client that draws
        TcpClient drawClient;

        //clients that can see the drawing
        List<TcpClient> slaveClients = new List<TcpClient>();

        // use these instead with server:
        int oldX = -1;
        int oldY = -1;

        int newX = -1;
        int newY = -1;

        Thread messageReceiver;


        //Determines whether it should draw or not
        private bool isDrawing = false;

        public ClientForm()
        {
            InitializeComponent();
        }

        //current functionality
        #region Prototype Methods
        /// <summary>
        /// Allows enables drawing and sets starting position from which to draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs mouse)
        {
            //Method should be in drawclient, and should sent its mouse coordinates

            isDrawing = true;


            oldX = this.PointToClient(Cursor.Position).X;
            oldY = this.PointToClient(Cursor.Position).Y;

        }

        private void panel1_MouseMove(object sender, MouseEventArgs mouse)
        {
            //Should be in an Update Method
            if (isDrawing && oldX != -1 && oldY != -1)
            {
                newX = this.PointToClient(Cursor.Position).X;
                newY = this.PointToClient(Cursor.Position).Y;

                if (oldX != newX && oldY != newY)
                {
                    //draw line from old position to new position

                    graphics.DrawLine(pen, new Point(oldX, oldY), new Point(newX, newY));

                    SendToServer(string.Format("{0},{1},{2},{3}", oldX, oldY, newX, newY));

                    oldX = newX;
                    oldY = newY;
                    //Method should be in drawclient, and should sent its mouse coordinates
                    //Update coordinates

                }




                Thread.Sleep(10);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs mouse)
        {
            isDrawing = false;

            x = -1;
            y = -1;

        }



        #endregion

        //To add:

        private void UpdateDrawing()
        {
            while (true)
            {


            }
        }

        /// <summary>
        /// Receives Messages from drawClient
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {

                //receive message from drawclient
                string message = ".";

                MessageHandler(message);


            }

        }
        /// <summary>
        /// Handles received data // not implemented
        /// </summary>
        private void MessageHandler(string message)
        {
            // if  "X,Y"
            if (message.Contains(","))
            {
                //Isolates the X and the Y
                ConvertCoordinates(message);
            }

            //Should it stop or start drawing? binary to minimize package size??? does it even matter???
            else if (message == "0")
            {
                isDrawing = false;
            }
            else if (message == "1")
            {
                isDrawing = true;
            }
        }

        private void SendCoordinates(int oX, int oY, int nX, int nY)
        {
            string message = string.Format("{0},{1},{2},{3}", oX, oY, nX, nY);


            //  ConvertCoordinates(message);

            SendToServer(message);

        }
        public void SendToServer(string coordinatesString)
        {
            String sData = null;
            _isConnected = true;



            sData = coordinatesString;


            // write data and make sure to flush, or the buffer will continue to 
            // grow, and your data might not be sent when you want it, and will
            // only be sent once the buffer is filled.
            try
            {
                _sWriter.WriteLine(sData);
                _sWriter.Flush();

                // if you want to receive anything

            }
            catch (IOException)
            {
                Console.WriteLine("Serveren er lukket ned");
                Console.ReadLine();
                Thread.CurrentThread.Abort();
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

            Point[] cords = new Point[1];



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
                Int32.TryParse(coordinates[1], out newX);
            }   //X
            if (coordinates[3].All(char.IsDigit))
            {
                //Updates new x position
                Int32.TryParse(coordinates[0], out newY);

            }

            cords[0] = new Point(oldX, oldY);
            cords[1] = new Point(newX, newY);


        }

        /// <summary>
        /// Runs when finished loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_Load(object sender, EventArgs e)
        {
            graphics = panel1.CreateGraphics();

            //Smoother edges
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            pen = new Pen(Color.Black, 6f);

            //Draws smoother lines
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;


            //Starts thread that receives messages from DrawClient
            //messageReceiver = new Thread();
            //messageReceiver.IsBackground = true;
            //messageReceiver.Start();


            _client = new TcpClient();
            _client.Connect(IPAddress.Parse(ip), port);
            NetworkStream ns = _client.GetStream();
            _sReader = new StreamReader(ns, Encoding.ASCII);
            _sWriter = new StreamWriter(ns, Encoding.ASCII);
            SendToServer("Draw");
        }
    }
}
