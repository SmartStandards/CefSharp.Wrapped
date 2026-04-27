//// Program.cs
//// TargetFramework: net8.0-windows
//// NuGet: CefSharp.WinForms.NETCore, Newtonsoft.Json
//// Build: x64 recommended for CefSharp runtime deployment scenarios.

//using CefSharp;
//using CefSharp.WinForms;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//internal static class Program {
//  [STAThread]
//  private static void Main() {
//    ApplicationConfiguration.Initialize();

//    CefBootstrapper cefBootstrapper = new CefBootstrapper();
//    cefBootstrapper.InitializeCef();

//    Application.ApplicationExit += (object sender, EventArgs e) =>
//    {
//      cefBootstrapper.ShutdownCef();
//    };

//    Application.Run(new MainForm());
//  }
//}

///// <summary>
///// Hosts a Chromium browser (CefSharp) and navigates to an in-memory virtual website.
///// </summary>
//internal sealed class MainForm : Form {
//  private readonly ChromiumWebBrowser _Browser;

//  public MainForm() {
//    this.Text = "CefSharp In-Memory Virtual HTTP Demo";
//    this.ClientSize = new Size(980, 700);

//    this._Browser = new ChromiumWebBrowser(VirtualSite.Origin + "/");
//    this._Browser.Dock = DockStyle.Fill;

//    this.Controls.Add(this._Browser);
//  }

//  protected override void Dispose(bool disposing) {
//    if (disposing) {
//      this._Browser.Dispose();
//    }

//    base.Dispose(disposing);
//  }
//}

///// <summary>
///// Central Cef initialization/shutdown.
///// </summary>
//internal sealed class CefBootstrapper {
//  private bool _IsInitialized;

//  /// <summary>
//  /// Initializes CefSharp and registers the virtual scheme/host handler.
//  /// </summary>
//  public void InitializeCef() {
//    if (this._IsInitialized) {
//      return;
//    }

//    // NOTE:
//    // For many real apps you might want to set BrowserSubprocessPath explicitly, configure cache,
//    // locale, log severity, etc. Keep minimal here.
//    CefSettings settings = new CefSettings();
//    settings.CachePath = string.Empty; // In-memory cache ("incognito")
//    settings.LogSeverity = LogSeverity.Verbose;

//    // Initialize Cef
//    bool initialized = Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
//    if (!initialized) {
//      throw new InvalidOperationException("Cef.Initialize failed.");
//    }

//    // Register a scheme handler for https://app.local/*
//    // Using https+domain is typically simpler than registering a completely custom scheme. :contentReference[oaicite:2]{index=2}
//    //Cef.RegisterSchemeHandlerFactory("https", VirtualSite.Host, new VirtualSiteSchemeHandlerFactory());

//    // With the following, which uses the correct API for registering scheme handler factories in CefSharp:
//    CefSharp.Cef.GetGlobalRequestContext().RegisterSchemeHandlerFactory(
//      "https", VirtualSite.Host, new VirtualSiteSchemeHandlerFactory()
//    );


//    this._IsInitialized = true;
//  }

//  /// <summary>
//  /// Shuts down CefSharp.
//  /// </summary>
//  public void ShutdownCef() {
//    if (!this._IsInitialized) {
//      return;
//    }

//    Cef.Shutdown();
//    this._IsInitialized = false;
//  }
//}

///// <summary>
///// Virtual site constants + in-memory resources (HTML/JS/CSS).
///// </summary>
//internal static class VirtualSite {
//  public static string Host {
//    get { return "app.local"; }
//  }

//  public static string Origin {
//    get { return "https://" + VirtualSite.Host; }
//  }

//  public static string IndexHtml {
//    get {
//      // Loads CSS/JS via GET from the virtual host.
//      return @"<!doctype html>
//<html>
//<head>
//  <meta charset=""utf-8"">
//  <meta name=""viewport"" content=""width=device-width,initial-scale=1"">
//  <title>In-Memory Chat</title>
//  <link rel=""stylesheet"" href=""/app.css"">
//</head>
//<body>
//  <div id=""app"">
//    <div id=""chat""></div>

//    <form id=""chatForm"">
//      <input id=""prompt"" type=""text"" placeholder=""Type a prompt..."" autocomplete=""off"" />
//      <button type=""submit"">Send</button>
//    </form>
//  </div>

//  <script src=""/app.js""></script>
//</body>
//</html>";
//    }
//  }

