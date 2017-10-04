using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.helper.devexpress
{
    public static class DataGridViewHelper
    {

        public static void DrawRowIndicator(this GridView gv, int width)
        {
            gv.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gv_CustomDrawRowIndicator);
            if (width != null)
            {
                if (width != 0)
                {
                    gv.IndicatorWidth = width;
                }
                else
                {
                    gv.IndicatorWidth = 30;
                }
            }
            else
            {
                gv.IndicatorWidth = 30;
            }

        }
        //行号设置  
        private static void gv_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
