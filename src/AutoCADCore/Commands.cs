using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsSystem;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace AutoCad2024SaveTest
{
    public class Commands
    {
        [CommandMethod(@"Test_AddPropsSaveDwg", CommandFlags.Session)]
        public void SdtAddModelToJob()
        {
            Document AcDocument = Application.DocumentManager.MdiActiveDocument;
            Editor AcEditor = AcDocument.Editor;
            DrawingPropFunctions.CreateDwgProperties("A Test", "In", "AutoCAD");
            //Save the file so that there is no issue with Drawing Property Set data not applying.
#if ACAD2023 || ACAD2024
            FileTasks.SaveDrawing();
            AcEditor.WriteMessage(Environment.NewLine + "Saved");
#endif
        }
    }
}
