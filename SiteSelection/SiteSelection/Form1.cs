using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace SiteSelection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void SiteSelection1()
        {
            //****** Author:  James Carpenter
            //******** Date:  2/28/2017
            //* Description:  This procedure automates the site selection process for
            //*               Jen and Barry's Ice Cream Store. The form takes in criteria
            //*               and the output is a selection of cities that fit that criteria.
            //****** Locals:  pMxDoc, pMap, pLayers, pLayer, pCountyLayer, pCityLayer, 
            //******          pQueryFilter, pCountyFClass, pFCursor, pEnumGeom, pEnumGeomBind,
            //******          pGeomFactory, pGeom, pSpatialFilter, pQueryFilter2, pCityFClass,
            //******          pFCursor2, pFeature2, pfSel, pActiveView
            //****************************************

            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;

            IMap pMap;
            pMap = pMxDoc.FocusMap;

            IEnumLayer pLayers;
            pLayers = pMap.Layers;

            ILayer pLayer;
            pLayer = pLayers.Next();

            // Delcaring the feature layers
            IFeatureLayer pCountyLayer = null;
            IFeatureLayer pCityLayer = null;

            // Declaring the names of the layers
            while (pLayer != null)
            {
                if (pLayer.Name == "counties")
                {
                    pCountyLayer = (IFeatureLayer)pLayer;
                    break;
                }
                else if (pLayer.Name == "cities")
                {
                    pCityLayer = (IFeatureLayer)pLayer;
                }
                pLayer = pLayers.Next();
            }

            // Declare variables
            int farms;
            farms = Convert.ToInt32(textBox1.Text);
            int labor;
            labor = Convert.ToInt32(textBox2.Text);
            int popden;
            popden = Convert.ToInt32(textBox3.Text);
            int uni;
            uni = Convert.ToInt32(textBox4.Text);
            double crime;
            crime = Convert.ToDouble(textBox5.Text);

            // Counties Query
            IQueryFilter pQueryFilter;
            pQueryFilter = new QueryFilter();
            pQueryFilter.WhereClause = "NO_FARMS87 > (" + farms + ") AND AGE_18_64 >= (" + labor + ") AND POP_SQMILE < (" + popden + ")";
            IFeatureClass pCountyFClass;
            pCountyFClass = pCountyLayer.FeatureClass;

            IFeatureCursor pFCursor;
            pFCursor = pCountyFClass.Search(pQueryFilter, true);


            // Move to the next feature
            IFeature pFeature;
            pFeature = pFCursor.NextFeature();

            // Merge the selected geometries

            IEnumGeometry pEnumGeom;
            pEnumGeom = new EnumFeatureGeometry();

            // Bind the selected county geometries based on the Query Filter
            IEnumGeometryBind pEnumGeomBind;
            pEnumGeomBind = (IEnumGeometryBind)pEnumGeom;
            pEnumGeomBind.BindGeometrySource(pQueryFilter, pCountyFClass);

            IGeometryFactory pGeomFactory;
            pGeomFactory = (IGeometryFactory)new GeometryEnvironment();

            // Create the new geometry
            IGeometry pGeom;
            pGeom = pGeomFactory.CreateGeometryFromEnumerator(pEnumGeom);

            // Spatial Filter
            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilter();

            pSpatialFilter.Geometry = pGeom;  // Setting equal to new geometry
            pSpatialFilter.GeometryField = "SHAPE";
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;  // Intersect new geometry


            // Cities Query
            IQueryFilter pQueryFilter2;
            pQueryFilter2 = new QueryFilter();
            pQueryFilter2 = (IQueryFilter)pSpatialFilter;   // Cast the spatial filter to the IQueryFilter Interface
            pQueryFilter2.WhereClause = "UNIVERSITY = (" + uni + ") AND CRIME_INDE <= (" + crime + ")";

            IFeatureClass pCityFClass;
            pCityFClass = pCityLayer.FeatureClass;

            IFeatureCursor pFCursor2;
            pFCursor2 = pCityFClass.Search(pQueryFilter2, true);


            // Move to the next feature
            IFeature pFeature2;
            pFeature2 = pFCursor2.NextFeature();

            // Feature Selection
            IFeatureSelection pFSel;
            pFSel = (IFeatureSelection)pCityLayer;  //** QI
            pFSel.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            // Message Box
            string strList = null;
            strList = "";

            int intNameField = 0;
            intNameField = pCityFClass.FindField("NAME");

            while (pFeature2 != null)
            {
                strList = strList + pFeature2.Value[intNameField] + Environment.NewLine;
                pFeature2 = pFCursor2.NextFeature();
            }

            MessageBox.Show("The following cities fit the criteria: \n" + Environment.NewLine + strList);

            // Refresh View
            IActiveView pActiveView;
            pActiveView = (IActiveView)pMap;
            pActiveView.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SiteSelection1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
