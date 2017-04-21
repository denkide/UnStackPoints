using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data;
using System.IO;
using System.Data.Sql;
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
using ESRI.ArcGIS.LocationUI;
using ESRI.ArcGIS.Editor;

namespace CoM_UnstackPoints
{
    class CSubs : IDisposable
    {
        #region Destructor

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// This function creates a copy of a feature class in memory.
        /// The fields are added - including an x/y field for the shape.x and shape.y 
        ///     for use in the copy features function.
        /// </summary>
        /// <param name="pFC">a Feature class -- any fc will do (ie: FC, shapefile)</param>
        /// <returns>IFeatureClass - an in memory fc with 2 additional fields</returns>
        /// <notes>
        ///     Modified from Kirk Kuykendall's code from this post 
        ///         http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
        /// </notes>
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created. 
        /// 
        /// </changelog>
        public IFeatureClass cacheToMemory(IFeatureClass pFC, string xValue, string yValue)
        {
            IWorkspaceFactory pWSF = new InMemoryWorkspaceFactoryClass();

            IName pName = (IName)pWSF.Create("", "inmemory", null, 0);
            IFeatureWorkspace pFWS = (IFeatureWorkspace)pName.Open();

            IFields pFlds = this.copyFields(pFC.Fields, xValue, yValue);
            char[] period = { '.' };
            string[] vShpFld = pFC.ShapeFieldName.Split(period);

            IFeatureClass pInMemFC = (IFeatureClass)pFWS.CreateFeatureClass(pFC.AliasName, pFlds, pFC.CLSID, null, pFC.FeatureType, vShpFld[vShpFld.GetUpperBound(0)].ToString(), "");

            return pInMemFC;
        }

        /// <summary>
        /// This function copies the features from one fc to another.
        /// If there is a where clause it is used to filter the features.
        /// If ther xyExists, the shape.X and shape.Y are loaded into the fields
        /// </summary>
        /// <param name="pSourceFC">which features</param>
        /// <param name="pDestinationFC">where to put the features</param>
        /// <param name="sWhere">you can limit the features you want to add</param>
        /// <param name="xyExists">is there an x and a y field to add the shape.x and shape.y to</param>
        /// <returns>void</returns>
        /// <notes>
        ///     Modified from Kirk Kuykendall's code from this post 
        ///         http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
        /// </notes>
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created.
        /// 
        ///      11-7-2007:     djr
        ///         added test for empty point 
        /// </changelog>
        public void copyFeatures(IFeatureClass pSourceFC, IFeatureClass pDestinationFC, string sWhere, bool xyExists)
        {
            IQueryFilter pQF = null;
            if (sWhere.Trim().Length > 0)
            {
                pQF = new QueryFilterClass();
                pQF.WhereClause = sWhere;
            }

            IFeatureCursor pInFCur = pSourceFC.Search(pQF, false);
            IFeature pFeat = pInFCur.NextFeature();
            do
            {
                if (pFeat.Shape != null)
                {
                    IFeature pOutFeat = (IFeature)pDestinationFC.CreateFeature();
                    for (int l = 0; l < pOutFeat.Fields.FieldCount - 2; l++)
                    {
                        // todo maybe search by field alias to find match
                        if (pOutFeat.Fields.get_Field(l).Editable)
                            pOutFeat.set_Value(l, pFeat.get_Value(l));
                    }
                    IPoint pPoint = (IPoint)pFeat.Shape;

                    if (!pPoint.IsEmpty)
                    {
                        if (xyExists)
                        {
                            pOutFeat.set_Value(pOutFeat.Fields.FieldCount - 2, pPoint.X.ToString());
                            pOutFeat.set_Value(pOutFeat.Fields.FieldCount - 1, pPoint.Y.ToString());
                        }

                        pOutFeat.Store();
                    }
                }
                pFeat = pInFCur.NextFeature();
            }
            while (pFeat != null);
        }


        /// <summary>
        /// Exports a dbf file to a designated place from a layer
        /// </summary>
        /// <param name="sOutputLocation">The location to create the dbf</param>
        /// <param name="pFLayer">the Featue layer from which to export the data</param>
        /// <returns>string - the name of the exported dbf (without the extension)</returns>
        /// <notes>
        ///     Modified from Kirk Kuykendall's code from this post 
        ///         http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
        /// </notes>
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created. 
        /// 
        /// </changelog>
        public string createDBF(string sOutputLocation, IFeatureLayer pFLayer)
        {
            if (pFLayer == null)
            {
                MessageBox.Show("Feature layer was not found");
                return "";
            }

            try
            {
                ITable pTable = (ITable)pFLayer.FeatureClass;
                IDataset pDs = (IDataset)pTable;
                IDatasetName pDsName = (IDatasetName)pDs.FullName;

                IWorkspaceFactory pWkSpFact = new ShapefileWorkspaceFactoryClass();
                IWorkspace pWkSp = pWkSpFact.OpenFromFile(sOutputLocation, 0);
                IDataset pWkSpDs = (IDataset)pWkSp;

                IWorkspaceName pWkSpName = (IWorkspaceName)pWkSpDs.FullName;
                IDatasetName pOutDsName = new TableNameClass();
                pOutDsName.Name = pDs.Name + "_" + DateTime.UtcNow.DayOfYear + DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second;
                //pOutDsName.Name = pDs.Name;
                pOutDsName.WorkspaceName = pWkSpName;

                IExportOperation pExpOp = new ExportOperationClass();
                pExpOp.ExportTable(pDsName, null, null, pOutDsName, 0);

                return pOutDsName.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errors occurred: \r\n\r\n" + ex.Message);
                return null;
            }
            finally
            {

            }
        }


