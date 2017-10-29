//using soho.domain;
//
//namespace soho.helper
//{
//    public static class EnvelopeHelper
//    {
//        public static void CalculateArray(this EnvelopeInfo envelopeInfo)
//        {
//            if (envelopeInfo.PaperSize != null && envelopeInfo.ProductSize != null)
//            {
//                if (envelopeInfo.ProductSize.Width != 0)
//                {
//                    envelopeInfo.ArrayColumn = envelopeInfo.PaperSize.Width / envelopeInfo.ProductSize.Width;
//                    envelopeInfo.NotifyPropertyChanged(() => envelopeInfo.ArrayColumn);
//                    envelopeInfo.NotifyPropertyChanged(() => envelopeInfo.HorizontalWhite);
//                }
//                if (envelopeInfo.ProductSize.Height != 0)
//                {
//                    envelopeInfo.ArrayRow = envelopeInfo.PaperSize.Height / envelopeInfo.ProductSize.Height;
//                    envelopeInfo.NotifyPropertyChanged(() => envelopeInfo.ArrayRow);
//                    envelopeInfo.NotifyPropertyChanged(() => envelopeInfo.VerticalWhite);
//                }
//            }
//        }
//    }
//}

