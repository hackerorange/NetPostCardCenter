namespace PostCardCenter.myController
{
    partial class CropBoxControl
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
            this.heightScale = new DevExpress.XtraEditors.TextEdit();
            this.widthScale = new DevExpress.XtraEditors.TextEdit();
            this.topScale = new DevExpress.XtraEditors.TextEdit();
            this.leftScale = new DevExpress.XtraEditors.TextEdit();
            this.postCardCropController2 = new PostCardCenter.myController.PostCardCropController();
            this.postCardCropController1 = new PostCardCenter.myController.PostCardCropController();
            this.rotateAngle = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotateAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.heightScale);
            this.layoutControl1.Controls.Add(this.widthScale);
            this.layoutControl1.Controls.Add(this.topScale);
            this.layoutControl1.Controls.Add(this.leftScale);
            this.layoutControl1.Controls.Add(this.postCardCropController2);
            this.layoutControl1.Controls.Add(this.postCardCropController1);
            this.layoutControl1.Controls.Add(this.rotateAngle);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(926, 627);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // heightScale
            // 
            this.heightScale.Location = new System.Drawing.Point(663, 428);
            this.heightScale.Name = "heightScale";
            this.heightScale.Properties.ReadOnly = true;
            this.heightScale.Size = new System.Drawing.Size(249, 20);
            this.heightScale.StyleController = this.layoutControl1;
            this.heightScale.TabIndex = 10;
            // 
            // widthScale
            // 
            this.widthScale.Location = new System.Drawing.Point(663, 452);
            this.widthScale.Name = "widthScale";
            this.widthScale.Properties.ReadOnly = true;
            this.widthScale.Size = new System.Drawing.Size(249, 20);
            this.widthScale.StyleController = this.layoutControl1;
            this.widthScale.TabIndex = 9;
            // 
            // topScale
            // 
            this.topScale.Location = new System.Drawing.Point(663, 404);
            this.topScale.Name = "topScale";
            this.topScale.Properties.ReadOnly = true;
            this.topScale.Size = new System.Drawing.Size(249, 20);
            this.topScale.StyleController = this.layoutControl1;
            this.topScale.TabIndex = 8;
            // 
            // leftScale
            // 
            this.leftScale.EditValue = "";
            this.leftScale.Location = new System.Drawing.Point(663, 380);
            this.leftScale.Name = "leftScale";
            this.leftScale.Properties.ReadOnly = true;
            this.leftScale.Size = new System.Drawing.Size(249, 20);
            this.leftScale.StyleController = this.layoutControl1;
            this.leftScale.TabIndex = 7;
            // 
            // postCardCropController2
            // 
            this.postCardCropController2.CropInfo = null;
            this.postCardCropController2.Image = null;
            this.postCardCropController2.IsPreview = true;
            this.postCardCropController2.Location = new System.Drawing.Point(612, 33);
            this.postCardCropController2.Name = "postCardCropController2";
            this.postCardCropController2.PostCardId = null;
            this.postCardCropController2.ProductSize = new System.Drawing.Size(148, 100);
            this.postCardCropController2.Scale = 0D;
            this.postCardCropController2.Size = new System.Drawing.Size(300, 300);
            this.postCardCropController2.TabIndex = 6;
            this.postCardCropController2.WhiteSpacePercent = 0.9D;
            // 
            // postCardCropController1
            // 
            this.postCardCropController1.CropInfo = null;
            this.postCardCropController1.Image = null;
            this.postCardCropController1.IsPreview = false;
            this.postCardCropController1.Location = new System.Drawing.Point(2, 2);
            this.postCardCropController1.Margin = new System.Windows.Forms.Padding(10);
            this.postCardCropController1.Name = "postCardCropController1";
            this.postCardCropController1.PostCardId = null;
            this.postCardCropController1.ProductSize = new System.Drawing.Size(148, 100);
            this.postCardCropController1.Scale = 0D;
            this.postCardCropController1.Size = new System.Drawing.Size(594, 623);
            this.postCardCropController1.TabIndex = 5;
            this.postCardCropController1.WhiteSpacePercent = 0.8D;
            this.postCardCropController1.CropInfoChanged += new PostCardCenter.myController.PostCardCropController.CropInfoChangeHandler(this.postCardCropController1_cropInfoChanged);
            this.postCardCropController1.Load += new System.EventHandler(this.postCardCropController1_Load);
            // 
            // rotateAngle
            // 
            this.rotateAngle.Location = new System.Drawing.Point(663, 476);
            this.rotateAngle.Name = "rotateAngle";
            this.rotateAngle.Properties.ReadOnly = true;
            this.rotateAngle.Size = new System.Drawing.Size(249, 20);
            this.rotateAngle.StyleController = this.layoutControl1;
            this.rotateAngle.TabIndex = 11;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3,
            this.layoutControlGroup2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(926, 627);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4});
            this.layoutControlGroup3.Location = new System.Drawing.Point(598, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(328, 347);
            this.layoutControlGroup3.Text = "预览区域";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.postCardCropController2;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(304, 304);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(304, 304);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(304, 304);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem2,
            this.layoutControlItem6});
            this.layoutControlGroup2.Location = new System.Drawing.Point(598, 347);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(328, 280);
            this.layoutControlGroup2.Text = "裁切区域";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 120);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(304, 117);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.leftScale;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(304, 24);
            this.layoutControlItem1.Text = "左侧比例";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.widthScale;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(304, 24);
            this.layoutControlItem5.Text = "宽度比例";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.rotateAngle;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(304, 24);
            this.layoutControlItem7.Text = "旋转角度";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.topScale;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(304, 24);
            this.layoutControlItem2.Text = "上侧比例";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.heightScale;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(304, 24);
            this.layoutControlItem6.Text = "高度比例";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.postCardCropController1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(598, 627);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // CropBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CropBoxControl";
            this.Size = new System.Drawing.Size(926, 627);
            this.Load += new System.EventHandler(this.CropBoxControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.heightScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotateAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private PostCardCropController postCardCropController1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private PostCardCropController postCardCropController2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.TextEdit topScale;
        private DevExpress.XtraEditors.TextEdit leftScale;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit heightScale;
        private DevExpress.XtraEditors.TextEdit widthScale;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.TextEdit rotateAngle;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        
    }
}
