//
using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using InterDesignCad.Util;

[assembly: CommandClass(typeof(InterDesignCad.Cmd.NZCommands))]


namespace InterDesignCad.Cmd
{
    public class NZCommands
    {


#if AUTOCAD_NEWER_THAN_2012
    const String acedTransOwner = "accore.dll";
#else
        const String acedTransOwner = "accore.dll";
#endif

#if AUTOCAD_NEWER_THAN_2014
    const String acedTrans_x86_Prefix = "_";
#else
        const String acedTrans_x86_Prefix = "";
#endif

        const String acedTransName = "acedTrans";

        [DllImport(acedTransOwner, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = acedTrans_x86_Prefix + acedTransName)]
        static extern Int32 acedTrans_x86(Double[] point, IntPtr fromRb,
          IntPtr toRb, Int32 disp, Double[] result);

        [DllImport(acedTransOwner, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = acedTransName)]
        static extern Int32 acedTrans_x64(Double[] point, IntPtr fromRb,
          IntPtr toRb, Int32 disp, Double[] result);

        public static Int32 acedTrans(Double[] point, IntPtr fromRb, IntPtr toRb,
          Int32 disp, Double[] result)
        {
            if (IntPtr.Size == 4)
                return acedTrans_x86(point, fromRb, toRb, disp, result);
            else
                return acedTrans_x64(point, fromRb, toRb, disp, result);
        }

        [CommandMethod("mlinedim", CommandFlags.NoTileMode)]

        static public void MlineDim()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // pick some point in PS

            PromptPointResult res = ed.GetPoint("选择第一根线端点。");

            if (res.Status == PromptStatus.OK)
            {

                // now to make sure this works for all viewpoints

                ResultBuffer psdcs = new ResultBuffer(new TypedValue(5003, 3));

                ResultBuffer dcs = new ResultBuffer(new TypedValue(5003, 2));

                ResultBuffer wcs = new ResultBuffer(new TypedValue(5003, 0));

                double[] retPoint = new double[] { 0, 0, 0 };



                // translate from the DCS of Paper Space (PSDCS) RTSHORT=3 to

                // the DCS of the current model space viewport RTSHORT=2

                acedTrans(retPoint, psdcs.UnmanagedObject, dcs.UnmanagedObject, 0, retPoint);

                //translate the DCS of the current model space viewport RTSHORT=2

                //to the WCS RTSHORT=0

                acedTrans(retPoint, dcs.UnmanagedObject, wcs.UnmanagedObject, 0, retPoint);



                ObjectId btId = ed.Document.Database.BlockTableId;

                // create a new DBPoint and add it to model space to show where we picked

                using (DBPoint pnt = new DBPoint(new Point3d(retPoint[0], retPoint[1], retPoint[2])))

                using (BlockTable bt = btId.Open(OpenMode.ForRead) as BlockTable)

                using (BlockTableRecord ms = bt[BlockTableRecord.ModelSpace].Open(OpenMode.ForWrite)

                                as BlockTableRecord)

                    ms.AppendEntity(pnt);

            }

        }

        [CommandMethod("ps2ms", CommandFlags.NoTileMode)]

        static public void ps2ms()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // pick some point in PS

            PromptPointResult res = ed.GetPoint("Pick Model Space Point");

