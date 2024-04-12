using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using CefSharp;

namespace System.Web.AbstractHosting.CEF {

  public partial class CefRuntimeAdapter {

    [DebuggerDisplay("Request: {HttpMethod} {Url}")]
    public class CefWebRequestWrapper { //: IWebRequest {

      #region  Fields & Constructor 

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private IRequest _WrappedCefRequest;

      public CefWebRequestWrapper(IRequest wrappedCefRequest) {
        _WrappedCefRequest = wrappedCefRequest;
      }

      #endregion

      #region  Properties 

      public IRequest WrappedCefRequest {
        get {
          return _WrappedCefRequest;
        }
      }

      public Stream InputStream {
        get {
          int postDataLength = 0;
          var postData = _WrappedCefRequest.PostData;

          foreach (var postDataElement in postData.Elements)
            postDataLength += postDataElement.Bytes.Length;

          var postDataStream = new MemoryStream(postDataLength);
          foreach (var postDataElement in postData.Elements)
            postDataStream.Write(postDataElement.Bytes, 0, postDataElement.Bytes.Length);
          postDataStream.Seek(0L, SeekOrigin.Begin);

          return postDataStream;
        }
      }

      public Uri Url {
        get {
          return new Uri(_WrappedCefRequest.Url);
        }
      }

      public string ClientHostname {
        get {
          return "localhost";
        }
      }

      public IPAddress ClientIpAddress {
        get {
          return IPAddress.Parse("127.0.0.1");
        }
      }

      public string HttpMethod {
        get {
          return _WrappedCefRequest.Method;
        }
      }

      public NameValueCollection Headers {
        get {
          return _WrappedCefRequest.Headers;
        }
      }

      private Dictionary<string, string> _Cookies = new Dictionary<string, string>();
      public Dictionary<string, string> Cookies {
        get {
          // TODO: not implemented!!!
          return _Cookies;
        }
      }

      public string Browser {
        get {
          return "Chromium (CEF)";
        }
      }

      #endregion

      #region  IDisposable 

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private bool _AlreadyDisposed = false;

      /// <summary>
      /// Dispose the current object instance
      /// </summary>
      protected virtual void Dispose(bool disposing) {
        if (!_AlreadyDisposed) {
          if (disposing) {
          }
          _AlreadyDisposed = true;
        }
      }

      /// <summary>
      /// Dispose the current object instance and suppress the finalizer
      /// </summary>
      public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
      }

      #endregion

    }

  }

}