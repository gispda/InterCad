using System.Drawing;
using System.Windows;
using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.EditorInput;

using InterDesignCad.View;
using InterDesignCad.Dialog.FT;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using System;

///////////
///////将该类从panel修改成无panel的形式
/////////

namespace InterDesignCad.Cmd
{
    public class SubJeCIFTCommand
    {
        //private static PaletteSet _ps = null;
        private static CmdDataFT cmddata = null;

        internal static CmdDataFT GetInstance()
        {
            if (cmddata == null)
            {
                cmddata = new CmdDataFT();

            }
            return cmddata;
        }

        //public static PaletteSet GetInstance()
        //{
        //    if (_ps == null)
        //    {
        //        _ps = new PaletteSet("简化简耳--C槽钢");
        //        _ps.Size = new Size(400, 300);
        //        _ps.DockEnabled = (DockSides)((int)DockSides.Left + (int)DockSides.Right);

        //        //PluginWindow window = new PluginWindow();
        //        //_ps.AddVisual("TestPlugin", window);
        //       //FtJeCIronForm paneljeciron = new FtJeCIronForm();
        //        //_ps.AddVisual("JZCIRON Panel", paneljeciron);

        //        _ps.KeepFocus = true;
        //        _ps.Visible = true;

        //    }
        //    return _ps;
        //}

        // Ф初始化函数（在加载插件时执行）.
        public void Initialize()
        {
           
        }

        // Ф加载插件时执行的函数.
        public void Terminate()
        {

        }

        // Ф在AutoCAD中执行命令时将调用该函数 .
        [CommandMethod("subFT1")]
        public void runsubFT1()
        {
            FtJeCIronForm jecironfrm = new FtJeCIronForm();
            var ed = AcAp.DocumentManager.MdiActiveDocument.Editor;

            AcAp.ShowModalDialog(jecironfrm);


            //_ps = GetInstance();
            //if (_ps.Visible == false)
            //    _ps.Visible = true;
                
           



        }

        internal static double GetTwoPointLength(IntPtr handle)
        {
            double length = 0;

            var ed = AcAp.DocumentManager.MdiActiveDocument.Editor;
            using(var eduserinteraction = ed.StartUserInteraction(handle))
            {

            PromptDistanceOptions opt1 = new PromptDistanceOptions("量取：");

            opt1.AllowNegative = false;
            opt1.AllowZero = false;
            opt1.AllowNone = false;
            opt1.UseDashedLine = true;

            PromptDoubleResult res = ed.GetDistance(opt1);
            ed.WriteMessage("\n距离是..."+res.Value);
            if (res != null)
                length = res.Value;
             }
            Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Focus();
            return length;

        }

        internal static double GetTwoPointHeight(IntPtr handle)
        {
            double length = 0;

            var ed = AcAp.DocumentManager.MdiActiveDocument.Editor;
            using (var eduserinteraction = ed.StartUserInteraction(handle))
            {

                PromptDistanceOptions opt1 = new PromptDistanceOptions("量取高度：");

                opt1.AllowNegative = false;
                opt1.AllowZero = false;
                opt1.AllowNone = false;
                opt1.UseDashedLine = true;

                PromptDoubleResult res = ed.GetDistance(opt1);
                ed.WriteMessage("\n高度是..." + res.Value);
                if (res != null)
                    length = res.Value;
            }
            Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Focus();
            return length;

        }




    }
}
