using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace soho.helper
{
    public static class Md5Helper
    {
        public static string GetMd5(this FileInfo fileInfo)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(fileInfo.FullName, FileMode.Open);
                var md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                foreach (var t in retVal)
                    sb.Append(t.ToString("x2"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
            finally
            {
                file.Close();
            }
        }
    }
}