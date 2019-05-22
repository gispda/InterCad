using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace InterDesignCad.Util
{
    internal sealed class PromptNestedEntityThroughViewportResult
    {
        private PromptStatus m_status;
        private string m_stringResult;
        private Point3d m_pickPoint;
        private ObjectId m_value;
        private ObjectId[] m_containers;
        private Matrix3d m_mat;
        private ObjectId m_viewport;

        internal PromptNestedEntityThroughViewportResult(PromptNestedEntityResult pneResult)
        {
            m_status = pneResult.Status;
            m_stringResult = pneResult.StringResult;
            m_pickPoint = pneResult.PickedPoint;
            m_value = pneResult.ObjectId;
            m_containers = pneResult.GetContainers();
            m_mat = pneResult.Transform;
            m_viewport = Autodesk.AutoCAD.DatabaseServices.ObjectId.Null;
        }

        internal PromptNestedEntityThroughViewportResult(PromptResult pneResult)
        {
            m_status = pneResult.Status;
            m_stringResult = pneResult.StringResult;
            m_pickPoint = Point3d.Origin;
            m_value = Autodesk.AutoCAD.DatabaseServices.ObjectId.Null;
            m_containers = null;
            m_mat = Matrix3d.Identity;
            m_viewport = Autodesk.AutoCAD.DatabaseServices.ObjectId.Null;
        }

        internal PromptNestedEntityThroughViewportResult(PromptNestedEntityResult pneResult, ObjectId viewport)
        {
            m_status = pneResult.Status;
            m_stringResult = pneResult.StringResult;
            m_pickPoint = pneResult.PickedPoint;
            m_value = pneResult.ObjectId;
            m_containers = pneResult.GetContainers();
            m_mat = pneResult.Transform;
            m_viewport = viewport;
        }

        public override string ToString()
        {
            return this.ToString(null);
        }

        public string ToString(IFormatProvider provider)
        {
            object[] args = new object[6];

            args[0] = m_status;
            args[1] = m_stringResult;
            args[2] = m_value;
            args[3] = m_pickPoint;
            args[4] = m_mat;
            args[5] = m_containers;

            return string.Format(provider, "({0},{1},{2},{3},{4},{5})", args);
        }

        public PromptStatus Status
        {
            get
            {
                return m_status;
            }
        }

        public string StringResult
        {
            get
            {
                return m_stringResult;
            }
        }

        public ObjectId ObjectId
        {
            get
            {
                return m_value;
            }
        }

        public Point3d PickedPoint
        {
            get
            {
                return m_pickPoint;
            }
        }

        public ObjectId[] GetContainers()
        {
            return m_containers;
        }

        public Matrix3d Transform
        {
            get
            {
                return m_mat;
            }
        }

        public ObjectId ViewportId
        {
            get
            {
                return m_viewport;
            }
        }
    }
}