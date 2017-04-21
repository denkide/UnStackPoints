using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.esriSystem;

namespace CoM_UnstackPoints
{
    [Guid("373cee22-4508-414b-9c54-f48aff242a38")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("CoM_UnstackPoints.CToolbar")]
    public class CToolbar : ESRI.ArcGIS.SystemUI.IToolBarDef
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Unregister(regKey);

        }

        #endregion
        #endregion

        #region "IToolBarDef Implementations"
        public string Caption
        {
            get
            {
                // TODO: Add ArcGISClass1.Caption getter implementation
                return "City of Medford: Unstack Points";//default(string);
            }
        }

        public void GetItemInfo(int pos, ESRI.ArcGIS.SystemUI.IItemDef itemDef)
        {
            // TODO: Add ArcGISClass1.GetItemInfo implementation
            UID pUID = new UIDClass();
            itemDef.Group = false;

            switch (pos)
            {
                case 0:
                    pUID.Value = "CoM_UnstackPoints.CUnstackPoints";
                    break;
            }

            itemDef.ID = pUID.Value.ToString();
        }

        public int ItemCount
        {
            get
            {
                // TODO: Add ArcGISClass1.ItemCount getter implementation
                return 1; // default(int);
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add ArcGISClass1.Name getter implementation
                return "City of Medford: Unstack Points";  //default(string);
            }
        }
        #endregion

    }
}
