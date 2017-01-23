using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ZoomToolbar
{
    static class Utilities
    {
        public static void Util_Extract(double dblLevel)
        {
            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;
            IActiveView pActiveView;
            pActiveView = pMxDoc.ActiveView;
            IEnvelope pEnv;
            pEnv = pActiveView.Extent;
            pEnv.Expand(dblLevel, dblLevel, true);
            pActiveView.Extent = pEnv;
            pActiveView.Refresh();
        }
    }
}
