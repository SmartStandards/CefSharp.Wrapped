namespace CefSharp.Wrapped.NetCoreDemo {
  partial class Form1 {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      cef = new CefSharpWrapper();
      this.SuspendLayout();
      // 
      // cef
      // 
      cef.Dock = DockStyle.Fill;
      cef.Location = new Point(0, 0);
      cef.Name = "cef";
      cef.Size = new Size(800, 450);
      cef.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new SizeF(7F, 15F);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Controls.Add(cef);
      this.Name = "Form1";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "CefSharp.Wrapped-Demo (.NET Core)";
      this.ResumeLayout(false);
    }

    #endregion

    private CefSharpWrapper cef;
  }
}