using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomIn125 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomIn125()
        {
        }
        public void ZoomIn_125()
        {
            double dblLevel;
            dblLevel = .8;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomIn_125();
        }
        protected override void OnUpdate()
        {
        }
    }
}
