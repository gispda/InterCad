using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.AutoCAD.ApplicationServices;
using InterDesignCad.Cmd;
using InterDesignCad.Control;
using InterDesignCad.Dialog.FT;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;


namespace InterDesignCad.View
{
    /// <summary>
    /// PanelFT.xaml 的交互逻辑
    /// </summary>
    public partial class PanelFT : UserControl
    {
        public PanelFT()
        {
            InitializeComponent();
        }

        private void btnJzCiron_Click(object sender, RoutedEventArgs e)
        {
           // jzcpanel.Visibility = Visibility.Visible;
            //FTCommand.GetInstance().Visible = false;
            ////FtJeCIronCtlView.ShowModelessDialog();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("subft1\n",false,false,true);
            //FtJeCIronForm ftjecironform = new FtJeCIronForm();

            //AcAp.ShowModalDialog(ftjecironform);

           // ftjecironform.Show();

           // mainftpanel.Visibility = Visibility.Hidden;
        }
    }
}
