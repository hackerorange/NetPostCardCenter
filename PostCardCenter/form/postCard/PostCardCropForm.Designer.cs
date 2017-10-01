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
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule1 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule2 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue2 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule3 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue3 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostCardCropForm));
            this.postCardStateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.envelopeInfoController1 = new PostCardCenter.myController.EnvelopeInfoController();
            this.cropControllerCrop = new PostCardCenter.myController.PostCardCropController();
            this.cropControllerPreview = new PostCardCenter.myController.PostCardCropController();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.postCardNameColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.envelopeDetailGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup7 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::PostCardCenter.form.postCard.SubmitWaitForm), true, true);
            this.PostCardCropFormlayoutControl2ConvertedLayout = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeDetailGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PostCardCropFormlayoutControl2ConvertedLayout)).BeginInit();
            this.PostCardCropFormlayoutControl2ConvertedLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
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
            this.layoutControl1.Location = new System.Drawing.Point(12, 12);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(55, 428, 925, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1265, 677);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // envelopeInfoController1
            // 
            this.envelopeInfoController1.EnvelopeId = "";
            this.envelopeInfoController1.Location = new System.Drawing.Point(24, 478);
            this.envelopeInfoController1.Name = "envelopeInfoController1";
            this.envelopeInfoController1.Size = new System.Drawing.Size(456, 175);
            this.envelopeInfoController1.TabIndex = 27;
            this.envelopeInfoController1.Load += new System.EventHandler(this.envelopeInfoController1_Load);
            // 
            // cropControllerCrop
            // 
            this.cropControllerCrop.CropInfo = null;
            this.cropControllerCrop.Image = null;
            this.cropControllerCrop.IsPreview = false;
            this.cropControllerCrop.Location = new System.Drawing.Point(496, 12);
            this.cropControllerCrop.Name = "cropControllerCrop";
            this.cropControllerCrop.Node = null;
            this.cropControllerCrop.PostCardId = null;
            this.cropControllerCrop.ProductSize = new System.Drawing.Size(148, 100);
            this.cropControllerCrop.Scale = 0D;
            this.cropControllerCrop.Size = new System.Drawing.Size(457, 653);
            this.cropControllerCrop.TabIndex = 25;
            this.cropControllerCrop.WhiteSpacePercent = 0.8D;
            this.cropControllerCrop.CropInfoChanged += new PostCardCenter.myController.PostCardCropController.CropInfoChangeHandler(this.cropControllerCrop_CropInfoChanged);
            this.cropControllerCrop.FailureSubmit += new PostCardCenter.myController.PostCardCropController.SubmitResultHandler(this.cropControllerCrop_FailureSubmit);
            this.cropControllerCrop.SuccessSubmit += new PostCardCenter.myController.PostCardCropController.SubmitResultHandler(this.cropControllerCrop_SuccessSubmit);
            this.cropControllerCrop.OnSubmit += new PostCardCenter.myController.PostCardCropController.SubmitEventHandler(this.cropControllerCrop_OnSubmit);
            // 
            // cropControllerPreview
            // 
            this.cropControllerPreview.CropInfo = null;
            this.cropControllerPreview.Image = null;
            this.cropControllerPreview.IsPreview = true;
            this.cropControllerPreview.Location = new System.Drawing.Point(969, 43);
            this.cropControllerPreview.Name = "cropControllerPreview";
            this.cropControllerPreview.Node = null;
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
            this.treeList1.Cursor = System.Windows.Forms.Cursors.Default;
            treeListFormatRule1.ApplyToRow = true;
            treeListFormatRule1.Column = this.postCardStateColumn;
            treeListFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Appearance.ForeColor = System.Drawing.Color.Green;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = "已提交";
            treeListFormatRule1.Rule = formatConditionRuleValue1;
            treeListFormatRule2.ApplyToRow = true;
            treeListFormatRule2.Column = this.postCardStateColumn;
            treeListFormatRule2.Name = "Format1";
            formatConditionRuleValue2.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValue2.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue2.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue2.Value1 = "未提交";
            treeListFormatRule2.Rule = formatConditionRuleValue2;
            treeListFormatRule3.ApplyToRow = true;
            treeListFormatRule3.Column = this.postCardStateColumn;
            treeListFormatRule3.Name = "Format2";
            formatConditionRuleValue3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            formatConditionRuleValue3.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue3.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue3.Value1 = "已修改";
            treeListFormatRule3.Rule = formatConditionRuleValue3;
            this.treeList1.FormatRules.Add(treeListFormatRule1);
            this.treeList1.FormatRules.Add(treeListFormatRule2);
            this.treeList1.FormatRules.Add(treeListFormatRule3);
            this.treeList1.Location = new System.Drawing.Point(24, 43);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeList1.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.treeList1.SelectImageList = this.imageCollection1;
            this.treeList1.Size = new System.Drawing.Size(456, 388);
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
            this.envelopeDetailGroup,
            this.layoutControlItem7,
            this.layoutControlGroup5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1265, 677);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(484, 435);
            this.layoutControlGroup4.Text = "明信片图片信息";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeList1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(460, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(460, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(460, 392);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // envelopeDetailGroup
            // 
            this.envelopeDetailGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem13});
            this.envelopeDetailGroup.Location = new System.Drawing.Point(0, 435);
            this.envelopeDetailGroup.Name = "envelopeDetailGroup";
            this.envelopeDetailGroup.Size = new System.Drawing.Size(484, 222);
            this.envelopeDetailGroup.Text = "集合相关信息";
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.envelopeInfoController1;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(460, 179);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cropControllerCrop;
            this.layoutControlItem7.Location = new System.Drawing.Point(484, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(461, 657);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.layoutControlGroup7});
            this.layoutControlGroup5.Location = new System.Drawing.Point(945, 0);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(300, 657);
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
            this.layoutControlGroup7.Size = new System.Drawing.Size(276, 357);
            this.layoutControlGroup7.Text = "相关信息";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(252, 314);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.Size = new System.Drawing.Size(1289, 129);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "逆时针90°";
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem4.ImageOptions.Image")));
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "顺时针90°";
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.Image")));
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "旋转180°";
            this.barButtonItem6.Id = 6;
            this.barButtonItem6.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem6.ImageOptions.Image")));
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "相关操作";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem4, "快捷键：L");
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem5, "快捷键R");
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem6);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "旋转";
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
            // PostCardCropFormlayoutControl2ConvertedLayout
            // 
            this.PostCardCropFormlayoutControl2ConvertedLayout.Controls.Add(this.layoutControl1);
            this.PostCardCropFormlayoutControl2ConvertedLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PostCardCropFormlayoutControl2ConvertedLayout.Location = new System.Drawing.Point(0, 129);
            this.PostCardCropFormlayoutControl2ConvertedLayout.Name = "PostCardCropFormlayoutControl2ConvertedLayout";
            this.PostCardCropFormlayoutControl2ConvertedLayout.Root = this.layoutControlGroup3;
            this.PostCardCropFormlayoutControl2ConvertedLayout.Size = new System.Drawing.Size(1289, 701);
            this.PostCardCropFormlayoutControl2ConvertedLayout.TabIndex = 3;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup3.GroupBordersVisible = false;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(1289, 701);
            this.layoutControlGroup3.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.layoutControl1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControl1item";
            this.layoutControlItem2.Size = new System.Drawing.Size(1269, 681);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // PostCardCropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 830);
            this.Controls.Add(this.PostCardCropFormlayoutControl2ConvertedLayout);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "PostCardCropForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "postCardCropForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PostCardCropForm_FormClosing);
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
            ((System.ComponentModel.ISupportInitialize)(this.envelopeDetailGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PostCardCropFormlayoutControl2ConvertedLayout)).EndInit();
            this.PostCardCropFormlayoutControl2ConvertedLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
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
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private PostCardCropController cropControllerCrop;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private EnvelopeInfoController envelopeInfoController1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.LayoutControlGroup envelopeDetailGroup;
        private DevExpress.XtraLayout.LayoutControl PostCardCropFormlayoutControl2ConvertedLayout;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}