using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerDraw
{
    public partial class ServerForm : Form
    {
        ServerTCPHandler SHandler;
        Thread serverHandeler;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            SHandler = new ServerTCPHandler();

            serverHandeler = new Thread(SHandler.TcpListener);
            serverHandeler.Start();
            serverHandeler.IsBackground = true;
        }


    }
}
