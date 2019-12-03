﻿// Khong xài extern alias nữa
using System;
using System.Text;


using aar19 = Autodesk.AutoCAD.Runtime;
using aApp = Autodesk.AutoCAD.ApplicationServices;


using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Interop;

using System.Reflection;
using swF = System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;


using Microsoft.Win32;


[assembly: aar19.ExtensionApplication(null)]
[assembly: aar19.CommandClass(typeof(CAD_Utils.Class1))]

namespace CAD_Utils
{
    public class Class1
    {
        public void sayHello()
        {
            swF.MessageBox.Show("HALO CAD");
        }

        [aar19.CommandMethod("c1")]
        static public void DoIt1()
        {
            // Get the current document and database, and start a transaction
            aApp.Document acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Starts a new transaction with the Transaction Manager
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;
                StringBuilder sb = new StringBuilder();

                /*----------------------------------------------------------------------
                Class1 cl = new Class1();
                cl.sayHello();

                while (acBlkTbl.GetEnumerator().MoveNext())
                {
                    sb.AppendLine(acBlkTbl.GetEnumerator().Current.ToString());
                }
                swF.MessageBox.Show(sb.ToString());
                ------------------------------------------------------------------------*/


                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                /* Creates a new MText object and assigns it a location,
                text value and text style */
                MText objText = new MText();

                // Set the default properties for the MText object
                objText.SetDatabaseDefaults();

                // Specify the insertion point of the MText object
                objText.Location = new Autodesk.AutoCAD.Geometry.Point3d(2, 2, 0);

                // Set the text string for the MText object
                objText.Contents = "Greetings, Welcome to the AutoCAD .NET Developer's Guide";

                // Set the text style for the MText object
                objText.TextStyleId = acCurDb.Textstyle;

                // Appends the new MText object to model space
                acBlkTblRec.AppendEntity(objText);

                // Appends to new MText object to the active transaction
                acTrans.AddNewlyCreatedDBObject(objText, true);

                // Saves the changes to the database and closes the transaction
                acTrans.Commit();
            }


        }
        [aar19.CommandMethod("c2")]
        static public void CurrentLayerID()
        {
            // Get the current document and database, and start a transaction
            aApp.Document acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            ObjectId cLayerId = acCurDb.Clayer;

            swF.MessageBox.Show(cLayerId.ToString());
        }
        [aar19.CommandMethod("c3")]
        static public void AttachXref()
        {
            string strFName, strBlkName;
            Autodesk.AutoCAD.DatabaseServices.ObjectId objId;

            strFName = @"C:\Autodesk\Autodesk_ObjectARX_2019_Win_64_and_32_Bit\classmap\Cad07-LinkXref.dwg";
            strBlkName = System.IO.Path.GetFileNameWithoutExtension(strFName);

            objId = aApp.Application.DocumentManager.MdiActiveDocument.Database.AttachXref(strFName, strBlkName);
            swF.MessageBox.Show(objId.ToString());
        }
        [aar19.CommandMethod("c4")]
        static public void DoIt4()
        {

            Autodesk.AutoCAD.DatabaseServices.Database acCurDb;
            acCurDb = aApp.Application.DocumentManager.MdiActiveDocument.Database;
        }
        [aar19.CommandMethod("c5")]
        public static void ListEntities()
        {
            // Get the current document and database, and start a transaction
            aApp.Document acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for read
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForRead) as BlockTableRecord;

                int nCnt = 0;
                acDoc.Editor.WriteMessage("\nModel space objects: ");

                int CountBlock = 0;

