using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace PostCardCrop.form
{
    public partial class ExportForm : DevExpress.XtraEditors.XtraForm
    {
        public ExportForm()
        {
            InitializeComponent();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {

        }

        public void RefreshProgress(double progress)
        {
            progressBar1.Value = progressBar1.Minimum + (int)(progress * (progressBar1.Maximum - progressBar1.Minimum));
            progressBar1.Refresh();
        }

    }
}