//  public static string AppCss {
//    get {
//      return @"html,body{height:100%;margin:0;font-family:Segoe UI,Arial,sans-serif;background:#0f1115;color:#e7e7e7;}
//#app{height:100%;display:flex;flex-direction:column;}
//#chat{flex:1;overflow:auto;padding:12px;display:flex;flex-direction:column;gap:10px;}
//.bubble{max-width:78%;padding:10px 12px;border-radius:14px;white-space:pre-wrap;line-height:1.25;}
//.user{align-self:flex-end;background:#2b6cb0;}
//.bot{align-self:flex-start;background:#2d3748;}
//#chatForm{display:flex;gap:8px;padding:12px;border-top:1px solid #222;}
//#prompt{flex:1;padding:10px 12px;border-radius:10px;border:1px solid #333;background:#151922;color:#e7e7e7;}
//button{padding:10px 14px;border-radius:10px;border:1px solid #333;background:#1f2430;color:#e7e7e7;cursor:pointer;}
//button:hover{background:#262c3b;}";
//    }
//  }

//  public static string AppJs {
//    get {
//      // Very small chat client:
//      // - Adds user bubble
//      // - POSTs prompt to /api/chat
//      // - Expects JSON string array and renders each as bot bubble
//      return @"(function () {
//  function el(id) { return document.getElementById(id); }

//  function addBubble(text, cssClass) {
//    var div = document.createElement('div');
//    div.className = 'bubble ' + cssClass;
//    div.textContent = text;
//    el('chat').appendChild(div);
//    el('chat').scrollTop = el('chat').scrollHeight;
//  }

//  async function sendPrompt(prompt) {
//    var payload = { prompt: prompt };

//    var res = await fetch('/api/chat', {
//      method: 'POST',
//      headers: { 'Content-Type': 'application/json' },
//      body: JSON.stringify(payload)
//    });

//    if (!res.ok) {
//      addBubble('Server error: ' + res.status, 'bot');
//      return;
//    }

//    var arr = await res.json();
//    if (!Array.isArray(arr)) {
//      addBubble('Invalid response format.', 'bot');
//      return;
//    }

//    for (var i = 0; i < arr.length; i++) {
//      addBubble(String(arr[i]), 'bot');
//    }
//  }

//  el('chatForm').addEventListener('submit', function (evt) {
//    evt.preventDefault();

//    var promptEl = el('prompt');
//    var prompt = (promptEl.value || '').trim();
//    if (prompt.length === 0) {
//      return;
//    }

//    addBubble(prompt, 'user');
//    promptEl.value = '';
//    sendPrompt(prompt);
//  });

//  addBubble('Ready. Type something and press Send.', 'bot');
//})();";
//    }
//  }
//}

///// <summary>
///// Scheme handler factory that serves HTML/JS/CSS + a simple JSON API from memory.
///// </summary>
//internal sealed class VirtualSiteSchemeHandlerFactory : ISchemeHandlerFactory {
//  public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request) {
//    if (request == null) {
//      return ResourceHandler.ForErrorMessage("Bad request.", HttpStatusCode.BadRequest);
//    }

//    Uri uri;
//    try {
//      uri = new Uri(request.Url);
//    }
//    catch (UriFormatException) {
//      return ResourceHandler.ForErrorMessage("Invalid URL.", HttpStatusCode.BadRequest);
//    }

//    string path = uri.AbsolutePath;
//    string method = (request.Method ?? string.Empty).Trim().ToUpperInvariant();

//    // Minimal routing
//    if (method == "GET") {
//      if (path == "/" || path == "/index.html") {
//        return VirtualResourceHandlers.FromString(VirtualSite.IndexHtml, "text/html; charset=utf-8");
//      }

//      if (path == "/app.js") {
//        return VirtualResourceHandlers.FromString(VirtualSite.AppJs, "text/javascript; charset=utf-8");
//      }

//      if (path == "/app.css") {
//        return VirtualResourceHandlers.FromString(VirtualSite.AppCss, "text/css; charset=utf-8");
//      }

//      return ResourceHandler.ForErrorMessage("Not Found", HttpStatusCode.NotFound);
//    }

//    if (method == "POST" && path == "/api/chat") {
//      return new ChatApiResourceHandler();
//    }

//    return ResourceHandler.ForErrorMessage("Method Not Allowed", HttpStatusCode.MethodNotAllowed);
//  }
//}

///// <summary>
///// Small helper wrapper for creating in-memory string resources.
///// </summary>
//internal static class VirtualResourceHandlers {
//  /// <summary>
//  /// Creates a resource handler for string content.
//  /// </summary>
//  public static IResourceHandler FromString(string content, string mimeType) {
//    // ResourceHandler.FromString has multiple overloads across versions; this overload is commonly available. :contentReference[oaicite:3]{index=3}
//    return ResourceHandler.FromString(content, encoding: Encoding.UTF8, includePreamble: false, mimeType: mimeType);
//  }
//}

///// <summary>
///// Handles POST /api/chat.
///// Reads JSON {"prompt":"..."} from request body and responds with JSON string array.
///// </summary>
//internal sealed class ChatApiResourceHandler : ResourceHandler {
//  private const string _JsonMimeType = "application/json; charset=utf-8";

