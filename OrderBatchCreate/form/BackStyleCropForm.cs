using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemSetting.backStyle.model;
using DevExpress.XtraEditors;
using OrderBatchCreate.model;
using pictureCroper.constant;
using pictureCroper.model;
using postCardCenterSdk.helper;

namespace OrderBatchCreate.form
{
    public partial class BackStyleCropForm : XtraForm
    {
        private readonly EnvelopeInfo _envelopeInfo;
        private readonly CustomerBackStyleInfo _backStyleInfo;

        public BackStyleCropForm(EnvelopeInfo envelopeInfo)
        {
            InitializeComponent();
            _envelopeInfo = envelopeInfo;
            if (_envelopeInfo.BackStyle is CustomerBackStyleInfo customerBackStyle)
            {
                _backStyleInfo = customerBackStyle;
            }
        }

        private void BackStyleCropForm_Load(object sender, EventArgs e)
        {
            if (_envelopeInfo == null)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
//            if ( && !())


            if (_envelopeInfo.BackStyle != null && _backStyleInfo != null)
            {
                var cropContext = new CropContext
                {
                    ProductSize = new Size()
                    {
                        Width = _envelopeInfo.ProductSize.Width,
                        Height = _envelopeInfo.ProductSize.Height
                    },
                    StyleInfo = StyleInfoConstant.Back,
                };
                try
                {
                    cropContext.Image = Image.FromFile(_backStyleInfo.FileInfo.FullName);
                    cropContext.CropInfo = new CropInfo(cropContext.Image.Size, cropContext.PicturePrintAreaSize,
                        fit: cropContext.StyleInfo.Fit);
                }
                catch
                {
                    XtraMessageBox.Show("自定义反面图像无法加载，图像路径[" + _backStyleInfo.FileInfo.FullName + "]，请处理后，选择新文件进行加载！");
                }

                pictureCropControl1.CropContext = cropContext;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            _backStyleInfo.FileInfo.UploadSynchronize(
                "明信片反面样式",
                //pictureCropControl1.CropContext.CropInfo.Rotation,
                //pictureCropControl1.CropContext.CropInfo.LeftScale,
                //pictureCropControl1.CropContext.CropInfo.TopScale,
                //pictureCropControl1.CropContext.CropInfo.WidthScale,
                //pictureCropControl1.CropContext.CropInfo.HeightScale,
                success =>
                {
                    if (success.ImageAvailable)
                    {
                        _backStyleInfo.FileId = success.FileId;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        XtraMessageBox.Show("反面文件格式不正确，请用Photoshop重新保存后再次上传！");
                    }
                }, msg => { XtraMessageBox.Show(msg); });
        }

        private void SimpleButton4_Click(object sender, EventArgs e)
        {
            pictureCropControl1.Rotate(270);
        }

        private void SimpleButton3_Click(object sender, EventArgs e)
        {
            pictureCropControl1.Rotate(90);
        }

        private void SimpleButton6_Click(object sender, EventArgs e)
        {
            pictureCropControl1.CropContext.CropInfo.WidthScale = 1;
            pictureCropControl1.CropContext.CropInfo.HeightScale = 1;
            pictureCropControl1.CropContext.CropInfo.LeftScale = 0;
            pictureCropControl1.CropContext.CropInfo.TopScale = 0;
            //刷新图像
            pictureCropControl1.RefreshImage();
        }

        private void SimpleButton5_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;
            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = _envelopeInfo.DirectoryInfo.FullName
            };
            if (fileDialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                var image = Image.FromFile(fileDialog.FileName);
                if (pictureCropControl1.CropContext.Image != null)
                {
                    try
                    {
                        pictureCropControl1.CropContext.Image.Dispose();
                        pictureCropControl1.CropContext.Image = null;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                pictureCropControl1.CropContext.Image = image;
                var cropContext = pictureCropControl1.CropContext;
                cropContext.CropInfo = new CropInfo(cropContext.Image.Size, cropContext.PicturePrintAreaSize,
                    fit: cropContext.StyleInfo.Fit);
                _backStyleInfo.FileInfo = new FileInfo(fileDialog.FileName);
                pictureCropControl1.Rotate(0);
                pictureCropControl1.RefreshImage();
            }
            catch
            {
                XtraMessageBox.Show("当前选择的文件无法作为图片加载！");
            }
        }
    }
}