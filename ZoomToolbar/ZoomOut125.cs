using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomOut125 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomOut125()
        {
        }
        public void ZoomOut_125()
        {
            double dblLevel;
            dblLevel = 1.25;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomOut_125();
        }
        protected override void OnUpdate()
        {
        }
    }
}
