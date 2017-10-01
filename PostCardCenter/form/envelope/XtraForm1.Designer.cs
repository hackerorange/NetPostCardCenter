namespace PostCardCenter.form.envelope
{
    partial class XtraForm1
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.envelopePaperNameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.envelopeProductWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.envelopeProductHeight = new DevExpress.XtraGrid.Columns.GridColumn();
            this.envelopeProcessStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.envelopeProcessName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1178, 434);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1154, 410);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.envelopePaperNameColumn,
            this.envelopeProductWidth,
            this.envelopeProductHeight,
            this.envelopeProcessStatus,
            this.envelopeProcessName});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // envelopePaperNameColumn
            // 
            this.envelopePaperNameColumn.Caption = "纸张名称";
            this.envelopePaperNameColumn.FieldName = "paperName";
            this.envelopePaperNameColumn.Name = "envelopePaperNameColumn";
            this.envelopePaperNameColumn.Visible = true;
            this.envelopePaperNameColumn.VisibleIndex = 0;
            // 
            // envelopeProductWidth
            // 
            this.envelopeProductWidth.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProductWidth.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProductWidth.Caption = "成品宽度";
            this.envelopeProductWidth.FieldName = "productWidth";
            this.envelopeProductWidth.MaxWidth = 100;
            this.envelopeProductWidth.MinWidth = 100;
            this.envelopeProductWidth.Name = "envelopeProductWidth";
            this.envelopeProductWidth.Visible = true;
            this.envelopeProductWidth.VisibleIndex = 1;
            this.envelopeProductWidth.Width = 100;
            // 
            // envelopeProductHeight
            // 
            this.envelopeProductHeight.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProductHeight.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProductHeight.Caption = "成品高度";
            this.envelopeProductHeight.FieldName = "productHeight";
            this.envelopeProductHeight.MaxWidth = 100;
            this.envelopeProductHeight.MinWidth = 100;
            this.envelopeProductHeight.Name = "envelopeProductHeight";
            this.envelopeProductHeight.Visible = true;
            this.envelopeProductHeight.VisibleIndex = 2;
            this.envelopeProductHeight.Width = 100;
            // 
            // envelopeProcessStatus
            // 
            this.envelopeProcessStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProcessStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProcessStatus.Caption = "处理进度";
            this.envelopeProcessStatus.FieldName = "processStatusText";
            this.envelopeProcessStatus.MaxWidth = 100;
            this.envelopeProcessStatus.MinWidth = 100;
            this.envelopeProcessStatus.Name = "envelopeProcessStatus";
            this.envelopeProcessStatus.Visible = true;
            this.envelopeProcessStatus.VisibleIndex = 4;
            this.envelopeProcessStatus.Width = 100;
            // 
            // envelopeProcessName
            // 
            this.envelopeProcessName.AppearanceHeader.Options.UseTextOptions = true;
            this.envelopeProcessName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.envelopeProcessName.Caption = "当前处理";
            this.envelopeProcessName.FieldName = "processorName";
            this.envelopeProcessName.MaxWidth = 100;
            this.envelopeProcessName.MinWidth = 100;
            this.envelopeProcessName.Name = "envelopeProcessName";
            this.envelopeProcessName.Visible = true;
            this.envelopeProcessName.VisibleIndex = 3;
            this.envelopeProcessName.Width = 100;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1178, 434);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1158, 414);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 434);
            this.Controls.Add(this.layoutControl1);
            this.Name = "XtraForm1";
            this.Text = "XtraForm1";
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn envelopePaperNameColumn;
        private DevExpress.XtraGrid.Columns.GridColumn envelopeProductWidth;
        private DevExpress.XtraGrid.Columns.GridColumn envelopeProductHeight;
        private DevExpress.XtraGrid.Columns.GridColumn envelopeProcessStatus;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn envelopeProcessName;
    }
}