namespace Hacker.Inko.PostCard.Library
{
    public class PostSize
    {
        /// <summary>
        ///     成品尺寸名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     成品尺寸宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     成品尺寸高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     获取显示文本
        /// </summary>
        public override string ToString()
        {
            return "[" + Width.ToString("000") + "×" + Height.ToString("000") + "] " + Name;
        }
    }
}