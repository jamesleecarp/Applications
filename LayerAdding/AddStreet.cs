using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Desktop.AddIns;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;


namespace LayerAdding
{
    public class AddStreet : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        IMxDocument pMxDoc;
        IMap pMap;
        IEnumLayer pLayers;
        ILayer pLoopLayer;
        IFeatureLayer pRoadIdxLayer;

        public AddStreet()
        {
        }
        public void SetStyle(ILayer pLoopLayer)
        {
            IFeatureLayer2 pFLayer;
            pFLayer = (IFeatureLayer2)pLoopLayer;
            IGeoFeatureLayer pGeoFLayer;
            pGeoFLayer = (IGeoFeatureLayer)pFLayer;
            ISimpleRenderer pRenderer;
            pRenderer = (ISimpleRenderer)pGeoFLayer.Renderer;

            //* The Color
            IRgbColor pRGBColor;
            pRGBColor = new RgbColor();
            pRGBColor.Red = 255;
            pRGBColor.Green = 0;
            pRGBColor.Blue = 0;

            //* Sets the Line Symbol properties
            ISimpleLineSymbol pLineSym;
            pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGBColor;
            pLineSym.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSDot;
            pLineSym.Width = 1;
            pRenderer.Symbol = (ISymbol)pLineSym;
            
        }
        protected override void OnUpdate()
        {
            pMxDoc = (IMxDocument)ArcMap.Application.Document;
            pMap = pMxDoc.FocusMap;

            //** If a brand new data frame, reading Layers property will generate an error
            if (pMap.LayerCount > 0)
            {
                pLayers = pMap.Layers;
                pLayers.Reset();
                pLoopLayer = pLayers.Next();
                pRoadIdxLayer = null;

                //** Loop through all layers
                while (!(pLoopLayer == null))
                {
                    if (pLoopLayer.Name == "road_idx")
                    {
                        //** Road_idx found.  Set the id var, enable the tool and exit the loop
                        pRoadIdxLayer = (IFeatureLayer)pLoopLayer;
                        Enabled = true;
                        break;
                    }
                    else
                    {
                        //** roads not found yet.  Move to next layer
                        pLoopLayer = pLayers.Next();
                    }
                }
            }
            else
            {
                pRoadIdxLayer = null;
            }

            if (pRoadIdxLayer == null)
            {
                //** roads must not be in data frame, disable the tool
                Enabled = false;
            }
        }
        public string GetFeatureVal(IPoint pPoint, IFeatureLayer pFLayer, string strField)
        {
            string functionReturnValue;
            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilter();
            pSpatialFilter.Geometry = pPoint;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pFeatureCursor;
            pFeatureCursor = pFLayer.Search(pSpatialFilter, true);

            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();

            if ((pFeature != null))
            {
                functionReturnValue = pFeature.Value[pFeatureCursor.FindField(strField)].ToString();
            }
            else
            {
                functionReturnValue = "Undefined";
            }
            return functionReturnValue;
        }
        protected override void OnMouseUp(MouseEventArgs arg)
        {
            base.OnMouseUp(arg);

            IPoint pPoint;
            pPoint = pMxDoc.CurrentLocation;

            string strNewIdField;
            strNewIdField = "NEWID";

            string strRoadIdVal;
            strRoadIdVal = GetFeatureVal(pPoint, pRoadIdxLayer, strNewIdField);

            //** Reports road id # clicked. Remove this line if you do not want this functionality.
            MessageBox.Show("Adding the road id #" + strRoadIdVal);

            IFeatureLayer pFLayer;
            pFLayer = new FeatureLayer();

            IWorkspaceFactory pWSFactory;
            pWSFactory = new ShapefileWorkspaceFactory();

            //* Loads file
            IWorkspace pWorkspace;
            pWorkspace = pWSFactory.OpenFromFile("C:/Users/jcarpenter/Documents/PennState/Geog489/Lesson3/Data/roads", ArcMap.Application.hWnd);

            IFeatureWorkspace pFWorkspace;
            pFWorkspace = (IFeatureWorkspace)pWorkspace;

            //* Concatenates file
            IFeatureClass pFClass;
            pFClass = pFWorkspace.OpenFeatureClass("roads_" + strRoadIdVal + ".shp");

            pFLayer.FeatureClass = pFClass;
            pFLayer.Name = "Road ID " + strRoadIdVal;

            //* Adds the road layer
            pMap.AddLayer(pFLayer);

            //** Set style
            SetStyle(pFLayer);

        }
    }
}