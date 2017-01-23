using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomIn400 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomIn400()
        {
        }
        public void ZoomIn_400()
        {
            double dblLevel;
            dblLevel = .25;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomIn_400();
        }
        protected override void OnUpdate()
        {
        }
    }
}