using System.Drawing;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using InterDesignCad.Util;
using InterDesignCad.View;


namespace InterDesignCad.Cmd
{
    public class FTCommand : IExtensionApplication
    {
        private static PaletteSet _ps = null;



        public static PaletteSet GetInstance()
        {
            if (_ps == null)
            {
                _ps = new PaletteSet("夹耳分解");
                _ps.Size = new Size(400, 600);
                _ps.DockEnabled = (DockSides)((int)DockSides.Left + (int)DockSides.Right);

                //PluginWindow window = new PluginWindow();
                //_ps.AddVisual("TestPlugin", window);
                PanelFT panelft = new PanelFT();
                _ps.AddVisual("FT Panel", panelft);

                _ps.KeepFocus = true;
                _ps.Visible = true;

            }
            return _ps;
        }

        // Ф初始化函数（在加载插件时执行）.
        public void Initialize()
        {
           // Log4NetHelper.InitLog4Net( "log4net.config");
        }

        // Ф加载插件时执行的函数.
        public void Terminate()
        {

        }

        // Ф在AutoCAD中执行命令时将调用该函数 .
        [CommandMethod("FT")]
        public void runFT()
        {


            _ps = GetInstance();
            if (_ps.Visible == false)
                _ps.Visible = true;
                
           
        }
    }
}
