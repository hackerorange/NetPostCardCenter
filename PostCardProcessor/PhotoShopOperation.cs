using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photoshop;

namespace PostCardProcessor
{
    public class PhotoShopOperation
    {

        private readonly PsUnits _oldPsUnits;
        public Application Application { get; }

        public Document OpenDocument(FileInfo fileInfo)
        {
            var document=Application.Open(fileInfo.FullName);
            //ChangeImageDpi();
            document.Info.Author = "仲崇滔";
            //document.ResizeImage(null,null,300);
            Application.ActiveDocument = document;
            return document;
        }

        public PhotoShopOperation()
        {
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
            Application.Preferences.RulerUnits= _oldPsUnits;
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