                // Step through each object in Model space and
                // display the type of object found
                StringBuilder dxfnames = new StringBuilder();
                foreach (ObjectId acObjId in acBlkTblRec)
                {
                    acDoc.Editor.WriteMessage("\n" + acObjId.ObjectClass.DxfName);
                    string dxfname = acObjId.ObjectClass.DxfName;
                    dxfnames.AppendLine(dxfname);
                    if (dxfname == "INSERT")
                    {
                        CountBlock++;
                    }
                    nCnt = nCnt + 1;
                }
                swF.MessageBox.Show(dxfnames.ToString());
                swF.MessageBox.Show(string.Format("Count Block = {0}", CountBlock));
                // If no objects are found then display a message
                if (nCnt == 0)
                {
                    acDoc.Editor.WriteMessage("\n  No objects found");
                }

                // Dispose of the transaction
            }
        }
        [aar19.CommandMethod("c6")]
        public static void UpdateScreen()
        {
            aApp.Application.UpdateScreen();
        }
        [aar19.CommandMethod("c7")]
        public static void GetAllLayers()
        {
            // Get the current document and start the Transaction Manager
            aApp.Document aDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = aDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // This example returns the layer table for the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                             OpenMode.ForRead) as LayerTable;

                // Step through the Layer table and print each layer name
                StringBuilder sb = new StringBuilder();
                foreach (ObjectId acObjId in acLyrTbl)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acObjId,
                                                    OpenMode.ForRead) as LayerTableRecord;

                    sb.AppendLine(acLyrTblRec.Name);
                }

                aDoc.Editor.WriteMessage(sb.ToString());

                swF.MessageBox.Show(sb.ToString());

                MessageBoxRichTextBox mbrtb = new MessageBoxRichTextBox(sb.ToString());
                mbrtb.ShowDialog();
            }
        }
        [aar19.CommandMethod("c8")]
        public static void CreateNewLayer()
        {
            // Get the current document and start the Transaction Manager
            aApp.Document aDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = aDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // This example returns the layer table for the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                             OpenMode.ForRead) as LayerTable;


                string layerName = "MyLayer";
                // Check to see if MyLayer exists in the Layer table
                if (acLyrTbl.Has(layerName) != true)
                {
                    // Open the Layer Table for write
                    acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite);

                    // Create a new layer table record and name the layer "MyLayer"
                    using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                    {
                        acLyrTblRec.Name = "MyLayer";

                        // Add the new layer table record to the layer table and the transaction
                        acLyrTbl.Add(acLyrTblRec);
                        acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);
                    }

                    // Commit the changes
                    acTrans.Commit();
                    // Dispose of the transaction

                }
                string res = string.Format("New Layer Id = {0}", acLyrTbl[layerName]);
                aDoc.Editor.WriteMessage(res);
                swF.MessageBox.Show(res);

            }
        }
        [aar19.CommandMethod("c9")]
        public static void FindLayer()
        {

        }
        [aar19.CommandMethod("c10")]
        public static void EraseLayer()
        {
            // Get the current document and start the Transaction Manager
            aApp.Document aDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = aDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Returns the layer table for the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                             OpenMode.ForRead) as LayerTable;

                // Check to see if MyLayer exists in the Layer table
                if (acLyrTbl.Has("MyLayer") == true)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acLyrTbl["MyLayer"],
                                                    OpenMode.ForWrite) as LayerTableRecord;

                    try
                    {
                        acLyrTblRec.Erase();
                        aDoc.Editor.WriteMessage("\n'MyLayer' was erased");

                        // Commit the changes
                        acTrans.Commit();
                    }
                    catch
                    {
                        aDoc.Editor.WriteMessage("\n'MyLayer' could not be erased");
                    }
                }
                else
                {
                    aDoc.Editor.WriteMessage("\n'MyLayer' does not exist");
                }

                // Dispose of the transaction
            }
        }
        [aar19.CommandMethod("RegisterMyApp")]
        public void RegisterMyApp()
        {
            // Get the AutoCAD Applications key
            string sProdKey = HostApplicationServices.Current.MachineRegistryProductRootKey;
            string sAppName = "MyApp";

            RegistryKey regAcadProdKey = Registry.CurrentUser.OpenSubKey(sProdKey);
            RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);

            // Check to see if the "MyApp" key exists
            string[] subKeys = regAcadAppKey.GetSubKeyNames();
            foreach (string subKey in subKeys)
            {
                // If the application is already registered, exit
                if (subKey.Equals(sAppName))
                {
                    regAcadAppKey.Close();
                    return;
                }
            }

            // Get the location of this module
            string sAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Register the application
            RegistryKey regAppAddInKey = regAcadAppKey.CreateSubKey(sAppName);
            regAppAddInKey.SetValue("DESCRIPTION", sAppName, RegistryValueKind.String);
            regAppAddInKey.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            regAppAddInKey.SetValue("LOADER", sAssemblyPath, RegistryValueKind.String);
            regAppAddInKey.SetValue("MANAGED", 1, RegistryValueKind.DWord);

            regAcadAppKey.Close();
        }
        [aar19.CommandMethod("UnregisterMyApp")]
        public void UnregisterMyApp()
        {
            // Get the AutoCAD Applications key
            string sProdKey = HostApplicationServices.Current.MachineRegistryProductRootKey;
            string sAppName = "MyApp";

            RegistryKey regAcadProdKey = Registry.CurrentUser.OpenSubKey(sProdKey);
            RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);

            // Delete the key for the application
            regAcadAppKey.DeleteSubKeyTree(sAppName);
            regAcadAppKey.Close();
        }
        [aar19.CommandMethod("ConnectToAcad")]
        public static void ConnectToAcad()
        {

            AcadApplication acAppComObj = null;
            const string strProgId = "AutoCAD.Application.23";
            // Get a running instance of AutoCAD
            try
            {
                acAppComObj = (AcadApplication)Marshal.GetActiveObject(strProgId);
            }
            catch // An error occurs if no instance is running
            {
                try
                {
                    // Create a new instance of AutoCAD
                    acAppComObj = (AcadApplication)Activator.CreateInstance(Type.GetTypeFromProgID(strProgId), true);
                }
                catch
                {
                    // If an instance of AutoCAD is not created then message and exit
                    System.Windows.Forms.MessageBox.Show("Instance of 'AutoCAD.Application'" +
                                                         " could not be created.");

                    return;
                }
            }

            // Display the application and return the name and version
            acAppComObj.Visible = true;
            //System.Windows.Forms.MessageBox.Show("Now running " + acAppComObj.Name +
            //                                     " version " + acAppComObj.Version);
            MessageBoxRichTextBox mbrtb1 = new MessageBoxRichTextBox("Now running " + acAppComObj.Name +
                                                 " version " + acAppComObj.Version);
            mbrtb1.ShowDialog();
            // Get the active document
            AcadDocument acDocComObj;
            acDocComObj = acAppComObj.ActiveDocument;


            MessageBoxRichTextBox mbrtb = new MessageBoxRichTextBox("(command " + (char)34 + "NETLOAD" + (char)34 + " " + (char)34 + "F:/016-NGHIEN CUU/CAD_API/CAD_API/CAD_Utils/bin/Debug/CAD_Utils.dll" + (char)34 + ") ");
            mbrtb.ShowDialog();
            // Optionally, load your assembly and start your command or if your assembly
            // is demandloaded, simply start the command of your in-process assembly.

            acDocComObj.SendCommand("(command " + (char)34 + "NETLOAD" + (char)34 + " " + (char)34 + "F:/016-NGHIEN CUU/CAD_API/CAD_API/CAD_Utils/bin/Debug/CAD_Utils.dll" + (char)34 + ") ");

            acDocComObj.SendCommand("c7 ");
        }

        [aar19.CommandMethod("CheckForPickfirstSelection", aar19.CommandFlags.UsePickSet | aar19.CommandFlags.NoBlockEditor)]
        public static void CheckForPickfirstSelection()
        {

        }
    }   


}
