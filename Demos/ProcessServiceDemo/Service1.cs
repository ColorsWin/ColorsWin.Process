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

        protected override void OnStart(string[] args)
        {            
            try
            {
                TestWinService.RunService();
            }
            catch (Exception ex)
            {
                LogHelper.Debug("Start Error:" + ex.Message);
            }
        }


        protected override void OnStop()
        {
            try
            {
                TestWinService.StopService();
            }
            catch (Exception ex)
            {
                LogHelper.Debug("Stop  Error:" + ex.Message);
            }
        }
    }
}
