namespace HomeDesignCAD.FT
{
    partial class FtJeCIronForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnIronrectpipe = new System.Windows.Forms.Button();
            this.btndistdragonbonetoline = new System.Windows.Forms.Button();
            this.rbdist50 = new System.Windows.Forms.RadioButton();
            this.rbdist100 = new System.Windows.Forms.RadioButton();
            this.rbdistinput = new System.Windows.Forms.RadioButton();
            this.tbinputdist = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnIronrectpipe
            // 
            this.btnIronrectpipe.Location = new System.Drawing.Point(12, 12);
            this.btnIronrectpipe.Name = "btnIronrectpipe";
            this.btnIronrectpipe.Size = new System.Drawing.Size(113, 23);
            this.btnIronrectpipe.TabIndex = 0;
            this.btnIronrectpipe.Text = "钢矩管参数";
            this.btnIronrectpipe.UseVisualStyleBackColor = true;
            this.btnIronrectpipe.Click += new System.EventHandler(this.btnIronrectpipe_Click);
            // 
            // btndistdragonbonetoline
            // 
            this.btndistdragonbonetoline.Location = new System.Drawing.Point(12, 53);
            this.btndistdragonbonetoline.Name = "btndistdragonbonetoline";
            this.btndistdragonbonetoline.Size = new System.Drawing.Size(127, 22);
            this.btndistdragonbonetoline.TabIndex = 1;
            this.btndistdragonbonetoline.Text = "<--龙骨离量取距离";
            this.btndistdragonbonetoline.UseVisualStyleBackColor = true;
            this.btndistdragonbonetoline.Click += new System.EventHandler(this.btndistdragonbonetoline_Click);
            // 
            // rbdist50
            // 
            this.rbdist50.AutoSize = true;
            this.rbdist50.Checked = true;
            this.rbdist50.Location = new System.Drawing.Point(170, 56);
            this.rbdist50.Name = "rbdist50";
            this.rbdist50.Size = new System.Drawing.Size(35, 16);
            this.rbdist50.TabIndex = 2;
            this.rbdist50.TabStop = true;
            this.rbdist50.Text = "50";
            this.rbdist50.UseVisualStyleBackColor = true;
            // 
            // rbdist100
            // 
            this.rbdist100.AutoSize = true;
            this.rbdist100.Location = new System.Drawing.Point(230, 56);
            this.rbdist100.Name = "rbdist100";
            this.rbdist100.Size = new System.Drawing.Size(41, 16);
            this.rbdist100.TabIndex = 3;
            this.rbdist100.TabStop = true;
            this.rbdist100.Text = "100";
            this.rbdist100.UseVisualStyleBackColor = true;
            // 
            // rbdistinput
            // 
            this.rbdistinput.AutoSize = true;
            this.rbdistinput.Location = new System.Drawing.Point(277, 57);
            this.rbdistinput.Name = "rbdistinput";
            this.rbdistinput.Size = new System.Drawing.Size(47, 16);
            this.rbdistinput.TabIndex = 4;
            this.rbdistinput.TabStop = true;
            this.rbdistinput.Text = "输入";
            this.rbdistinput.UseVisualStyleBackColor = true;
            // 
            // tbinputdist
            // 
            this.tbinputdist.Location = new System.Drawing.Point(330, 55);
            this.tbinputdist.Name = "tbinputdist";
            this.tbinputdist.Size = new System.Drawing.Size(100, 21);
            this.tbinputdist.TabIndex = 5;
            this.tbinputdist.Text = "150";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(230, 89);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(355, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FtJeCIronForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 118);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbinputdist);
            this.Controls.Add(this.rbdistinput);
            this.Controls.Add(this.rbdist100);
            this.Controls.Add(this.rbdist50);
            this.Controls.Add(this.btndistdragonbonetoline);
            this.Controls.Add(this.btnIronrectpipe);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FtJeCIronForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "简化简耳--C槽钢";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FtJeCIronForm_FormClosed);
            this.Load += new System.EventHandler(this.FtJeCIronForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIronrectpipe;
        private System.Windows.Forms.Button btndistdragonbonetoline;
        private System.Windows.Forms.RadioButton rbdist50;
        private System.Windows.Forms.RadioButton rbdist100;
        private System.Windows.Forms.RadioButton rbdistinput;
        private System.Windows.Forms.TextBox tbinputdist;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}