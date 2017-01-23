using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomOut400 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomOut400()
        {
        }
        public void ZoomOut_400()
        {
            double dblLevel;
            dblLevel = 4;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomOut_400();
        }
        protected override void OnUpdate()
        {
        }
    }
}
