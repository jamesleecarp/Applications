using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace SiteSelection
{
    public class SiteSelection : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public SiteSelection()
        {
        }

        protected override void OnClick()
        {
            Form1 myForm = new Form1();
            myForm.ShowDialog();
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