        /// <summary>
        /// This function sorts an input table,
        /// then it loops through each feature of the table to see if the x values are the same
        /// if they are the same, the x and y values are incremented by the offset sum amount
        /// if they are not the same the offset is reset to the original offset
        /// </summary>
        /// <param name="pTable">the table to sort and create offset xy values for the xy events</param>
        /// <param name="dblOffset">the amount in map units to offset the x and y</param>
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created. 
        /// 
        ///      11-7-2007:     djr
        ///         added test to see if med_X has a length greater than 0
        /// </changelog>
        public void createOffset(ITable pTable, Double dblXOffset, Double dblYOffset, string xValue, string yValue)
        {
            ITableSort pTableSort = new TableSortClass();
            pTableSort.Fields = xValue + "," + yValue;
            pTableSort.set_Ascending(xValue, false);
            pTableSort.set_Ascending(yValue, false);
            pTableSort.Table = pTable;
            pTableSort.Sort(null);

            ICursor pCursor = pTableSort.Rows; //pTable.Update(null, true);
            IRow pRow = pCursor.NextRow();

            string x1 = "";
            string y = "";
            string x2 = "";

            double dblXTemp = 0;
            double dblYTemp = 0;

            double dblXFactor = dblXOffset;
            double dblYFactor = dblYOffset;

            do
            {
                x1 = pRow.get_Value(pRow.Fields.FindField(xValue)).ToString();

                if (x1.Length > 0)
                {
                    if (x1 == x2)
                    {
                        y = pRow.get_Value(pRow.Fields.FindField(yValue)).ToString();

                        dblXTemp = (Double.Parse(x1)) + dblXFactor;
                        dblYTemp = (Double.Parse(y)) + dblYFactor;

                        pRow.set_Value(pRow.Fields.FindField(xValue), dblXTemp.ToString());
                        pRow.set_Value(pRow.Fields.FindField(yValue), dblYTemp.ToString());
                        pRow.Store();

                        dblXFactor += dblXOffset;
                        dblYFactor += dblYOffset;
                    }
                    else
                    {
                        dblXFactor = dblXOffset;
                        dblYFactor = dblYOffset;
                    }
                }
                x2 = x1;
                pRow = pCursor.NextRow();
            }
            while (pRow != null);
            pCursor.Flush();
        }


