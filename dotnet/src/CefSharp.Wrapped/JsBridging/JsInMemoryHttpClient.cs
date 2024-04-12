//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;
//using System.Web.AbstractHosting.InMemory;

//namespace System.Web.AbstractHosting.CEF.JsBridging {

//  [JsBridging.BindToJsObject("w3bstractHttpClient")]
//  public class JsInMemoryHttpClient {

//    #region  nested Class 'RequestProcessingContext' 

//    public class RequestProcessingContext {

//      public RequestProcessingContext(IWebRequestHandler webRequestHandler, IWebSessionStateManager webSessionStateManager, int handle, string httpMethod, string url, string payload, Action<RequestProcessingContext, int, string> callback) {
//        this.Handle = handle;

//        Task.Run(() => {
//          string response = null;
//          int statusCode = 500; // internal server error

//          try {
//            using (var reqStream = new MemoryStream()) {

//              if (!string.IsNullOrWhiteSpace(payload)) {
//                var sw = new StreamWriter(reqStream);
//                sw.Write(payload);
//                sw.Flush();
//                reqStream.Position = 0L;
//              }

//              using (var req = new InMemoryWebRequest(httpMethod, url, reqStream))
//              using (var res = new InMemoryWebResponse()) {

//                var webSessionState = webSessionStateManager.GetSessionState(req);

//                webRequestHandler.ProcessRequest(req, res, webSessionState);
//                statusCode = res.StatusCode;

//                if (webSessionState.RequestSessionReset) {
//                  webSessionStateManager.ResetSession(webSessionState);
//                }

//                if (res.Stream.Position > 0L && res.Stream.CanSeek) {
//                  res.Stream.Position = 0L;
//                }
//                using (var sr = new StreamReader(res.Stream)) {
//                  response = sr.ReadToEnd();
//                }

//              }

//            }
//          }
//          finally {
//            callback.Invoke(this, statusCode, response);
//          }
//        });

//      }

//      public int Handle { get; private set; }

//    }

//    #endregion

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private int _LastHandle = 0;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private object _HandleLock = new object();

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private List<RequestProcessingContext> _Requests = new List<RequestProcessingContext>();

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private IWebRequestHandler _WebRequestHandler;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private IWebSessionStateManager _WebSessionStateManager;


//    public JsInMemoryHttpClient(IWebRequestHandler webRequestHandler, IWebSessionStateManager webSessionStateManager) {
//      _WebRequestHandler = webRequestHandler;
//      _WebSessionStateManager = webSessionStateManager;
//    }

//    public IWebRequestHandler WebRequestHandler {
//      get {
//        return _WebRequestHandler;
//      }
//    }

//    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
//    public RequestProcessingContext[] RequestQueue {
//      get {
//        lock (_Requests)
//          return _Requests.ToArray();
//      }
//    }

//    [JsBridging.PublicateAsJsMethodAttribute("queueRequest")]
//    public int QueueRequest(string httpMethod, string url, string payload) {

//      int currentHandle;
//      lock (_HandleLock) {
//        if (_LastHandle == int.MaxValue) {
//          _LastHandle = 1;
//        }
//        else {
//          _LastHandle += 1;
//        }
//        currentHandle = _LastHandle;
//      }

//      var context = new RequestProcessingContext(_WebRequestHandler, _WebSessionStateManager, currentHandle, httpMethod, url, payload, this.HandleResponse);
//      lock (_Requests)
//        _Requests.Add(context);

//      return currentHandle;
//    }

//    private void HandleResponse(RequestProcessingContext context, int httpStatus, string response) {

//      if (this.ResponseCallback != null) {
//        Task.Run(() => { try { this.ResponseCallback.Invoke(context.Handle, httpStatus, response); } catch { } });
//      }

//      lock (_Requests)
//        _Requests.Remove(context);

//    }

//    private Action<int, int, string> _ResponseCallback;

//    [JsBridging.InjectJsMethodHandleAttribute("handleResponse")] // NOTE: will be injected from outside by the JsHoockingAdapter
//    public Action<int, int, string> ResponseCallback {
//      get {
//        return _ResponseCallback;
//      }
//      set {
//        _ResponseCallback = value;

//        // convention -> this is to inform the js representation about the fact, that is has been hoocked!
//        _ResponseCallback.Invoke(-1, 200, "");

//      }
//    }

//    #region  Push 

//    [JsBridging.AttachJsEventHandlerAttribute("onPushReceived")]
//    public event PushEventHandler Push;

//    public delegate void PushEventHandler(string message);

//    public void SendPush(string message) {
//      if (this.Push != null) {
//        Push?.Invoke(message);
//      }
//    }

//    #endregion

//  }

//}