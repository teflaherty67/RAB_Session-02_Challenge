#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#endregion

namespace RAB_Session_02_Challenge
{
    [Transaction(TransactionMode.Manual)]
    public class cmdProjectSetup : IExternalCommand
    {
        internal double ConvertMetersToFeet(double meters)
        {
            double feet = meters * 3.28084;

            return feet;
        }
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

            string filePathLevels = @"S:\Personal Folders\Training Material\ArchSmarter\Revit Add-in Bootcamp\Session 02\RAB_Session_02_Challenge_Levels.csv";
            string filePathSheets = @"S:\Personal Folders\Training Material\ArchSmarter\Revit Add-in Bootcamp\Session 02\RAB_Session_02_Challenge_Sheets.csv";

            List<string[]> levelData = new List<string[]>();
            List<string[]> sheetData = new List<string[]>();

            // read text file data

            string[] arrayLevels = File.ReadAllLines(filePathLevels);
            string[] arraySheets = File.ReadAllLines(filePathSheets);

            // loop through file data and put into lists

            foreach(string levelString in arrayLevels)
            {
                string[] cellData = levelString.Split(',');

                levelData.Add(cellData);
            }

            foreach (string sheetString in arraySheets)
            {
                string[] cellData = sheetString.Split(',');

                sheetData.Add(cellData);
            }

            // remove header rows

            levelData.RemoveAt(0);
            sheetData.RemoveAt(0);

            // create levels

            Transaction t1 = new Transaction(doc);
            t1.Start("Create Levels");

            foreach (string[] curLevelData in levelData)
            {
                string stringHeight = curLevelData[1];
                double levelHeight = 0;
                bool convertFeet = double.TryParse(stringHeight, out levelHeight);

                if(convertFeet == false)
                {
                    TaskDialog.Show("Error", "Could not convert value. Defaulting to 0");
                }

                string stringMeters = curLevelData[2];
                double heightMeters = 0;
                bool convertMeters = double.TryParse(stringMeters, out heightMeters);
                double metersToFeet = ConvertMetersToFeet(heightMeters);

                Level curLevel = Level.Create(doc, levelHeight);
                curLevel.Name = curLevelData[0];
            }

            t1.Commit();
            t1.Dispose();

            // get titleblock element ID

            FilteredElementCollector colTBlocks = new FilteredElementCollector(doc);
            colTBlocks.OfCategory(BuiltInCategory.OST_TitleBlocks);
            ElementId tblockID = colTBlocks.FirstElementId();

            // create sheets

            Transaction t2 = new Transaction(doc);
            t2.Start("Create Sheets");

            foreach (string[] curSheetData in sheetData)
            {
                ViewSheet curSheet = ViewSheet.Create(doc, tblockID);
                curSheet.SheetNumber = curSheetData[0];
                curSheet.Name = curSheetData[1];
            }

            t2.Commit();
            t2.Dispose();

            return Result.Succeeded;
        }
    }
}
