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
using AutoCADCore;

namespace AutoCadCore
{
    public class Commands : IExtensionApplication
    {
        private DocumentCollection _DocumentManager;

        // This dictionary will keep track of the Document - Controller pairs
        private static Dictionary<Document, PerDocController> _Controllers;

        [CommandMethod(@"Test_AddPropsSaveDwg", CommandFlags.Session)]
        public void CommandAddProps()
        {
            Document AcDocument = Application.DocumentManager.MdiActiveDocument;
            Editor AcEditor = AcDocument.Editor;
            DrawingPropFunctions.CreateDwgProperties("A Test", "In", "AutoCAD");
            //Save the file so that there is no issue with Drawing Property Set data not applying.

            FileTasks.SaveDrawing();
            AcEditor.WriteMessage(Environment.NewLine + "Saved");
        }

        public void Initialize()
        {
            Autodesk.AutoCAD.ApplicationServices.Application.QuitWillStart += OnClose;

            try
            {
                _DocumentManager = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;

                _Controllers = new Dictionary<Document, PerDocController>();

                // We initialize the dictionary with all the existing open documents
                foreach (Document document in _DocumentManager)
                {
                    if (DrawingPropFunctions.CheckIfFileHasProperties(document.Database))
                    {
                        _Controllers.Add(document, new PerDocController(document));
                    }
                }

                //Add the events to monitor for the document manager
                _DocumentManager.DocumentBecameCurrent += DocumentActivated;
                _DocumentManager.DocumentActivated += DocumentActivated;
                _DocumentManager.DocumentCreated += DocumentActivated;
                _DocumentManager.DocumentToBeDestroyed += DocumentToBeDestroyed;
            }
            catch (System.Exception e)
            {
                Document AcDocument = Application.DocumentManager.MdiActiveDocument;
                Editor AcEditor = AcDocument.Editor;
                AcEditor.WriteMessage($"Error occurred: {e.Message}");
            }
        }

        public void Terminate()
        {
            try
            {
                foreach (Document document in _DocumentManager)
                {
                    if (DrawingPropFunctions.CheckIfFileHasProperties(document.Database))
                    {
                        _Controllers.Remove(document);
                    }
                }

                //Remove document Manager events on application close
                _DocumentManager.DocumentBecameCurrent -= DocumentActivated;
                _DocumentManager.DocumentActivated -= DocumentActivated;
                _DocumentManager.DocumentCreated -= DocumentActivated;
                _DocumentManager.DocumentToBeDestroyed -= DocumentToBeDestroyed;
            }
            catch (System.Exception e)
            {
                Document AcDocument = Application.DocumentManager.MdiActiveDocument;
                Editor AcEditor = AcDocument.Editor;
                AcEditor.WriteMessage($"Error occurred: {e.Message}");
            }

            Autodesk.AutoCAD.ApplicationServices.Application.QuitWillStart -= OnClose;
        }

        #region EventHandlers

        private void OnClose(object sender, System.EventArgs e)
        {
            //does nothing
        }

        private void DocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
            //Verify document is not null, if we don't, on AutoCAD start page we'll get Fatal error
            if (e.Document != null)
            {
                Globals.GetDocuments();
                //If controllers collection contains the document AND the document has SDT Properties, then we need to register it and it's document events
                if (!(_Controllers.ContainsKey(e.Document)) && DrawingPropFunctions.CheckIfFileHasProperties() == true)
                    _Controllers.Add(e.Document, new PerDocController(e.Document));
            }
        }

        private void DocumentToBeDestroyed(object sender, DocumentCollectionEventArgs e)
        {
            //Verify document is not null, if we don't, on AutoCAD start page we'll get Fatal error
            if (e.Document != null)
            {
                //If our controllers collection contains the document, then we need to remove it from there. That will fire the Document events to be closed as well
                if ((_Controllers.ContainsKey(e.Document)))
                {
                    _Controllers[e.Document].Destroy();
                    _Controllers.Remove(e.Document);
                }
            }
        }
        #endregion
    }

    public class PerDocController
    {
        private Document _Document;

        public PerDocController(Document document)
        {
            _Document = document;

            //Add the begin save event
            _Document.Database.BeginSave += Database_BeginSave;
        }

        public void Destroy()
        {
            //Remove the save events
            _Document.Database.BeginSave -= Database_BeginSave;
        }

        #region EventHandlers
        void Database_BeginSave(object sender, DatabaseIOEventArgs e)
        {
            Globals.GetDocuments();
            Globals.AcEditor.WriteMessage($"File Saving Start...");
        }
        #endregion
    }
}
