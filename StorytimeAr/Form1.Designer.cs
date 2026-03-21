namespace StorytimeAr {
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
      components = new System.ComponentModel.Container();
      splitContainer1 = new SplitContainer();
      splitContainer2 = new SplitContainer();
      treeView1 = new TreeView();
      cmsTreeview = new ContextMenuStrip(components);
      miAddNew = new ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
      splitContainer2.Panel2.SuspendLayout();
      splitContainer2.SuspendLayout();
      cmsTreeview.SuspendLayout();
      SuspendLayout();
      // 
      // splitContainer1
      // 
      splitContainer1.Dock = DockStyle.Fill;
      splitContainer1.Location = new Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(splitContainer2);
      splitContainer1.Size = new Size(921, 560);
      splitContainer1.SplitterDistance = 307;
      splitContainer1.TabIndex = 0;
      // 
      // splitContainer2
      // 
      splitContainer2.Dock = DockStyle.Fill;
      splitContainer2.Location = new Point(0, 0);
      splitContainer2.Name = "splitContainer2";
      splitContainer2.Orientation = Orientation.Horizontal;
      // 
      // splitContainer2.Panel2
      // 
      splitContainer2.Panel2.Controls.Add(treeView1);
      splitContainer2.Size = new Size(307, 560);
      splitContainer2.SplitterDistance = 135;
      splitContainer2.TabIndex = 0;
      // 
      // treeView1
      // 
      treeView1.ContextMenuStrip = cmsTreeview;
      treeView1.Dock = DockStyle.Fill;
      treeView1.Location = new Point(0, 0);
      treeView1.Name = "treeView1";
      treeView1.Size = new Size(307, 421);
      treeView1.TabIndex = 0;
      // 
      // cmsTreeview
      // 
      cmsTreeview.Items.AddRange(new ToolStripItem[] { miAddNew });
      cmsTreeview.Name = "cmsTreeview";
      cmsTreeview.Size = new Size(124, 26);
      // 
      // miAddNew
      // 
      miAddNew.Name = "miAddNew";
      miAddNew.Size = new Size(123, 22);
      miAddNew.Text = "Add New";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(921, 560);
      Controls.Add(splitContainer1);
      Name = "Form1";
      Text = "Storytime";
      splitContainer1.Panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
      splitContainer2.ResumeLayout(false);
      cmsTreeview.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private TreeView treeView1;
    private ContextMenuStrip cmsTreeview;
    private ToolStripMenuItem miAddNew;
  }
}
