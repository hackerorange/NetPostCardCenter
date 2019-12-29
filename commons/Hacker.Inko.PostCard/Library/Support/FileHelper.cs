using System.Drawing;
using System.IO;

namespace postCardCenterSdk.helper
{
    public static class FileHelper
    {
        /// <summary>
        ///     检测文件是否为图像
        /// </summary>
        /// <param name="file">要检测的文件</param>
        /// <returns></returns>
        public static bool CheckIsImage(this FileInfo file)
        {
            //如果文件不存在，返回false;
            if (!file.Exists) return false;
            //创建文件流
            var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            try
            {
                //根据流创建图像
                var iamge = Image.FromStream(fileStream);
                //创建成功，返回true;
                return true;
            }
            catch
            {
                //创建失败，返回false;
                return false;
            }
            finally
            {
                //关闭流
                fileStream.Close();
            }
        }
    }
}