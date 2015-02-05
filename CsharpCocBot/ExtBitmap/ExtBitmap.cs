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
	public partial class ExtBitmap : IDisposable
	{
		public Bitmap BitMap { get; private set; }
		public ExtBitmap()
		{
			data = null;
			FileName = null;
			dataChanged = false;
			BitMap = null;
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
				data = null;
				dataChanged = false;
			}
		}

		public string FileName { get; set; }
		byte[] data;
		byte bitsPerPixel { get; set; }
		int bytesPerPixel { get; set; }
		int size { get; set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		int Stride { get; set; } // Number of bytes per line
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

		public bool LoadFromFile(string fileName)
		{
			FileName = fileName;
			dataChanged = false;
			try
			{
				BitMap = new Bitmap(fileName);
				if (!FillDataFromBitmap()) return false;
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

		private bool FillDataFromBitmap()
		{
			if (BitMap == null) return false;

			BitmapData bData = BitMap.LockBits(new Rectangle(0, 0, BitMap.Width, BitMap.Height), ImageLockMode.ReadOnly, BitMap.PixelFormat);
			Height = bData.Height;
			Width = bData.Width;
			Stride = bData.Stride;
			bitsPerPixel = GetBitsPerPixel(bData.PixelFormat);
			bytesPerPixel = bitsPerPixel / 8;
			size = bData.Stride * bData.Height;

			data = new byte[size];
			System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
			BitMap.UnlockBits(bData);
			return true;
		}

		private int PixelPos(int x, int y)
		{
			if (data == null) return -1;
			if (x < 0 || x >= Width) return -1;
			if (y < 0 || x >= Height) return -1;
			return y * Stride + x * bytesPerPixel;
		}

		public int GetPixel(int x, int y)
		{
			int pos = PixelPos(x, y);
			if (pos == -1) return 0;
			return ((data[pos + 2] << 16) & 0x00FF0000) + ((data[pos + 1] << 8) & 0x0000FF00) + (data[pos] & 0x000000FF);
		}

		public Color GetPixelColor(int x, int y)
		{
			int pos = PixelPos(x, y);
			if (pos == -1) return Color.Empty;
			return Color.FromArgb(data[pos + 2], data[pos + 1], data[pos]);
		}

		public bool SetPixel(int x, int y, int color)
		{
			int pos = PixelPos(x, y);
			if (pos == -1) return false;
			data[pos] = (byte)(color & 0x000000FF);
			data[pos + 1] = (byte)((color >> 8) & 0x000000FF);
			data[pos + 2] = (byte)((color >> 16) & 0x000000FF);
			return true;
		}

		public bool SaveDataIntoBitmap()
		{
			if (BitMap == null) return false;
			if (data == null) return false;
			BitmapData bData = BitMap.LockBits(new Rectangle(0, 0, BitMap.Width, BitMap.Height), ImageLockMode.WriteOnly, BitMap.PixelFormat);
			int size = bData.Stride * bData.Height;
			Debug.Assert(size == data.Length);
			//data = new byte[size];
			System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);
			BitMap.UnlockBits(bData);
			return true;
		}

		public Dictionary<int, int> CountColors()
		{
			Dictionary<int, int> dico = new Dictionary<int, int>();
			if (BitMap == null || data == null) return dico;

			for (int i = 0; i < size; i += bytesPerPixel)
			{
				int pixel = ((data[i + 2] << 16) & 0x00FF0000) + ((data[i + 1] << 8) & 0x0000FF00) + (data[i] & 0x000000FF);
				int count = 0;
				if (dico.TryGetValue(pixel, out count))
					dico[pixel] = count + 1;
				else
					dico[pixel] = 1;
			}
			Debug.WriteLine("{0}: {1}x{2}, {3} pixels per line => {4} different colors", FileName, Width, Height, Stride / bytesPerPixel, dico.Count);
			return dico;
		}
	}
}
