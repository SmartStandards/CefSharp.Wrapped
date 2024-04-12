using System;
using System.Collections.Generic;
using System.Text;
using CefSharp;
using CefSharp.WinForms;

namespace System.Windows.Forms {

  public class CefSharpWrapper : UserControl {

    public CefSharpWrapper() {



    }

    protected override void OnCreateControl() {
      base.OnCreateControl();

      var btn = new ChromiumWebBrowser();
      btn.Dock = DockStyle.Fill;
      this.Controls.Add(btn);
      btn.Show();
      btn.LoadUrl("https://www.google.de");
    }

    private string _BrowserInstanceSharingGroup = string.Empty;
    public string BrowserInstanceSharingGroup {
      get {
        return _BrowserInstanceSharingGroup; 
      }
      set {
        _BrowserInstanceSharingGroup = value;
      } 
    } 

  }

}