//  /// <summary>
//  /// Processes the request on CEF IO thread without using async/await.
//  /// </summary>
//  public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback) {
//    try {
//      string body = this.TryReadRequestBody(request);
//      string prompt = this.TryParsePrompt(body);

//      string[] answerLines = this.GenerateToyAnswer(prompt);

//      string json = JsonConvert.SerializeObject(answerLines);

//      byte[] bytes = Encoding.UTF8.GetBytes(json);
//      MemoryStream stream = new MemoryStream(bytes, writable: false);

//      this.Stream = stream;
//      this.MimeType = _JsonMimeType;
//      this.StatusCode = (int)HttpStatusCode.OK;
//      this.StatusText = "OK";
//      this.Headers["Cache-Control"] = "no-store";

//      callback.Continue();
//      return CefReturnValue.Continue;
//    }
//    catch (JsonException ex) {
//      DevLogger.LogError(ex);
//      this.SetErrorResponse((int)HttpStatusCode.BadRequest, "Invalid JSON.");
//      callback.Continue();
//      return CefReturnValue.Continue;
//    }
//    catch (InvalidOperationException ex) {
//      DevLogger.LogError(ex);
//      this.SetErrorResponse((int)HttpStatusCode.BadRequest, ex.Message);
//      callback.Continue();
//      return CefReturnValue.Continue;
//    }
//    catch (Exception) {
//      this.SetErrorResponse((int)HttpStatusCode.InternalServerError, "Server error.");
//      callback.Continue();
//      return CefReturnValue.Continue;
//    }
//  }

//  /// <summary>
//  /// Builds a minimal "chat-like" response.
//  /// </summary>
//  private string[] GenerateToyAnswer(string prompt) {
//    if (string.IsNullOrWhiteSpace(prompt)) {
//      return new string[]
//      {
//                "Please enter a prompt."
//      };
//    }

//    string trimmed = prompt.Trim();

//    return new string[]
//    {
//            "You said: " + trimmed,
//            "Length: " + trimmed.Length.ToString(),
//            "Tip: Replace GenerateToyAnswer(...) with your real backend logic."
//    };
//  }

//  /// <summary>
//  /// Attempts to read POST body as string using CefSharp PostDataExtensions.GetBody().
//  /// </summary>
//  private string TryReadRequestBody(IRequest request) {
//    if (request == null) {
//      throw new InvalidOperationException("Request is missing.");
//    }

//    IPostData postData = request.PostData;
//    if (postData == null) {
//      throw new InvalidOperationException("POST body is missing.");
//    }

//    IList<IPostDataElement> elements = postData.Elements;
//    if (elements == null || elements.Count == 0) {
//      throw new InvalidOperationException("POST body is empty.");
//    }

//    StringBuilder sb = new StringBuilder();

//    // For render-process originated requests, post data is typically a single Bytes element. :contentReference[oaicite:4]{index=4}
//    for (int i = 0; i < elements.Count; i++) {
//      IPostDataElement element = elements[i];
//      if (element == null) {
//        continue;
//      }

//      if (element.Type == PostDataElementType.Bytes) {
//        // Uses CefSharp.PostDataExtensions.GetBody(...)
//        string part = element.GetBody(charSet: "utf-8"); // :contentReference[oaicite:5]{index=5}
//        if (!string.IsNullOrEmpty(part)) {
//          sb.Append(part);
//        }
//      }
//    }

//    return sb.ToString();
//  }

//  /// <summary>
//  /// Parses {"prompt":"..."} and returns prompt.
//  /// </summary>
//  private string TryParsePrompt(string body) {
//    if (string.IsNullOrWhiteSpace(body)) {
//      return string.Empty;
//    }

//    JObject obj = JObject.Parse(body);
//    JToken token = obj["prompt"];
//    if (token == null) {
//      return string.Empty;
//    }

//    return token.Type == JTokenType.String ? (string)token : token.ToString();
//  }

//  /// <summary>
//  /// Produces a JSON error response.
//  /// </summary>
//  private void SetErrorResponse(int statusCode, string message) {
//    string[] payload = new string[] { message };
//    string json = JsonConvert.SerializeObject(payload);

//    byte[] bytes = Encoding.UTF8.GetBytes(json);
//    MemoryStream stream = new MemoryStream(bytes, writable: false);

//    this.Stream = stream;
//    this.MimeType = _JsonMimeType;
//    this.StatusCode = statusCode;
//    this.StatusText = "Error";
//    this.Headers["Cache-Control"] = "no-store";
//  }
//}

///// <summary>
///// Minimal logger placeholder per your conventions.
///// Replace with your real logger implementation.
///// </summary>
//internal static class DevLogger {
//  public static void LogError(Exception ex) {
//    // Intentionally minimal.
//    // In production: write to file/eventlog/telemetry.
//    Console.Error.WriteLine(ex.ToString());
//  }

//  public static void LogTrace(int a, int b, string message) {
//    Console.WriteLine(message);
//  }
//}
