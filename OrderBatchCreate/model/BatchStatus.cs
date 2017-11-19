using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraEditors;

namespace OrderBatchCreate.model
{
    public class BatchStatus
    {
        /// <summary>
        ///     未设置(订单)
        /// </summary>
        public static BatchStatus OrderNotSet = new BatchStatus("未设置", Color.Gray);

        /// <summary>
        ///     未就绪
        /// </summary>
        public static BatchStatus OrderNotReady = new BatchStatus("未就绪", Color.Gray);

        /// <summary>
        ///     明信片未就绪
        /// </summary>
        public static BatchStatus EnvelopeNotReady = new BatchStatus("未就绪", Color.Red);

        /// <summary>
        ///     明信片未就绪
        /// </summary>
        public static BatchStatus EnvelopeEmptyPath = new BatchStatus("空路径", Color.Gray);


        /// <summary>
        ///     明信片未就绪
        /// </summary>
        public static BatchStatus EnvelopeAlready = new BatchStatus("准备就绪", Color.Green);

        /// <summary>
        ///     已设置完成
        /// </summary>
        public static BatchStatus OrderAlready = new BatchStatus("待提交", Color.Blue);

        /// <summary>
        ///     已设置完成
        /// </summary>
        public static BatchStatus OrderEmpty = new BatchStatus("空订单", Color.DarkOliveGreen);

        /// <summary>
        ///     已提交
        /// </summary>
        public static BatchStatus OrderHaveSubmit = new BatchStatus("已提交", Color.Green);

        /// <summary>
        ///     明信片未上传
        /// </summary>
        public static BatchStatus PostCardBeforeUpload = new BatchStatus("未上传", Color.Gray);

        /// <summary>
        ///     明信片未上传
        /// </summary>
        public static BatchStatus PostCardUploading = new BatchStatus("正在上传", Color.Orange);


        /// <summary>
        ///     明信片未上传
        /// </summary>
        public static BatchStatus PostCardUploadSuccess = new BatchStatus("上传成功", Color.Green);

        /// <summary>
        ///     明信片上传失败
        /// </summary>
        public static BatchStatus PostCardUploadFailure = new BatchStatus("上传失败", Color.Red);

        /// <summary>
        ///     明信片上传失败
        /// </summary>
        public static BatchStatus PostCardTypeError = new BatchStatus("图片格式错误", Color.DeepPink);


        public static List<BatchStatus> StatusList = new List<BatchStatus>
        {
            OrderNotSet,
            OrderEmpty,
            OrderNotReady,
            EnvelopeNotReady,
            OrderAlready,
            OrderHaveSubmit,
            EnvelopeAlready,
            EnvelopeEmptyPath,
            PostCardBeforeUpload,
            PostCardUploadSuccess,
            PostCardUploading,
            PostCardUploadFailure,
            PostCardTypeError
        };

        private readonly Color _backColor = Color.Transparent;

        private readonly Color _foreColor;

        public BatchStatus(string text, Color foreColor)
        {
            StatusText = text;
            _foreColor = foreColor;
        }

        public BatchStatus(string text, Color foreColor, Color backColor) : this(text, foreColor)
        {
            _backColor = backColor;
        }


        public string StatusText { get; }

        public FormatConditionRuleValue GenerateRuleFormat()
        {
            var ruleFormat = new FormatConditionRuleValue
            {
                Condition = FormatCondition.Equal,
                Value1 = this
            };
            ruleFormat.Appearance.ForeColor = _foreColor;
            ruleFormat.Appearance.BackColor = _backColor;
            return ruleFormat;
        }

        public override string ToString()
        {
            return StatusText;
        }
    }
}