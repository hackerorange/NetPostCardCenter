using postCardCenterSdk.sdk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static postCardCenterSdk.sdk.WebServiceInvoker;

namespace soho.translator
{
    public static class FileHelper
    {
        /// <summary>
        /// 向服务器上传文件
        /// </summary>
        /// <param name="file">要上传到服务器的文件</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>服务器返回的文件ID</returns>
        public static void Upload(this FileInfo file, Success<string> success, Failure failure = null)
        {
            WebServiceInvoker.Upload(file, succ => { success?.Invoke(succ.Id); }, message => { failure?.Invoke(message); });
        }

        /// <summary>
        /// 检测文件是否为图像
        /// </summary>
        /// <param name="file">要检测的文件</param>
        /// <returns></returns>
        public static bool CheckIsImage(this FileInfo file)
        {
            //如果文件不存在，返回false;
            if (!file.Exists) return false;
            //创建文件流
            FileStream fileStream = new FileStream(file.FullName,FileMode.Open,FileAccess.Read);
            try
            {
                //根据流创建图像
                Image iamge = Image.FromStream(fileStream);
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
