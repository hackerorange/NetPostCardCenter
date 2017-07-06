using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraPrinting.Native;
using PostCardCenter.helper;
using soho.domain;

namespace PostCardCenter.myController
{
    public partial class CropBoxControl : UserControl
    {
        private CropInfo _cropInfo;

        public Size ProductSize
        {
            set { postCardCropController2.ProductSize = postCardCropController1.ProductSize = postCardCropController2.ProductSize = value; }
            get { return postCardCropController1.ProductSize; }
        }

        public CropInfo CropInfo
        {
            get { return postCardCropController1.CropInfo; }
            set { postCardCropController1.CropInfo = postCardCropController2.CropInfo = value; }
        }


        public Padding Margin
        {
            get { return postCardCropController1.Margin; }
            set { postCardCropController1.Margin = postCardCropController2.Margin = value; }
        }

        /// <summary>
        /// 图片
        /// </summary>
        public Image Image
        {
            get { return postCardCropController1.Image; }
            set { postCardCropController1.Image = postCardCropController2.Image = value; }
        }


        public CropBoxControl()
        {
            InitializeComponent();
        }


        private Size _productSize;
        private Padding _margin;

        public double Scale
        {
            get { return postCardCropController1.Scale; }
            set { postCardCropController1.Scale = postCardCropController2.Scale = value; }
        }

        private void postCardCropController1_cropInfoChanged(CropInfo newCropInfo)
        {
            postCardCropController2.CropInfo = newCropInfo;
            postCardCropController2.RefreshPostCard();
            if (newCropInfo != null)
            {
                leftScale.Text = newCropInfo.leftScale.ToString();
                widthScale.Text = newCropInfo.widthScale.ToString();
                heightScale.Text = newCropInfo.heightScale.ToString();
                topScale.Text = newCropInfo.topScale.ToString();
                rotateAngle.Text = newCropInfo.rotation.ToString();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var a = keyData.ToString().Split(',');
            for (var i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim().ToUpper();
            }
            if (a.Contains("R"))
            {
                rotate(90);
            }
            if (a.Contains("L"))
            {
                rotate(-90);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void RefreshPostCard(){
            postCardCropController1.RefreshPostCard();
            postCardCropController2.RefreshPostCard();
        }

        private void CropBoxControl_Load(object sender, EventArgs e)
        {
        }

        private void postCardCropController1_Load(object sender, EventArgs e)
        {
        }

        public void rotate(int angle)
        {
            postCardCropController1.Rotate(angle);
        }
    }
}