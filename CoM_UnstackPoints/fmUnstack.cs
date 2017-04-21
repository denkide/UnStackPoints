using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.Sql;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMap;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geoprocessing;

///
/// This application takes a point feature class as an input
/// then creates a copy of the points in memory. 
/// Those points are then used to create an xy events layer that
/// has the points offset at the user specified distance 
/// in a north easterly direction.
/// 
/// The idea of the application was to create a tool that would
/// allow stacked points to be symbolized and labeled independently.
/// 
/// This code has been created with the help from various posts on
/// the ESRI Support site along with custom code written by me.
/// 
/// Kirk Kuykendall's code from this post 
/// http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
/// is what is used for the creating in memory feature class.
/// 
/// The application has been tested with point data from shapefiles
/// and geodatabase feature classes.  
/// 
/// Contact Info
/// -------------------------------------------------
/// Organization:   City Of Medford GIS
/// Location:       Medford, Or, United States
/// Programmer:     David Renz
/// Email:          djrenz@cityofmedford.org
/// 

namespace CoM_UnstackPoints
{
    public partial class fmUnstack : Form
    {
        #region Module Level Variables

        private IApplication m_pApp;
        private IGxObject m_pGXObj;
        private ISpatialReference m_pSpatRef;

        #endregion

        #region Properties

        public IApplication App
        {
            get { return m_pApp; }
            set { m_pApp = value; }
        }

        #endregion

        #region Constructor

        public fmUnstack()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void fmUnStack_Load(object sender, EventArgs e)
        {
            m_pGXObj = null;
        }

        private void btnDataSource_Click(object sender, EventArgs e)
        {
            try
            {
                IGxCatalog pGxCat = new GxCatalogClass();
                IGxDialog pGxDialog = new GxDialogClass();

                pGxDialog.AllowMultiSelect = false;
                pGxDialog.ButtonCaption = "Open";

                IGxObjectFilter pGxFilter = new GxFilterPointFeatureClassesClass();
                pGxDialog.ObjectFilter = pGxFilter;

                IEnumGxObject pDataFiles;
                pGxDialog.DoModalOpen(0, out pDataFiles);

                if (pDataFiles != null)
                {
                    m_pGXObj = pDataFiles.Next();
                    ///
                    /// added check for null to handle null m_pGXObj
                    /// DJR: 11/05/2007
                    /// 
                    if (m_pGXObj != null)
                        txtOtherData.Text = m_pGXObj.FullName.ToString();
                }
            }
            catch (Exception ex)
            {
                m_pGXObj = null;
            }
            this.Focus();
        }

        /// <summary>
        /// btnOK_Click
        ///     Standard click event.
        ///     This kicks off the whole process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///<notes>
        ///     11-7-2007:      djr
        ///         added a new lblMessage textbox for error messages and a 
        ///         try catch to catch all errors.
        /// </notes> 
        ///
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!validateForm())
                return;

            this.lblMessage.Text = "";
            this.lblMessage.ForeColor = Color.Black;

            try
            {
                if (m_pGXObj is IGxDataset)
                {
                    IGxDataset pGxDS = (IGxDataset)m_pGXObj;
                    IDataset pDS = (IDataset)pGxDS.Dataset;

                    if (pDS is IFeatureClass)
                    {
                        this.Enabled = false;
                        this.Cursor = Cursors.WaitCursor;

                        IMxDocument pMxDoc = (IMxDocument)this.App.Document;

                        IFeatureLayer pFLayer = new FeatureLayerClass();
                        pFLayer.Name = pDS.Name + "_2";

                        using (CSubs oSubs = new CSubs())
                        {
                            string xValue = "med_X";
                            string yValue = "med_Y";

                            pFLayer.FeatureClass = oSubs.cacheToMemory(pDS as IFeatureClass, xValue, yValue);
                            oSubs.copyFeatures(pDS as IFeatureClass, pFLayer.FeatureClass, "", true);

                            ITable pTable = (ITable)pFLayer.FeatureClass;
                            oSubs.createOffset(pTable, Double.Parse(txtXOffset.Text), Double.Parse(txtYOffset.Text), xValue, yValue);

                            string sTableName = oSubs.createDBF(txtOutputLocation.Text, pFLayer);
                            if (sTableName.Length > 0)
                                oSubs.addXYEvents(pMxDoc, txtOutputLocation.Text, sTableName, m_pSpatRef, xValue, yValue, "");
                        }
                    }
                    this.closeForm();
                }
                else
                {
                    MessageBox.Show("The selected data is not valid.");
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "Errors Occurred: " + ex.Message;
                this.lblMessage.ForeColor = Color.Red;
                this.txtOtherData.Text = "";
                this.txtOutputLocation.Text = "";
                this.txtSpatialRef.Text = "";
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                this.m_pGXObj = null;
                this.m_pSpatRef = null;
            }
        }

        private void btnOutputLoc_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputLocation.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSpatialRef_Click(object sender, EventArgs e)
        {
            if (m_pGXObj == null)
            {
                MessageBox.Show("Please select a FeatureLayer first");
                return;
            }
            ISpatialReferenceDialog pSpatRefDialog = new SpatialReferenceDialogClass();
            m_pSpatRef = pSpatRefDialog.DoModalCreate(false, false, false, 0);

            txtSpatialRef.Text = m_pSpatRef.Name.ToString();
            this.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.closeForm();
        }

        private void fmUnstack_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.closeForm();
        }

        #endregion

        #region Private Functions

        private bool validateForm()
        {
            try { Double.Parse(txtXOffset.Text); }
            catch
            {
                MessageBox.Show("Please enter a numeric value for the X offset value.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            try { Double.Parse(txtYOffset.Text); }
            catch
            {
                MessageBox.Show("Please enter a numeric value for the Y offset value.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (txtOtherData.Text.Length < 1)
            {
                MessageBox.Show("Please select the source data that you would like to use.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (txtOutputLocation.Text.Length < 1)
            {
                MessageBox.Show("Please select the output location to use for the export.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!Directory.Exists(txtOutputLocation.Text))
            {
                MessageBox.Show("The output location could not be found.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            try
            {
                string sStub = "_" + DateTime.UtcNow.DayOfYear + DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + "_";
                System.IO.FileStream fs = File.Create(txtOutputLocation.Text + "\\test" + sStub + ".txt");
                fs.Close();
                File.Delete(txtOutputLocation.Text + "\\test" + sStub + ".txt");
            }
            catch
            {
                MessageBox.Show("The output location could not be written to.", "Errors Occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void closeForm()
        {
            this.App.CurrentTool = null;
            //this.Hide();
            this.Dispose();
        }

        #endregion


    }
}