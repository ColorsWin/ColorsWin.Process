using ColorsWin.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Process.ClientDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProcessMessageConfig.NamePieService = false;
            ProcessMessageManager.Init();
        }

        string processKey = "qwe";
        private void btnRun_Click(object sender, EventArgs e)
        {
            ProcessMessageManager.SendMessage(processKey, "运行");
            textBox2.Text = processKey + ProcessMessageManager.ReadMessage(processKey);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ProcessMessageManager.UnInit(processKey);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
