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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      splitContainer1 = new SplitContainer();
      splitContainer2 = new SplitContainer();
      btnReloadTree = new Button();
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
      miGenerateStory = new ToolStripMenuItem();
      miGenerateScene = new ToolStripMenuItem();
      miGenerateBeat = new ToolStripMenuItem();
      miGenerateCallSheet = new ToolStripMenuItem();
      miGeneratePerformance = new ToolStripMenuItem();
      miGenerateDeliverable = new ToolStripMenuItem();
      toolStripSeparator3 = new ToolStripSeparator();
      miDeleteItem = new ToolStripMenuItem();
      treeList = new ImageList(components);
      tabControl1 = new TabControl();
      tpBrowse = new TabPage();
      label6 = new Label();
      tbTestOut = new TextBox();
      lbLMStudioModels = new ListBox();
      cbClaudeModel = new ComboBox();
      rbClaudeCode = new RadioButton();
      rbLMStudio = new RadioButton();
      label8 = new Label();
      lbClaudeLaunch = new LinkLabel();
      lbCurrentModel = new Label();
      edLmStudioModel = new TextBox();
      btnGetLmStudioModels = new Button();
      tpItems = new TabPage();
      btnCancelRelation = new Button();
      btnUpdateRelation = new Button();
      label7 = new Label();
      edRank = new NumericUpDown();
      label5 = new Label();
      label4 = new Label();
      cbRelItem = new ComboBox();
      cbRelRelation = new ComboBox();
      lbRelationId = new Label();
      lbRelItemName = new Label();
      label1 = new Label();
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
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
      splitContainer2.Panel1.SuspendLayout();
      splitContainer2.Panel2.SuspendLayout();
      splitContainer2.SuspendLayout();
      cmsTreeview.SuspendLayout();
      tabControl1.SuspendLayout();
      tpBrowse.SuspendLayout();
      tpItems.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)edRank).BeginInit();
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
      // splitContainer2.Panel1
      // 
      splitContainer2.Panel1.Controls.Add(btnReloadTree);
      // 
      // splitContainer2.Panel2
      // 
      splitContainer2.Panel2.Controls.Add(tvKb);
      splitContainer2.Size = new Size(280, 539);
      splitContainer2.SplitterDistance = 64;
      splitContainer2.TabIndex = 0;
      // 
      // btnReloadTree
      // 
      btnReloadTree.Location = new Point(3, 38);
      btnReloadTree.Name = "btnReloadTree";
      btnReloadTree.Size = new Size(137, 23);
      btnReloadTree.TabIndex = 0;
      btnReloadTree.Text = "Agents done Reload";
      btnReloadTree.UseVisualStyleBackColor = true;
      btnReloadTree.Click += btnReloadTree_Click;
      // 
      // tvKb
      // 
      tvKb.ContextMenuStrip = cmsTreeview;
      tvKb.Dock = DockStyle.Fill;
      tvKb.ImageIndex = 0;
      tvKb.ImageList = treeList;
      tvKb.Location = new Point(0, 0);
      tvKb.Name = "tvKb";
      tvKb.SelectedImageIndex = 0;
      tvKb.Size = new Size(280, 471);
      tvKb.TabIndex = 0;
      tvKb.AfterSelect += tvKb_AfterSelect;
      // 
      // cmsTreeview
      // 
      cmsTreeview.Items.AddRange(new ToolStripItem[] { reloadTreeToolStripMenuItem, toolStripSeparator1, miAddProject, miAddStory, miAddScene, miAddBeat, miAddCharacter, miAddRefCharacter, miAddLocation, miAddRule, toolStripSeparator2, miGenerateStory, miGenerateScene, miGenerateBeat, miGenerateCallSheet, miGeneratePerformance, miGenerateDeliverable, toolStripSeparator3, miDeleteItem });
      cmsTreeview.Name = "cmsTreeview";
      cmsTreeview.Size = new Size(195, 374);
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
      // miGenerateStory
      // 
      miGenerateStory.Name = "miGenerateStory";
      miGenerateStory.Size = new Size(194, 22);
      miGenerateStory.Text = "Generate Story";
      miGenerateStory.Click += miGenerateStory_Click;
      // 
      // miGenerateScene
      // 
      miGenerateScene.Name = "miGenerateScene";
      miGenerateScene.Size = new Size(194, 22);
      miGenerateScene.Text = "Generate Scene";
      miGenerateScene.Click += miGenerateScene_Click;
      // 
      // miGenerateBeat
      // 
      miGenerateBeat.Name = "miGenerateBeat";
      miGenerateBeat.Size = new Size(194, 22);
      miGenerateBeat.Text = "Generate Beat Set";
      miGenerateBeat.Click += miGenerateBeat_Click;
      // 
      // miGenerateCallSheet
      // 
      miGenerateCallSheet.Name = "miGenerateCallSheet";
      miGenerateCallSheet.Size = new Size(194, 22);
      miGenerateCallSheet.Text = "Generate CallSheet";
      miGenerateCallSheet.Click += miGenerateCallSheet_Click;
      // 
      // miGeneratePerformance
      // 
      miGeneratePerformance.Name = "miGeneratePerformance";
      miGeneratePerformance.Size = new Size(194, 22);
      miGeneratePerformance.Text = "Generate Performance";
      miGeneratePerformance.Click += miGeneratePerformance_Click;
      // 
      // miGenerateDeliverable
      // 
      miGenerateDeliverable.Name = "miGenerateDeliverable";
      miGenerateDeliverable.Size = new Size(194, 22);
      miGenerateDeliverable.Text = "Generate Deliverable";
      miGenerateDeliverable.Click += miGenerateDeliverable_Click;
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new Size(191, 6);
      // 
      // miDeleteItem
      // 
      miDeleteItem.Name = "miDeleteItem";
      miDeleteItem.Size = new Size(194, 22);
      miDeleteItem.Text = "Delete Item";
      miDeleteItem.Click += miDeleteItem_Click;
      // 
      // treeList
      // 
      treeList.ColorDepth = ColorDepth.Depth32Bit;
      treeList.ImageStream = (ImageListStreamer)resources.GetObject("treeList.ImageStream");
      treeList.TransparentColor = Color.Transparent;
      treeList.Images.SetKeyName(0, "transparent.png");
      treeList.Images.SetKeyName(1, "folder.png");
      treeList.Images.SetKeyName(2, "story.png");
      treeList.Images.SetKeyName(3, "scene.png");
      treeList.Images.SetKeyName(4, "beat.png");
      treeList.Images.SetKeyName(5, "character.png");
      treeList.Images.SetKeyName(6, "location.png");
      treeList.Images.SetKeyName(7, "rule.png");
      treeList.Images.SetKeyName(8, "tone.png");
      treeList.Images.SetKeyName(9, "callsheet.png");
      treeList.Images.SetKeyName(10, "performance.png");
      treeList.Images.SetKeyName(11, "deliverable.png");
      treeList.Images.SetKeyName(12, "narration.png");
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
      tpBrowse.Controls.Add(label6);
      tpBrowse.Controls.Add(tbTestOut);
      tpBrowse.Controls.Add(lbLMStudioModels);
      tpBrowse.Controls.Add(cbClaudeModel);
      tpBrowse.Controls.Add(rbClaudeCode);
      tpBrowse.Controls.Add(rbLMStudio);
      tpBrowse.Controls.Add(label8);
      tpBrowse.Controls.Add(lbClaudeLaunch);
      tpBrowse.Controls.Add(lbCurrentModel);
      tpBrowse.Controls.Add(edLmStudioModel);
      tpBrowse.Controls.Add(btnGetLmStudioModels);
      tpBrowse.Location = new Point(4, 24);
      tpBrowse.Name = "tpBrowse";
      tpBrowse.Padding = new Padding(3);
      tpBrowse.Size = new Size(549, 511);
      tpBrowse.TabIndex = 2;
      tpBrowse.Text = "Settings";
      tpBrowse.UseVisualStyleBackColor = true;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(38, 302);
      label6.Name = "label6";
      label6.Size = new Size(52, 15);
      label6.TabIndex = 11;
      label6.Text = "Error log";
      // 
      // tbTestOut
      // 
      tbTestOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tbTestOut.Location = new Point(37, 327);
      tbTestOut.Multiline = true;
      tbTestOut.Name = "tbTestOut";
      tbTestOut.Size = new Size(504, 176);
      tbTestOut.TabIndex = 10;
      // 
      // lbLMStudioModels
      // 
      lbLMStudioModels.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      lbLMStudioModels.FormattingEnabled = true;
      lbLMStudioModels.Location = new Point(173, 145);
      lbLMStudioModels.Name = "lbLMStudioModels";
      lbLMStudioModels.Size = new Size(368, 64);
      lbLMStudioModels.TabIndex = 9;
      lbLMStudioModels.DoubleClick += lbLMStudioModels_DoubleClick;
      // 
      // cbClaudeModel
      // 
      cbClaudeModel.FormattingEnabled = true;
      cbClaudeModel.Items.AddRange(new object[] { "sonnet", "opus" });
      cbClaudeModel.Location = new Point(173, 252);
      cbClaudeModel.Name = "cbClaudeModel";
      cbClaudeModel.Size = new Size(368, 23);
      cbClaudeModel.TabIndex = 8;
      cbClaudeModel.Text = "Sonnet";
      cbClaudeModel.TextChanged += cbClaudeModel_TextChanged;
      // 
      // rbClaudeCode
      // 
      rbClaudeCode.AutoSize = true;
      rbClaudeCode.Location = new Point(37, 253);
      rbClaudeCode.Name = "rbClaudeCode";
      rbClaudeCode.Size = new Size(93, 19);
      rbClaudeCode.TabIndex = 7;
      rbClaudeCode.Tag = "model";
      rbClaudeCode.Text = "Claude Code";
      rbClaudeCode.UseVisualStyleBackColor = true;
      rbClaudeCode.CheckedChanged += rbClaudeCode_CheckedChanged;
      // 
      // rbLMStudio
      // 
      rbLMStudio.AutoSize = true;
      rbLMStudio.Checked = true;
      rbLMStudio.Location = new Point(37, 94);
      rbLMStudio.Name = "rbLMStudio";
      rbLMStudio.Size = new Size(79, 19);
      rbLMStudio.TabIndex = 6;
      rbLMStudio.TabStop = true;
      rbLMStudio.Tag = "model";
      rbLMStudio.Text = "LM Studio";
      rbLMStudio.UseVisualStyleBackColor = true;
      rbLMStudio.CheckedChanged += rbLMStudio_CheckedChanged;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Location = new Point(23, 22);
      label8.Name = "label8";
      label8.Size = new Size(120, 15);
      label8.TabIndex = 5;
      label8.Text = "Agent Launch Folder:";
      // 
      // lbClaudeLaunch
      // 
      lbClaudeLaunch.AutoSize = true;
      lbClaudeLaunch.Location = new Point(173, 22);
      lbClaudeLaunch.Name = "lbClaudeLaunch";
      lbClaudeLaunch.Size = new Size(93, 15);
      lbClaudeLaunch.TabIndex = 4;
      lbClaudeLaunch.TabStop = true;
      lbClaudeLaunch.Text = "lbClaudeLaunch";
      lbClaudeLaunch.LinkClicked += lbClaudeLaunch_LinkClicked;
      // 
      // lbCurrentModel
      // 
      lbCurrentModel.AutoSize = true;
      lbCurrentModel.Location = new Point(23, 56);
      lbCurrentModel.Name = "lbCurrentModel";
      lbCurrentModel.Size = new Size(133, 15);
      lbCurrentModel.TabIndex = 3;
      lbCurrentModel.Text = " LMStudioModel to use:";
      // 
      // edLmStudioModel
      // 
      edLmStudioModel.Location = new Point(173, 93);
      edLmStudioModel.Name = "edLmStudioModel";
      edLmStudioModel.Size = new Size(368, 23);
      edLmStudioModel.TabIndex = 2;
      edLmStudioModel.Text = "nvidia/nemotron-3-nano-4b";
      // 
      // btnGetLmStudioModels
      // 
      btnGetLmStudioModels.Location = new Point(58, 145);
      btnGetLmStudioModels.Name = "btnGetLmStudioModels";
      btnGetLmStudioModels.Size = new Size(85, 23);
      btnGetLmStudioModels.TabIndex = 1;
      btnGetLmStudioModels.Text = "Get Models";
      btnGetLmStudioModels.UseVisualStyleBackColor = true;
      btnGetLmStudioModels.Click += btnGetLmStudioModels_Click;
      // 
      // tpItems
      // 
      tpItems.Controls.Add(btnCancelRelation);
      tpItems.Controls.Add(btnUpdateRelation);
      tpItems.Controls.Add(label7);
      tpItems.Controls.Add(edRank);
      tpItems.Controls.Add(label5);
      tpItems.Controls.Add(label4);
      tpItems.Controls.Add(cbRelItem);
      tpItems.Controls.Add(cbRelRelation);
      tpItems.Controls.Add(lbRelationId);
      tpItems.Controls.Add(lbRelItemName);
      tpItems.Controls.Add(label1);
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
      // btnCancelRelation
      // 
      btnCancelRelation.Location = new Point(227, 9);
      btnCancelRelation.Name = "btnCancelRelation";
      btnCancelRelation.Size = new Size(75, 23);
      btnCancelRelation.TabIndex = 22;
      btnCancelRelation.Text = "Cancel";
      btnCancelRelation.UseVisualStyleBackColor = true;
      // 
      // btnUpdateRelation
      // 
      btnUpdateRelation.Location = new Point(146, 8);
      btnUpdateRelation.Name = "btnUpdateRelation";
      btnUpdateRelation.Size = new Size(75, 23);
      btnUpdateRelation.TabIndex = 21;
      btnUpdateRelation.Text = "Update";
      btnUpdateRelation.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(6, 64);
      label7.Name = "label7";
      label7.Size = new Size(66, 15);
      label7.TabIndex = 20;
      label7.Text = "Rank Order";
      // 
      // edRank
      // 
      edRank.Location = new Point(83, 62);
      edRank.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      edRank.Name = "edRank";
      edRank.Size = new Size(120, 23);
      edRank.TabIndex = 19;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(-1, 123);
      label5.Name = "label5";
      label5.Size = new Size(75, 15);
      label5.TabIndex = 18;
      label5.Text = "Existing Item";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(22, 94);
      label4.Name = "label4";
      label4.Size = new Size(50, 15);
      label4.TabIndex = 17;
      label4.Text = "Relation";
      // 
      // cbRelItem
      // 
      cbRelItem.FormattingEnabled = true;
      cbRelItem.Location = new Point(83, 120);
      cbRelItem.Name = "cbRelItem";
      cbRelItem.Size = new Size(350, 23);
      cbRelItem.TabIndex = 16;
      // 
      // cbRelRelation
      // 
      cbRelRelation.FormattingEnabled = true;
      cbRelRelation.Location = new Point(83, 91);
      cbRelRelation.Name = "cbRelRelation";
      cbRelRelation.Size = new Size(350, 23);
      cbRelRelation.TabIndex = 15;
      // 
      // lbRelationId
      // 
      lbRelationId.AutoSize = true;
      lbRelationId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbRelationId.Location = new Point(6, 8);
      lbRelationId.Name = "lbRelationId";
      lbRelationId.Size = new Size(94, 21);
      lbRelationId.TabIndex = 14;
      lbRelationId.Text = "RelationId: x";
      // 
      // lbRelItemName
      // 
      lbRelItemName.AutoSize = true;
      lbRelItemName.Location = new Point(83, 44);
      lbRelItemName.Name = "lbRelItemName";
      lbRelItemName.Size = new Size(66, 15);
      lbRelItemName.TabIndex = 13;
      lbRelItemName.Text = "Item Name";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(29, 44);
      label1.Name = "label1";
      label1.Size = new Size(41, 15);
      label1.TabIndex = 12;
      label1.Text = "Parent";
      // 
      // lbItemId
      // 
      lbItemId.AutoSize = true;
      lbItemId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbItemId.Location = new Point(21, 165);
      lbItemId.Name = "lbItemId";
      lbItemId.Size = new Size(68, 21);
      lbItemId.TabIndex = 10;
      lbItemId.Text = "ItemId: x";
      // 
      // btnAbortItem
      // 
      btnAbortItem.Location = new Point(227, 163);
      btnAbortItem.Name = "btnAbortItem";
      btnAbortItem.Size = new Size(75, 23);
      btnAbortItem.TabIndex = 9;
      btnAbortItem.Text = "Abort";
      btnAbortItem.UseVisualStyleBackColor = true;
      btnAbortItem.Click += btnAbortItem_Click;
      // 
      // btnUpdateItem
      // 
      btnUpdateItem.Location = new Point(146, 163);
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
      label3.Location = new Point(29, 343);
      label3.Name = "label3";
      label3.Size = new Size(31, 15);
      label3.TabIndex = 7;
      label3.Text = "Data";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(0, 261);
      label2.Name = "label2";
      label2.Size = new Size(67, 15);
      label2.TabIndex = 6;
      label2.Text = "Description";
      // 
      // lbType
      // 
      lbType.AutoSize = true;
      lbType.Location = new Point(41, 229);
      lbType.Name = "lbType";
      lbType.Size = new Size(31, 15);
      lbType.TabIndex = 5;
      lbType.Text = "Type";
      // 
      // lbItemName
      // 
      lbItemName.AutoSize = true;
      lbItemName.Location = new Point(33, 200);
      lbItemName.Name = "lbItemName";
      lbItemName.Size = new Size(39, 15);
      lbItemName.TabIndex = 4;
      lbItemName.Text = "Name";
      // 
      // edItemData
      // 
      edItemData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edItemData.Location = new Point(76, 340);
      edItemData.Multiline = true;
      edItemData.Name = "edItemData";
      edItemData.ScrollBars = ScrollBars.Both;
      edItemData.Size = new Size(465, 163);
      edItemData.TabIndex = 3;
      edItemData.TextChanged += edItemName_TextChanged;
      // 
      // edItemDesc
      // 
      edItemDesc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edItemDesc.Location = new Point(76, 261);
      edItemDesc.Multiline = true;
      edItemDesc.Name = "edItemDesc";
      edItemDesc.ScrollBars = ScrollBars.Both;
      edItemDesc.Size = new Size(465, 73);
      edItemDesc.TabIndex = 2;
      edItemDesc.TextChanged += edItemName_TextChanged;
      // 
      // edItemType
      // 
      edItemType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemType.FormattingEnabled = true;
      edItemType.Location = new Point(77, 226);
      edItemType.Name = "edItemType";
      edItemType.Size = new Size(465, 23);
      edItemType.TabIndex = 1;
      edItemType.TextChanged += edItemName_TextChanged;
      // 
      // edItemName
      // 
      edItemName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemName.Location = new Point(77, 197);
      edItemName.Name = "edItemName";
      edItemName.Size = new Size(465, 23);
      edItemName.TabIndex = 0;
      edItemName.TextChanged += edItemName_TextChanged;
      // 
      // tpRelations
      // 
      tpRelations.Location = new Point(4, 24);
      tpRelations.Name = "tpRelations";
      tpRelations.Padding = new Padding(3);
      tpRelations.Size = new Size(549, 511);
      tpRelations.TabIndex = 1;
      tpRelations.Text = "Factory Floor";
      tpRelations.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(841, 539);
      Controls.Add(splitContainer1);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "Form1";
      Text = "Storytime";
      Shown += Form1_Shown;
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      splitContainer2.Panel1.ResumeLayout(false);
      splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
      splitContainer2.ResumeLayout(false);
      cmsTreeview.ResumeLayout(false);
      tabControl1.ResumeLayout(false);
      tpBrowse.ResumeLayout(false);
      tpBrowse.PerformLayout();
      tpItems.ResumeLayout(false);
      tpItems.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)edRank).EndInit();
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
    private Label lbItemId;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem miDeleteItem;
    private Button btnGetLmStudioModels;
    private Label lbCurrentModel;
    private TextBox edLmStudioModel;
    private Button btnReloadTree;
    private ToolStripMenuItem miGenerateStory;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem miGenerateScene;
    private ToolStripMenuItem miGenerateBeat;
    private ToolStripMenuItem miGenerateCallSheet;
    private ToolStripMenuItem miGeneratePerformance;
    private ToolStripMenuItem miGenerateDeliverable;
    private LinkLabel lbClaudeLaunch;
    private Label label8;
    private RadioButton rbLMStudio;
    private RadioButton rbClaudeCode;
    private ComboBox cbClaudeModel;
    private ListBox lbLMStudioModels;
    private Label label6;
    private TextBox tbTestOut;
    private Label label7;
    private NumericUpDown edRank;
    private Label label5;
    private Label label4;
    private ComboBox cbRelItem;
    private ComboBox cbRelRelation;
    private Label lbRelationId;
    private Label lbRelItemName;
    private Label label1;
    private Button btnCancelRelation;
    private Button btnUpdateRelation;
    private ImageList treeList;
  }
}
