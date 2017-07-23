using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.domain
{
    public class OrderInfo : Order
    {
        public string processorName { get; set; }
        public string processStatus { get; set; }


        public DateTime createDate { get; set; }

        public string paperType { get; set; }
    }
}