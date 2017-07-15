using PostCardCenter.myController;

namespace PostCardCenter.form.postCard
{
    partial class PostCardCropForm
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule4 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue4 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule5 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue5 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule6 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue6 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostCardCropForm));
            this.postCardStateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.envelopeInfoController1 = new PostCardCenter.myController.envelopeInfoController();
            this.cropControllerCrop = new PostCardCenter.myController.PostCardCropController();
            this.cropControllerPreview = new PostCardCenter.myController.PostCardCropController();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.postCardNameColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup6 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup7 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::PostCardCenter.form.postCard.SubmitWaitForm), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // postCardStateColumn
            // 
            this.postCardStateColumn.Caption = "状态";
            this.postCardStateColumn.FieldName = "status";
            this.postCardStateColumn.MinWidth = 60;
            this.postCardStateColumn.Name = "postCardStateColumn";
            this.postCardStateColumn.OptionsColumn.AllowEdit = false;
            this.postCardStateColumn.OptionsColumn.AllowFocus = false;
            this.postCardStateColumn.OptionsColumn.AllowMove = false;
            this.postCardStateColumn.OptionsColumn.AllowSort = false;
            this.postCardStateColumn.OptionsColumn.ReadOnly = true;
            this.postCardStateColumn.Visible = true;
            this.postCardStateColumn.VisibleIndex = 1;
            this.postCardStateColumn.Width = 60;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.envelopeInfoController1);
            this.layoutControl1.Controls.Add(this.cropControllerCrop);
            this.layoutControl1.Controls.Add(this.cropControllerPreview);
            this.layoutControl1.Controls.Add(this.treeList1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 147);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1295, 661);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // envelopeInfoController1
            // 
            this.envelopeInfoController1.Location = new System.Drawing.Point(24, 467);
            this.envelopeInfoController1.Name = "envelopeInfoController1";
            this.envelopeInfoController1.Size = new System.Drawing.Size(456, 170);
            this.envelopeInfoController1.TabIndex = 27;
            // 
            // cropControllerCrop
            // 
            this.cropControllerCrop.CropInfo = null;
            this.cropControllerCrop.Image = null;
            this.cropControllerCrop.IsPreview = false;
            this.cropControllerCrop.Location = new System.Drawing.Point(508, 43);
            this.cropControllerCrop.Name = "cropControllerCrop";
            this.cropControllerCrop.PostCardId = null;
            this.cropControllerCrop.ProductSize = new System.Drawing.Size(148, 100);
            this.cropControllerCrop.Scale = 0D;
            this.cropControllerCrop.Size = new System.Drawing.Size(463, 594);
            this.cropControllerCrop.TabIndex = 25;
            this.cropControllerCrop.WhiteSpacePercent = 0.8D;
            this.cropControllerCrop.CropInfoChanged += new PostCardCenter.myController.PostCardCropController.CropInfoChangeHandler(this.cropControllerCrop_CropInfoChanged);
            this.cropControllerCrop.BeforeSubmit += new PostCardCenter.myController.PostCardCropController.SubmitEventHandler(this.cropControllerCrop_BeforeSubmit);
            this.cropControllerCrop.AfterSubmit += new PostCardCenter.myController.PostCardCropController.SubmitEventHandler(this.cropControllerCrop_AfterSubmit);
            this.cropControllerCrop.OnSubmit += new PostCardCenter.myController.PostCardCropController.SubmitEventHandler(this.cropControllerCrop_OnSubmit);
            this.cropControllerCrop.Load += new System.EventHandler(this.cropControllerCrop_Load);
            // 
            // cropControllerPreview
            // 
            this.cropControllerPreview.CropInfo = null;
            this.cropControllerPreview.Image = null;
            this.cropControllerPreview.IsPreview = true;
            this.cropControllerPreview.Location = new System.Drawing.Point(987, 74);
            this.cropControllerPreview.Name = "cropControllerPreview";
            this.cropControllerPreview.PostCardId = null;
            this.cropControllerPreview.ProductSize = new System.Drawing.Size(148, 100);
            this.cropControllerPreview.Scale = 0D;
            this.cropControllerPreview.Size = new System.Drawing.Size(272, 253);
            this.cropControllerPreview.TabIndex = 16;
            this.cropControllerPreview.WhiteSpacePercent = 0.8D;
            this.cropControllerPreview.Load += new System.EventHandler(this.postCardCropController1_Load);
            this.cropControllerPreview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.postCardCropController1_KeyDown);
            this.cropControllerPreview.KeyUp += new System.Windows.Forms.KeyEventHandler(this.postCardCropController1_KeyUp);
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.postCardNameColumn,
            this.postCardStateColumn});
            treeListFormatRule4.ApplyToRow = true;
            treeListFormatRule4.Column = this.postCardStateColumn;
            treeListFormatRule4.Name = "Format0";
            formatConditionRuleValue4.Appearance.ForeColor = System.Drawing.Color.Green;
            formatConditionRuleValue4.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue4.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue4.Value1 = "已提交";
            treeListFormatRule4.Rule = formatConditionRuleValue4;
            treeListFormatRule5.ApplyToRow = true;
            treeListFormatRule5.Column = this.postCardStateColumn;
            treeListFormatRule5.Name = "Format1";
            formatConditionRuleValue5.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValue5.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue5.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue5.Value1 = "未提交";
            treeListFormatRule5.Rule = formatConditionRuleValue5;
            treeListFormatRule6.ApplyToRow = true;
            treeListFormatRule6.Column = this.postCardStateColumn;
            treeListFormatRule6.Name = "Format2";
            formatConditionRuleValue6.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            formatConditionRuleValue6.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue6.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue6.Value1 = "已修改";
            treeListFormatRule6.Rule = formatConditionRuleValue6;
            this.treeList1.FormatRules.Add(treeListFormatRule4);
            this.treeList1.FormatRules.Add(treeListFormatRule5);
            this.treeList1.FormatRules.Add(treeListFormatRule6);
            this.treeList1.Location = new System.Drawing.Point(24, 43);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList1.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.treeList1.SelectImageList = this.imageCollection1;
            this.treeList1.Size = new System.Drawing.Size(456, 377);
            this.treeList1.TabIndex = 6;
            this.treeList1.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
            // 
            // postCardNameColumn
            // 
            this.postCardNameColumn.Caption = "名称";
            this.postCardNameColumn.FieldName = "name";
            this.postCardNameColumn.MinWidth = 88;
            this.postCardNameColumn.Name = "postCardNameColumn";
            this.postCardNameColumn.OptionsColumn.AllowEdit = false;
            this.postCardNameColumn.OptionsColumn.AllowFocus = false;
            this.postCardNameColumn.OptionsColumn.AllowMove = false;
            this.postCardNameColumn.OptionsColumn.AllowSort = false;
            this.postCardNameColumn.OptionsColumn.ReadOnly = true;
            this.postCardNameColumn.Visible = true;
            this.postCardNameColumn.VisibleIndex = 0;
            this.postCardNameColumn.Width = 378;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertGalleryImage("open_16x16.png", "images/actions/open_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png"), 0);
            this.imageCollection1.Images.SetKeyName(0, "open_16x16.png");
            this.imageCollection1.InsertGalleryImage("bofileattachment_16x16.png", "images/business%20objects/bofileattachment_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/business%20objects/bofileattachment_16x16.png"), 1);
            this.imageCollection1.Images.SetKeyName(1, "bofileattachment_16x16.png");
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup4,
            this.layoutControlGroup6,
            this.layoutControlGroup2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1295, 661);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(484, 424);
            this.layoutControlGroup4.Text = "明信片图片信息";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeList1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(460, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(460, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(460, 381);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup6
            // 
            this.layoutControlGroup6.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.layoutControlGroup5});
            this.layoutControlGroup6.Location = new System.Drawing.Point(484, 0);
            this.layoutControlGroup6.Name = "layoutControlGroup6";
            this.layoutControlGroup6.Size = new System.Drawing.Size(791, 641);
            this.layoutControlGroup6.Text = "明信片裁切窗口";
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cropControllerCrop;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(467, 598);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.layoutControlGroup7});
            this.layoutControlGroup5.Location = new System.Drawing.Point(467, 0);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(300, 598);
            this.layoutControlGroup5.Text = "成品预览";
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.cropControllerPreview;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(276, 257);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(276, 257);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(276, 257);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlGroup7
            // 
            this.layoutControlGroup7.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1});
            this.layoutControlGroup7.Location = new System.Drawing.Point(0, 257);
            this.layoutControlGroup7.Name = "layoutControlGroup7";
            this.layoutControlGroup7.Size = new System.Drawing.Size(276, 298);
            this.layoutControlGroup7.Text = "相关信息";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(252, 255);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem13});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 424);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(484, 217);
            this.layoutControlGroup2.Text = "集合相关信息";
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.envelopeInfoController1;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(460, 174);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1295, 147);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "A版";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "B版";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "C版";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "逆时针90°";
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem4.ImageOptions.Image")));
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "顺时针90°";
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.Image")));
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "旋转180°";
            this.barButtonItem6.Id = 6;
            this.barButtonItem6.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem6.ImageOptions.Image")));
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "正面版式";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem5);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem6);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "旋转";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 808);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1295, 31);
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // PostCardCropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 839);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "PostCardCropForm";
            this.Ribbon = this.ribbonControl1;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "postCardCropForm";
            this.Load += new System.EventHandler(this.postCardCropForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PostCardCropForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PostCardCropForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn postCardNameColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn postCardStateColumn;
        private PostCardCropController cropControllerPreview;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private PostCardCropController cropControllerCrop;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private envelopeInfoController envelopeInfoController1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
    }
}