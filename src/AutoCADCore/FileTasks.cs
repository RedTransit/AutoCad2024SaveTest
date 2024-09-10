using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCadCore
{
    internal class FileTasks
    {
        public static void SaveDrawing()
        {
            object obj = Application.GetSystemVariable(@"DBMOD");

            // Check the value of DBMOD, if 0 then the drawing has no unsaved changes

            if (System.Convert.ToInt16(obj) != 0)
            {
                try
                {
                    Document acDoc = Application.DocumentManager.MdiActiveDocument;
                    acDoc.Database.SaveAs(acDoc.Name, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                }
                catch (Exception)
                {
                  return;
                }
            }
        }
    }
}
