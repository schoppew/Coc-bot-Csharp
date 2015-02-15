using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace ExtBitmap
{
    unsafe public partial class ExtBitmap3 : IDisposable
    {
        public Bitmap BitMap { get; private set; }
        public ExtBitmap3()
        {
            Data = null;
            FileName = null;
            dataChanged = false;
            BitMap = null;
        }
        public bool HasAphaData
        {
            get;
            private set;
        }
        #region IDisposable interface
        public void Dispose()
        {
            FreeCurrentImage();
        }
        #endregion IDisposable interface

        private void FreeCurrentImage()
        {
            if (BitMap != null)
            {
                BitMap.Dispose();
                BitMap = null;
                Data = null;
                dataChanged = false;
            }
        }

        public string FileName { get; set; }
		public Int32[] Data { get; private set; }
        byte bitsPerPixel { get; set; }
        int bytesPerPixel { get; set; }
        int size { get; set; } // Total number of int in the data array
        public int Width { get; private set; }
        public int Height { get; private set; }
        int stride { get; set; } // Number of int per line
        bool dataChanged { get; set; }

        public bool Save(string fileName)
        {
            if (dataChanged && !SaveDataIntoBitmap()) return false;
            if (BitMap == null) return false;
            try
            {
                BitMap.Save(fileName);
                dataChanged = false;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                return false;
            }
            return true;
        }

        public Color NativeGetPixel(int x, int y)
        {
            if (BitMap == null) return Color.Empty;
            return BitMap.GetPixel(x, y);
        }

        public bool LoadFromFile(string fileName, bool keepAlpha = false)
        {
            FileName = fileName;
            dataChanged = false;
            try
            {
                BitMap = new Bitmap(fileName);
                if (!FillDataFromBitmap(keepAlpha)) return false;
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Debug.Assert(false, ex.Message);
                return false;
            }
        }

        private byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                //case PixelFormat.Format24bppRgb:
                //    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                default:
                    throw new ArgumentException("Only 24 and 32 bit images are supported");
            }
        }

        private bool FillDataFromBitmap(bool keepAlpha=false)
        {
            if (BitMap == null) return false;

            BitmapData bData = BitMap.LockBits(new Rectangle(0, 0, BitMap.Width, BitMap.Height), ImageLockMode.ReadOnly, BitMap.PixelFormat);
            Height = bData.Height;
            Width = bData.Width;
            stride = bData.Stride/sizeof(Int32);
            bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);
            bytesPerPixel = bitsPerPixel / 8;
            size = Math.Abs(stride * bData.Height);

            Data = new Int32[size];
            if (stride > 0)
                System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, Data, 0, size);
            else
            {
                stride = -stride;

                for (int i = 0; i < Height; i++)
                {
                    IntPtr pointer = new IntPtr(bData.Scan0.ToInt32() - stride * i * sizeof(Int32));
                    System.Runtime.InteropServices.Marshal.Copy(pointer, Data, stride * i, stride);
                }
            }
            BitMap.UnlockBits(bData);
            return true;
        }

        private int PixelPos(int x, int y)
        {
            if (Data == null) return -1;
            if (x < 0 || x >= Width) return -1;
            if (y < 0 || x >= Height) return -1;
            return y * stride + x;
        }

        public int GetPixel(int x, int y)
        {
            int pos = PixelPos(x, y);
            if (pos == -1) return 0;
            return Data[pos];
        }

        public Color GetPixelColor(int x, int y)
        {
            int pos = PixelPos(x, y);
            if (pos == -1) return Color.Empty;
            return Color.FromArgb(Data[pos]);
        }

        public bool SetPixel(int x, int y, int color)
        {
            int pos = PixelPos(x, y);
            if (pos == -1) return false;
            Data[pos] = color;
            return true;
        }

        public bool SaveDataIntoBitmap()
        {
            if (BitMap == null) return false;
            if (Data == null) return false;
            BitmapData bData = BitMap.LockBits(new Rectangle(0, 0, BitMap.Width, BitMap.Height), ImageLockMode.WriteOnly, BitMap.PixelFormat);
            bool bottomToTop = bData.Stride < 0;
            int size = Math.Abs(bData.Stride) * bData.Height / sizeof(Int32);
            Debug.Assert(size == Data.Length);

            if (!bottomToTop)
                System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, Data, 0, size);
            else
            {
                for (int i = 0; i < Height; i++)
                {
                    IntPtr pointer = new IntPtr(bData.Scan0.ToInt32() - stride * sizeof(Int32) * i);
                    System.Runtime.InteropServices.Marshal.Copy(Data, stride * i, pointer, stride);
                }
            }

            BitMap.UnlockBits(bData);
            return true;
        }

        public Dictionary<int, int> CountColors()
        {
            Dictionary<int, int> dico = new Dictionary<int, int>();
            if (BitMap == null || Data == null) return dico;

            for (int i = 0; i < size; i++)
            {
                int pixel = Data[i];
                int count = 0;
                if (dico.TryGetValue(pixel, out count))
                    dico[pixel] = count + 1;
                else
                    dico[pixel] = 1;
            }
            Debug.WriteLine("{0}: {1}x{2}, {3} pixels per line => {4} different colors", FileName, Width, Height, stride, dico.Count);
            return dico;
        }
    }
}
