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

namespace LayerAdding
{
    public class AddOrtho : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        IMxDocument pMxDoc;
        IMap pMap;
        IEnumLayer pLayers;
        ILayer pLoopLayer;
        IFeatureLayer pOrthoIdxLayer;

        public AddOrtho()
        {
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
                pOrthoIdxLayer = null;

                //** Loop through all layers
                while (!(pLoopLayer == null))
                {
                    if (pLoopLayer.Name == "orth_idx")
                    {
                        //** Orth_idx found.  Set the id var, enable the tool and exit the loop
                        pOrthoIdxLayer = (IFeatureLayer)pLoopLayer;
                        Enabled = true;
                        break;
                    }
                    else
                    {
                        //** ortho not found yet.  Move to next layer
                        pLoopLayer = pLayers.Next();
                    }
                }
            }
            else
            {
                pOrthoIdxLayer = null;
            }

            if (pOrthoIdxLayer == null)
            {
                //** ortho must not be in data frame, disable the tool
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

            string strOrthoidField;
            strOrthoidField = "ORTHOID";

            string strOrthoidVal;
            strOrthoidVal = GetFeatureVal(pPoint, pOrthoIdxLayer, strOrthoidField);

            //** Reports ortho id # clicked. Remove this line if you do not want this functionality.
            MessageBox.Show("Adding the ortho id #" + strOrthoidVal);

            IRasterLayer pRLayer;
            pRLayer = new RasterLayer();

            //* Loads and concatenates file
            pRLayer.CreateFromFilePath("C:/Users/jcarpenter/Documents/PennState/Geog489/Lesson3/Data/orthos/" + strOrthoidVal + ".tif");
            pRLayer.Name = "Photo - Tile " + strOrthoidVal;
            pMap.AddLayer(pRLayer);

            //* Generates index from layer count
            int toIndex;
            toIndex = pMap.LayerCount;

            //* Moves the layer to the bottom
            pMap.MoveLayer(pRLayer,toIndex);

            IActiveView pActiveView;
            pActiveView = (IActiveView)pMap;
            pActiveView.Refresh();
        }
    }
}