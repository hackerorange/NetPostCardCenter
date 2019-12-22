using DevExpress.XtraGrid.Views.Grid;

using PostCardCenter.devexpress;

namespace PostCardCenter.devexpress
{
    public static class DataGridViewHelper
    {
        public static void DrawRowIndicator(this GridView gv, int width)
        {
            gv.CustomDrawRowIndicator += gv_CustomDrawRowIndicator;
        }

        //行号设置  
        private static void gv_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
    }
}