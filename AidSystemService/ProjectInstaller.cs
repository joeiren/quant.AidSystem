using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Diagnostics;


namespace AidSystemService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            AidServiceEventLogInstaller eventInstaller = new AidServiceEventLogInstaller();
            this.Installers.Add(eventInstaller.EventInstaller);

            this.aidServiceInstaller.AfterInstall += new InstallEventHandler(aidServiceInstaller_AfterInstall);
            this.aidServiceInstaller.AfterRollback += new InstallEventHandler(aidServiceInstaller_AfterRollback);
            this.aidServiceInstaller.AfterUninstall += new InstallEventHandler(aidServiceInstaller_AfterUninstall);

            this.aidServiceInstaller.BeforeInstall += new InstallEventHandler(aidServiceInstaller_BeforeInstall);
            this.aidServiceInstaller.BeforeRollback += new InstallEventHandler(aidServiceInstaller_BeforeRollback);
            this.aidServiceInstaller.BeforeUninstall += new InstallEventHandler(aidServiceInstaller_BeforeUninstall);
        }

        void aidServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            StopService();
        }

        void aidServiceInstaller_BeforeRollback(object sender, InstallEventArgs e)
        {
            StopService();
        }

        void aidServiceInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            StopService();
        }

        void aidServiceInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            StopService();
        }

        void aidServiceInstaller_AfterRollback(object sender, InstallEventArgs e)
        {
            StopService();
        }

        void aidServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            StartService();
        }

        void StartService()
        {
            System.ServiceProcess.ServiceController serverContorler = new ServiceController(this.aidServiceInstaller.ServiceName);

            if (serverContorler != null)
            {
                if (serverContorler.Status != ServiceControllerStatus.Running)
                {
                    serverContorler.Start();
                    serverContorler.Dispose();
                }

            }

        }

        void StopService()
        {
            System.ServiceProcess.ServiceController serverContorler = new ServiceController(this.aidServiceInstaller.ServiceName);

            if (serverContorler != null)
            {
                if (serverContorler.CanStop)
                {
                    serverContorler.Stop();
                    serverContorler.Dispose();
                }

            }
        }
    }
}
