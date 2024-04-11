using System.Drawing;
using System.Windows.Forms;

namespace CefSharp.Wrapped.NetFxDemo {

  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.cef = new System.Windows.Forms.CefSharpWrapper();
      this.SuspendLayout();
      // 
      // cef
      // 
      this.cef.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cef.Location = new System.Drawing.Point(0, 0);
      this.cef.Name = "cef";
      this.cef.Size = new System.Drawing.Size(800, 450);
      this.cef.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.cef);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "CefSharp.Wrapped-Demo (.NET Framework)";
      this.ResumeLayout(false);

    }

    #endregion

    private CefSharpWrapper cef;
  }

}
