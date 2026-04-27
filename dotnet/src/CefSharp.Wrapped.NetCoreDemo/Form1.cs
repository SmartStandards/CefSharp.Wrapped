
using System;
using System.Windows.Forms;

namespace CefSharp.Wrapped.NetCoreDemo {

  public partial class Form1 : Form {

    public Form1() {
      this.InitializeComponent();

      cef.InitializeBrowser("https://www.google.de/");
      cef.LoadUrl("https://www.google.de/");

    }

    private void button1_Click(object sender, EventArgs e) {

    }

  }

}
