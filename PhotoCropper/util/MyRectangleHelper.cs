using PhotoCropper.viewModel;

namespace PhotoCropper.util
{
    public static class MyRectangleHelper
    {
        public static bool CheckSize(this MyRectangle myRectangle)
        {
            return !(myRectangle.Width <= 0) && !(myRectangle.Height <= 0);
        }
    }
}