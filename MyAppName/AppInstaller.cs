using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAppName
{
    [RunInstaller(true)]
    public partial class AppInstaller : System.Configuration.Install.Installer
    {
        public AppInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            bool isStartup = Context.Parameters["chb__IsStartup"] == "1";
            bool isAllUsers = Context.Parameters["allusers"] == "1";
            string appName = "MyAppName";
            string assemblypath = this.Context.Parameters["assemblypath"];
            if (isStartup && string.IsNullOrEmpty(assemblypath) == false)
            {
                try
                {
                    if (isAllUsers)
                    {
                        RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        reg.SetValue(appName, this.Context.Parameters["assemblypath"]);
                    }
                    else
                    {
                        RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                        reg.SetValue(appName, this.Context.Parameters["assemblypath"]);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Application.ProductName, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            string appName = "MyAppName";

            RegistryKey regAllUsers = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey regCurrentUsers = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            if (regAllUsers.GetValue(appName) != null)
            {
                try
                {
                    regAllUsers.DeleteValue(appName);
                }
                catch { }
            }

            if (regCurrentUsers.GetValue(appName) != null)
            {
                try
                {
                    regCurrentUsers.DeleteValue(appName);
                }
                catch { }
            }
        }
    }
}
