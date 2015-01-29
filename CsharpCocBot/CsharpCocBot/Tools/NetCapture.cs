using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpCocBot.Tools
{
  public class NetCapture
  {
    public Bitmap[] Capture(CaptureType typeOfCapture)
    {
      // used to capture then screen in memory
      Bitmap memoryImage;
      // number of screens to capture,
      // will be updated below if necessary
      int count = 1;

      try
      {
        Screen[] screens = Screen.AllScreens;
        Rectangle rc;

        // setup the area to capture
        // depending on the supplied parameter
        switch (typeOfCapture)
        {
          case CaptureType.PrimaryScreen:
            rc = Screen.PrimaryScreen.Bounds;
            break;
          case CaptureType.VirtualScreen:
            rc = SystemInformation.VirtualScreen;
            break;
          case CaptureType.WorkingArea:
            rc = Screen.PrimaryScreen.WorkingArea;
            break;
          case CaptureType.AllScreens:
            count = screens.Length;
            typeOfCapture = CaptureType.WorkingArea;
            rc = screens[0].WorkingArea;
            break;
          default:
            rc = SystemInformation.VirtualScreen;
            break;
        }
        // allocate a member for saving the captured image(s)
        images = new Bitmap[count];

        // cycle across all desired screens
        for (int index = 0; index < count; index++)
        {
          if (index > 0)
            rc = screens[index].WorkingArea;
          // redefine the size on multiple screens

          memoryImage = new Bitmap(rc.Width, rc.Height,
                        PixelFormat.Format32bppArgb);
          using (Graphics memoryGrahics =
                  Graphics.FromImage(memoryImage))
          {
            // copy the screen data
            // to the memory allocated above
            memoryGrahics.CopyFromScreen(rc.X, rc.Y,
               0, 0, rc.Size, CopyPixelOperation.SourceCopy);
          }
          images[index] = memoryImage;
          // save it in the class member for later use
        }
      }
      catch (Exception ex)
      {
        // handle any erros which occured during capture
        MessageBox.Show(ex.ToString(), "Capture failed",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return images;
    }

    private Bitmap capture(Control window, Rectangle rc)
    {
      Bitmap memoryImage = null;
      images = new Bitmap[1];

      // Create new graphics object using handle to window.
      using (Graphics graphics = window.CreateGraphics())
      {
        memoryImage = new Bitmap(rc.Width,
                      rc.Height, graphics);

        using (Graphics memoryGrahics =
                Graphics.FromImage(memoryImage))
        {
          memoryGrahics.CopyFromScreen(rc.X, rc.Y,
             0, 0, rc.Size, CopyPixelOperation.SourceCopy);
        }
      }
      images[0] = memoryImage;
      return memoryImage;
    }
  }
}
