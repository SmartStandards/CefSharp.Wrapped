using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using CefSharp;

namespace System.Web.AbstractHosting.CEF {

  public partial class CefRuntimeAdapter {

    [DebuggerDisplay("Response: {ContentMimeType}")]
    public class CefWebResponseWrapper { //}: IWebResponse {

      #region  Fields & Constructor 

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private IResponse _WrappedCefResponse;

      //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
      //private IWebSessionState _State;

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private StreamWriter _Writer = null;

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private MemoryStream _OutputStream = new MemoryStream(4096);

      //public CefWebResponseWrapper(IResponse wrappedCefResponse, IWebSessionState state) {
      public CefWebResponseWrapper(IResponse wrappedCefResponse) {
        //_State = state;
        _WrappedCefResponse = wrappedCefResponse;
      }

      #endregion

      public IResponse WrappedCefResponse {
        get {
          return _WrappedCefResponse;
        }
      }

      public string ContentMimeType {
        get {
          return _WrappedCefResponse.MimeType;
        }
        set {
          _WrappedCefResponse.MimeType = value;
        }
      }

      public int StatusCode {
        get {
          return _WrappedCefResponse.StatusCode;
        }
        set {
          _WrappedCefResponse.StatusCode = value;
        }
      }

      public TextWriter ContentWriter {
        get {
          if (_Writer is null) {
            _Writer = new StreamWriter(_OutputStream);
            _Writer.AutoFlush = true;
          }
          return _Writer;
        }
      }

      public Stream Stream {
        get {
          return _OutputStream;
        }
      }

      public string get_Header(string name) {
        return _WrappedCefResponse.Headers[name];
      }

      public void set_Header(string name, string value) {
        _WrappedCefResponse.Headers[name] = value;
      }

      #region  IDisposable 

      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private bool _AlreadyDisposed = false;

      /// <summary>
      /// Dispose the current object instance
      /// </summary>
      protected virtual void Dispose(bool disposing) {
        if (!_AlreadyDisposed) {
          if (disposing) {
            if (_Writer != null) {
              _Writer.Flush();
              _Writer.Dispose();
              _Writer = null;
            }
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