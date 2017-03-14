using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;


namespace BatchFeaturesToGraphics
{
    public class Button1 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Button1()
        {
        }
        
        public void BatchFeaturesToGraphics()
        {
            //****** Author:  James Carpenter
            //******** Date:  3/12/2017
            //* Description:  This procedure converts multiple features to graphics when selected in the Table of Contents.
            //******          This procedure only converts those features within the screen display visible bounds for better performance. 
            //******          Subsitiute "null" for "pSpatailFilter" to convert all features to graphics. Line 123.
            //******          The procedure will set the active view as Layout View. 
            //******          If you want to stay in Data View, manually switch back, or remove Line 184.
            //******
            //****** Locals:  pMxDoc, pMap, pActiveView, pPageLayout, pMap, pFeature, pElement, pGraphicsContainer, pEnvelope, pSet, 
            //******          pSpatialFilter, pContentsView, pContentsViewSelection, pLayer, layername, numberOfLayers, pGeoFeatureLayer,
            //******          pFeatureClass, pFeatureCursor, pFeatureRenderer, pMarkerElement, pLineElement, pFillShapeElement
            //******          
            //*****************************************************************************************************************************

            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;
            IActiveView pActiveView;
            pActiveView = pMxDoc.ActiveView;
            IPageLayout pPageLayout;
            pPageLayout = pMxDoc.PageLayout;
            IMap pMap;
            pMap = pActiveView.FocusMap;

            // Delcare features and elements
            IFeature pFeature;
            IElement pElement = null;
            IGraphicsContainer pGraphicsContainer;

            // Set Envelope to Screen Display Visible Bounds
            IEnvelope pEnvelope = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds;

            // Spatial filter
            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilter();

            pSpatialFilter.Geometry = pEnvelope;  // Setting equal to envelope
            pSpatialFilter.GeometryField = "SHAPE";
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // Intersect the screen view

            // Get the TOC
            IContentsView pContentsView = pMxDoc.CurrentContentsView;

            IContentsViewSelection pContentsViewSelection = (IContentsViewSelection)pContentsView;

            
            // Get the selected items in the Table of Contents, and put in set
            ESRI.ArcGIS.esriSystem.ISet pSet;
            pSet = pContentsViewSelection.SelectedItems;

            // Check if multiple layers are selected in the Table of Contents
            if (pSet == null || pSet.Count == 1) 
            {
                MessageBox.Show("Select multiple layers in the Table of Contents");
            }

            else
            {

                // Check if in user is in Data View or Layout View
                if (pActiveView == pPageLayout) // If user is in Layout View
                {
                    pMxDoc.ActiveView = (IActiveView)pMap; // Switch to Data View

                    BatchFeaturesToGraphics(); // Run the entire tool again
                }
                else
                {

                // Get the selected layers from the set
                ILayer pLayer;
                pLayer = (ILayer)pSet.Next();

                    while (pLayer != null)
                    {
                            
                        // Set the layerName variable and get the name of the selected layer
                        string layerName;
                        layerName = pLayer.Name;

                        // Get the number of layers in the map document
                        int numberOfLayers = pMap.LayerCount;

                        // Loop through all the layers in the map document, and get each layer index
                        for (int j = 0; j < numberOfLayers; j++)
                        {
                            // Check if the name of the selected layer and the layer in the map document match
                            if (layerName == pMap.get_Layer(j).Name)
                            {

                                int[] array = { j }; // Create an array to store the layer indexes

                                pGraphicsContainer = (IGraphicsContainer)pMap;
                                foreach (var i in array) // Loop through each item in the array
                                {

                                    IGeoFeatureLayer pGeoFeatureLayer = (IGeoFeatureLayer)pMap.get_Layer(i);

                                    IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;

                                    // Pass in spatial filter, use "null" to convert all features
                                    IFeatureCursor pFeatureCursor = pFeatureClass.Search(pSpatialFilter, true); 

                                    IFeatureRenderer pFeatureRenderer = pGeoFeatureLayer.Renderer;

                                    pFeature = pFeatureCursor.NextFeature();

                                    pGeoFeatureLayer.Visible = false; // Turn layers off in Table of Contents

                                    while (pFeature != null)
                                    {
                                        // Point Features
                                        if (pFeature.Shape.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                                        {
                                            IMarkerElement pMarkerElement = new MarkerElementClass();

                                            // Declare symbol
                                            ISymbol pSymbol = pFeatureRenderer.get_SymbolByFeature(pFeature);

                                            pMarkerElement.Symbol = (IMarkerSymbol)pSymbol;

                                            
                                            pElement = (IElement)pMarkerElement; // Declare element

                                        }

                                        // Line Features
                                        if (pFeature.Shape.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                                        {
                                            ILineElement pLineElement = new LineElementClass();

                                            pLineElement.Symbol = (ILineSymbol)pFeatureRenderer.get_SymbolByFeature(pFeature); // Declare symbol

                                            pElement = (IElement)pLineElement; // Declare element
                                        }

                                        // Polygon Features
                                        if (pFeature.Shape.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
                                        {
                                            IFillShapeElement pFillShapeElement = new PolygonElementClass();

                                            pFillShapeElement.Symbol = (IFillSymbol)pFeatureRenderer.get_SymbolByFeature(pFeature); // Declare symbol

                                            pElement = (IElement)pFillShapeElement; // Declare element
                                        }

                                        // Create graphic
                                        if (pElement != null)
                                        {
                                            pElement.Geometry = pFeature.Shape; // Get the geometry and shape
                                            pGraphicsContainer.AddElement(pElement, 0); // Add the new graphic element to the map
                                        }

                                        pFeature = pFeatureCursor.NextFeature();
                                    }
                                }
                            }
                        }
                        pLayer = (ILayer)pSet.Next(); // Loop back to the next selected layer in the set
                    }
                    

                    pMxDoc.ActiveView = (IActiveView)pPageLayout; // Switch back to Layout View. Remove this line to stay in Data View
                    pMxDoc.UpdateContents();
                    pMxDoc.ActiveView.Refresh();
                }
                
            }
            
        }
        protected override void OnClick()
        {
            BatchFeaturesToGraphics();
        }

        protected override void OnUpdate()
        {
        }
    }

}
