using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpCocBot
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Tools.ClickOnPointTool.ClickOnPoint2(Tools.BlueStackHelper.GetBlueStackWindowHandle(), new Point(209, 699));
    }
  }
}