            if (res.Status == PromptStatus.OK)
            {

                // now to make sure this works for all viewpoints

                ResultBuffer psdcs = new ResultBuffer(new TypedValue(5003, 3));

                ResultBuffer dcs = new ResultBuffer(new TypedValue(5003, 2));

                ResultBuffer wcs = new ResultBuffer(new TypedValue(5003, 0));

                double[] retPoint = new double[] { 0, 0, 0 };



                // translate from the DCS of Paper Space (PSDCS) RTSHORT=3 to

                // the DCS of the current model space viewport RTSHORT=2

                acedTrans(retPoint, psdcs.UnmanagedObject, dcs.UnmanagedObject, 0, retPoint);

                //translate the DCS of the current model space viewport RTSHORT=2

                //to the WCS RTSHORT=0

                acedTrans(retPoint, dcs.UnmanagedObject, wcs.UnmanagedObject, 0, retPoint);



                ObjectId btId = ed.Document.Database.BlockTableId;

                // create a new DBPoint and add it to model space to show where we picked

                using (DBPoint pnt = new DBPoint(new Point3d(retPoint[0], retPoint[1], retPoint[2])))

                using (BlockTable bt = btId.Open(OpenMode.ForRead) as BlockTable)

                using (BlockTableRecord ms = bt[BlockTableRecord.ModelSpace].Open(OpenMode.ForWrite)

                                as BlockTableRecord)

                    ms.AppendEntity(pnt);

            }

        }
        // select all entities in Model Space using Paper Space viewport
        // by Fenton Webb, DevTech, Autodesk, 02/Apr/2012
        [CommandMethod("selectMsFromPs", CommandFlags.NoTileMode)]
        static public void selectMsFromPs()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // pick a PS Viewport
            PromptEntityOptions opts = new PromptEntityOptions("Pick PS Viewport");
            opts.SetRejectMessage("Must select PS Viewport objects only");
            opts.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Viewport), false);
            PromptEntityResult res = ed.GetEntity(opts);
            if (res.Status == PromptStatus.OK)
            {
                int vpNumber = 0;
                // extract the viewport points
                Point3dCollection psVpPnts = new Point3dCollection();
                using (Autodesk.AutoCAD.DatabaseServices.Viewport psVp = res.ObjectId.Open(OpenMode.ForRead)
            as Autodesk.AutoCAD.DatabaseServices.Viewport)
                {
                    // get the vp number
                    vpNumber = psVp.Number;
                    // now extract the viewport geometry
                    psVp.GetGripPoints(psVpPnts, new IntegerCollection(), new IntegerCollection());
                }

                // let's assume a rectangular vport for now, make the cross-direction grips square
                Point3d tmp = psVpPnts[2];
                psVpPnts[2] = psVpPnts[1];
                psVpPnts[1] = tmp;

                // Transform the PS points to MS points
                ResultBuffer rbFrom = new ResultBuffer(new TypedValue(5003, 3));
                ResultBuffer rbTo = new ResultBuffer(new TypedValue(5003, 2));
                double[] retPoint = new double[] { 0, 0, 0 };
                // loop the ps points 
                Point3dCollection msVpPnts = new Point3dCollection();
                foreach (Point3d pnt in psVpPnts)
                {
                    // translate from from the DCS of Paper Space (PSDCS) RTSHORT=3 and 
                    // the DCS of the current model space viewport RTSHORT=2
                    acedTrans(pnt.ToArray(), rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, retPoint);
                    // add the resulting point to the ms pnt array
                    msVpPnts.Add(new Point3d(retPoint));
                    ed.WriteMessage("\n" + new Point3d(retPoint).ToString());
                }

                // now switch to MS
                ed.SwitchToModelSpace();
                // set the CVPort
                Application.SetSystemVariable("CVPORT", vpNumber);
                // once switched, we can use the normal selection mode to select
                PromptSelectionResult selectionresult = ed.SelectCrossingPolygon(msVpPnts);
                // now switch back to PS
                ed.SwitchToPaperSpace();
            }
        }

        //Use viewport boundary as selecting window/polygon
        //to find entities in modelspace visible in each viewport
        [CommandMethod("VpSelect")]
        public static void SelectByViewport()
        {
            Document dwg = Application.DocumentManager.MdiActiveDocument;
            Editor ed = dwg.Editor;

            //Save current layout name
            string curLayout = LayoutManager.Current.CurrentLayout;

            try
            {
                //Get viewport information on current layout
                ViewportInfo[] vports = GetViewportInfoOnCurrentLayout();
                if (vports == null) return;

                //Switch to modelspace
                LayoutManager.Current.CurrentLayout = "Model";

                //Select entities in modelspace that are visible
                foreach (ViewportInfo vInfo in vports)
                {
                    ObjectId[] ents = SelectEntitisInModelSpaceByViewport(
                        dwg, vInfo.BoundaryInModelSpace);
                    ed.WriteMessage("\n{0} entit{1} found via Viewport \"{2}\"",
                        ents.Length,
                        ents.Length > 1 ? "ies" : "y",
                        vInfo.ViewportId.ToString());
                }

                Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nCommand \"VpSelect\" failed:");
                ed.WriteMessage("\n{0}\n{1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                //Restore back to original layout
                if (LayoutManager.Current.CurrentLayout != curLayout)
                {
                    LayoutManager.Current.CurrentLayout = curLayout;
                }

                Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
            }
        }

        //Determine a given entity in modelspace is visible in
        //which viewports
        [CommandMethod("GetViewports")]
        public static void FindContainingViewport()
        {
            Document dwg = Application.DocumentManager.MdiActiveDocument;
            Editor ed = dwg.Editor;

            //Switch to modelspace
            string curLayout = LayoutManager.Current.CurrentLayout;

            try
            {
                //Get viewport information on current layout
                ViewportInfo[] vports = GetViewportInfoOnCurrentLayout();
                if (vports == null) return;

                //Pick an entity in modelspace
                LayoutManager.Current.CurrentLayout = "Model";
                ObjectId entId = PickEntity(ed);
                if (entId.IsNull)
                {
                    ed.WriteMessage("\n*Cancel*");
                }
                else
                {
                    //Find viewport in which the selected entity is visible
                    List<ObjectId> lst = new List<ObjectId>();
                    foreach (ViewportInfo vpInfo in vports)
                    {
                        if (IsEntityInsideViewportBoundary(
                            dwg, entId, vpInfo.BoundaryInModelSpace))
                        {
                            lst.Add(vpInfo.ViewportId);
                            ed.WriteMessage(
                                "\nSelected entity is visible in viewport \"{0}\"",
                               vpInfo.ViewportId.ToString());
                        }
                    }

                    if (lst.Count == 0)
                        ed.WriteMessage(
                            "\nSelected entity is not visible in all viewports");
                    else
                        ed.WriteMessage(
                            "\nSelected entity is visible in {0} viewport{1}.",
                            lst.Count, lst.Count > 1 ? "s" : "");
                }

                Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nCommand \"GetViewports\" failed:");
                ed.WriteMessage("\n{0}\n{1}", ex.Message, ex.StackTrace);
            }
            finally
            {
                //Restore back to original layout
                if (LayoutManager.Current.CurrentLayout != curLayout)
                {
                    LayoutManager.Current.CurrentLayout = curLayout;
                }

                Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
            }
        }

        private static ViewportInfo[] GetViewportInfoOnCurrentLayout()
        {
            string layoutName = LayoutManager.Current.CurrentLayout;
            if (layoutName.ToUpper() == "MODEL")
            {
                Application.ShowAlertDialog("Please set a layout as active layout!");
                return null;
            }
            else
            {
                Document dwg = Application.DocumentManager.MdiActiveDocument;
                ViewportInfo[] vports =
                    CadHelper.SelectLockedViewportInfoOnLayout(dwg, layoutName);
                if (vports.Length == 0)
                {
                    Application.ShowAlertDialog(
                        "No locked viewport found on layout \"" + layoutName + "\".");
                    return null;
                }
                else
                {
                    return vports;
                }
            }
        }

        private static ObjectId[] SelectEntitisInModelSpaceByViewport(
            Document dwg, Point3dCollection boundaryInModelSpace)
        {
            ObjectId[] ids = null;

            using (Transaction tran = dwg.TransactionManager.StartTransaction())
            {
                //Zoom to the extents of the viewport boundary in modelspace
                //before calling Editor.SelectXxxxx()
                ZoomToWindow(boundaryInModelSpace);

                PromptSelectionResult res =
                    dwg.Editor.SelectCrossingPolygon(boundaryInModelSpace);
                if (res.Status == PromptStatus.OK)
                {
                    ids = res.Value.GetObjectIds();
                }

                //Restored to previous view (view before zoomming)
                tran.Abort();
            }

            return ids;
        }

        private static void ZoomToWindow(Point3dCollection boundaryInModelSpace)
        {
            Extents3d ext =
                    GetViewportBoundaryExtentsInModelSpace(boundaryInModelSpace);

            double[] p1 = new double[] { ext.MinPoint.X, ext.MinPoint.Y, 0.00 };
            double[] p2 = new double[] { ext.MaxPoint.X, ext.MaxPoint.Y, 0.00 };

            dynamic acadApp = Application.AcadApplication;
            acadApp.ZoomWindow(p1, p2);
        }

        private static Extents3d GetViewportBoundaryExtentsInModelSpace(
            Point3dCollection points)
        {
            Extents3d ext = new Extents3d();
            foreach (Point3d p in points)
            {
                ext.AddPoint(p);
            }

            return ext;
        }

        private static ObjectId PickEntity(Editor ed)
        {
            PromptEntityOptions opt =
                new PromptEntityOptions("\nSelect an entity:");
            PromptEntityResult res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
            {
                return res.ObjectId;
            }
            else
            {
                return ObjectId.Null;
            }
        }

        private static bool IsEntityInsideViewportBoundary(
            Document dwg, ObjectId entId, Point3dCollection boundaryInModelSpace)
        {
            bool inside = false;
            using (Transaction tran = dwg.TransactionManager.StartTransaction())
            {
                //Zoom to the extents of the viewport boundary in modelspace
                //before calling Editor.SelectXxxxx()
                ZoomToWindow(boundaryInModelSpace);

                PromptSelectionResult res =
                    dwg.Editor.SelectCrossingPolygon(boundaryInModelSpace);
                if (res.Status == PromptStatus.OK)
                {
                    foreach (ObjectId id in res.Value.GetObjectIds())
                    {
                        if (id == entId)
                        {
                            inside = true;
                            break;
                        }
                    }
                }

                //Restored to previous view (before zoomming)
                tran.Abort();
            }

            return inside;
        }
    }
}