using ColorsWin.Process;
using System;
using System.Windows.Forms;

namespace Process.ServiceDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProcessMessageConfig.NamePieService = true;
            ProcessMessageManager.Init();
        }

        string processKey = "qwe";
        private void btnCheckRun_Click(object sender, EventArgs e)
        {           
            this.Text = "服务器";
            var runing = ProcessHelper.IsRuning(processKey);
            if (runing)
            {
                textBox1.Text = "运行";
            }
            else
            {
                textBox1.Text = "未运行";
            }
        }
    }
}
