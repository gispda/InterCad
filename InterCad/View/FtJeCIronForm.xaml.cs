using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using HomeDesignCad.Dialog.FT;

namespace HomeDesignCad.View
{
    /// <summary>
    /// FtJeCIronForm.xaml 的交互逻辑
    /// </summary>
    public partial class FtJeCIronForm : UserControl
    {
        public FtJeCIronForm()
        {
            InitializeComponent();
        }

        private void btnIronrectpipe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FgLgScForm fglgscfrm = new FgLgScForm(); ;
            fglgscfrm.Show();
        }
        /// <summary>
        /// 龙骨选择距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndistdragonbonetoline_Click(object sender, System.Windows.RoutedEventArgs e)
        {


        }
        public double pickDistDragonBonetoLine()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptDistanceOptions opt1 = new PromptDistanceOptions("量取：");

            opt1.AllowNegative = false;
            opt1.AllowZero = false;
            opt1.AllowNone = false;
            opt1.UseDashedLine = true;

            PromptDoubleResult res = ed.GetDistance(opt1);

            if (res == null)
                return 0;
            else
                return res.Value;

        }

       
    }
}
