namespace pictureCroper.form
{
    partial class CropTestForm
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.cropLeft = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cropTop = new DevExpress.XtraEditors.TextEdit();
            this.cropWidth = new DevExpress.XtraEditors.TextEdit();
            this.cropHeight = new DevExpress.XtraEditors.TextEdit();
            this.cropRotate = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.pictureCropControl1 = new pictureCroper.control.PictureCropControl();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropTop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRotate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.cropLeft);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.pictureCropControl1);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Controls.Add(this.cropTop);
            this.layoutControl1.Controls.Add(this.cropWidth);
            this.layoutControl1.Controls.Add(this.cropHeight);
            this.layoutControl1.Controls.Add(this.cropRotate);
            this.layoutControl1.Controls.Add(this.simpleButton3);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(701, 464);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // cropLeft
            // 
            this.cropLeft.Location = new System.Drawing.Point(39, 336);
            this.cropLeft.Name = "cropLeft";
            this.cropLeft.Size = new System.Drawing.Size(650, 20);
            this.cropLeft.StyleController = this.layoutControl1;
            this.cropLeft.TabIndex = 7;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(12, 310);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(223, 22);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "左旋转";
            this.simpleButton1.Click += new System.EventHandler(this.SimpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(239, 310);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(223, 22);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "右旋转";
            this.simpleButton2.Click += new System.EventHandler(this.SimpleButton2_Click);
            // 
            // cropTop
            // 
            this.cropTop.Location = new System.Drawing.Point(39, 360);
            this.cropTop.Name = "cropTop";
            this.cropTop.Size = new System.Drawing.Size(650, 20);
            this.cropTop.StyleController = this.layoutControl1;
            this.cropTop.TabIndex = 8;
            // 
            // cropWidth
            // 
            this.cropWidth.Location = new System.Drawing.Point(39, 384);
            this.cropWidth.Name = "cropWidth";
            this.cropWidth.Size = new System.Drawing.Size(650, 20);
            this.cropWidth.StyleController = this.layoutControl1;
            this.cropWidth.TabIndex = 9;
            // 
            // cropHeight
            // 
            this.cropHeight.Location = new System.Drawing.Point(39, 408);
            this.cropHeight.Name = "cropHeight";
            this.cropHeight.Size = new System.Drawing.Size(650, 20);
            this.cropHeight.StyleController = this.layoutControl1;
            this.cropHeight.TabIndex = 10;
            // 
            // cropRotate
            // 
            this.cropRotate.Location = new System.Drawing.Point(39, 432);
            this.cropRotate.Name = "cropRotate";
            this.cropRotate.Size = new System.Drawing.Size(650, 20);
            this.cropRotate.StyleController = this.layoutControl1;
            this.cropRotate.TabIndex = 11;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(466, 310);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(223, 22);
            this.simpleButton3.StyleController = this.layoutControl1;
            this.simpleButton3.TabIndex = 12;
            this.simpleButton3.Text = "simpleButton3";
            this.simpleButton3.Click += new System.EventHandler(this.SimpleButton3_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(701, 464);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.simpleButton1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 298);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(227, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButton2;
            this.layoutControlItem3.Location = new System.Drawing.Point(227, 298);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(227, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.cropLeft;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 324);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(681, 24);
            this.layoutControlItem4.Text = "左侧";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.cropTop;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 348);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(681, 24);
            this.layoutControlItem5.Text = "上侧";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.cropWidth;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 372);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(681, 24);
            this.layoutControlItem6.Text = "宽度";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cropHeight;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 396);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(681, 24);
            this.layoutControlItem7.Text = "高度";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.cropRotate;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 420);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(681, 24);
            this.layoutControlItem8.Text = "角度";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(24, 14);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.simpleButton3;
            this.layoutControlItem9.Location = new System.Drawing.Point(454, 298);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(227, 26);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // pictureCropControl1
            // 
            this.pictureCropControl1.CropBoxBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pictureCropControl1.CropContext = null;
            this.pictureCropControl1.IsPreview = false;
            this.pictureCropControl1.Location = new System.Drawing.Point(12, 12);
            this.pictureCropControl1.Name = "pictureCropControl1";
            this.pictureCropControl1.Size = new System.Drawing.Size(677, 294);
            this.pictureCropControl1.TabIndex = 4;
            this.pictureCropControl1.CropInfoChanged += new pictureCroper.control.CropInfoHandler(this.PictureCropControl1_CropInfoChanged);
            this.pictureCropControl1.Load += new System.EventHandler(this.PictureCropControl1_Load);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.pictureCropControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(681, 298);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // CropTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 464);
            this.Controls.Add(this.layoutControl1);
            this.Name = "CropTestForm";
            this.Text = "MyForm";
            this.Load += new System.EventHandler(this.MyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cropLeft.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropTop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropRotate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private control.PictureCropControl pictureCropControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.TextEdit cropLeft;
        private DevExpress.XtraEditors.TextEdit cropTop;
        private DevExpress.XtraEditors.TextEdit cropWidth;
        private DevExpress.XtraEditors.TextEdit cropHeight;
        private DevExpress.XtraEditors.TextEdit cropRotate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}