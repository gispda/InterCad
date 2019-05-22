using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using System.Collections;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

namespace InterDesignCad.Util
{
    internal sealed class PromptNestedEntityThroughViewportOptions : PromptOptions
    {
        private PromptNestedEntityOptions m_options;

        public PromptNestedEntityThroughViewportOptions(string message)
            : base(message)
        {
            this.m_options = new PromptNestedEntityOptions(message);
        }

        public PromptNestedEntityThroughViewportOptions(string messageAndKeywords, string globalKeywords)
            : base(messageAndKeywords, globalKeywords)
        {
            this.m_options = new PromptNestedEntityOptions(messageAndKeywords, globalKeywords);
        }

        public bool AllowNone
        {
            get
            {
                return this.m_options.AllowNone;
            }
            set
            {
                this.m_options.AllowNone = value;
            }
        }

        public new bool AppendKeywordsToMessage
        {
            get
            {
                return this.m_options.AppendKeywordsToMessage;
            }
            set
            {
                this.m_options.AppendKeywordsToMessage = value;
            }
        }

        public new bool IsReadOnly
        {
            get
            {
                return this.m_options.IsReadOnly;
            }
        }

        public new KeywordCollection Keywords
        {
            get
            {
                return this.m_options.Keywords;
            }
        }

        public new string Message
        {
            get
            {
                return this.m_options.Message;
            }
            set
            {
                this.m_options.Message = value;
            }
        }

        public Point3d NonInteractivePickPoint
        {
            get
            {
                return this.m_options.NonInteractivePickPoint;
            }
            set
            {
                this.m_options.NonInteractivePickPoint = value;
            }
        }

        public new void SetMessageAndKeywords(string messageAndKeywords, string globalKeywords)
        {
            this.m_options.SetMessageAndKeywords(messageAndKeywords, globalKeywords);
        }

        public bool UseNonInteractivePickPoint
        {
            get
            {
                return this.m_options.UseNonInteractivePickPoint;
            }
            set
            {
                this.m_options.UseNonInteractivePickPoint = value;
            }
        }

        public PromptNestedEntityOptions Options
        {
            get
            {
                return this.m_options;
            }
        }
    }
}