using System;
using System.Collections.Generic;
using System.Text;
using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Windows.Forms;
using System.Web.AbstractHosting.CEF.JsBridging;
using System.Web.AbstractHosting.CEF;

namespace System.Windows.Forms {

  public class CefControl : UserControl {

    public CefControl() {
      //_Browser = null/* TODO Change to default(_) if this is not a reference type */;
      //_RuntimeAdapter = null/* TODO Change to default(_) if this is not a reference type */;
      //this.InitializeComponent();
    }

    protected override void OnCreateControl() {
      base.OnCreateControl();

      //var btn = new ChromiumWebBrowser();
      //btn.Dock = DockStyle.Fill;
      //this.Controls.Add(btn);
      //btn.Show();
      //btn.LoadUrl("https://www.google.de");
    }

    //private string _BrowserInstanceSharingGroup = string.Empty;
    //public string BrowserInstanceSharingGroup {
    //  get {
    //    return _BrowserInstanceSharingGroup; 
    //  }
    //  set {
    //    _BrowserInstanceSharingGroup = value;
    //  } 
    //}

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CefSharp.WinForms.ChromiumWebBrowser __Browser;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CefSharp.WinForms.ChromiumWebBrowser _Browser {
      [MethodImpl(MethodImplOptions.Synchronized)]
      get {
        return __Browser;
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      set {
        if (__Browser != null) {
          __Browser.FrameLoadEnd -= CefBrowser_FrameLoadEnd;
          __Browser.ConsoleMessage -= CefBrowser_ConsoleMessage;
        }

        __Browser = value;
        if (__Browser != null) {
          __Browser.FrameLoadEnd += CefBrowser_FrameLoadEnd;
          __Browser.ConsoleMessage += CefBrowser_ConsoleMessage;
        }
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CefRuntimeAdapter __RuntimeAdapter;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private CefRuntimeAdapter _RuntimeAdapter {
      [MethodImpl(MethodImplOptions.Synchronized)]
      get {
        return __RuntimeAdapter;
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      set {
        if (__RuntimeAdapter != null) {
        }

        __RuntimeAdapter = value;
        if (__RuntimeAdapter != null) {
        }
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<JsHookingAdapter> _JsBridgedObjects = new List<JsHookingAdapter>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool _FirstFrameLoaded = false;

    public event BrowserInitializedEventHandler BrowserInitialized;

    public delegate void BrowserInitializedEventHandler();

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public JsHookingAdapter[] JsBridgedObjects {
      get {
        return _JsBridgedObjects.ToArray();
      }
    }

    public CefSharp.WinForms.ChromiumWebBrowser Browser {
      get {
        return _Browser;
      }
    }

    public CefRuntimeAdapter RuntimeAdapter {
      get {
        return _RuntimeAdapter;
      }
    }

    //public void InitializeBrowser(IWebRequestHandler requestHandler, IWebSessionStateManager webSessionStateManager, params object[] objectsToBridgeIntoJs) {
    public void InitializeBrowser(params object[] objectsToBridgeIntoJs) {
      if ((_Browser != null))
        return;

      var rootUrl = "http://inMemory.local/";
      _Browser = new CefSharp.WinForms.ChromiumWebBrowser(rootUrl);

      if ((objectsToBridgeIntoJs != null)) {
        //CefSharpSettings.LegacyJavascriptBindingEnabled = true;
    
        foreach (var jsBridgedObject in objectsToBridgeIntoJs) {
          JsHookingAdapter adapter = new JsHookingAdapter(jsBridgedObject);
          _JsBridgedObjects.Add(adapter);
          adapter.RegisterHook(_Browser);
        }
      }

      this.Controls.Add(_Browser);

      _Browser.Name = "CefBrowser";
      _Browser.Location = new System.Drawing.Point(247, 167);
      _Browser.Size = new System.Drawing.Size(39, 13);
      _Browser.AutoSize = true;
      _Browser.Dock = DockStyle.Fill;
      _Browser.BackColor = Color.White;
      _Browser.TabIndex = 0;

      _Browser.Visible = true;
      _Browser.Show();
      _Browser.Select();

      // initialize an adapter as interactor to serve the bundle directly into the browser control 
      //_RuntimeAdapter = new CefRuntimeAdapter(_Browser, requestHandler, webSessionStateManager, rootUrl);
      _RuntimeAdapter = new CefRuntimeAdapter(_Browser, rootUrl);

      _RuntimeAdapter.Run();
    }

    private void CefBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
      if ((this.InvokeRequired)) {
        this.Invoke(this.OnFrameLoadEnd);
      }
      else {
        this.OnFrameLoadEnd();
      }
    }
    private void OnFrameLoadEnd() {
      this.InjectJsHooks();
      if ((BrowserInitialized != null && !_FirstFrameLoaded))
        BrowserInitialized?.Invoke();
      _FirstFrameLoaded = true;
    }

    private void InjectJsHooks() {
      foreach (var obj in _JsBridgedObjects) {
        foreach (var adapter in _JsBridgedObjects)
          adapter.ActivateHook(_Browser.GetMainFrame());
      }
    }

    private void CefBrowser_Initialized() {
      if ((_BrowserDevToolsVisible))
        _Browser.ShowDevTools();
    }


    private bool _BrowserDevToolsVisible = false;

    public bool BrowserDevToolsVisible {
      get {
        return _BrowserDevToolsVisible;
      }
      set {
        if ((_BrowserDevToolsVisible == value))
          return;

        if ((_BrowserDevToolsVisible)) {
          if ((_FirstFrameLoaded))
            _Browser.CloseDevTools();
          _BrowserDevToolsVisible = false;
        }
        else {
          if ((_FirstFrameLoaded))
            _Browser.ShowDevTools();
          _BrowserDevToolsVisible = true;
        }
      }
    }

    public Action<string> BrowserConsoleErrorLoggingMethod { get; set; } = null;
    public Action<string> BrowserConsoleWarningLoggingMethod { get; set; } = null;
    public Action<string> BrowserConsoleInfoLoggingMethod { get; set; } = null;

    private void CefBrowser_ConsoleMessage(object sender, ConsoleMessageEventArgs e) {
      if(e.Level == LogSeverity.Error) {
        Trace.TraceError("    JAVA-SCRIPT:| " + e.Message);
        if ((this.BrowserConsoleErrorLoggingMethod != null))
          this.BrowserConsoleErrorLoggingMethod.Invoke(e.Message);
      }
      else if (e.Level == LogSeverity.Warning) {
        Trace.TraceWarning("    JAVA-SCRIPT:| " + e.Message);
        if ((this.BrowserConsoleWarningLoggingMethod != null))
          this.BrowserConsoleWarningLoggingMethod.Invoke(e.Message);
      }
      else {
        if ((this.BrowserConsoleInfoLoggingMethod != null))
          this.BrowserConsoleInfoLoggingMethod.Invoke(e.Message);
      }
    }

    private void CefControl_Load(object sender, EventArgs e) {
    }

  }

}
