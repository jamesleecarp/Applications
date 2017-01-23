using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomOut200 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomOut200()
        {
        }
        public void ZoomOut_200()
        {
            double dblLevel;
            dblLevel = 2;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomOut_200();
        }
        protected override void OnUpdate()
        {
        }
    }
}
