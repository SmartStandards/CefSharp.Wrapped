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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.cef = new System.Windows.Forms.CefControl();
      this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.backButton = new System.Windows.Forms.ToolStripButton();
      this.forwardButton = new System.Windows.Forms.ToolStripButton();
      this.urlTextBox = new System.Windows.Forms.ToolStripTextBox();
      this.goButton = new System.Windows.Forms.ToolStripButton();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.toolStripContainer.ContentPanel.SuspendLayout();
      this.toolStripContainer.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // cef
      // 
      this.cef.BrowserConsoleErrorLoggingMethod = null;
      this.cef.BrowserConsoleInfoLoggingMethod = null;
      this.cef.BrowserConsoleWarningLoggingMethod = null;
      this.cef.BrowserDevToolsVisible = false;
      this.cef.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cef.Location = new System.Drawing.Point(0, 0);
      this.cef.Name = "cef";
      this.cef.Size = new System.Drawing.Size(726, 401);
      this.cef.TabIndex = 0;
      // 
      // toolStripContainer
      // 
      // 
      // toolStripContainer.ContentPanel
      // 
      this.toolStripContainer.ContentPanel.Controls.Add(this.cef);
      this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(726, 401);
      this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
      this.toolStripContainer.Name = "toolStripContainer";
      this.toolStripContainer.Size = new System.Drawing.Size(726, 426);
      this.toolStripContainer.TabIndex = 1;
      this.toolStripContainer.Text = "toolStripContainer1";
      // 
      // toolStripContainer.TopToolStripPanel
      // 
      this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.urlTextBox,
            this.goButton});
      this.toolStrip1.Location = new System.Drawing.Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(605, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Layout += new System.Windows.Forms.LayoutEventHandler(this.HandleToolStripLayout);
      // 
      // backButton
      // 
      this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
      this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.backButton.Name = "backButton";
      this.backButton.Size = new System.Drawing.Size(23, 22);
      this.backButton.Text = "toolStripButton1";
      // 
      // forwardButton
      // 
      this.forwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.forwardButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardButton.Image")));
      this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.forwardButton.Name = "forwardButton";
      this.forwardButton.Size = new System.Drawing.Size(23, 22);
      this.forwardButton.Text = "toolStripButton2";
      // 
      // urlTextBox
      // 
      this.urlTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.urlTextBox.Name = "urlTextBox";
      this.urlTextBox.Size = new System.Drawing.Size(500, 25);
      // 
      // goButton
      // 
      this.goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.goButton.Image = ((System.Drawing.Image)(resources.GetObject("goButton.Image")));
      this.goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.goButton.Name = "goButton";
      this.goButton.Size = new System.Drawing.Size(23, 22);
      this.goButton.Text = "toolStripButton3";
      // 
      // menuStrip1
      // 
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(726, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(726, 450);
      this.Controls.Add(this.toolStripContainer);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "CefSharp.Wrapped-Demo (.NET Framework)";
      this.toolStripContainer.ContentPanel.ResumeLayout(false);
      this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer.TopToolStripPanel.PerformLayout();
      this.toolStripContainer.ResumeLayout(false);
      this.toolStripContainer.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private CefControl cef;
    private ToolStripContainer toolStripContainer;
    private MenuStrip menuStrip1;
    private ToolStrip toolStrip1;
    private ToolStripButton backButton;
    private ToolStripButton forwardButton;
    private ToolStripTextBox urlTextBox;
    private ToolStripButton goButton;
  }

}
