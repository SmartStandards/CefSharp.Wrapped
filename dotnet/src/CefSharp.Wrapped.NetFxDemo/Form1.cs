using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.Wrapped.NetFxDemo {

  public partial class Form1 : Form {

    private CefControl _CefControl;

    public Form1() {
      InitializeComponent();

      cef.InitializeBrowser("http://localhost:3000/");
      cef.Browser.LoadUrl("http://localhost:3000/");

      cef.Browser.FrameLoadStart += (sender, args) => {
        cef.ExecuteJs(@"(function() {localStorage.setItem('test',2);})();");
        string tokenKey = "localhost:3000/token_902fb942-2f53-4ca3-96a7-88eedcfa4e53";
        string tokenValue = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE3MTM4MDMwOTcsImV4cCI6MTc0NTMzOTA5NywiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2NrZXQiLCJFbWFpbCI6Impyb2NrZXRAZXhhbXBsZS5jb20iLCJSb2xlIjpbIk1hbmFnZXIiLCJQcm9qZWN0IEFkbWluaXN0cmF0b3IiXX0.ZF2YKGsT8taGkQDTyzqiOchsYrYFa_y65BSbO7N96xQ";
        string js = $"localStorage.setItem('{tokenKey}','{tokenValue}');";
        js = @"(function() {" + js + @"})();";
        cef.ExecuteJs(js);
        cef.Browser.ShowDevTools();
      };
      cef.Browser.LoadingStateChanged += OnLoadingStateChanged;
      //this.Controls.Add(_CefControl);

      //var _Test = new TextBox();
      //_Test.Dock = DockStyle.Fill;
      //_Test.BackColor = Color.Blue;
      //_Test.Text = "Test";
      //this.Controls.Add(_Test);
    }

    private void HandleToolStripLayout(object sender, LayoutEventArgs e) {
      HandleToolStripLayout();
    }

    private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args) {
      //SetCanGoBack(args.CanGoBack);
      //SetCanGoForward(args.CanGoForward);

      this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
    }

    private void SetIsLoading(bool isLoading) {
      goButton.Text = isLoading ?
          "Stop" :
          "Go";

      HandleToolStripLayout();
    }

    private void HandleToolStripLayout() {
      var width = toolStrip1.Width;
      foreach (ToolStripItem item in toolStrip1.Items) {
        if (item != urlTextBox) {
          width -= item.Width - item.Margin.Horizontal;
        }
      }
      urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
      urlTextBox.Width = 1800;
      this.Update();
    }
  }
}
