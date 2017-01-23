using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    public class ZoomIn200 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ZoomIn200()
        {
        }
        public void ZoomIn_200()
        {
            double dblLevel;
            dblLevel = .5;
            Utilities.Util_Extract(dblLevel);
        }
        protected override void OnClick()
        {
            ZoomIn_200();
        }
        protected override void OnUpdate()
        {
        }
    }
}
