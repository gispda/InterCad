using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InterDesignCad.Util
{
    internal static class SelectThroughViewport
    {
        public static PromptNestedEntityThroughViewportResult GetNestedEntityThroughViewport(this Editor acadEditor, string prompt)
        {
            return acadEditor.GetNestedEntityThroughViewport(new PromptNestedEntityThroughViewportOptions(prompt));
        }

        public static PromptNestedEntityThroughViewportResult GetNestedEntityThroughViewport(this Editor acadEditor, PromptNestedEntityThroughViewportOptions options, out Viewport viewport)
        {
            Document acadDocument = acadEditor.Document;
            Database acadDatabase = acadDocument.Database;
            LayoutManager layoutManager = LayoutManager.Current;

            SelectThroughViewportJig stvpJig = new SelectThroughViewportJig(options);

            PromptResult pointResult = acadEditor.Drag(stvpJig);

            if (pointResult.Status == PromptStatus.OK)
            {
                Point3d pickedPoint = stvpJig.Result.Value;

                PromptNestedEntityOptions pneOpions = options.Options;
                pneOpions.NonInteractivePickPoint = pickedPoint;
                pneOpions.UseNonInteractivePickPoint = true;

                PromptNestedEntityResult pickResult = acadEditor.GetNestedEntity(pneOpions);

                if ((pickResult.Status == PromptStatus.OK) ||
                    (acadDatabase.TileMode) ||
                    (acadDatabase.PaperSpaceVportId != acadEditor.CurrentViewportObjectId))
                {

                    viewport = null;
                    return new PromptNestedEntityThroughViewportResult(pickResult);
                }
                else
                {
                    SelectionFilter vportFilter = new SelectionFilter(new TypedValue[] { new TypedValue(0, "VIEWPORT"),
                                                                                                 new TypedValue(-4, "!="),
                                                                                                 new TypedValue(69, 1),
                                                                                                 new TypedValue(410, layoutManager.CurrentLayout) });

                    PromptSelectionResult vportResult = acadEditor.SelectAll(vportFilter);

                    if (vportResult.Status == PromptStatus.OK)
                    {
                        using (Transaction trans = acadDocument.TransactionManager.StartTransaction())
                        {
                            foreach (ObjectId objectId in vportResult.Value.GetObjectIds())
                            {
                                viewport = (Viewport)trans.GetObject(objectId, OpenMode.ForRead, false, false);

                                if (viewport.ContainsPoint(pickedPoint))
                                {
                                    pneOpions.NonInteractivePickPoint = TranslatePointPsToMs(viewport, pickedPoint);
                                    pneOpions.UseNonInteractivePickPoint = true;

                                    acadEditor.SwitchToModelSpace();

                                    Application.SetSystemVariable("CVPORT", viewport.Number);

                                    PromptNestedEntityResult pneResult = acadEditor.GetNestedEntity(pneOpions);

                                    acadEditor.SwitchToPaperSpace();

                                    if (pneResult.Status == PromptStatus.OK)
                                    {
                                        return new PromptNestedEntityThroughViewportResult(pneResult, objectId);
                                    }
                                }
                            }
                        }
                    }
                    viewport = null;
                    return new PromptNestedEntityThroughViewportResult(pickResult);
                }
            }
            else
            {
                viewport = null;
                return new PromptNestedEntityThroughViewportResult(pointResult);
            }
        }

        public static PromptNestedEntityThroughViewportResult GetNestedEntityThroughViewport(this Editor acadEditor, PromptNestedEntityThroughViewportOptions options)
        {
            Document acadDocument = acadEditor.Document;
            Database acadDatabase = acadDocument.Database;
            LayoutManager layoutManager = LayoutManager.Current;

            SelectThroughViewportJig stvpJig = new SelectThroughViewportJig(options);

            PromptResult pointResult = acadEditor.Drag(stvpJig);

            if (pointResult.Status == PromptStatus.OK)
            {
                Point3d pickedPoint = stvpJig.Result.Value;

                PromptNestedEntityOptions pneOpions = options.Options;
                pneOpions.NonInteractivePickPoint = pickedPoint;
                pneOpions.UseNonInteractivePickPoint = true;

                PromptNestedEntityResult pickResult = acadEditor.GetNestedEntity(pneOpions);

                if ((pickResult.Status == PromptStatus.OK) ||
                    (acadDatabase.TileMode) ||
                    (acadDatabase.PaperSpaceVportId != acadEditor.CurrentViewportObjectId))
                {
                    return new PromptNestedEntityThroughViewportResult(pickResult);
                }
                else
                {
                    SelectionFilter vportFilter = new SelectionFilter(new TypedValue[] { new TypedValue(0, "VIEWPORT"),
                                                                                                 new TypedValue(-4, "!="),
                                                                                                 new TypedValue(69, 1),
                                                                                                 new TypedValue(410, layoutManager.CurrentLayout) });

                    PromptSelectionResult vportResult = acadEditor.SelectAll(vportFilter);

                    if (vportResult.Status == PromptStatus.OK)
                    {
                        using (Transaction trans = acadDocument.TransactionManager.StartTransaction())
                        {
                            foreach (ObjectId objectId in vportResult.Value.GetObjectIds())
                            {
                                Viewport viewport = (Viewport)trans.GetObject(objectId, OpenMode.ForRead, false, false);

                                if (viewport.ContainsPoint(pickedPoint))
                                {
                                    pneOpions.NonInteractivePickPoint = TranslatePointPsToMs(viewport, pickedPoint);
                                    pneOpions.UseNonInteractivePickPoint = true;

                                    acadEditor.SwitchToModelSpace();

                                    Application.SetSystemVariable("CVPORT", viewport.Number);

                                    PromptNestedEntityResult pneResult = acadEditor.GetNestedEntity(pneOpions);

                                    acadEditor.SwitchToPaperSpace();

                                    if (pneResult.Status == PromptStatus.OK)
                                    {
                                        return new PromptNestedEntityThroughViewportResult(pneResult, objectId);
                                    }
                                }
                            }
                        }
                    }

                    return new PromptNestedEntityThroughViewportResult(pickResult);
                }
            }
            else
            {
                return new PromptNestedEntityThroughViewportResult(pointResult);
            }
        }

        private static Point3d TranslatePointPsToMs(Viewport viewport, Point3d psPoint)
        {
            Point3d msPoint = psPoint.TransformBy(Matrix3d.Displacement(new Vector3d(viewport.CenterPoint.X, viewport.CenterPoint.Y, viewport.CenterPoint.Z).Negate()))
                                     .TransformBy(Matrix3d.Scaling(1.0 / viewport.CustomScale, Point3d.Origin))
                                     .TransformBy(Matrix3d.Rotation(-viewport.TwistAngle, Vector3d.ZAxis, Point3d.Origin))
                                     .TransformBy(Matrix3d.Displacement(new Vector3d(viewport.ViewCenter.X, viewport.ViewCenter.Y, 0.0)))
                                     .TransformBy(Matrix3d.PlaneToWorld(new Plane(viewport.ViewTarget, viewport.ViewDirection)));



            return msPoint;
        }

        private static bool ContainsPoint(this Viewport viewport, Point3d point)
        {
            // TODO: Need to consider viewport clipping boundary

            return (((viewport.CenterPoint.X - (viewport.Width / 2.0)) <= point.X) &&
                    ((viewport.CenterPoint.X + (viewport.Width / 2.0)) >= point.X) &&
                    ((viewport.CenterPoint.Y - (viewport.Height / 2.0)) <= point.Y) &&
                    ((viewport.CenterPoint.Y + (viewport.Height / 2.0)) >= point.Y) &&
                    (viewport.CenterPoint.Z == point.Z));
        }

        private class SelectThroughViewportJig : DrawJig
        {
            private PromptNestedEntityThroughViewportOptions options;

            public SelectThroughViewportJig(PromptNestedEntityThroughViewportOptions options)
            {
                this.options = options;
            }

            public PromptPointResult Result { get; private set; }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                JigPromptPointOptions jigOptions = new JigPromptPointOptions(options.Message);
                jigOptions.Cursor = CursorType.EntitySelect;

                Result = prompts.AcquirePoint(jigOptions);

                if (Result.Status == PromptStatus.OK)
                {
                    return SamplerStatus.OK;
                }
                else if (Result.Status == PromptStatus.Cancel)
                {
                    return SamplerStatus.OK;
                }

                return SamplerStatus.NoChange;
            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                return true;
            }
        }
    }
    static class Extension
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


        //private static extern int acedTrans(
        //    double[] point,
        //    IntPtr fromResbuf,
        //    IntPtr toResbuf,
        //    int displacement,
        //    double[] result
        //);

        public enum CoordSystem
        {
            WCS = 0,
            UCS = 1,
            DCS = 2,
            PSDCS = 3
        }

        // Coordinates System / Coordinates System
        public static Point3d Trans(this Point3d pt, CoordSystem from, CoordSystem to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Coordinates System / Coordinates System (displacement)
        public static Point3d Trans(this Point3d pt, CoordSystem from, CoordSystem to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Entity / Entity
        public static Point3d Trans(this Point3d pt, ObjectId from, ObjectId to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Entity / Entity (displacement)
        public static Point3d Trans(this Point3d pt, ObjectId from, ObjectId to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Vector / Vector
        public static Point3d Trans(this Point3d pt, Vector3d from, Vector3d to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Vector / Vector (displacement)
        public static Point3d Trans(this Point3d pt, Vector3d from, Vector3d to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Entity / Coordinates System
        public static Point3d Trans(this Point3d pt, ObjectId from, CoordSystem to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Entity / Coordinates System (displacement)
        public static Point3d Trans(this Point3d pt, ObjectId from, CoordSystem to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Coordinates System / Entity
        public static Point3d Trans(this Point3d pt, CoordSystem from, ObjectId to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Coordinates System / Entity (displacement)
        public static Point3d Trans(this Point3d pt, CoordSystem from, ObjectId to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Coordinates System / Vector)
        public static Point3d Trans(this Point3d pt, CoordSystem from, Vector3d to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Coordinates System / Vector (displacement)
        public static Point3d Trans(this Point3d pt, CoordSystem from, Vector3d to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5003, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Vector / Coordinates System
        public static Point3d Trans(this Point3d pt, Vector3d from, CoordSystem to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Vector / Coordinates System (displacement)
        public static Point3d Trans(this Point3d pt, Vector3d from, CoordSystem to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5003, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Entity / Vector
        public static Point3d Trans(this Point3d pt, ObjectId from, Vector3d to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Entity / Vector (displacement)
        public static Point3d Trans(this Point3d pt, ObjectId from, Vector3d to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5006, from)).UnmanagedObject,
                new ResultBuffer(new TypedValue(5009, new Point3d(to.X, to.Y, to.Z))).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
        // Vector / Entity
        public static Point3d Trans(this Point3d pt, Vector3d from, ObjectId to)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                0,
                result);
            return new Point3d(result);
        }
        // Vector / Entity (displacement)
        public static Point3d Trans(this Point3d pt, Vector3d from, ObjectId to, int disp)
        {
            double[] result = new double[] { 0, 0, 0 };
            acedTrans(pt.ToArray(),
                new ResultBuffer(new TypedValue(5009, new Point3d(from.X, from.Y, from.Z))).UnmanagedObject,
                new ResultBuffer(new TypedValue(5006, to)).UnmanagedObject,
                disp,
                result);
            return new Point3d(result);
        }
    }
        public class ViewportInfo
        {
            public ObjectId ViewportId { set; get; }
            public ObjectId NonRectClipId { set; get; }
            public Point3dCollection BoundaryInPaperSpace { set; get; }
            public Point3dCollection BoundaryInModelSpace { set; get; }
        }
  
        //Class to hold Viewport information, obtained
        //in single Transaction


        public class CadHelper
        {

            private static Dictionary<Vector3d, Point3d> dict = new Dictionary<Vector3d, Point3d>();


            public static void ClearDict()
            {
                dict.Clear();
            }
            public static void AddVPDict(Vector3d vc, Point3d ep)
            {
                if (ep != null || vc != null)
                    dict.Add(vc, ep);
                else
                    Log4NetHelper.WriteErrorLog("出错了" + ep + "\n");

            }
            public static ViewportInfo GetViewInfo(Viewport vport, Transaction tran)
            {

                ViewportInfo vpInfo = null;
                Log4NetHelper.WriteInfoLog("视口的数字" + vport.Number + "\n");
                if (vport.Number != 1 && vport.Locked)
                {
                    vpInfo = new ViewportInfo();
                    vpInfo.ViewportId = vport.ObjectId;
                    vpInfo.NonRectClipId = vport.NonRectClipEntityId;
                    if (!vport.NonRectClipEntityId.IsNull &&
                        vport.NonRectClipOn)
                    {
                        Polyline2d pl = (Polyline2d)tran.GetObject(
                            vport.NonRectClipEntityId, OpenMode.ForRead);
                        vpInfo.BoundaryInPaperSpace =
                            GetNonRectClipBoundary(pl, tran);
                    }
                    else
                    {
                        vpInfo.BoundaryInPaperSpace =
                            GetViewportBoundary(vport);
                    }

                    Matrix3d mt = PaperToModel(vport);
                    vpInfo.BoundaryInModelSpace =
                        TransformPaperSpacePointToModelSpace(
                        vpInfo.BoundaryInPaperSpace, mt);

                }
                return vpInfo;
            }
            public static Point3d GetEndPoint(Vector3d svc)
            {
                if (dict.Count > 0)
                    return dict[svc];
                else
                    return new Point3d(0, 0, 0);
            }

            //Get needed Viewport information
            public static ViewportInfo[] SelectLockedViewportInfoOnLayout(
                Document dwg, string layoutName)
            {
                List<ViewportInfo> lst = new List<ViewportInfo>();
                TypedValue[] vals = new TypedValue[]{
                    new TypedValue((int)DxfCode.Start, "VIEWPORT"),
                    new TypedValue((int)DxfCode.LayoutName,layoutName)
                };

                PromptSelectionResult res =
                    dwg.Editor.SelectAll(new SelectionFilter(vals));
                if (res.Status == PromptStatus.OK)
                {
                    using (Transaction tran =
                        dwg.TransactionManager.StartTransaction())
                    {
                        foreach (ObjectId id in res.Value.GetObjectIds())
                        {
                            Viewport vport = (Viewport)tran.GetObject(
                                id, OpenMode.ForRead);
                            if (vport.Number != 1 && vport.Locked)
                            {
                                ViewportInfo vpInfo = new ViewportInfo();
                                vpInfo.ViewportId = id;
                                vpInfo.NonRectClipId = vport.NonRectClipEntityId;
                                if (!vport.NonRectClipEntityId.IsNull &&
                                    vport.NonRectClipOn)
                                {
                                    Polyline2d pl = (Polyline2d)tran.GetObject(
                                        vport.NonRectClipEntityId, OpenMode.ForRead);
                                    vpInfo.BoundaryInPaperSpace =
                                        GetNonRectClipBoundary(pl, tran);
                                }
                                else
                                {
                                    vpInfo.BoundaryInPaperSpace =
                                        GetViewportBoundary(vport);
                                }

                                Matrix3d mt = PaperToModel(vport);
                                vpInfo.BoundaryInModelSpace =
                                    TransformPaperSpacePointToModelSpace(
                                    vpInfo.BoundaryInPaperSpace, mt);

                                lst.Add(vpInfo);
                            }
                        }

                        tran.Commit();
                    }
                }

                return lst.ToArray();
            }

            private static Point3dCollection GetViewportBoundary(Viewport vport)
            {
                Point3dCollection points = new Point3dCollection();

                Extents3d ext = vport.GeometricExtents;
                points.Add(new Point3d(ext.MinPoint.X, ext.MinPoint.Y, 0.0));
                points.Add(new Point3d(ext.MinPoint.X, ext.MaxPoint.Y, 0.0));
                points.Add(new Point3d(ext.MaxPoint.X, ext.MaxPoint.Y, 0.0));
                points.Add(new Point3d(ext.MaxPoint.X, ext.MinPoint.Y, 0.0));

                return points;
            }

            private static Point3dCollection GetNonRectClipBoundary(
                Polyline2d polyline, Transaction tran)
            {
                Point3dCollection points = new Point3dCollection();

                foreach (ObjectId vxId in polyline)
                {
                    Vertex2d vx = (Vertex2d)tran.GetObject(vxId, OpenMode.ForRead);
                    points.Add(polyline.VertexPosition(vx));
                }

                return points;
            }

            private static Point3dCollection TransformPaperSpacePointToModelSpace(
                Point3dCollection paperSpacePoints, Matrix3d mt)
            {
                Point3dCollection points = new Point3dCollection();

                foreach (Point3d p in paperSpacePoints)
                {
                    points.Add(p.TransformBy(mt));
                }

                return points;
            }

            #region
            //**********************************************************************
            //Create coordinate transform matrix
            //between modelspace and paperspace viewport
            //The code is borrowed from
            //http://www.theswamp.org/index.php?topic=34590.msg398539#msg398539
            //*********************************************************************
            public static Matrix3d PaperToModel(Viewport vp)
            {
                Matrix3d mx = ModelToPaper(vp);
                return mx.Inverse();
            }

            public static Matrix3d ModelToPaper(Viewport vp)
            {
                Vector3d vd = vp.ViewDirection;
                Point3d vc = new Point3d(vp.ViewCenter.X, vp.ViewCenter.Y, 0);
                Point3d vt = vp.ViewTarget;
                Point3d cp = vp.CenterPoint;
                double ta = -vp.TwistAngle;
                double vh = vp.ViewHeight;
                double height = vp.Height;
                double width = vp.Width;
                double scale = vh / height;
                double lensLength = vp.LensLength;
                Vector3d zaxis = vd.GetNormal();
                Vector3d xaxis = Vector3d.ZAxis.CrossProduct(vd);
                Vector3d yaxis;

                if (!xaxis.IsZeroLength())
                {
                    xaxis = xaxis.GetNormal();
                    yaxis = zaxis.CrossProduct(xaxis);
                }
                else if (zaxis.Z < 0)
                {
                    xaxis = Vector3d.XAxis * -1;
                    yaxis = Vector3d.YAxis;
                    zaxis = Vector3d.ZAxis * -1;
                }
                else
                {
                    xaxis = Vector3d.XAxis;
                    yaxis = Vector3d.YAxis;
                    zaxis = Vector3d.ZAxis;
                }
                Matrix3d pcsToDCS = Matrix3d.Displacement(Point3d.Origin - cp);
                pcsToDCS = pcsToDCS * Matrix3d.Scaling(scale, cp);
                Matrix3d dcsToWcs = Matrix3d.Displacement(vc - Point3d.Origin);
                Matrix3d mxCoords = Matrix3d.AlignCoordinateSystem(
                    Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis,
                    Vector3d.ZAxis, Point3d.Origin,
                    xaxis, yaxis, zaxis);
                dcsToWcs = mxCoords * dcsToWcs;
                dcsToWcs = Matrix3d.Displacement(vt - Point3d.Origin) * dcsToWcs;
                dcsToWcs = Matrix3d.Rotation(ta, zaxis, vt) * dcsToWcs;

                Matrix3d perspectiveMx = Matrix3d.Identity;
                if (vp.PerspectiveOn)
                {
                    double vSize = vh;
                    double aspectRatio = width / height;
                    double adjustFactor = 1.0 / 42.0;
                    double adjstLenLgth = vSize * lensLength *
                        Math.Sqrt(1.0 + aspectRatio * aspectRatio) * adjustFactor;
                    double iDist = vd.Length;
                    double lensDist = iDist - adjstLenLgth;
                    double[] dataAry = new double[]
                   {
                       1,0,0,0,0,1,0,0,0,0,
                       (adjstLenLgth-lensDist)/adjstLenLgth,
                       lensDist*(iDist-adjstLenLgth)/adjstLenLgth,
                       0,0,-1.0/adjstLenLgth,iDist/adjstLenLgth
                   };

                    perspectiveMx = new Matrix3d(dataAry);
                }

                Matrix3d finalMx =
                    pcsToDCS.Inverse() * perspectiveMx * dcsToWcs.Inverse();

                return finalMx;
            }

            #endregion
        }
}