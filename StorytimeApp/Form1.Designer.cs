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
      tvKb = new TreeView();
      cmsTreeview = new ContextMenuStrip(components);
      reloadTreeToolStripMenuItem = new ToolStripMenuItem();
      toolStripSeparator1 = new ToolStripSeparator();
      miAddProject = new ToolStripMenuItem();
      miAddStory = new ToolStripMenuItem();
      miAddScene = new ToolStripMenuItem();
      miAddBeat = new ToolStripMenuItem();
      miAddCharacter = new ToolStripMenuItem();
      miAddRefCharacter = new ToolStripMenuItem();
      miAddLocation = new ToolStripMenuItem();
      miAddRule = new ToolStripMenuItem();
      toolStripSeparator2 = new ToolStripSeparator();
      miDeleteItem = new ToolStripMenuItem();
      tabControl1 = new TabControl();
      tpBrowse = new TabPage();
      tpItems = new TabPage();
      lbItemId = new Label();
      btnAbortItem = new Button();
      btnUpdateItem = new Button();
      label3 = new Label();
      label2 = new Label();
      lbType = new Label();
      lbItemName = new Label();
      edItemData = new TextBox();
      edItemDesc = new TextBox();
      edItemType = new ComboBox();
      edItemName = new TextBox();
      tpRelations = new TabPage();
      lbRelationId = new Label();
      btnCancelRelation = new Button();
      btnUpdateRelation = new Button();
      lbRelItemName = new Label();
      label5 = new Label();
      label4 = new Label();
      label1 = new Label();
      cbRelItem = new ComboBox();
      cbRelRelation = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
      splitContainer2.Panel2.SuspendLayout();
      splitContainer2.SuspendLayout();
      cmsTreeview.SuspendLayout();
      tabControl1.SuspendLayout();
      tpItems.SuspendLayout();
      tpRelations.SuspendLayout();
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
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(tabControl1);
      splitContainer1.Size = new Size(841, 539);
      splitContainer1.SplitterDistance = 280;
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
      splitContainer2.Panel2.Controls.Add(tvKb);
      splitContainer2.Size = new Size(280, 539);
      splitContainer2.SplitterDistance = 64;
      splitContainer2.TabIndex = 0;
      // 
      // tvKb
      // 
      tvKb.ContextMenuStrip = cmsTreeview;
      tvKb.Dock = DockStyle.Fill;
      tvKb.Location = new Point(0, 0);
      tvKb.Name = "tvKb";
      tvKb.Size = new Size(280, 471);
      tvKb.TabIndex = 0;
      tvKb.AfterSelect += tvKb_AfterSelect;
      // 
      // cmsTreeview
      // 
      cmsTreeview.Items.AddRange(new ToolStripItem[] { reloadTreeToolStripMenuItem, toolStripSeparator1, miAddProject, miAddStory, miAddScene, miAddBeat, miAddCharacter, miAddRefCharacter, miAddLocation, miAddRule, toolStripSeparator2, miDeleteItem });
      cmsTreeview.Name = "cmsTreeview";
      cmsTreeview.Size = new Size(195, 258);
      cmsTreeview.Opening += cmsTreeview_Opening;
      // 
      // reloadTreeToolStripMenuItem
      // 
      reloadTreeToolStripMenuItem.Name = "reloadTreeToolStripMenuItem";
      reloadTreeToolStripMenuItem.Size = new Size(194, 22);
      reloadTreeToolStripMenuItem.Text = "Reload Tree";
      reloadTreeToolStripMenuItem.Click += reloadTreeToolStripMenuItem_Click;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(191, 6);
      // 
      // miAddProject
      // 
      miAddProject.Name = "miAddProject";
      miAddProject.Size = new Size(194, 22);
      miAddProject.Text = "Add Project";
      miAddProject.Click += miAddProject_Click;
      // 
      // miAddStory
      // 
      miAddStory.Name = "miAddStory";
      miAddStory.Size = new Size(194, 22);
      miAddStory.Text = "Add Story";
      miAddStory.Click += miAddStory_Click;
      // 
      // miAddScene
      // 
      miAddScene.Name = "miAddScene";
      miAddScene.Size = new Size(194, 22);
      miAddScene.Text = "Add Scene";
      miAddScene.Click += miAddScene_Click;
      // 
      // miAddBeat
      // 
      miAddBeat.Name = "miAddBeat";
      miAddBeat.Size = new Size(194, 22);
      miAddBeat.Text = "Add Beat";
      miAddBeat.Click += miAddBeat_Click;
      // 
      // miAddCharacter
      // 
      miAddCharacter.Name = "miAddCharacter";
      miAddCharacter.Size = new Size(194, 22);
      miAddCharacter.Text = "Add Character";
      miAddCharacter.Click += miAddCharacter_Click;
      // 
      // miAddRefCharacter
      // 
      miAddRefCharacter.Name = "miAddRefCharacter";
      miAddRefCharacter.Size = new Size(194, 22);
      miAddRefCharacter.Text = "Add Existing Character";
      miAddRefCharacter.Click += miAddRefCharacter_Click;
      // 
      // miAddLocation
      // 
      miAddLocation.Name = "miAddLocation";
      miAddLocation.Size = new Size(194, 22);
      miAddLocation.Text = "Add Location";
      miAddLocation.Click += miAddLocation_Click;
      // 
      // miAddRule
      // 
      miAddRule.Name = "miAddRule";
      miAddRule.Size = new Size(194, 22);
      miAddRule.Text = "Add Rule";
      miAddRule.Click += miAddRule_Click;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new Size(191, 6);
      // 
      // miDeleteItem
      // 
      miDeleteItem.Name = "miDeleteItem";
      miDeleteItem.Size = new Size(194, 22);
      miDeleteItem.Text = "Delete Item";
      miDeleteItem.Click += miDeleteItem_Click;
      // 
      // tabControl1
      // 
      tabControl1.Controls.Add(tpBrowse);
      tabControl1.Controls.Add(tpItems);
      tabControl1.Controls.Add(tpRelations);
      tabControl1.Dock = DockStyle.Fill;
      tabControl1.Location = new Point(0, 0);
      tabControl1.Name = "tabControl1";
      tabControl1.SelectedIndex = 0;
      tabControl1.Size = new Size(557, 539);
      tabControl1.TabIndex = 0;
      // 
      // tpBrowse
      // 
      tpBrowse.Location = new Point(4, 24);
      tpBrowse.Name = "tpBrowse";
      tpBrowse.Padding = new Padding(3);
      tpBrowse.Size = new Size(549, 511);
      tpBrowse.TabIndex = 2;
      tpBrowse.Text = "Browse";
      tpBrowse.UseVisualStyleBackColor = true;
      // 
      // tpItems
      // 
      tpItems.Controls.Add(lbItemId);
      tpItems.Controls.Add(btnAbortItem);
      tpItems.Controls.Add(btnUpdateItem);
      tpItems.Controls.Add(label3);
      tpItems.Controls.Add(label2);
      tpItems.Controls.Add(lbType);
      tpItems.Controls.Add(lbItemName);
      tpItems.Controls.Add(edItemData);
      tpItems.Controls.Add(edItemDesc);
      tpItems.Controls.Add(edItemType);
      tpItems.Controls.Add(edItemName);
      tpItems.Location = new Point(4, 24);
      tpItems.Name = "tpItems";
      tpItems.Padding = new Padding(3);
      tpItems.Size = new Size(549, 511);
      tpItems.TabIndex = 0;
      tpItems.Text = "Items";
      tpItems.UseVisualStyleBackColor = true;
      // 
      // lbItemId
      // 
      lbItemId.AutoSize = true;
      lbItemId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbItemId.Location = new Point(20, 16);
      lbItemId.Name = "lbItemId";
      lbItemId.Size = new Size(68, 21);
      lbItemId.TabIndex = 10;
      lbItemId.Text = "ItemId: x";
      // 
      // btnAbortItem
      // 
      btnAbortItem.Location = new Point(226, 14);
      btnAbortItem.Name = "btnAbortItem";
      btnAbortItem.Size = new Size(75, 23);
      btnAbortItem.TabIndex = 9;
      btnAbortItem.Text = "Abort";
      btnAbortItem.UseVisualStyleBackColor = true;
      btnAbortItem.Click += btnAbortItem_Click;
      // 
      // btnUpdateItem
      // 
      btnUpdateItem.Location = new Point(145, 14);
      btnUpdateItem.Name = "btnUpdateItem";
      btnUpdateItem.Size = new Size(75, 23);
      btnUpdateItem.TabIndex = 8;
      btnUpdateItem.Text = "Update";
      btnUpdateItem.UseVisualStyleBackColor = true;
      btnUpdateItem.Click += btnUpdateItem_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(39, 349);
      label3.Name = "label3";
      label3.Size = new Size(31, 15);
      label3.TabIndex = 7;
      label3.Text = "Data";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(2, 112);
      label2.Name = "label2";
      label2.Size = new Size(67, 15);
      label2.TabIndex = 6;
      label2.Text = "Description";
      // 
      // lbType
      // 
      lbType.AutoSize = true;
      lbType.Location = new Point(40, 80);
      lbType.Name = "lbType";
      lbType.Size = new Size(31, 15);
      lbType.TabIndex = 5;
      lbType.Text = "Type";
      // 
      // lbItemName
      // 
      lbItemName.AutoSize = true;
      lbItemName.Location = new Point(32, 51);
      lbItemName.Name = "lbItemName";
      lbItemName.Size = new Size(39, 15);
      lbItemName.TabIndex = 4;
      lbItemName.Text = "Name";
      // 
      // edItemData
      // 
      edItemData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edItemData.Location = new Point(76, 346);
      edItemData.Multiline = true;
      edItemData.Name = "edItemData";
      edItemData.Size = new Size(465, 157);
      edItemData.TabIndex = 3;
      edItemData.TextChanged += edItemName_TextChanged;
      // 
      // edItemDesc
      // 
      edItemDesc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edItemDesc.Location = new Point(76, 109);
      edItemDesc.Multiline = true;
      edItemDesc.Name = "edItemDesc";
      edItemDesc.Size = new Size(465, 231);
      edItemDesc.TabIndex = 2;
      edItemDesc.TextChanged += edItemName_TextChanged;
      // 
      // edItemType
      // 
      edItemType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemType.FormattingEnabled = true;
      edItemType.Location = new Point(76, 77);
      edItemType.Name = "edItemType";
      edItemType.Size = new Size(465, 23);
      edItemType.TabIndex = 1;
      edItemType.TextChanged += edItemName_TextChanged;
      // 
      // edItemName
      // 
      edItemName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemName.Location = new Point(76, 48);
      edItemName.Name = "edItemName";
      edItemName.Size = new Size(465, 23);
      edItemName.TabIndex = 0;
      edItemName.TextChanged += edItemName_TextChanged;
      // 
      // tpRelations
      // 
      tpRelations.Controls.Add(lbRelationId);
      tpRelations.Controls.Add(btnCancelRelation);
      tpRelations.Controls.Add(btnUpdateRelation);
      tpRelations.Controls.Add(lbRelItemName);
      tpRelations.Controls.Add(label5);
      tpRelations.Controls.Add(label4);
      tpRelations.Controls.Add(label1);
      tpRelations.Controls.Add(cbRelItem);
      tpRelations.Controls.Add(cbRelRelation);
      tpRelations.Location = new Point(4, 24);
      tpRelations.Name = "tpRelations";
      tpRelations.Padding = new Padding(3);
      tpRelations.Size = new Size(549, 511);
      tpRelations.TabIndex = 1;
      tpRelations.Text = "Relation";
      tpRelations.UseVisualStyleBackColor = true;
      // 
      // lbRelationId
      // 
      lbRelationId.AutoSize = true;
      lbRelationId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbRelationId.Location = new Point(27, 22);
      lbRelationId.Name = "lbRelationId";
      lbRelationId.Size = new Size(94, 21);
      lbRelationId.TabIndex = 11;
      lbRelationId.Text = "RelationId: x";
      // 
      // btnCancelRelation
      // 
      btnCancelRelation.Location = new Point(192, 228);
      btnCancelRelation.Name = "btnCancelRelation";
      btnCancelRelation.Size = new Size(75, 23);
      btnCancelRelation.TabIndex = 7;
      btnCancelRelation.Text = "Cancel";
      btnCancelRelation.UseVisualStyleBackColor = true;
      btnCancelRelation.Click += btnCancelRelation_Click;
      // 
      // btnUpdateRelation
      // 
      btnUpdateRelation.Location = new Point(95, 228);
      btnUpdateRelation.Name = "btnUpdateRelation";
      btnUpdateRelation.Size = new Size(75, 23);
      btnUpdateRelation.TabIndex = 6;
      btnUpdateRelation.Text = "Update";
      btnUpdateRelation.UseVisualStyleBackColor = true;
      btnUpdateRelation.Click += btnUpdateRelation_Click;
      // 
      // lbRelItemName
      // 
      lbRelItemName.AutoSize = true;
      lbRelItemName.Location = new Point(143, 63);
      lbRelItemName.Name = "lbRelItemName";
      lbRelItemName.Size = new Size(66, 15);
      lbRelItemName.TabIndex = 5;
      lbRelItemName.Text = "Item Name";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(46, 157);
      label5.Name = "label5";
      label5.Size = new Size(75, 15);
      label5.TabIndex = 4;
      label5.Text = "Existing Item";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(70, 112);
      label4.Name = "label4";
      label4.Size = new Size(50, 15);
      label4.TabIndex = 3;
      label4.Text = "Relation";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(70, 63);
      label1.Name = "label1";
      label1.Size = new Size(51, 15);
      label1.TabIndex = 2;
      label1.Text = "Selected";
      // 
      // cbRelItem
      // 
      cbRelItem.FormattingEnabled = true;
      cbRelItem.Location = new Point(143, 154);
      cbRelItem.Name = "cbRelItem";
      cbRelItem.Size = new Size(350, 23);
      cbRelItem.TabIndex = 1;
      cbRelItem.SelectedIndexChanged += cbRelItem_SelectedIndexChanged;
      // 
      // cbRelRelation
      // 
      cbRelRelation.FormattingEnabled = true;
      cbRelRelation.Location = new Point(143, 109);
      cbRelRelation.Name = "cbRelRelation";
      cbRelRelation.Size = new Size(350, 23);
      cbRelRelation.TabIndex = 0;
      cbRelRelation.SelectedIndexChanged += cbRelRelation_SelectedIndexChanged;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(841, 539);
      Controls.Add(splitContainer1);
      Name = "Form1";
      Text = "Storytime";
      Shown += Form1_Shown;
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
      splitContainer2.ResumeLayout(false);
      cmsTreeview.ResumeLayout(false);
      tabControl1.ResumeLayout(false);
      tpItems.ResumeLayout(false);
      tpItems.PerformLayout();
      tpRelations.ResumeLayout(false);
      tpRelations.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private TreeView tvKb;
    private ContextMenuStrip cmsTreeview;
    private ToolStripMenuItem miAddProject;
    private TabControl tabControl1;
    private TabPage tpItems;
    private TabPage tpRelations;
    private ToolStripMenuItem reloadTreeToolStripMenuItem;
    private TextBox edItemDesc;
    private ComboBox edItemType;
    private TextBox edItemName;
    private TextBox edItemData;
    private Label label3;
    private Label label2;
    private Label lbType;
    private Label lbItemName;
    private Button btnAbortItem;
    private Button btnUpdateItem;
    private TabPage tpBrowse;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem miAddStory;
    private ToolStripMenuItem miAddScene;
    private ToolStripMenuItem miAddBeat;
    private ToolStripMenuItem miAddCharacter;
    private ToolStripMenuItem miAddLocation;
    private ToolStripMenuItem miAddRule;
    private ToolStripMenuItem miAddRefCharacter;
    private ComboBox cbRelItem;
    private ComboBox cbRelRelation;
    private Button btnCancelRelation;
    private Button btnUpdateRelation;
    private Label lbRelItemName;
    private Label label5;
    private Label label4;
    private Label label1;
    private Label lbItemId;
    private Label lbRelationId;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem miDeleteItem;
  }
}
