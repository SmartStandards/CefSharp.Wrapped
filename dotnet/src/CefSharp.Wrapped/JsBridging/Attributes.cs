using System;

namespace System.Web.AbstractHosting.CEF.JsBridging {

  /// <summary>
  /// Note: This works ONLY FOR VALUE-TYPE PARAMETERS! (and byRef arguments are currently not supported!!!)
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class InjectJsMethodHandleAttribute : Attribute {

    public InjectJsMethodHandleAttribute(string jsMethodName) {
      this.JsMethodName = jsMethodName;
    }

    public string JsMethodName { get; private set; }

  }

  /// <summary>
  /// Note: This works ONLY FOR VALUE-TYPE PARAMETERS!
  /// </summary>
  [AttributeUsage(AttributeTargets.Event, AllowMultiple = false)]
  public class AttachJsEventHandlerAttribute : Attribute {

    public AttachJsEventHandlerAttribute(string jsMethodName) {
      this.JsMethodName = jsMethodName;
    }

    public string JsMethodName { get; private set; }

  }

  /// <summary>
  /// Note: This works ONLY FOR VALUE-TYPE PARAMETERS!
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class PublicateAsJsMethodAttribute : Attribute {

    public PublicateAsJsMethodAttribute(string jsMethodName) {
      this.JsMethodName = jsMethodName;
    }

    public string JsMethodName { get; private set; }

  }

  /// <summary>
  /// Note: This works ONLY FOR VALUE-TYPE PARAMETERS!
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class BindToJsObject : Attribute {

    /// <summary>
    /// </summary>
    /// <param name="jsObjectName">The given object name must be available under 'window.objName'</param>
    public BindToJsObject(string jsObjectName) {
      this.JsObjectName = jsObjectName;
    }

    public string JsObjectName { get; private set; }

  }

}