using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Tools.FastFind
{
  public class ColorList : List<Color>
  {
    void Add(int color)
    {
      Add(Color.FromArgb(color));
    }
  }
}
