using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace SiteSelection
{
    public class SiteSelection : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public SiteSelection()
        {
        }
        public void Practice3Exercise()
        {
            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;

            IMap pMap;
            pMap = pMxDoc.FocusMap;

            IEnumLayer pLayers;
            pLayers = pMap.Layers;

            ILayer pLayer;
            pLayer = pLayers.Next();

            IFeatureLayer pCountyLayer = null;
            IFeatureLayer pCityLayer = null;

            while (pLayer != null)
            {
                if (pLayer.Name == "counties")
                {
                    pCountyLayer = (IFeatureLayer)pLayer;
                    break;
                }
                pLayer = pLayers.Next();
            }

            IQueryFilter pQueryFilter;
            pQueryFilter = new QueryFilter();
            pQueryFilter.WhereClause = "NO_FARMS87 > 500 AND POP_SQMILE < 150 AND AGE_18_64 >= 25000";

            IFeatureClass pCountyFClass;
            pCountyFClass = pCountyLayer.FeatureClass;

            int intNameField = 0;
            intNameField = pCountyFClass.FindField("NAME");

            IFeatureCursor pFCursor;
            pFCursor = pCountyFClass.Search(pQueryFilter, true);

            IFeature pFeature;
            pFeature = pFCursor.NextFeature();

            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilter();

            string strList = null;
            strList = "";

            while (pFeature != null)
            {
                strList = strList + pFeature.Value[intNameField] + Environment.NewLine;
                pFeature = pFCursor.NextFeature();
            }

            MessageBox.Show("The following counties fit the criteria" + Environment.NewLine + strList);

            IFeatureSelection pFSel;
            pFSel = (IFeatureSelection)pLayer;  //** QI
            pFSel.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            IActiveView pActiveView;
            pActiveView = (IActiveView)pMap;
            pActiveView.Refresh();
        }
        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null;
            Practice3Exercise();
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
