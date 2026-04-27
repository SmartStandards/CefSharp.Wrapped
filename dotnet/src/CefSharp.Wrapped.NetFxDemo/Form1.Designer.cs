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
      this.cef = new System.Windows.Forms.CefControl();
      this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
      this.toolStripContainer.ContentPanel.SuspendLayout();
      this.toolStripContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // cef
      // 
      this.cef.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cef.Location = new System.Drawing.Point(0, 0);
      this.cef.Name = "cef";
      this.cef.Size = new System.Drawing.Size(983, 661);
      this.cef.TabIndex = 0;
      // 
      // toolStripContainer
      // 
      // 
      // toolStripContainer.ContentPanel
      // 
      this.toolStripContainer.ContentPanel.Controls.Add(this.cef);
      this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(983, 661);
      this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer.Name = "toolStripContainer";
      this.toolStripContainer.Size = new System.Drawing.Size(983, 661);
      this.toolStripContainer.TabIndex = 1;
      this.toolStripContainer.Text = "toolStripContainer1";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(983, 661);
      this.Controls.Add(this.toolStripContainer);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "CefSharp.Wrapped-Demo (.NET Framework)";
      this.toolStripContainer.ContentPanel.ResumeLayout(false);
      this.toolStripContainer.ResumeLayout(false);
      this.toolStripContainer.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private CefControl cef;
    private ToolStripContainer toolStripContainer;
  }

}
