using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

using InterDesignCad.Cmd;
using InterDesignCad.Util;

namespace InterDesignCad.Dialog.FT
{
    public partial class FtJeCIronForm : Form
    {
        private bool isPickup;
        public FtJeCIronForm()
        {
            isPickup = false;
            InitializeComponent();
        }

        private void btnIronrectpipe_Click(object sender, EventArgs e)
        {
            FtLgScForm ftlgscfrm = new FtLgScForm(); ;
            //ftlgscfrm.Show();

            var result = AcAp.ShowModalDialog(ftlgscfrm);

            if(result == DialogResult.OK)
            { 
                AcAp.ShowAlertDialog("Pickup point"); 
            } 
            else 
            {
                AcAp.ShowModalDialog(ftlgscfrm);


            } 


        }

        private void btndistdragonbonetoline_Click(object sender, EventArgs e)
        {
            double length = 0;

            length = SubJeCIFTCommand.GetTwoPointLength(this.Handle);

            Log4NetHelper.WriteInfoLog("距离是为"+length);

            SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = length;

            isPickup = true;


        }

        private void rbdist50_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdist50.Checked == true)
            {
                isPickup = false;
                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = 50;
                Log4NetHelper.WriteInfoLog("距离是为:50");
            }
        }

        private void rbdist100_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdist100.Checked == true)
            {
                isPickup = false;
                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = 100;
                Log4NetHelper.WriteInfoLog("距离是为:100");
            }
        }

        private void rbdistinput_CheckedChanged(object sender, EventArgs e)
        {
            double temp = 0;
            if (rbdistinput.Checked == true)
            {
                isPickup = false;
                temp = Convert.ToDouble(tbinputdist.Text);

                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = temp;
                Log4NetHelper.WriteInfoLog("距离是为:" + temp);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((isPickup==false&&rbdist50.Checked==true) == true)
            {
                isPickup = false;
                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = 50;
                Log4NetHelper.WriteInfoLog("距离是为:50");
            }
            if ((isPickup==false&& rbdist100.Checked==true) == true)
            {
                isPickup = false;
                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = 100;
                Log4NetHelper.WriteInfoLog("距离是为:100");
            }

            double temp = 0;
            if ((isPickup==false && rbdistinput.Checked==true) == true)
            {
                isPickup = false;
                temp = Convert.ToDouble(tbinputdist.Text);

                SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine = temp;
                Log4NetHelper.WriteInfoLog("距离是为:" + temp);
            }
            Log4NetHelper.WriteInfoLog("龙骨量取距离是为:" + SubJeCIFTCommand.GetInstance().DistanceDragonBoneToSelectLine);
            Log4NetHelper.WriteInfoLog("钢矩管参数高度是为:" + SubJeCIFTCommand.GetInstance().IronRectPipePara.height);
            Log4NetHelper.WriteInfoLog("钢矩管参数是否要芯筒为:" + SubJeCIFTCommand.GetInstance().IronRectPipePara.isDrawXinTong);
            Log4NetHelper.WriteInfoLog("钢矩管参数长度是为:" + SubJeCIFTCommand.GetInstance().IronRectPipePara.length);
            Log4NetHelper.WriteInfoLog("钢矩管参数宽度是为:" + SubJeCIFTCommand.GetInstance().IronRectPipePara.width);
            Log4NetHelper.WriteInfoLog("钢矩管参数厚度是为:" + SubJeCIFTCommand.GetInstance().IronRectPipePara.thick);
        }
    }
}
