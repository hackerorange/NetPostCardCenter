using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetting.backStyle.model
{
    public class CustomerBackStyleInfo : BackStyleInfo
    {
        public CustomerBackStyleInfo(FileInfo fileInfo)
        {
            Name = "自定义";
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; set; }
    }
}