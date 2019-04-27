using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InterDesignCad.Cmd;
using InterDesignCad.Util;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;



namespace InterDesignCad.Dialog.FT
{
    public partial class FtLgScForm : Form
    {
        private GroupBox groupBox1;
        private Button btnPickup;
        private TextBox tbheight;
        private RadioButton rbheightpickup;
        private RadioButton rbheightinput;
        private CheckBox cbxintongdim;
        private CheckBox cbxintong;
        private GroupBox groupBox2;
        private Label label1;
        private RadioButton rbspec5;
        private RadioButton rbspec4;
        private RadioButton rbspec3;
        private RadioButton rbspec2;
        private TextBox tbspec;
        private RadioButton rbspec1;
        private GroupBox groupBox3;
        private Button btnReturnMainMenu;
        private bool heighttype;
        //public double DragonHeight { set; get; }
        //public bool IsXintong { set; get; }
        //public int Length { set; get; }
        //public int Width { set; get; }
        //public int Height { set; get; }





        public FtLgScForm()
        {
            heighttype = false;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPickup = new System.Windows.Forms.Button();
            this.tbheight = new System.Windows.Forms.TextBox();
            this.rbheightpickup = new System.Windows.Forms.RadioButton();
            this.rbheightinput = new System.Windows.Forms.RadioButton();
            this.cbxintongdim = new System.Windows.Forms.CheckBox();
            this.cbxintong = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbspec5 = new System.Windows.Forms.RadioButton();
            this.rbspec4 = new System.Windows.Forms.RadioButton();
            this.rbspec3 = new System.Windows.Forms.RadioButton();
            this.rbspec2 = new System.Windows.Forms.RadioButton();
            this.tbspec = new System.Windows.Forms.TextBox();
            this.rbspec1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnReturnMainMenu = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPickup);
            this.groupBox1.Controls.Add(this.tbheight);
            this.groupBox1.Controls.Add(this.rbheightpickup);
            this.groupBox1.Controls.Add(this.rbheightinput);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 45);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "高度";
            // 
            // btnPickup
            // 
            this.btnPickup.Location = new System.Drawing.Point(264, 13);
            this.btnPickup.Name = "btnPickup";
            this.btnPickup.Size = new System.Drawing.Size(76, 24);
            this.btnPickup.TabIndex = 3;
            this.btnPickup.Text = "量取";
            this.btnPickup.UseVisualStyleBackColor = true;
            this.btnPickup.Click += new System.EventHandler(this.btnPickup_Click);
            // 
            // tbheight
            // 
            this.tbheight.Location = new System.Drawing.Point(158, 16);
            this.tbheight.Name = "tbheight";
            this.tbheight.Size = new System.Drawing.Size(71, 21);
            this.tbheight.TabIndex = 2;
            this.tbheight.Text = "800";
            // 
            // rbheightpickup
            // 
            this.rbheightpickup.AutoSize = true;
            this.rbheightpickup.Location = new System.Drawing.Point(91, 21);
            this.rbheightpickup.Name = "rbheightpickup";
            this.rbheightpickup.Size = new System.Drawing.Size(47, 16);
            this.rbheightpickup.TabIndex = 1;
            this.rbheightpickup.Text = "量取";
            this.rbheightpickup.UseVisualStyleBackColor = true;
            this.rbheightpickup.CheckedChanged += new System.EventHandler(this.rbheightpickup_CheckedChanged);
            // 
            // rbheightinput
            // 
            this.rbheightinput.AutoSize = true;
            this.rbheightinput.Checked = true;
            this.rbheightinput.Location = new System.Drawing.Point(23, 21);
            this.rbheightinput.Name = "rbheightinput";
            this.rbheightinput.Size = new System.Drawing.Size(47, 16);
            this.rbheightinput.TabIndex = 0;
            this.rbheightinput.TabStop = true;
            this.rbheightinput.Text = "输入";
            this.rbheightinput.UseVisualStyleBackColor = true;
            this.rbheightinput.CheckedChanged += new System.EventHandler(this.rbheightinput_CheckedChanged);
            // 
            // cbxintongdim
            // 
            this.cbxintongdim.AutoSize = true;
            this.cbxintongdim.Location = new System.Drawing.Point(157, 19);
            this.cbxintongdim.Name = "cbxintongdim";
            this.cbxintongdim.Size = new System.Drawing.Size(72, 16);
            this.cbxintongdim.TabIndex = 3;
            this.cbxintongdim.Text = "芯筒标注";
            this.cbxintongdim.UseVisualStyleBackColor = true;
            // 
            // cbxintong
            // 
            this.cbxintong.AutoSize = true;
            this.cbxintong.Checked = true;
            this.cbxintong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxintong.Location = new System.Drawing.Point(23, 20);
            this.cbxintong.Name = "cbxintong";
            this.cbxintong.Size = new System.Drawing.Size(84, 16);
            this.cbxintong.TabIndex = 2;
            this.cbxintong.Text = "是否要芯筒";
            this.cbxintong.UseVisualStyleBackColor = true;
            this.cbxintong.CheckedChanged += new System.EventHandler(this.cbxintong_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxintongdim);
            this.groupBox2.Controls.Add(this.cbxintong);
            this.groupBox2.Location = new System.Drawing.Point(16, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 45);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(378, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "高度";
            // 
            // rbspec5
            // 
            this.rbspec5.AutoSize = true;
            this.rbspec5.Location = new System.Drawing.Point(321, 19);
            this.rbspec5.Name = "rbspec5";
            this.rbspec5.Size = new System.Drawing.Size(47, 16);
            this.rbspec5.TabIndex = 7;
            this.rbspec5.Text = "其它";
            this.rbspec5.UseVisualStyleBackColor = true;
            this.rbspec5.CheckedChanged += new System.EventHandler(this.rbspec5_CheckedChanged);
            // 
            // rbspec4
            // 
            this.rbspec4.AutoSize = true;
            this.rbspec4.Location = new System.Drawing.Point(245, 18);
            this.rbspec4.Name = "rbspec4";
            this.rbspec4.Size = new System.Drawing.Size(71, 16);
            this.rbspec4.TabIndex = 6;
            this.rbspec4.Text = "120*60*4";
            this.rbspec4.UseVisualStyleBackColor = true;
            this.rbspec4.CheckedChanged += new System.EventHandler(this.rbspec4_CheckedChanged);
            // 
            // rbspec3
            // 
            this.rbspec3.AutoSize = true;
            this.rbspec3.Location = new System.Drawing.Point(166, 18);
            this.rbspec3.Name = "rbspec3";
            this.rbspec3.Size = new System.Drawing.Size(71, 16);
            this.rbspec3.TabIndex = 5;
            this.rbspec3.Text = "100*50*4";
            this.rbspec3.UseVisualStyleBackColor = true;
            this.rbspec3.CheckedChanged += new System.EventHandler(this.rbspec3_CheckedChanged);
            // 
            // rbspec2
            // 
            this.rbspec2.AutoSize = true;
            this.rbspec2.Location = new System.Drawing.Point(92, 19);
            this.rbspec2.Name = "rbspec2";
            this.rbspec2.Size = new System.Drawing.Size(65, 16);
            this.rbspec2.TabIndex = 4;
            this.rbspec2.Text = "80*60*4";
            this.rbspec2.UseVisualStyleBackColor = true;
            this.rbspec2.CheckedChanged += new System.EventHandler(this.rbspec2_CheckedChanged);
            // 
            // tbspec
            // 
            this.tbspec.Location = new System.Drawing.Point(420, 16);
            this.tbspec.Name = "tbspec";
            this.tbspec.Size = new System.Drawing.Size(71, 21);
            this.tbspec.TabIndex = 2;
            this.tbspec.Text = "150*100*4";
            // 
            // rbspec1
            // 
            this.rbspec1.AutoSize = true;
            this.rbspec1.Checked = true;
            this.rbspec1.Location = new System.Drawing.Point(23, 21);
            this.rbspec1.Name = "rbspec1";
            this.rbspec1.Size = new System.Drawing.Size(65, 16);
            this.rbspec1.TabIndex = 0;
            this.rbspec1.TabStop = true;
            this.rbspec1.Text = "60*40*4";
            this.rbspec1.UseVisualStyleBackColor = true;
            this.rbspec1.CheckedChanged += new System.EventHandler(this.rbspec1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.rbspec5);
            this.groupBox3.Controls.Add(this.rbspec4);
            this.groupBox3.Controls.Add(this.rbspec3);
            this.groupBox3.Controls.Add(this.rbspec2);
            this.groupBox3.Controls.Add(this.tbspec);
            this.groupBox3.Controls.Add(this.rbspec1);
            this.groupBox3.Location = new System.Drawing.Point(21, 170);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(502, 45);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // btnReturnMainMenu
            // 
            this.btnReturnMainMenu.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReturnMainMenu.Location = new System.Drawing.Point(421, 221);
            this.btnReturnMainMenu.Name = "btnReturnMainMenu";
            this.btnReturnMainMenu.Size = new System.Drawing.Size(102, 23);
            this.btnReturnMainMenu.TabIndex = 6;
            this.btnReturnMainMenu.Text = "返回主菜单";
            this.btnReturnMainMenu.UseVisualStyleBackColor = true;
            this.btnReturnMainMenu.Click += new System.EventHandler(this.btnReturnMainMenu_Click);
            // 
            // FtLgScForm
            // 
            this.ClientSize = new System.Drawing.Size(549, 257);
            this.Controls.Add(this.btnReturnMainMenu);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FtLgScForm";
            this.Text = "龙骨生成";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnPickup_Click(object sender, EventArgs e)
        {

            double dh;

            dh = SubJeCIFTCommand.GetTwoPointHeight(this.Handle);

            Log4NetHelper.WriteInfoLog("高度是为" + dh);

            SubJeCIFTCommand.GetInstance().IronRectPipePara.height = dh;


        }

        private void btnReturnMainMenu_Click(object sender, EventArgs e)
        {
            ///返回主菜单
            ///保存钢矩管参数
            /// 
            ///
            //gcmddata.IronRectPipePara.height = this.
            string temptext;

            if (heighttype == true)
            {
                return;
            }

            if (this.rbheightinput.Checked == true)
            {
                //temptext =  this.tbheight.Text;
                SubJeCIFTCommand.GetInstance().IronRectPipePara.height = Convert.ToDouble(this.tbheight.Text);
            }
            else
            {

            }

            if (this.cbxintong.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.isDrawXinTong = true;
            }
            else
                SubJeCIFTCommand.GetInstance().IronRectPipePara.isDrawXinTong = false;


            if (this.rbspec1.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec1.Text);

            }
            if (this.rbspec2.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec2.Text);
            }
            if (this.rbspec3.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec3.Text);
            }
            if (this.rbspec4.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec4.Text);
            }
            string ss = this.tbspec.Text;



            if (this.rbspec5.Checked == true)
            {
                if (ss.CompareTo("") != 0)
                    SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(ss);
            }
            
        }

        private void rbheightinput_CheckedChanged(object sender, EventArgs e)
        {
           
            double temp = 0;

            if (rbheightinput.Checked == true)
            {
                temp = Convert.ToDouble(tbheight.Text);
                heighttype = false;
                SubJeCIFTCommand.GetInstance().IronRectPipePara.height = temp;
                Log4NetHelper.WriteInfoLog("高度为:" + temp);
            }
            else
            {
                heighttype = true;
            }
        }

        private void rbheightpickup_CheckedChanged(object sender, EventArgs e)
        {
            if (rbheightpickup.Checked == true)
            {
                heighttype = true;

            }
            else
                heighttype = false;
            Log4NetHelper.WriteInfoLog("选择量取方式" );

        }

        private void cbxintong_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbxintong.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.isDrawXinTong = true;
            }
            else
                SubJeCIFTCommand.GetInstance().IronRectPipePara.isDrawXinTong = false;

        }

        private void rbspec1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbspec1.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec1.Text);

            }
        }

        private void rbspec2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbspec2.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec2.Text);

            }
        }

        private void rbspec3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbspec3.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec3.Text);

            }
        }

        private void rbspec4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbspec4.Checked == true)
            {
                SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(rbspec4.Text);

            }
        }

        private void rbspec5_CheckedChanged(object sender, EventArgs e)
        {
            string ss = this.tbspec.Text;



            if (this.rbspec5.Checked == true)
            {
                if (ss.CompareTo("") != 0)
                    SubJeCIFTCommand.GetInstance().IronRectPipePara.parseSpec(ss);
            }
        }
    }
}
