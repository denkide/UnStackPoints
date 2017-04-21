using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;

namespace CoM_UnstackPoints
{
    [RunInstaller(true)]
    public partial class CInstaller : Installer
    {
        public CInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            RegistrationServices regSrv = new RegistrationServices();
            regSrv.RegisterAssembly(base.GetType().Assembly,
              AssemblyRegistrationFlags.SetCodeBase);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            RegistrationServices regSrv = new RegistrationServices();
            regSrv.UnregisterAssembly(base.GetType().Assembly);
        }

    }
}