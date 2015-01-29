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

    }

    public void RunBot()
    {
        do
        {
            GlobalVariables.screenCap = new Tools.ScreenCapture();
            GlobalVariables.restart = false;

            Functions.Other.Sleep(1000);
            Functions.MainScreen.CheckMainScreen();
            Functions.Other.Sleep(1000);
            Functions.MainScreen.ZoomOut();

        } while (true);
    }
  }
}
