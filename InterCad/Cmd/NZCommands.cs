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

 

    public class NZCommands : IExtensionApplication
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


        [CommandMethod("PaperSpaceCrossingWindow")]
        public void PaperSpaceCrossingWindow()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction trx = db.TransactionManager.StartTransaction())
            {
                BlockTable blocktable = trx.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord currentSpace = trx.GetObject(db.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                if (currentSpace.ObjectId == blocktable[BlockTableRecord.ModelSpace])
                {
                    Application.ShowAlertDialog("Use a paer space layout");
                    return;
                }
                //// Note using db.Extmin & db.Extmax uses extents of model space not current space extents
                PromptSelectionResult psr = ed.SelectCrossingWindow(db.Extmin, db.Extmax);
                ed.WriteMessage("\n" + db.Extmin.ToString() + "  " + db.Extmax.ToString());
                foreach (ObjectId objId in psr.Value.GetObjectIds())
                {
                    Entity ent = (Entity)trx.GetObject(objId, OpenMode.ForWrite);
                    ent.ColorIndex = 1;
                }
                trx.Commit();
            }
        }

        [CommandMethod("qr", CommandFlags.NoTileMode)]

        static public void NZ_qr()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            Editor ed = acDoc.Editor;

            var opt = new PromptNestedEntityThroughViewportOptions("选择实体.");


            var res = SelectThroughViewport.GetNestedEntityThroughViewport(ed, opt);


            if (res.Status != PromptStatus.OK)
            {
                Log4NetHelper.WriteInfoLog("没有选择到实体.\n");
                ed.WriteMessage("没有选择到实体.\n\t");
                return;
            }

            Entity ent;
            using (Transaction trx = acCurDb.TransactionManager.StartTransaction())
            {

                ObjectId lid = res.ObjectId;
                ent = (Entity)trx.GetObject(lid, OpenMode.ForWrite);
                Log4NetHelper.WriteInfoLog("实体的类型是："+ent.Visible+"\n");
                ed.WriteMessage("实体的类型是：" + ent.Visible + "\n");
                ent.ColorIndex = 1;
                ent.Visible = false;

                trx.Commit();
            }
            
            ////Select a Viewport
            //var vpId = SelectEntity(
            //    ed,
            //    typeof(Viewport),
            //    "\nSelect a viewport:");
            //if (vpId.IsNull)
            //{
            //    ed.WriteMessage("\n*Cancel*");
            //    return;
            //}


            ////Select a polyline in the same layout
            //var polyId = SelectEntity(
            //    ed,
            //    typeof(Polyline),
            //    "\nSelect a polyline in the same layout as the selected viewport:");
            //if (polyId.IsNull)
            //{
            //    HighlightEntity(vpId, false);
            //    ed.WriteMessage("\n*Cancel*");
            //    return;
            //}

            //HighlightEntity(vpId, false);



        }

        private static void HighlightEntity(ObjectId entId, bool highlight)
        {
            using (var tran = entId.Database.TransactionManager.StartTransaction())
            {
                var ent = (Entity)tran.GetObject(entId, OpenMode.ForRead);
                if (highlight)
                    ent.Highlight();
                else
                    ent.Unhighlight();

                tran.Commit();
            }
        }
        private static ObjectId SelectEntity(Editor ed, Type entityType, string msg)
        {
            var opt = new PromptEntityOptions(msg);
            opt.SetRejectMessage("\nInvalid selection: not a " + entityType.Name);
            opt.AddAllowedClass(entityType, true);

            var res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
                return res.ObjectId;
            else
                return ObjectId.Null;
        }

        [CommandMethod("mlinedim", CommandFlags.NoTileMode)]

        static public void MlineDim()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // pick some point in PS
            Point3d p1, p2;
            PromptPointResult res = ed.GetPoint("选择第一根线端点。");

            if (res.Status != PromptStatus.OK)
                return;


            PromptPointResult resend = ed.GetPoint("选择第二根线端点。");
            if (resend.Status != PromptStatus.OK)
                return;
            p1 = res.Value;
            p2 = resend.Value;


            Log4NetHelper.WriteInfoLog("第一个点" + p1.ToString() + "\n");
            Log4NetHelper.WriteInfoLog("第二个点" + p2.ToString() + "\n");



            CadHelper.ClearDict();
            PromptSelectionResult acSSPrompt;


            Point3d mp1 = Extension.Trans(p1, Extension.CoordSystem.PSDCS, Extension.CoordSystem.DCS);

            Point3d mp2 = Extension.Trans(p2, Extension.CoordSystem.PSDCS, Extension.CoordSystem.DCS);

            Log4NetHelper.WriteInfoLog("转换后第一个点" + mp1.ToString() + "\n");
            Log4NetHelper.WriteInfoLog("转换后第二个点" + mp2.ToString() + "\n");


            //acSSPrompt = ed.SelectCrossingWindow(mp1,
            //                                          mp2);
            ed.SwitchToModelSpace();
            TypedValue[] tvs = new TypedValue[] { new TypedValue((int)DxfCode.Start, "LINE") };

            SelectionFilter sf = new SelectionFilter(tvs);
            acSSPrompt = ed.SelectCrossingWindow(mp1, mp2, sf);





            if (acSSPrompt.Status != PromptStatus.OK)
            {
                Log4NetHelper.WriteInfoLog("没有选择到实体.\n");
                return;
            }


            SelectionSet acSSet = acSSPrompt.Value;

            Log4NetHelper.WriteInfoLog("Number of objects selected: " +
                                        acSSet.Count.ToString());

            ed.SwitchToPaperSpace();
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            Vector3dCollection vcs = new Vector3dCollection();
            // Start a transaction
            Vector3d ev, sv, sbv;
            int pii = 0;

            Point3d ep1, ep2,pp1,pp2,dp1,dp2;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.PaperSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                foreach (ObjectId lid in acSSet.GetObjectIds())
                {
                    var ln = acTrans.GetObject(lid, OpenMode.ForRead) as Line;

                    if (ln != null)
                    {

                        ev = new Vector3d(ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z);
                        sv = new Vector3d(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z);

                        sbv = ev.Subtract(sv);
                        CadHelper.AddVPDict(sbv, ln.EndPoint);
                        vcs.Add(sbv);

                        pii = pii + 1;

                    }

                }


                if (vcs.Count >= 2)
                {
                    for (int i = 0; i < vcs.Count - 1; i++)
                    {
                        if (vcs[i].IsCodirectionalTo(vcs[i + 1]) == true)
                        {

                            ep1 = CadHelper.GetEndPoint(vcs[i]);
                            ep2 = CadHelper.GetEndPoint(vcs[i + 1]);

                            dp1 = Extension.Trans(ep1, Extension.CoordSystem.WCS, Extension.CoordSystem.DCS);
                            pp1 = Extension.Trans(dp1, Extension.CoordSystem.DCS, Extension.CoordSystem.PSDCS);
                            dp2 = Extension.Trans(ep2, Extension.CoordSystem.WCS, Extension.CoordSystem.DCS);
                            pp2 = Extension.Trans(dp2, Extension.CoordSystem.DCS, Extension.CoordSystem.PSDCS);

                            // Create the rotated dimension
                            using (RotatedDimension acRotDim = new RotatedDimension())
                            {
                                acRotDim.XLine1Point = pp1;
                                acRotDim.XLine2Point = pp2;
                                acRotDim.Rotation = 0;
                                acRotDim.DimLinePoint = new Point3d(0, 5, 0);
                                acRotDim.DimensionStyle = acCurDb.Dimstyle;

                                // Add the new object to Model space and the transaction
                                acBlkTblRec.AppendEntity(acRotDim);
                                acTrans.AddNewlyCreatedDBObject(acRotDim, true);
                            }

                            // Commit the changes and dispose of the transaction


                        }
                    }
                }
                acTrans.Commit();
            }

            // Start a transaction
           
            // now to make sure this works for all viewpoints





            //ObjectId btId = ed.Document.Database.BlockTableId;

            //// create a new DBPoint and add it to model space to show where we picked

            //using (DBPoint pnt = new DBPoint(new Point3d(retPoint[0], retPoint[1], retPoint[2])))

            //using (BlockTable bt = btId.Open(OpenMode.ForRead) as BlockTable)

            //using (BlockTableRecord ms = bt[BlockTableRecord.ModelSpace].Open(OpenMode.ForWrite)

            //                as BlockTableRecord)

            //    ms.AppendEntity(pnt);



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

        public void Initialize()
        {
            Log4NetHelper.InitLog4Net(SysUtil.getCfgPath()+"log4net.config");

        }

        public void Terminate()
        {

        }
    }
}