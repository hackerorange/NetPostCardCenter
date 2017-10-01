namespace PostCardCenter.myController
{
    partial class EnvelopeInfoController
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.customerName = new DevExpress.XtraEditors.TextEdit();
            this.paperName = new DevExpress.XtraEditors.TextEdit();
            this.productWidth = new DevExpress.XtraEditors.TextEdit();
            this.processorName = new DevExpress.XtraEditors.TextEdit();
            this.downloadProductFile = new DevExpress.XtraEditors.SimpleButton();
            this.productHeight = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paperName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productWidth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processorName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.progressBarControl1);
            this.layoutControl1.Controls.Add(this.customerName);
            this.layoutControl1.Controls.Add(this.paperName);
            this.layoutControl1.Controls.Add(this.productWidth);
            this.layoutControl1.Controls.Add(this.processorName);
            this.layoutControl1.Controls.Add(this.downloadProductFile);
            this.layoutControl1.Controls.Add(this.productHeight);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(712, 337);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // customerName
            // 
            this.customerName.Location = new System.Drawing.Point(63, 12);
            this.customerName.Name = "customerName";
            this.customerName.Size = new System.Drawing.Size(637, 20);
            this.customerName.StyleController = this.layoutControl1;
            this.customerName.TabIndex = 4;
            // 
            // paperName
            // 
            this.paperName.Location = new System.Drawing.Point(63, 36);
            this.paperName.Name = "paperName";
            this.paperName.Size = new System.Drawing.Size(637, 20);
            this.paperName.StyleController = this.layoutControl1;
            this.paperName.TabIndex = 5;
            // 
            // productWidth
            // 
            this.productWidth.Location = new System.Drawing.Point(63, 60);
            this.productWidth.Name = "productWidth";
            this.productWidth.Size = new System.Drawing.Size(291, 20);
            this.productWidth.StyleController = this.layoutControl1;
            this.productWidth.TabIndex = 6;
            // 
            // processorName
            // 
            this.processorName.Location = new System.Drawing.Point(63, 84);
            this.processorName.Name = "processorName";
            this.processorName.Size = new System.Drawing.Size(637, 20);
            this.processorName.StyleController = this.layoutControl1;
            this.processorName.TabIndex = 7;
            // 
            // downloadProductFile
            // 
            this.downloadProductFile.Location = new System.Drawing.Point(550, 303);
            this.downloadProductFile.Name = "downloadProductFile";
            this.downloadProductFile.Size = new System.Drawing.Size(150, 22);
            this.downloadProductFile.StyleController = this.layoutControl1;
            this.downloadProductFile.TabIndex = 8;
            this.downloadProductFile.Text = "下载文件(&D)";
            this.downloadProductFile.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // productHeight
            // 
            this.productHeight.Location = new System.Drawing.Point(409, 60);
            this.productHeight.Name = "productHeight";
            this.productHeight.Size = new System.Drawing.Size(291, 20);
            this.productHeight.StyleController = this.layoutControl1;
            this.productHeight.TabIndex = 9;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(396, 303);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(150, 22);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "刷新信息(&R)";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
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
            this.emptySpaceItem1,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(712, 337);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.customerName;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(692, 24);
            this.layoutControlItem1.Text = "客户名称";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.paperName;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(692, 24);
            this.layoutControlItem2.Text = "纸张名称";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.productWidth;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(346, 24);
            this.layoutControlItem3.Text = "成品宽度";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.processorName;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(692, 24);
            this.layoutControlItem4.Text = "处理人";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 14);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 96);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(692, 195);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.downloadProductFile;
            this.layoutControlItem5.Location = new System.Drawing.Point(538, 291);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(154, 26);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(154, 26);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(154, 26);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "下载文件";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.productHeight;
            this.layoutControlItem6.Location = new System.Drawing.Point(346, 48);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(346, 24);
            this.layoutControlItem6.Text = "成品高度";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.simpleButton2;
            this.layoutControlItem7.Location = new System.Drawing.Point(384, 291);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(154, 26);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(154, 26);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(154, 26);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(63, 303);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Properties.Step = 1;
            this.progressBarControl1.Size = new System.Drawing.Size(329, 18);
            this.progressBarControl1.StyleController = this.layoutControl1;
            this.progressBarControl1.TabIndex = 11;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.progressBarControl1;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 291);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(384, 26);
            this.layoutControlItem8.Text = "下载进度";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(48, 14);
            // 
            // EnvelopeInfoController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "EnvelopeInfoController";
            this.Size = new System.Drawing.Size(712, 337);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paperName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productWidth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processorName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit customerName;
        private DevExpress.XtraEditors.TextEdit paperName;
        private DevExpress.XtraEditors.TextEdit productWidth;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.TextEdit processorName;
        private DevExpress.XtraEditors.SimpleButton downloadProductFile;
        private DevExpress.XtraEditors.TextEdit productHeight;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
    }
}
