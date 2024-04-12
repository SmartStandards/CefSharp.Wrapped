using System.Diagnostics;
using System.IO;
using System.Net;
using CefSharp;
using CefSharp.Handler;

namespace System.Web.AbstractHosting.CEF {

  public partial class CefRuntimeAdapter {

    [DebuggerDisplay("CefRequestContext {Identifier}")]
    public sealed class CefRequestContext : ResourceRequestHandler {

      // https://stackoverflow.com/questions/28697613/working-with-locally-built-web-page-in-cefsharp

      #region  Fields & Constructor 

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private CefRuntimeAdapter _Owner;

      //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
      //private IWebRequestHandler _WebRequestHandler;

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private CefRuntimeAdapter.CefWebRequestWrapper _Request;

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private CefRuntimeAdapter.CefWebResponseWrapper _Response = (CefRuntimeAdapter.CefWebResponseWrapper)null;

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private string _Url;

      //public CefRequestContext(CefRuntimeAdapter owner, IRequest cefRequest, IWebRequestHandler webRequestHandler) {
      public CefRequestContext(CefRuntimeAdapter owner, IRequest cefRequest) {
        _Owner = owner;
        _Url = cefRequest.Url;
        //_WebRequestHandler = webRequestHandler;
      }

      #endregion

      #region  Info Properties 

      public ulong Identifier {
        get {
          return _Request.WrappedCefRequest.Identifier;
        }
      }

      //public IWebRequest Request {
      //  get {
      //    return _Request;
      //  }
      //}

      //public IWebResponse Response {
      //  get {
      //    return _Response;
      //  }
      //}

      public string Url {
        get {
          return _Url;
        }
      }

      #endregion

      #region  Processing 
        
      public override CefReturnValue ProcessRequestAsync(IRequest cefRequest, ICallback callback) {
        if ((cefRequest.Url ?? "") == (_Url ?? "")) {
          callback.Continue();
          _Request = new CefRuntimeAdapter.CefWebRequestWrapper(cefRequest);
          _Owner.NotifyRequestBegin(this);
          return CefReturnValue.Continue;
        }
        else {
          callback.Cancel();
          return CefReturnValue.Cancel;
        }
      }

      //public override Stream GetResponse(IResponse cefResponse, out long responseLength, out string redirectUrl) {

      //  var sessionState = _Owner.WebSessionStateManager.GetSessionState(_Request);

      //  _Response = new CefRuntimeAdapter.CefWebResponseWrapper(cefResponse, sessionState);
      //  _WebRequestHandler.ProcessRequest(_Request, _Response, sessionState);
      //  responseLength = _Response.Stream.Length;
      //  if (_Response.Stream.Position > 0L) {
      //    _Response.Stream.Position = 0L;
      //  }
      //  if (sessionState.RequestSessionReset) {
      //    _Owner.WebSessionStateManager.ResetSession(sessionState);
      //  }
      //  _Owner.NotifyRequestEnd(this);
      //  // Me.AutoDisposeStream = True
      //  return _Response.Stream;
      //}

      #endregion

    }

  }

}