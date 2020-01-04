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
using Hacker.Inko.Net.Base;

namespace Inko.Security.form
{
    public partial class GlobalSettingsForm : DevExpress.XtraEditors.XtraForm
    {
        public GlobalSettingsForm()
        {
            InitializeComponent();
        }

        private void GlobalSettingsForm_Load(object sender, EventArgs e)
        {
            textEdit1.Text = NetGlobalInfo.Host;
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            NetGlobalInfo.Host = textEdit1.Text;
        }
    }
}