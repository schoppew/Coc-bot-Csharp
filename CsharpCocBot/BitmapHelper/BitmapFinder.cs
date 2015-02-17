using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapHelper
{
    public class BitmapFinder
    {
        private readonly Bitmap _smallImage;
        private readonly Bitmap _bigImage;

        public BitmapFinder(string smallImage, string bigImage)
        {
            _smallImage = new Bitmap(Image.FromFile(smallImage));
            _bigImage = new Bitmap(Image.FromFile(bigImage));
        }

        public BitmapFinder(Bitmap smallImage, Bitmap bigImage)
        {
            _smallImage = smallImage;
            _bigImage = bigImage;
        }

        public Point SearchBitmap(double tolerance = 0.20)
        {
            if (tolerance > 1 || tolerance < 0)
                tolerance = tolerance/100.0;

            if ((_smallImage.Width > _bigImage.Width) || (_smallImage.Height > _bigImage.Height))
                return Point.Empty;

            BitmapData smallData = _smallImage.LockBits(new Rectangle(0, 0, _smallImage.Width, _smallImage.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);
            BitmapData bigData = _bigImage.LockBits(new Rectangle(0, 0, _bigImage.Width, _bigImage.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = _bigImage.Width;
            int bigHeight = _bigImage.Height - _smallImage.Height + 1;
            int smallWidth = _smallImage.Width * 3;
            int smallHeight = _smallImage.Height;

            Point location = Point.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int bigOffset = bigStride - _bigImage.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        // Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            // Restore Pointers
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            // Go to next row
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            break;
                        }
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            _bigImage.UnlockBits(bigData);
            _smallImage.UnlockBits(smallData);

            return location;
        }

    }
}
