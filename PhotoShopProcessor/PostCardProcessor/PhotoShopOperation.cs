using System;
using System.IO;
using Photoshop;

namespace PostCardProcessor
{
    public class PhotoShopOperation
    {
        private FileInfo _fileInfo;
        private readonly PsUnits _oldPsUnits;
        public Application Application { get; }

        public Document OpenDocument()
        {
            try
            {
                var document = Application.Open(_fileInfo.FullName);
                //ChangeImageDpi();
                document.Info.Author = "仲崇滔";
                //document.ResizeImage(null,null,300);
                Application.ActiveDocument = document;
                return document;
            }
            catch (Exception e)
            {
                if (RefreshFileInfo())
                {
                    return OpenDocument();
                }
                else
                {
                    throw e;
                }
            }
        }

        private bool RefreshFileInfo()
        {
            var extention = _fileInfo.Extension;
            var fullName = _fileInfo.FullName;
            fullName = fullName.Replace(extention, "(convertToJpg).jpg");
            try
            {
                using (System.Diagnostics.Process myProcess = new System.Diagnostics.Process())
                {
                    myProcess.StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        Arguments = string.Format("\"-> JPG\" \"Original Size\" \"{0}\" \"{1}\" /hide", _fileInfo.FullName.Replace(@"\", @"/"), fullName.Replace(@"\", @"/")),
                        CreateNoWindow = true,
                        FileName = "FormatFactory.exe"
                    };

                    Console.Out.WriteLine(myProcess.StartInfo.FileName + " " + myProcess.StartInfo.Arguments);
                    myProcess.Start();
                    myProcess.WaitForExit();
                    _fileInfo.Delete();
                    File.Move(fullName, _fileInfo.FullName);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public PhotoShopOperation(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            //初始化对象
            Application = new Photoshop.Application();
            //设置为毫米
            _oldPsUnits = Application.Preferences.RulerUnits;
            //重置颜色
            ResetColor();
        }

        ~PhotoShopOperation()
        {
            //重置单位
            
        }


        public void SwitchRuler(PsUnits units)
        {
            Application.Preferences.RulerUnits = units;
        }

        public void ResetRuler()
        {
            Application.Preferences.RulerUnits = _oldPsUnits;
        }

        public void ResetColor()
        {
            if (Application == null) throw new ArgumentNullException(nameof(Application));
            var desc49 = new ActionDescriptor();
            var ref22 = new ActionReference();
            ref22.PutProperty(Application.CharIDToTypeID("Clr "), Application.CharIDToTypeID("Clrs"));
            desc49.PutReference(Application.CharIDToTypeID("null"), ref22);
            Application.ExecuteAction(Application.CharIDToTypeID("Rset"), desc49, 3);
        }

    }
}
