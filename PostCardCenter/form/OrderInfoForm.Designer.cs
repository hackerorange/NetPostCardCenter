namespace PostCardCenter.form
{
    partial class OrderInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderInfoForm));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.envelopeDirectory = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.envelopePaperName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.envelopeFrontStyle = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.envelopeBackStyle = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.envelopeProductWidth = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.envelopeProductHeigtht = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.envelopePostCardCount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.batchCreateFromDesktop = new DevExpress.XtraBars.BarButtonItem();
            this.removeEnvelopeButton = new DevExpress.XtraBars.BarButtonItem();
            this.closeButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.saveButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.orderDirectoryTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.orderTaobaoIdTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.orderIsUrgentCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.orderInfoValidationProvider = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDirectoryTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderTaobaoIdTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderIsUrgentCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderInfoValidationProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Controls.Add(this.orderDirectoryTextEdit);
            this.layoutControl1.Controls.Add(this.orderTaobaoIdTextEdit);
            this.layoutControl1.Controls.Add(this.orderIsUrgentCheckEdit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 129);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(512, 310, 450, 344);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(890, 672);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(24, 134);
            this.gridControl1.MainView = this.advBandedGridView1;
            this.gridControl1.MenuManager = this.ribbon;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(842, 514);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand3,
            this.gridBand2,
            this.gridBand4});
            this.advBandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.envelopePaperName,
            this.envelopeProductWidth,
            this.envelopeProductHeigtht,
            this.envelopeFrontStyle,
            this.envelopeDirectory,
            this.envelopeBackStyle,
            this.envelopePostCardCount});
            this.advBandedGridView1.GridControl = this.gridControl1;
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.OptionsDetail.EnableMasterViewMode = false;
            this.advBandedGridView1.OptionsView.ShowDetailButtons = false;
            this.advBandedGridView1.OptionsView.ShowFooter = true;
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            this.advBandedGridView1.DoubleClick += new System.EventHandler(this.advBandedGridView1_DoubleClick);
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "文件相关";
            this.gridBand1.Columns.Add(this.envelopeDirectory);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.OptionsBand.AllowSize = false;
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 240;
            // 
            // envelopeDirectory
            // 
            this.envelopeDirectory.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeDirectory.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeDirectory.Caption = "文件夹名称";
            this.envelopeDirectory.FieldName = "directory.Name";
            this.envelopeDirectory.Name = "envelopeDirectory";
            this.envelopeDirectory.OptionsColumn.AllowEdit = false;
            this.envelopeDirectory.OptionsColumn.AllowFocus = false;
            this.envelopeDirectory.OptionsColumn.AllowMove = false;
            this.envelopeDirectory.OptionsColumn.AllowSize = false;
            this.envelopeDirectory.OptionsColumn.ReadOnly = true;
            this.envelopeDirectory.Visible = true;
            this.envelopeDirectory.Width = 240;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "版式信息";
            this.gridBand3.Columns.Add(this.envelopePaperName);
            this.gridBand3.Columns.Add(this.envelopeFrontStyle);
            this.gridBand3.Columns.Add(this.envelopeBackStyle);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 1;
            this.gridBand3.Width = 290;
            // 
            // envelopePaperName
            // 
            this.envelopePaperName.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopePaperName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopePaperName.Caption = "纸张名称";
            this.envelopePaperName.FieldName = "paperName";
            this.envelopePaperName.MinWidth = 150;
            this.envelopePaperName.Name = "envelopePaperName";
            this.envelopePaperName.OptionsColumn.AllowEdit = false;
            this.envelopePaperName.OptionsColumn.AllowFocus = false;
            this.envelopePaperName.OptionsColumn.AllowMove = false;
            this.envelopePaperName.OptionsColumn.AllowSize = false;
            this.envelopePaperName.OptionsColumn.ReadOnly = true;
            this.envelopePaperName.Visible = true;
            this.envelopePaperName.Width = 150;
            // 
            // envelopeFrontStyle
            // 
            this.envelopeFrontStyle.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeFrontStyle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeFrontStyle.Caption = "正面版式";
            this.envelopeFrontStyle.FieldName = "frontStyle";
            this.envelopeFrontStyle.MinWidth = 70;
            this.envelopeFrontStyle.Name = "envelopeFrontStyle";
            this.envelopeFrontStyle.OptionsColumn.AllowEdit = false;
            this.envelopeFrontStyle.OptionsColumn.AllowFocus = false;
            this.envelopeFrontStyle.OptionsColumn.AllowMove = false;
            this.envelopeFrontStyle.OptionsColumn.AllowSize = false;
            this.envelopeFrontStyle.OptionsColumn.ReadOnly = true;
            this.envelopeFrontStyle.Visible = true;
            this.envelopeFrontStyle.Width = 70;
            // 
            // envelopeBackStyle
            // 
            this.envelopeBackStyle.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeBackStyle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeBackStyle.Caption = "反面样式";
            this.envelopeBackStyle.FieldName = "backStyle";
            this.envelopeBackStyle.MinWidth = 70;
            this.envelopeBackStyle.Name = "envelopeBackStyle";
            this.envelopeBackStyle.OptionsColumn.AllowEdit = false;
            this.envelopeBackStyle.OptionsColumn.AllowFocus = false;
            this.envelopeBackStyle.OptionsColumn.AllowMove = false;
            this.envelopeBackStyle.OptionsColumn.AllowSize = false;
            this.envelopeBackStyle.OptionsColumn.ReadOnly = true;
            this.envelopeBackStyle.Visible = true;
            this.envelopeBackStyle.Width = 70;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "成品尺寸";
            this.gridBand2.Columns.Add(this.envelopeProductWidth);
            this.gridBand2.Columns.Add(this.envelopeProductHeigtht);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.OptionsBand.AllowMove = false;
            this.gridBand2.OptionsBand.AllowSize = false;
            this.gridBand2.VisibleIndex = 2;
            this.gridBand2.Width = 140;
            // 
            // envelopeProductWidth
            // 
            this.envelopeProductWidth.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProductWidth.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProductWidth.Caption = "宽度";
            this.envelopeProductWidth.FieldName = "productWidth";
            this.envelopeProductWidth.MinWidth = 70;
            this.envelopeProductWidth.Name = "envelopeProductWidth";
            this.envelopeProductWidth.OptionsColumn.AllowEdit = false;
            this.envelopeProductWidth.OptionsColumn.AllowFocus = false;
            this.envelopeProductWidth.OptionsColumn.AllowMove = false;
            this.envelopeProductWidth.OptionsColumn.AllowSize = false;
            this.envelopeProductWidth.OptionsColumn.ReadOnly = true;
            this.envelopeProductWidth.Visible = true;
            this.envelopeProductWidth.Width = 70;
            // 
            // envelopeProductHeigtht
            // 
            this.envelopeProductHeigtht.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProductHeigtht.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProductHeigtht.Caption = "高度";
            this.envelopeProductHeigtht.FieldName = "productHeight";
            this.envelopeProductHeigtht.MinWidth = 70;
            this.envelopeProductHeigtht.Name = "envelopeProductHeigtht";
            this.envelopeProductHeigtht.OptionsColumn.AllowEdit = false;
            this.envelopeProductHeigtht.OptionsColumn.AllowFocus = false;
            this.envelopeProductHeigtht.OptionsColumn.AllowMove = false;
            this.envelopeProductHeigtht.OptionsColumn.AllowSize = false;
            this.envelopeProductHeigtht.OptionsColumn.ReadOnly = true;
            this.envelopeProductHeigtht.Visible = true;
            this.envelopeProductHeigtht.Width = 70;
            // 
            // gridBand4
            // 
            this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand4.Caption = "统计相关";
            this.gridBand4.Columns.Add(this.envelopePostCardCount);
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.VisibleIndex = 3;
            this.gridBand4.Width = 100;
            // 
            // envelopePostCardCount
            // 
            this.envelopePostCardCount.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopePostCardCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopePostCardCount.Caption = "明信片张数";
            this.envelopePostCardCount.FieldName = "postCardCount";
            this.envelopePostCardCount.MinWidth = 100;
            this.envelopePostCardCount.Name = "envelopePostCardCount";
            this.envelopePostCardCount.OptionsColumn.AllowEdit = false;
            this.envelopePostCardCount.OptionsColumn.AllowFocus = false;
            this.envelopePostCardCount.OptionsColumn.AllowMove = false;
            this.envelopePostCardCount.OptionsColumn.AllowSize = false;
            this.envelopePostCardCount.OptionsColumn.ReadOnly = true;
            this.envelopePostCardCount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "postCardCount", "{0:0}")});
            this.envelopePostCardCount.Visible = true;
            this.envelopePostCardCount.Width = 100;
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.batchCreateFromDesktop,
            this.removeEnvelopeButton,
            this.closeButtonItem,
            this.saveButtonItem,
            this.barEditItem1,
            this.skinRibbonGalleryBarItem1});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 8;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbon.Size = new System.Drawing.Size(890, 129);
            // 
            // batchCreateFromDesktop
            // 
            this.batchCreateFromDesktop.Caption = "获取集合";
            this.batchCreateFromDesktop.Id = 1;
            this.batchCreateFromDesktop.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("batchCreateFromDesktop.ImageOptions.Image")));
            this.batchCreateFromDesktop.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.D));
            this.batchCreateFromDesktop.Name = "batchCreateFromDesktop";
            this.batchCreateFromDesktop.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.batchCreateFromDesktop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.batchCreateFromDesktop_ItemClick);
            // 
            // removeEnvelopeButton
            // 
            this.removeEnvelopeButton.Caption = "删除选定集合";
            this.removeEnvelopeButton.Id = 2;
            this.removeEnvelopeButton.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("removeEnvelopeButton.ImageOptions.Image")));
            this.removeEnvelopeButton.Name = "removeEnvelopeButton";
            this.removeEnvelopeButton.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.removeEnvelopeButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.removeEnvelopeButton_ItemClick);
            // 
            // closeButtonItem
            // 
            this.closeButtonItem.Caption = "关闭";
            this.closeButtonItem.Id = 3;
            this.closeButtonItem.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("closeButtonItem.ImageOptions.Image")));
            this.closeButtonItem.Name = "closeButtonItem";
            this.closeButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.closeButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.closeButtonItem_ItemClick);
            // 
            // saveButtonItem
            // 
            this.saveButtonItem.Caption = "保存订单";
            this.saveButtonItem.Id = 4;
            this.saveButtonItem.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("saveButtonItem.ImageOptions.Image")));
            this.saveButtonItem.Name = "saveButtonItem";
            this.saveButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.saveButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.saveButtonItem_ItemClick);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Edit = null;
            this.barEditItem1.Id = 6;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // skinRibbonGalleryBarItem1
            // 
            this.skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            this.skinRibbonGalleryBarItem1.Id = 7;
            this.skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.batchCreateFromDesktop);
            this.ribbonPageGroup1.ItemLinks.Add(this.removeEnvelopeButton);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "明信片集合";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.saveButtonItem);
            this.ribbonPageGroup2.ItemLinks.Add(this.closeButtonItem);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "相关操作";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.skinRibbonGalleryBarItem1);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "皮肤选择";
            // 
            // orderDirectoryTextEdit
            // 
            this.orderDirectoryTextEdit.Location = new System.Drawing.Point(63, 43);
            this.orderDirectoryTextEdit.MenuManager = this.ribbon;
            this.orderDirectoryTextEdit.Name = "orderDirectoryTextEdit";
            this.orderDirectoryTextEdit.Properties.ReadOnly = true;
            this.orderDirectoryTextEdit.Size = new System.Drawing.Size(803, 20);
            this.orderDirectoryTextEdit.StyleController = this.layoutControl1;
            this.orderDirectoryTextEdit.TabIndex = 0;
            // 
            // orderTaobaoIdTextEdit
            // 
            this.orderTaobaoIdTextEdit.Location = new System.Drawing.Point(63, 67);
            this.orderTaobaoIdTextEdit.MenuManager = this.ribbon;
            this.orderTaobaoIdTextEdit.Name = "orderTaobaoIdTextEdit";
            this.orderTaobaoIdTextEdit.Size = new System.Drawing.Size(729, 20);
            this.orderTaobaoIdTextEdit.StyleController = this.layoutControl1;
            this.orderTaobaoIdTextEdit.TabIndex = 2;
            this.orderTaobaoIdTextEdit.EditValueChanged += new System.EventHandler(this.orderTaobaoIdTextEdit_EditValueChanged);
            // 
            // orderIsUrgentCheckEdit
            // 
            this.orderIsUrgentCheckEdit.Location = new System.Drawing.Point(796, 67);
            this.orderIsUrgentCheckEdit.MenuManager = this.ribbon;
            this.orderIsUrgentCheckEdit.Name = "orderIsUrgentCheckEdit";
            this.orderIsUrgentCheckEdit.Properties.Caption = "加急单";
            this.orderIsUrgentCheckEdit.Size = new System.Drawing.Size(70, 19);
            this.orderIsUrgentCheckEdit.StyleController = this.layoutControl1;
            this.orderIsUrgentCheckEdit.TabIndex = 3;
            this.orderIsUrgentCheckEdit.CheckedChanged += new System.EventHandler(this.orderIsUrgentCheckEdit_CheckedChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(890, 672);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 91);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(870, 561);
            this.layoutControlGroup2.Text = "明信片集合";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(846, 518);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(870, 91);
            this.layoutControlGroup3.Text = "订单信息";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.orderDirectoryTextEdit;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(846, 24);
            this.layoutControlItem2.Text = "文件夹";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(36, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.orderTaobaoIdTextEdit;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(772, 24);
            this.layoutControlItem3.Text = "淘宝ID";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(36, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.orderIsUrgentCheckEdit;
            this.layoutControlItem4.Location = new System.Drawing.Point(772, 24);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(74, 23);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(74, 23);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(74, 24);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Text = "是否加急";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // OrderInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 801);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.ribbon);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(916, 814);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(904, 808);
            this.Name = "OrderInfoForm";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "订单明细";
            this.Load += new System.EventHandler(this.OrderInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDirectoryTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderTaobaoIdTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderIsUrgentCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderInfoValidationProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.BarButtonItem batchCreateFromDesktop;
        private DevExpress.XtraBars.BarButtonItem removeEnvelopeButton;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopePaperName;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopeProductWidth;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopeProductHeigtht;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopeFrontStyle;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.TextEdit orderDirectoryTextEdit;
        private DevExpress.XtraEditors.TextEdit orderTaobaoIdTextEdit;
        private DevExpress.XtraEditors.CheckEdit orderIsUrgentCheckEdit;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider orderInfoValidationProvider;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopeDirectory;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopeBackStyle;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn envelopePostCardCount;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
        private DevExpress.XtraBars.BarButtonItem closeButtonItem;
        private DevExpress.XtraBars.BarButtonItem saveButtonItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
    }
}