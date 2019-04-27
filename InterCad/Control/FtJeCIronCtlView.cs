using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using InterDesignCad.View;


namespace InterDesignCad.Control
{

    /// <summary>
    /// 控制wpf组件FtJeCIronForm的开关显示隐藏
    /// </summary>
    class FtJeCIronCtlView
    {
        private static PaletteSet _ps = null;
        public static PaletteSet GetInstance()
        {
            if (_ps == null)
            {
                _ps = new PaletteSet("简化简耳--C槽钢");
                _ps.Size = new Size(400, 300);
                _ps.DockEnabled = (DockSides)((int)DockSides.Left + (int)DockSides.Right);

                //PluginWindow window = new PluginWindow();
                //_ps.AddVisual("TestPlugin", window);
               
                _ps.KeepFocus = true;
                _ps.Visible = true;

            }
            return _ps;
        }

        public static void ShowModelessDialog()
        {
            GetInstance();
            if (_ps.Visible == false)
                _ps.Visible = true;
        
        }

    }
}
