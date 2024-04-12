using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using CefSharp;
using CefSharp.WinForms;
//using System.Web.AbstractHosting.InMemory;

namespace System.Web.AbstractHosting.CEF {

  public partial class CefRuntimeAdapter : IResourceRequestHandlerFactory {

    #region  Fields & Constructor 

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<CefRuntimeAdapter.CefRequestContext> _CurrentRequests = new List<CefRuntimeAdapter.CefRequestContext>();

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    //private IWebRequestHandler _WebRequestHandler;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    //private IWebSessionStateManager _WebSessionStateManager;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ChromiumWebBrowser __BrowserControl;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ChromiumWebBrowser _BrowserControl {
      [MethodImpl(MethodImplOptions.Synchronized)]
      get {
        return __BrowserControl;
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      set {
        if (__BrowserControl != null) {
          __BrowserControl.TitleChanged -= BrowserControl_TitleChanged;
        }

        __BrowserControl = value;
        if (__BrowserControl != null) {
          __BrowserControl.TitleChanged += BrowserControl_TitleChanged;
        }
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string _VirtualRootUrl;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string _PageTitle = "loading...";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Icon _FavIcon = null;

    //public CefRuntimeAdapter(ChromiumWebBrowser browserControl, IWebRequestHandler webRequestHandler, IWebSessionStateManager webSessionStateManager, string virtualRootUrl = "http://inMemory.local/") {
    //  _WebRequestHandler = webRequestHandler;
    //  _WebSessionStateManager = webSessionStateManager;
    //  this._BrowserControl = browserControl;
    //  this._BrowserControl.ResourceRequestHandlerFactory = this;
    //  _VirtualRootUrl = virtualRootUrl.ToLower();
    //  if (!_VirtualRootUrl.EndsWith("/")) {
    //    _VirtualRootUrl = _VirtualRootUrl + "/";
    //  }
    //}
    public CefRuntimeAdapter(ChromiumWebBrowser browserControl, string virtualRootUrl = "http://inMemory.local/") {
      //_WebRequestHandler = webRequestHandler;
      //_WebSessionStateManager = webSessionStateManager;
      this._BrowserControl = browserControl;
      this._BrowserControl.ResourceRequestHandlerFactory = this;
      _VirtualRootUrl = virtualRootUrl.ToLower();
      if (!_VirtualRootUrl.EndsWith("/")) {
        _VirtualRootUrl = _VirtualRootUrl + "/";
      }
    }

    #endregion

    #region  Info Properties 

    //public IWebRequestHandler WebRequestHandler {
    //  get {
    //    return _WebRequestHandler;
    //  }
    //}

    //public IWebSessionStateManager WebSessionStateManager {
    //  get {
    //    return _WebSessionStateManager;
    //  }
    //}

    private bool HasHandlers {
      get {
        return true;
      }
    }

    //bool IResourceHandlerFactory.HasHandlers {
    //  get => {
    //    return this.HasHandlers;
    //  }
    //}

    public string PageTitle {
      get {
        return _PageTitle;
      }
    }

    public Icon FavIcon {
      get {
        return _FavIcon;
      }
    }

    #endregion

    #region  Registration 

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public CefRuntimeAdapter.CefRequestContext[] CurrentRequests {
      get {
        lock (_CurrentRequests)
          return _CurrentRequests.ToArray();
      }
    }

    bool IResourceRequestHandlerFactory.HasHandlers => throw new NotImplementedException();

    private void NotifyRequestBegin(CefRuntimeAdapter.CefRequestContext context) {
      lock (_CurrentRequests)
        _CurrentRequests.Add(context);
    }

    private void NotifyRequestEnd(CefRuntimeAdapter.CefRequestContext context) {
      lock (_CurrentRequests)
        _CurrentRequests.Remove(context);
    }

    #endregion

    private void BrowserControl_TitleChanged(object sender, TitleChangedEventArgs e) {
      _PageTitle = e.Title;
    }

    //private IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request) {
    //  if (!request.Url.StartsWith(_VirtualRootUrl)) {
    //    return null;
    //  }
    //  else {
    //    return new CefRuntimeAdapter.CefRequestContext(this, request, _WebRequestHandler);
    //  }
    //}
    //IResourceHandler IResourceHandlerFactory.GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request) => this.GetResourceHandler(browserControl, browser, frame, request);

    public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling) {
      if (!request.Url.StartsWith(_VirtualRootUrl)) {
        return null;
      }
      else {
        //return new CefRuntimeAdapter.CefRequestContext(this, request, _WebRequestHandler);
        return new CefRuntimeAdapter.CefRequestContext(this, request);
      }
    }


    public void Run() {
      this._BrowserControl.Load(_VirtualRootUrl);

      //using (var imReq = new InMemoryWebRequest("GET", _VirtualRootUrl + "favicon.ico"))
      //using (var imSess = new InMemorySessionState())
      //using (var imRes = new InMemoryWebResponse()) {

      //  _WebRequestHandler.ProcessRequest(imReq, imRes, imSess);

      //  if (imRes.Stream.Position > 0L) {
      //    imRes.Stream.Position = 0L;
      //  }

      //  if (imRes.StatusCode == 200) {
      //    _FavIcon = new Icon(imRes.Stream);
      //  }

      //}

    }


  }

}