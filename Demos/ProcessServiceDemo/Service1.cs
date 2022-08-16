using ColorsWin.Process;
using ColorsWin.Process.Helpers;
using Process.ShareTest;
using System;
using System.ServiceProcess;

namespace ProcessServiceDemo
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private string processCommandLineTag = "ProcessCommandLineTag";
        private string processCommandLine = "ProcessCommandLine";
        private string processCommandLineResult = "processCommandLineResult";

        protected override void OnStart(string[] args)
        {
            ProcessMessageConfig.Log = new FileLog("d:\\log.txt");
            try
            {
                ProcessMessageManager.InitMessageType(processCommandLineTag, ProcessMessageType.File);               
                ProcessMessageManager.SendMessage(processCommandLineTag, DateTime.Now.ToString() + "_Runing");

                ProcessMessageManager.InitMessageType(processCommandLine, ProcessMessageType.File);
                ProcessMessageManager.AcceptMessage(processCommandLine, InvokeCommand, true);           

                ProcessMessageManager.InitMessageType(processCommandLineResult, ProcessMessageType.File);

            }
            catch (Exception ex)
            {
                LogHelper.Debug("Start Error:" + ex.Message);
            }
        }

        private void InvokeCommand(string arg)
        {
            LogHelper.Debug("Message:" + arg);

            System.Threading.Thread.Sleep(new Random().Next(10, 600));

            ProcessMessageManager.SendMessage(processCommandLineResult, DateTime.Now + " Action Dome");
        }

        protected override void OnStop()
        {
            try
            {
                ProcessMessageManager.SendMessage(processCommandLineTag, "");
            }
            catch (Exception ex)
            {
                LogHelper.Debug("Stop  Error:" + ex.Message);
            }
        }
    }
}
