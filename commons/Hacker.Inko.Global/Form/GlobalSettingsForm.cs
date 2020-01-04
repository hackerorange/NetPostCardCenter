using System;
using System.Windows.Forms;


namespace Hacker.Inko.Global.Form
{
    public partial class GlobalSettingsForm : DevExpress.XtraEditors.XtraForm
    {
        public GlobalSettingsForm()
        {
            InitializeComponent();
        }

        private void GlobalSettingsForm_Load(object sender, EventArgs e)
        {
            textEdit1.Text = Net.Properties.Settings.Default.Host;
            textEdit2.Text = Properties.Settings.Default.BrokerUrl;
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            Net.Properties.Settings.Default.Host = textEdit1.Text;
            Net.Properties.Settings.Default.Save();
            Properties.Settings.Default.BrokerUrl = textEdit2.Text;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
        }
    }
}