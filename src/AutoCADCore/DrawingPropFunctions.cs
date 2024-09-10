using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCADCore;

namespace AutoCadCore
{
    internal class DrawingPropFunctions
    {
        public static void CreateDwgProperties(string testNm, string testId, string testingId)
        {
            Document AcDocument = Application.DocumentManager.MdiActiveDocument;
            Database AcDatabase = AcDocument.Database;
            Editor AcEditor = AcDocument.Editor;

            DatabaseSummaryInfoBuilder databaseSummaryInfoBuilder = new DatabaseSummaryInfoBuilder(AcDatabase.SummaryInfo);
            using (AcDocument.LockDocument())
            {
                databaseSummaryInfoBuilder.Author = "Autodesk";
                databaseSummaryInfoBuilder.CustomPropertyTable.Add("testNm", testNm);
                databaseSummaryInfoBuilder.CustomPropertyTable.Add("testId", testId);
                databaseSummaryInfoBuilder.CustomPropertyTable.Add("testingId", testingId);
                AcDatabase.SummaryInfo = databaseSummaryInfoBuilder.ToDatabaseSummaryInfo();
            }
        }

        public static bool CheckIfFileHasProperties()
        {
            DatabaseSummaryInfoBuilder builder = new DatabaseSummaryInfoBuilder(Globals.AcDatabase.SummaryInfo);
            IDictionary acDic = builder.CustomPropertyTable;
            if (acDic.Contains("testNm") == true && acDic.Contains("testId") && acDic.Contains("testingId"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfFileHasProperties(Database db)
        {
            DatabaseSummaryInfoBuilder builder = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
            IDictionary acDic = builder.CustomPropertyTable;
            if (acDic.Contains("testNm") == true && acDic.Contains("testId") && acDic.Contains("testingId"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
