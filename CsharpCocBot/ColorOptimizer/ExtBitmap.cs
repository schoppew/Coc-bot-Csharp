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

namespace ColorOptimizer
{

    public class ExtBitmap : IDisposable
    {
        public Bitmap BitMap { get; private set; }
       
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
                
            }
        }              

        public bool Save(string fileName)
        {
            if (BitMap == null) return false;
            try
            {
                BitMap.Save(fileName);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                return false;
            }
            return true;
        }
       
		public Color GetPixel(int x, int y)
		{
			if (BitMap == null) return Color.Empty;
			return BitMap.GetPixel(x, y);
		}

		public bool LoadFromFile(string fileName)
		{
			try
			{
				BitMap = new Bitmap(fileName);
				return true;
			}
			catch(FileNotFoundException ex)
			{
				Debug.Assert(false, ex.Message);                
				return false;
			}
		}

		private byte GetBitsPerPixel(PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Format24bppRgb:
					return 24;
				case PixelFormat.Format32bppArgb:
				case PixelFormat.Format32bppPArgb:
				case PixelFormat.Format32bppRgb:
					return 32;
				default:
					throw new ArgumentException("Only 24 and 32 bit images are supported");
			}
		}

		public Dictionary<int, int> CountColors()
		{
			Dictionary<int, int> dico = new Dictionary<int, int>();
			if (BitMap == null) return dico;

			BitmapData bData = BitMap.LockBits(new Rectangle(0, 0, BitMap.Width, BitMap.Height), ImageLockMode.ReadWrite, BitMap.PixelFormat);

			byte bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);

			int size = bData.Stride * bData.Height;
			byte[] data = new byte[size];

			System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);

			for (int i = 0; i < size; i += bitsPerPixel / 8)
			{
				int pixel = bitsPerPixel == 24 ? data[i] << 16 + data[i + 1] << 8 + data[i + 2] : data[i] << 24 + data[i + 1] << 16 + data[i + 2] << 8 + data[i + 3];
				int count = 0;
				if (dico.TryGetValue(pixel, out count))
					dico[pixel] = count+1;
				else
					dico[pixel] = 1;								
			}

			System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);
			BitMap.UnlockBits(bData);
			return dico;		
		}
	}
}
