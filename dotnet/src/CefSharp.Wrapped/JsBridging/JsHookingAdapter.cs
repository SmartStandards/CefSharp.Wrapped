using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace System.Web.AbstractHosting.CEF.JsBridging {

  public class JsHookingAdapter {
    #region ...

    private string _JsObjectNameForDotNetClass;
    private string _JsObjectNameToHook;
    private IFrame _Frame = null;
    private Dictionary<string, MethodInfo> _IncludedMethods = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="jsObjectToHook">The given object name must be available under 'window.objName'</param>
    internal JsHookingAdapter(object obj, string jsObjectToHook = null) {
      this.DotNetObject = obj;

      if (string.IsNullOrWhiteSpace(jsObjectToHook)) {
        var attr = obj.GetType().GetCustomAttribute<BindToJsObject>();
        if (attr is null) {
          this.ObjectName = obj.GetType().Name;
        }
        else {
          this.ObjectName = attr.JsObjectName;
        }
      }
      else {
        this.ObjectName = jsObjectToHook;
      }

      _JsObjectNameToHook = "window." + this.ObjectName;
      _JsObjectNameForDotNetClass = this.ObjectName + "Hook";

    }

    public object DotNetObject { get; private set; }
    public string ObjectName { get; private set; }

    internal void RegisterHook(ChromiumWebBrowser browser) {
      // browser.RegisterJsObject(_JsObjectNameForDotNetClass, Me.DotNetObject)
      //browser.RegisterJsObject(_JsObjectNameForDotNetClass, this);
      //CefSharpSettings.WcfEnabled = true;
      browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
      browser.JavascriptObjectRepository.Register(_JsObjectNameForDotNetClass, this, isAsync: false, options: BindingOptions.DefaultBinder);
    }

    private MethodInfo[] IncludedMethods {
      get {
        if (_IncludedMethods is null) {
          _IncludedMethods = new Dictionary<string, MethodInfo>();

          string[] excludedMethods = new[] { nameof(ToString), nameof(GetType), nameof(InvokeMethodOnJsObject), nameof(InvokeMethodOnNetObject), nameof(Equals), nameof(this.GetHashCode), nameof(ResultCallback), nameof(ActivateHook) };

          foreach (var m in this.DotNetObject.GetType().GetMethods().Where(mi => !excludedMethods.Contains(mi.Name)).ToArray())
            _IncludedMethods.Add(m.Name, m);

        }
        lock (_IncludedMethods)
          return _IncludedMethods.Values.ToArray();
      }
    }

    private string MakeFirstLetterLCase(string name) {
      return name.Substring(0, 1).ToLower() + name.Substring(1);
    }

    #endregion

    private bool _ApplicationIsRunning = true;

    internal void ActivateHook(IFrame frame) {

      if (frame is null) {
        throw new ArgumentException();
      }

      if (_Frame != null) {
        return;
      }

      _Frame = frame;

      this.HoockMethods();

      this.WireUpDelegates();

      this.WireUpEvents();

      Application.ApplicationExit += (s,e) => _ApplicationIsRunning = false;

    }

    #region  Call JS -> .NET (method-redirection) 

    private void HoockMethods() {

      foreach (var @method in this.IncludedMethods) {

        var attr = @method.GetCustomAttribute<PublicateAsJsMethodAttribute>();
        if (attr != null) {

          string jsMethodNameToRedirect = attr.JsMethodName;  // Me.MakeFirstLetterLCase(method.Name)

          // NOTE: when injecting a .net object into CEF, the name of .net method-names will automatically
          // change their fist character into lcase!!!
          // Dim jsMethodNameOnDotNetClass As String = Me.MakeFirstLetterLCase(method.Name)
          string jsMethodNameOnDotNetClass = this.MakeFirstLetterLCase(nameof(InvokeMethodOnNetObject));

          string[] parameterNames = @method.GetParameters().Select(p => p.Name).ToArray();
          string parameterSignature = string.Join(", ", parameterNames);

          string optionalReturn = "";
          if (!(@method.ReturnType == typeof(void))) {
            optionalReturn = "return ";
          }

          // Dim hoockingJsCommand As String = (
          // $"{_JsObjectNameToHook}.{jsMethodNameToRedirect} = function({parameterSignature}) " +
          // "{" +
          // $"{optionalReturn}{_JsObjectNameForDotNetClass}.{jsMethodNameOnDotNetClass}({parameterSignature});" +
          // "}"
          // )

          string hoockingJsCommand = $"{_JsObjectNameToHook}.{jsMethodNameToRedirect} = function({parameterSignature}) " + "{" + $"{optionalReturn}{_JsObjectNameForDotNetClass}.{jsMethodNameOnDotNetClass}('{@method.Name}', [{parameterSignature}]);" + "}";

          _Frame.ExecuteJavaScriptAsync(hoockingJsCommand);

        }

      }
    }

    public object InvokeMethodOnNetObject(string methodName, object[] args) {
      MethodInfo targetMethod = null;
      lock (_IncludedMethods) {
        if (!_IncludedMethods.ContainsKey(methodName)) {
          return null;
        }
        targetMethod = _IncludedMethods[methodName];
      }

      try {
        // TODO: umbau auf über task.run ASYNC damit JS keines freeze erlebt! hierzu muss ein resukt-handle zurück gegeben werden!!
        // und dann in der js welt gepollt werden bis das reulst da ist ->> evtl RxJS obserbale!!!
        return targetMethod.Invoke(this.DotNetObject, args);
      }

      catch (Exception ex) {
        return ex;
      }

    }

    #endregion

    #region  Call .NET -> JS (Delegates) 

    private void WireUpDelegates() {

      foreach (var prp in this.DotNetObject.GetType().GetProperties().Where(p => p.CanWrite && typeof(Delegate).IsAssignableFrom(p.PropertyType))) {

        var attr = prp.GetCustomAttribute<InjectJsMethodHandleAttribute>();
        if (attr != null) {

          var invoker = this.BuildDynamicDelegate(prp.PropertyType, args => this.InvokeMethodOnJsObject(attr.JsMethodName, args));

          prp.SetValue(this.DotNetObject, invoker);

        }

      }

    }

    private Dictionary<string, object> _CallbackBuffer = new Dictionary<string, object>();

    protected object InvokeMethodOnJsObject(string methodName, params object[] args) {

      if (!_ApplicationIsRunning) {
        return null;
      }

      string[] argStrings = args.Select(a => {

        if (a is null) {
          return "null";
        }

        // TODO: richtiger serializer!!!!
        if (a is int || a is long || a is decimal || a is bool) {
          return a.ToString();
        }
        else {
          return "'" + a.ToString() + "'";
        }

      }).ToArray();

      string callId = Guid.NewGuid().ToString();
      string hoockingJsCommand = "{ var result = " + _JsObjectNameToHook + "." + methodName + "(" + string.Join(", ", argStrings) + "); " + _JsObjectNameForDotNetClass + ".resultCallback('" + callId + "', result); }";
      _Frame.ExecuteJavaScriptAsync(hoockingJsCommand);


      while (_ApplicationIsRunning) {
        lock (_CallbackBuffer) {
          if (_CallbackBuffer.ContainsKey(callId)) {
            var result = _CallbackBuffer[callId];
            _CallbackBuffer.Remove(callId);
            return result;
          }
        }
        System.Threading.Thread.Sleep(20);
        Application.DoEvents();
      }

      return null;
    }

    public void ResultCallback(string callId, object result) {
      Task.Run(() => { lock (_CallbackBuffer) _CallbackBuffer.Add(callId, result); });
    }

    #endregion

    #region  Events .NET -> JS (naming-convention based) 

    // https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/how-to-hook-up-a-delegate-using-reflection

    private void WireUpEvents() {

      foreach (var evt in this.DotNetObject.GetType().GetEvents()) {

        var attr = evt.GetCustomAttribute<AttachJsEventHandlerAttribute>();
        if (attr != null) {

          var handler = this.BuildDynamicDelegate(evt, args => this.InvokeMethodOnJsObject(attr.JsMethodName, args), parameterFilter: p => !(p.Type.IsClass || p.Type.IsInterface || p.Name == "sender"));

          evt.AddEventHandler(this.DotNetObject, handler);

        }

      }
    }

    private Delegate BuildDynamicDelegate(EventInfo evt, Action<object[]> innerHandler, Func<ParameterExpression, bool> parameterFilter = null) {
      return this.BuildDynamicDelegate(evt.EventHandlerType, innerHandler, parameterFilter);
    }

    private Delegate BuildDynamicDelegate(Type outerDelegateType, Action<object[]> innerHandler, Func<ParameterExpression, bool> parameterFilter = null) {
      return this.BuildDynamicDelegate(outerDelegateType, args => {
        innerHandler.Invoke(args);
        return null;
      }, parameterFilter);
    }

    private Delegate BuildDynamicDelegate(Type outerDelegateType, Func<object[], object> innerHandler, Func<ParameterExpression, bool> parameterFilter = null) {

      var protectedInnerHandlerHandler = innerHandler;
      var parameterExpressions = new List<ParameterExpression>();
      var outerDelegateMethod = outerDelegateType.GetMethod("Invoke");

      // Protection to avoid a NullReferenceException when Nothing is returned by the 
      // inner Handler and the lambda tries to convert it into a value type
      if (!(outerDelegateMethod.ReturnType == typeof(void))) {
        protectedInnerHandlerHandler = args => {
          var result = innerHandler.Invoke(args);

          if (result != null && !outerDelegateMethod.ReturnType.IsAssignableFrom(result.GetType())) {
            result = null;
          }

          if (result is null && outerDelegateMethod.ReturnType.IsValueType) {
            // default value of the value type
            return Activator.CreateInstance(outerDelegateMethod.ReturnType);
          }

          return result;
        };
      }

      if (parameterFilter is null) {
        parameterFilter = new Func<ParameterExpression, bool>(p => true);
      }

      foreach (var prm in outerDelegateMethod.GetParameters())
        parameterExpressions.Add(Expression.Parameter(prm.ParameterType, prm.Name));

      var castedParameterExpressions = parameterExpressions.Where(pe => parameterFilter.Invoke(pe)).Select(p => Expression.Convert(p, typeof(object)));

      var objArryBuilderExpresssion = Expression.NewArrayInit(typeof(object), castedParameterExpressions);


      var callResult = Expression.Call(Expression.Constant(protectedInnerHandlerHandler.Target), protectedInnerHandlerHandler.Method, objArryBuilderExpresssion);

      Expression body;
      if (outerDelegateMethod.ReturnType == typeof(void)) {
        body = callResult;
      }
      else {
        body = Expression.Convert(callResult, outerDelegateMethod.ReturnType);
      }


      var lambda = Expression.Lambda(outerDelegateType, body, parameterExpressions);

      return lambda.Compile();
    }

    #endregion

  }

}