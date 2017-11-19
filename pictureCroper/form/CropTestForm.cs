using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pictureCroper.constant;
using pictureCroper.model;

namespace pictureCroper.form
{
    public partial class CropTestForm : Form
    {
        public CropTestForm()
        {
            InitializeComponent();
        }

        private void MyForm_Load(object sender, EventArgs e)
        {
            var image = Image.FromFile("E:\\Desktop\\123123.jpg");
            var cropContext = new CropContext()
            {
                Image = Image.FromFile("E:\\Desktop\\123123.jpg"),
                StyleInfo = StyleInfoConstant.A,
                ProductSize = new Size(100, 110)
            };
            cropContext.CropInfo = new CropInfo(image.Size, cropContext.PicturePrintAreaSize, fit: false);
            pictureCropControl1.CropContext = cropContext;
            pictureCropControl1.CropBoxBackColor=Color.LightSeaGreen;
        }

        private void PictureCropControl1_Load(object sender, EventArgs e)
        {
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            pictureCropControl1.Rotate(90);
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            pictureCropControl1.Rotate(270);
        }

        private void PictureCropControl1_CropInfoChanged(CropInfo cropInfo)
        {
            if (cropInfo == null) return;
            cropLeft.EditValue = cropInfo.LeftScale;
            cropTop.EditValue = cropInfo.TopScale;
            cropWidth.EditValue = cropInfo.WidthScale;
            cropHeight.EditValue = cropInfo.HeightScale;
            cropRotate.EditValue = cropInfo.Rotation;
        }


        private void SimpleButton3_Click(object sender, EventArgs e)
        {
            pictureCropControl1.IsPreview = !pictureCropControl1.IsPreview;
        }
    }
}