        /// <summary>
        /// this function takes a table and creates a shapefile (XY events) layer 
        /// that is then added to the map
        /// </summary>
        /// <param name="pMxDoc">the map doc</param>
        /// <param name="sWorkspacePath">where to put the shapefile that is generated from the xy events / and where the dbf lives</param>
        /// <param name="sTableName">the name of the dbf to open</param>
        /// <param name="pSpatRef">the spatial ref for the prj file for the shapefile</param>\        
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created. 
        /// 
        /// </changelog>
        public void addXYEvents(IMxDocument pMxDoc, string sWorkspacePath, string sTableName, ISpatialReference pSpatRef, string xField, string yField, string zField)
        {
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWSF.OpenFromFile(sWorkspacePath, 0);
            ITable pTable = (ITable)pFeatureWorkspace.OpenTable(sTableName);

            if (pTable == null)
            {
                MessageBox.Show("The table was not found");
                return;
            }

            // Create a new standalone table and add it
            // to the collection of the focus map
            IStandaloneTable pStTab = new StandaloneTableClass();
            pStTab.Table = (ITable)pTable;

            //sortTable(ref pTable);

            // Get the table name object
            IDataset pDataSet = (IDataset)pStTab;
            IName pTableName = pDataSet.FullName;

            // Specify the X and Y fields
            IXYEvent2FieldsProperties pXYEvent2FieldsProperties = new XYEvent2FieldsPropertiesClass();
            pXYEvent2FieldsProperties.XFieldName = xField;
            pXYEvent2FieldsProperties.YFieldName = yField;
            pXYEvent2FieldsProperties.ZFieldName = zField;

            // Create the XY name object and set it's properties
            IXYEventSourceName pXYEventSourceName = new XYEventSourceNameClass();
            pXYEventSourceName.EventProperties = pXYEvent2FieldsProperties;
            if (pSpatRef != null)
                pXYEventSourceName.SpatialReference = pSpatRef;
            pXYEventSourceName.EventTableName = pTableName;
            IName pXYName = (IName)pXYEventSourceName;
            IXYEventSource pXYEventSource = (IXYEventSource)pXYName.Open();

            // Create a new Map Layer
            IFeatureLayer pFLayer = new FeatureLayerClass();
            pFLayer.FeatureClass = (IFeatureClass)pXYEventSource;
            pFLayer.Name = sTableName;

            //Add the layer extension (this is done so that when you edit
            //the layer's Source properties and click the Set Data Source
            //button, the Add XY Events Dialog appears)
            XYDataSourcePageExtension pRESPageExt = new XYDataSourcePageExtension();
            ILayerExtensions pLayerExt = (ILayerExtensions)pFLayer;
            pLayerExt.AddExtension(pRESPageExt);

            //Get the FcName from the featureclass
            IFeatureClass pFc = pFLayer.FeatureClass;
            pDataSet = (IDataset)pFc;
            IFeatureClassName pINFeatureClassName = (IFeatureClassName)pDataSet.FullName;
            IDatasetName pInDsName = (IDatasetName)pINFeatureClassName;

            //Get the selection set
            IFeatureSelection pFSel = (IFeatureSelection)pFLayer;
            ISelectionSet pSelSet = (ISelectionSet)pFSel.SelectionSet;

            //Define the output feature class name
            IFeatureClassName pFeatureClassName = new FeatureClassNameClass();
            IDatasetName pOutDatasetName = (IDatasetName)pFeatureClassName;
            //string sDSName = ensureDataName(pDataSet.Name, sWorkspacePath);

            string sDSName = pDataSet.Name; // +DateTime.UtcNow.DayOfYear + DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second;
            pOutDatasetName.Name = sDSName;

            IWorkspaceName pWorkspaceName = new WorkspaceNameClass();
            pWorkspaceName.PathName = sWorkspacePath;
            pWorkspaceName.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapeFileWorkspaceFactory";

            pOutDatasetName.WorkspaceName = pWorkspaceName;
            pFeatureClassName.FeatureType = esriFeatureType.esriFTSimple;
            pFeatureClassName.ShapeType = esriGeometryType.esriGeometryPoint;
            pFeatureClassName.ShapeFieldName = "Shape";

            //Export
            IExportOperation pExportOp = new ExportOperationClass();
            pExportOp.ExportFeatureClass(pInDsName, null, null, null, pOutDatasetName as IFeatureClassName, 0);

            IFeatureClass pClass = (IFeatureClass)pFeatureWorkspace.OpenFeatureClass(sDSName);
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pClass;
            pLayer.Name = sDSName;//pClass.AliasName;

            pMxDoc.AddLayer(pLayer);
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pLayer, null);
        }

        #endregion

        #region Private Functions

        private IClone clone(IClone pClone)
        {
            return pClone.Clone();
        }

        /// <summary>
        /// this makes a copy of the fields that are inputted.
        /// if an x / y field is included, those are added as well
        /// </summary>
        /// <param name="pFlds">the fields to copy</param>
        /// <param name="sXFieldName">the name of the x field</param>
        /// <param name="sYFieldName">the name of the y field</param>
        /// <returns>IFields - the new fields collection</returns>
        /// <notes>
        ///     Modified from Kirk Kuykendall's code from this post 
        ///         http://forums.esri.com/Thread.asp?c=93&f=993&t=210767&mc=12#msgid652601
        /// </notes>
        /// <changelog>
        ///      10-31-2007:    djr
        ///         comments created. 
        /// 
        /// </changelog>
        private IFields copyFields(IFields pFlds, string sXFieldName, string sYFieldName)
        {
            IFieldsEdit pFldsEdit = new FieldsClass();
            for (int i = 0; i < pFlds.FieldCount; i++)
            {
                char[] period = { '.' };
                string[] v = pFlds.get_Field(i).Name.Split(period);

                IFieldEdit pFldEdit = (IFieldEdit)this.clone(pFlds.get_Field(i) as IClone);
                pFldEdit.Name_2 = v[v.GetUpperBound(0)].ToString();
                pFldsEdit.AddField((IField)pFldEdit);
            }

            if (sXFieldName.Length > 0)
            {
                IField pFieldX = new FieldClass();
                IFieldEdit pFieldEditX = (IFieldEdit)pFieldX;

                pFieldEditX.AliasName_2 = sXFieldName;
                pFieldEditX.Name_2 = sXFieldName;
                pFieldEditX.Type_2 = esriFieldType.esriFieldTypeDouble;
                pFieldEditX.Editable_2 = true;
                pFieldEditX.IsNullable_2 = true;

                pFldsEdit.AddField((IField)pFieldEditX);
            }

            if (sYFieldName.Length > 0)
            {
                IField pFieldY = new FieldClass();
                IFieldEdit pFieldEditY = (IFieldEdit)pFieldY;

                pFieldEditY.AliasName_2 = sYFieldName;
                pFieldEditY.Name_2 = sYFieldName;
                pFieldEditY.Type_2 = esriFieldType.esriFieldTypeDouble;
                pFieldEditY.Editable_2 = true;
                pFieldEditY.IsNullable_2 = true;

                pFldsEdit.AddField((IField)pFieldEditY);
            }

            return pFldsEdit;
        }

        #endregion
    }
}
