#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RAB_Session_02_Challenge
{
    [Transaction(TransactionMode.Manual)]
    public class cmdProjectSetup : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // declare variables

            string filePathLevels = @"S:\Personal Folders\Training Material\ArchSmarter\Revit Add-in Bootcamp\Session 02\Session 02_Room List.csv";
            string fileText = System.IO.File.ReadAllText(filePath);

            // read text file data

            // loop through file data and put into lists

            // remove header rows

            // create levels

            // get titleblock element ID

            // create sheets


            return Result.Succeeded;
        }
    }
}
