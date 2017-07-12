using System;
using System.Windows.Forms;
using soho.domain;

namespace PostCardCenter.myController
{
    public partial class envelopeInfoController : UserControl
    {
        private Envelope _envelope;

        public envelopeInfoController()
        {
            InitializeComponent();
        }


        public string EnvelopeId
        {
            get { return _envelope.envelopeId; }
            set
            {
                _envelope.envelopeId = value; 
                


            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
        }
    }
}