using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;

namespace CoM_UnstackPoints
{
    [Guid("3c6e2da5-425c-47fd-98b8-f07c74da9aaf")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("CoM_UnstackPoints.CUnstackPoints")]
    public class CUnstackPoints : ESRI.ArcGIS.SystemUI.ICommand
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
            MxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IntPtr m_hBitmap;
        private System.Drawing.Bitmap m_bitmap;
        private IApplication m_pApplication;
        private fmUnstack m_oForm;
        private bool m_bChecked;

        #region "ICommand Implementations"
        public int Bitmap
        {
            get
            {
                return m_hBitmap.ToInt32();
            }
        }

        public string Caption
        {
            get
            {
                return "City of Medford: Unstack Points";
            }
        }

        public string Category
        {
            get
            {
                return "Medford GIS Tools";
            }
        }

        public bool Checked
        {
            get
            {
                return this.m_bChecked;
            }
        }

        public bool Enabled
        {
            get
            {
                return true;
            }
        }

        public int HelpContextID
        {
            get
            {
                return default(int);
            }
        }

        public string HelpFile
        {
            get
            {
                return default(string);
            }
        }

        public string Message
        {
            get
            {
                return "City of Medford: Unstack Points";
            }
        }

        public string Name
        {
            get
            {
                return "Medtools Unstack Points";
            }
        }

        public void OnClick()
        {
            this.m_bChecked = true;

            m_oForm = new fmUnstack();
            m_oForm.App = this.m_pApplication;
            m_oForm.ShowDialog(new WindowWrapper((System.IntPtr)m_pApplication.hWnd));
            m_oForm.Focus();

            this.m_bChecked = false;
        }

        public void OnCreate(object hook)
        {
            
            this.m_bChecked = false;

            m_pApplication = hook as IApplication;

            m_bitmap = new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream("CoM_UnstackPoints.Images.MovePoint.bmp"));
            if (m_bitmap != null)
            {
                m_bitmap.MakeTransparent(m_bitmap.GetPixel(1, 1));
                m_hBitmap = m_bitmap.GetHbitmap();
            }
        }

        public string Tooltip
        {
            get
            {
                return default(string);
            }
        }
        #endregion

    }
}
