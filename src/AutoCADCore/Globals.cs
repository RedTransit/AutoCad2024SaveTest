using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCADCore
{
    public class Globals
    {
        public static Document AcDocument;
        public static Database AcDatabase;
        public static Editor AcEditor;

        public static void GetDocuments()
        {
            AcDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            AcDatabase = AcDocument.Database;
            AcEditor = AcDocument.Editor;
        }
    }
}
