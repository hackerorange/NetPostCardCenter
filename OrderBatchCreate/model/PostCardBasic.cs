using System.IO;
using SystemSetting.backStyle.model;
using SystemSetting.size.model;
using soho.domain;

namespace OrderBatchCreate.model
{
    public class PostCardBasic
    {
        public PostCardBasic()
        {
            //显示的图片Index为1
            //默认份数为1份
        }

        public PostCardBasic(PostCardBasic postCardBasic) : this()
        {
            Parent = postCardBasic;
        }

        public PostCardBasic Key => this;


        public PostCardBasic Parent { set; get; }

        /// <summary>
        ///     /// 成品尺寸
        /// </summary>
        public virtual PostSize ProductSize { get; set; }

        /// <summary>
        ///     显示在左侧的图标的Index
        /// </summary>
        public virtual int ImageIndex { get; } = 0;

        /// <summary>
        ///     状态
        /// </summary>
        public virtual BatchStatus Status { get; set; }

        /// <summary>
        ///     双面打印
        /// </summary>
        public virtual bool DoubleSide { get; set; } = true;


        /// <summary>
        ///     纸张名称
        /// </summary>
        public virtual string PaperName { get; set; }


        public virtual int Copy { get; set; } = 1;

        /// <summary>
        ///     正面样式
        /// </summary>
        public string FrontStyle { get; set; }


        /// <summary>
        ///     反面样式
        /// </summary>
        public BackStyleInfo BackStyle { get; set; }

        /// <summary>
        ///     文件系统详情
        /// </summary>
        public FileSystemInfo DirectoryInfo { get; set; }

        public string Path => DirectoryInfo.Name;
    }
}