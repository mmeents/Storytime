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
      miAddLocation = new ToolStripMenuItem();
      miAddRule = new ToolStripMenuItem();
      miDuplicateItem = new ToolStripMenuItem();
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
      lbLaunchCmd = new LinkLabel();
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
      btnArchive = new Button();
      btnCancelRelation = new Button();
      btnUpdateRelation = new Button();
      label7 = new Label();
      edRank = new NumericUpDown();
      label4 = new Label();
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
      tpSchedule = new TabPage();
      btnDeleteQueueItem = new Button();
      edRunLogOut = new TextBox();
      lbRunItemName = new Label();
      lbWorkingStatus = new Label();
      btnRunNextScheduled = new Button();
      btnStartStop = new Button();
      btnAddToSchedule = new Button();
      lbAgentQueue = new ListBox();
      label5 = new Label();
      tabControl2 = new TabControl();
      tpScheduleJob = new TabPage();
      rbDeliverable = new RadioButton();
      rbPerformance = new RadioButton();
      rbCallSheet = new RadioButton();
      rbBeat = new RadioButton();
      rbScene = new RadioButton();
      rbStory = new RadioButton();
      lbScheduleItem = new Label();
      tpExport = new TabPage();
      btnAbortExport = new Button();
      btnUpdateExport = new Button();
      lbExportItemName = new Label();
      lbExportFilePath = new Label();
      lbLinkExport = new LinkLabel();
      cbExportRecurse = new CheckBox();
      btnExport = new Button();
      fclbDescription = new FastColoredTextBoxNS.FastColoredTextBox();
      runTimer = new System.Windows.Forms.Timer(components);
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
      tpSchedule.SuspendLayout();
      tabControl2.SuspendLayout();
      tpScheduleJob.SuspendLayout();
      tpExport.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)fclbDescription).BeginInit();
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
      splitContainer1.Size = new Size(813, 538);
      splitContainer1.SplitterDistance = 270;
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
      splitContainer2.Size = new Size(270, 538);
      splitContainer2.SplitterDistance = 63;
      splitContainer2.TabIndex = 0;
      // 
      // btnReloadTree
      // 
      btnReloadTree.Location = new Point(12, 12);
      btnReloadTree.Name = "btnReloadTree";
      btnReloadTree.Size = new Size(180, 41);
      btnReloadTree.TabIndex = 0;
      btnReloadTree.Text = "Agents are done Reload";
      btnReloadTree.UseVisualStyleBackColor = true;
      btnReloadTree.Click += btnReloadTree_Click;
      // 
      // tvKb
      // 
      tvKb.AllowDrop = true;
      tvKb.ContextMenuStrip = cmsTreeview;
      tvKb.Dock = DockStyle.Fill;
      tvKb.ImageIndex = 0;
      tvKb.ImageList = treeList;
      tvKb.Location = new Point(0, 0);
      tvKb.Name = "tvKb";
      tvKb.SelectedImageIndex = 0;
      tvKb.Size = new Size(270, 471);
      tvKb.TabIndex = 0;
      tvKb.ItemDrag += tvKb_ItemDrag;
      tvKb.AfterSelect += tvKb_AfterSelect;
      tvKb.DragDrop += tvKb_DragDrop;
      tvKb.DragEnter += tvKb_DragEnter;
      tvKb.DragOver += tvKb_DragOver;
      // 
      // cmsTreeview
      // 
      cmsTreeview.Items.AddRange(new ToolStripItem[] { reloadTreeToolStripMenuItem, toolStripSeparator1, miAddProject, miAddStory, miAddScene, miAddBeat, miAddCharacter, miAddLocation, miAddRule, miDuplicateItem, toolStripSeparator2, miGenerateStory, miGenerateScene, miGenerateBeat, miGenerateCallSheet, miGeneratePerformance, miGenerateDeliverable, toolStripSeparator3, miDeleteItem });
      cmsTreeview.Name = "cmsTreeview";
      cmsTreeview.Size = new Size(193, 374);
      cmsTreeview.Opening += cmsTreeview_Opening;
      // 
      // reloadTreeToolStripMenuItem
      // 
      reloadTreeToolStripMenuItem.Name = "reloadTreeToolStripMenuItem";
      reloadTreeToolStripMenuItem.Size = new Size(192, 22);
      reloadTreeToolStripMenuItem.Text = "Reload Tree";
      reloadTreeToolStripMenuItem.Click += reloadTreeToolStripMenuItem_Click;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(189, 6);
      // 
      // miAddProject
      // 
      miAddProject.Name = "miAddProject";
      miAddProject.Size = new Size(192, 22);
      miAddProject.Text = "Add Project";
      miAddProject.Click += miAddProject_Click;
      // 
      // miAddStory
      // 
      miAddStory.Name = "miAddStory";
      miAddStory.Size = new Size(192, 22);
      miAddStory.Text = "Add Story";
      miAddStory.Click += miAddStory_Click;
      // 
      // miAddScene
      // 
      miAddScene.Name = "miAddScene";
      miAddScene.Size = new Size(192, 22);
      miAddScene.Text = "Add Scene";
      miAddScene.Click += miAddScene_Click;
      // 
      // miAddBeat
      // 
      miAddBeat.Name = "miAddBeat";
      miAddBeat.Size = new Size(192, 22);
      miAddBeat.Text = "Add Beat";
      miAddBeat.Click += miAddBeat_Click;
      // 
      // miAddCharacter
      // 
      miAddCharacter.Name = "miAddCharacter";
      miAddCharacter.Size = new Size(192, 22);
      miAddCharacter.Text = "Add Character";
      miAddCharacter.Click += miAddCharacter_Click;
      // 
      // miAddLocation
      // 
      miAddLocation.Name = "miAddLocation";
      miAddLocation.Size = new Size(192, 22);
      miAddLocation.Text = "Add Location";
      miAddLocation.Click += miAddLocation_Click;
      // 
      // miAddRule
      // 
      miAddRule.Name = "miAddRule";
      miAddRule.Size = new Size(192, 22);
      miAddRule.Text = "Add Rule";
      miAddRule.Click += miAddRule_Click;
      // 
      // miDuplicateItem
      // 
      miDuplicateItem.Name = "miDuplicateItem";
      miDuplicateItem.Size = new Size(192, 22);
      miDuplicateItem.Text = "Duplicate Item";
      miDuplicateItem.Click += miDuplicateItem_Click;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new Size(189, 6);
      // 
      // miGenerateStory
      // 
      miGenerateStory.Name = "miGenerateStory";
      miGenerateStory.Size = new Size(192, 22);
      miGenerateStory.Text = "Generate Story";
      miGenerateStory.Click += miGenerateStory_Click;
      // 
      // miGenerateScene
      // 
      miGenerateScene.Name = "miGenerateScene";
      miGenerateScene.Size = new Size(192, 22);
      miGenerateScene.Text = "Generate Scene";
      miGenerateScene.Click += miGenerateScene_Click;
      // 
      // miGenerateBeat
      // 
      miGenerateBeat.Name = "miGenerateBeat";
      miGenerateBeat.Size = new Size(192, 22);
      miGenerateBeat.Text = "Generate Beat Set";
      miGenerateBeat.Click += miGenerateBeat_Click;
      // 
      // miGenerateCallSheet
      // 
      miGenerateCallSheet.Name = "miGenerateCallSheet";
      miGenerateCallSheet.Size = new Size(192, 22);
      miGenerateCallSheet.Text = "Generate CallSheet";
      miGenerateCallSheet.Click += miGenerateCallSheet_Click;
      // 
      // miGeneratePerformance
      // 
      miGeneratePerformance.Name = "miGeneratePerformance";
      miGeneratePerformance.Size = new Size(192, 22);
      miGeneratePerformance.Text = "Generate Performance";
      miGeneratePerformance.Click += miGeneratePerformance_Click;
      // 
      // miGenerateDeliverable
      // 
      miGenerateDeliverable.Name = "miGenerateDeliverable";
      miGenerateDeliverable.Size = new Size(192, 22);
      miGenerateDeliverable.Text = "Generate Deliverable";
      miGenerateDeliverable.Click += miGenerateDeliverable_Click;
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new Size(189, 6);
      // 
      // miDeleteItem
      // 
      miDeleteItem.Name = "miDeleteItem";
      miDeleteItem.Size = new Size(192, 22);
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
      tabControl1.Controls.Add(tpSchedule);
      tabControl1.Controls.Add(tpExport);
      tabControl1.Dock = DockStyle.Fill;
      tabControl1.Location = new Point(0, 0);
      tabControl1.Name = "tabControl1";
      tabControl1.SelectedIndex = 0;
      tabControl1.Size = new Size(539, 538);
      tabControl1.TabIndex = 0;
      tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
      // 
      // tpBrowse
      // 
      tpBrowse.Controls.Add(lbLaunchCmd);
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
      tpBrowse.Size = new Size(531, 510);
      tpBrowse.TabIndex = 2;
      tpBrowse.Text = "Settings";
      tpBrowse.UseVisualStyleBackColor = true;
      // 
      // lbLaunchCmd
      // 
      lbLaunchCmd.AutoSize = true;
      lbLaunchCmd.Location = new Point(149, 22);
      lbLaunchCmd.Name = "lbLaunchCmd";
      lbLaunchCmd.Size = new Size(31, 15);
      lbLaunchCmd.TabIndex = 12;
      lbLaunchCmd.TabStop = true;
      lbLaunchCmd.Text = "cmd";
      lbLaunchCmd.LinkClicked += lbLaunchCmd_LinkClicked;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(23, 374);
      label6.Name = "label6";
      label6.Size = new Size(52, 15);
      label6.TabIndex = 11;
      label6.Text = "Error log";
      // 
      // tbTestOut
      // 
      tbTestOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tbTestOut.Location = new Point(19, 392);
      tbTestOut.Multiline = true;
      tbTestOut.Name = "tbTestOut";
      tbTestOut.Size = new Size(504, 110);
      tbTestOut.TabIndex = 10;
      // 
      // lbLMStudioModels
      // 
      lbLMStudioModels.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      lbLMStudioModels.FormattingEnabled = true;
      lbLMStudioModels.Location = new Point(149, 122);
      lbLMStudioModels.Name = "lbLMStudioModels";
      lbLMStudioModels.Size = new Size(368, 64);
      lbLMStudioModels.TabIndex = 9;
      lbLMStudioModels.DoubleClick += lbLMStudioModels_DoubleClick;
      // 
      // cbClaudeModel
      // 
      cbClaudeModel.FormattingEnabled = true;
      cbClaudeModel.Items.AddRange(new object[] { "sonnet", "opus" });
      cbClaudeModel.Location = new Point(149, 252);
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
      lbClaudeLaunch.Location = new Point(228, 22);
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
      lbCurrentModel.Size = new Size(108, 15);
      lbCurrentModel.TabIndex = 3;
      lbCurrentModel.Text = " LLM Model to use:";
      // 
      // edLmStudioModel
      // 
      edLmStudioModel.Location = new Point(149, 93);
      edLmStudioModel.Name = "edLmStudioModel";
      edLmStudioModel.Size = new Size(368, 23);
      edLmStudioModel.TabIndex = 2;
      edLmStudioModel.Text = "nvidia/nemotron-3-nano-4b";
      // 
      // btnGetLmStudioModels
      // 
      btnGetLmStudioModels.Location = new Point(58, 140);
      btnGetLmStudioModels.Name = "btnGetLmStudioModels";
      btnGetLmStudioModels.Size = new Size(85, 23);
      btnGetLmStudioModels.TabIndex = 1;
      btnGetLmStudioModels.Text = "Get Models";
      btnGetLmStudioModels.UseVisualStyleBackColor = true;
      btnGetLmStudioModels.Click += btnGetLmStudioModels_Click;
      // 
      // tpItems
      // 
      tpItems.Controls.Add(btnArchive);
      tpItems.Controls.Add(btnCancelRelation);
      tpItems.Controls.Add(btnUpdateRelation);
      tpItems.Controls.Add(label7);
      tpItems.Controls.Add(edRank);
      tpItems.Controls.Add(label4);
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
      tpItems.Size = new Size(531, 510);
      tpItems.TabIndex = 0;
      tpItems.Text = "Items";
      tpItems.UseVisualStyleBackColor = true;
      // 
      // btnArchive
      // 
      btnArchive.Location = new Point(351, 131);
      btnArchive.Name = "btnArchive";
      btnArchive.Size = new Size(75, 23);
      btnArchive.TabIndex = 23;
      btnArchive.Text = "Archive";
      btnArchive.UseVisualStyleBackColor = true;
      btnArchive.Click += btnArchive_Click;
      // 
      // btnCancelRelation
      // 
      btnCancelRelation.Location = new Point(270, 62);
      btnCancelRelation.Name = "btnCancelRelation";
      btnCancelRelation.Size = new Size(75, 23);
      btnCancelRelation.TabIndex = 22;
      btnCancelRelation.Text = "Cancel";
      btnCancelRelation.UseVisualStyleBackColor = true;
      // 
      // btnUpdateRelation
      // 
      btnUpdateRelation.Location = new Point(189, 61);
      btnUpdateRelation.Name = "btnUpdateRelation";
      btnUpdateRelation.Size = new Size(75, 23);
      btnUpdateRelation.TabIndex = 21;
      btnUpdateRelation.Text = "Update";
      btnUpdateRelation.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(6, 63);
      label7.Name = "label7";
      label7.Size = new Size(66, 15);
      label7.TabIndex = 20;
      label7.Text = "Rank Order";
      // 
      // edRank
      // 
      edRank.Location = new Point(83, 61);
      edRank.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      edRank.Name = "edRank";
      edRank.Size = new Size(100, 23);
      edRank.TabIndex = 19;
      edRank.ValueChanged += cbRelItem_SelectedIndexChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(22, 93);
      label4.Name = "label4";
      label4.Size = new Size(50, 15);
      label4.TabIndex = 17;
      label4.Text = "Relation";
      // 
      // cbRelRelation
      // 
      cbRelRelation.FormattingEnabled = true;
      cbRelRelation.Location = new Point(83, 90);
      cbRelRelation.Name = "cbRelRelation";
      cbRelRelation.Size = new Size(100, 23);
      cbRelRelation.TabIndex = 15;
      cbRelRelation.SelectedIndexChanged += cbRelRelation_SelectedIndexChanged;
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
      lbRelItemName.Location = new Point(83, 38);
      lbRelItemName.Name = "lbRelItemName";
      lbRelItemName.Size = new Size(66, 15);
      lbRelItemName.TabIndex = 13;
      lbRelItemName.Text = "Item Name";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(29, 38);
      label1.Name = "label1";
      label1.Size = new Size(41, 15);
      label1.TabIndex = 12;
      label1.Text = "Parent";
      // 
      // lbItemId
      // 
      lbItemId.AutoSize = true;
      lbItemId.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbItemId.Location = new Point(21, 133);
      lbItemId.Name = "lbItemId";
      lbItemId.Size = new Size(68, 21);
      lbItemId.TabIndex = 10;
      lbItemId.Text = "ItemId: x";
      // 
      // btnAbortItem
      // 
      btnAbortItem.Location = new Point(270, 131);
      btnAbortItem.Name = "btnAbortItem";
      btnAbortItem.Size = new Size(75, 23);
      btnAbortItem.TabIndex = 9;
      btnAbortItem.Text = "Abort";
      btnAbortItem.UseVisualStyleBackColor = true;
      btnAbortItem.Click += btnAbortItem_Click;
      // 
      // btnUpdateItem
      // 
      btnUpdateItem.Location = new Point(189, 131);
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
      label2.Location = new Point(0, 222);
      label2.Name = "label2";
      label2.Size = new Size(67, 15);
      label2.TabIndex = 6;
      label2.Text = "Description";
      // 
      // lbType
      // 
      lbType.AutoSize = true;
      lbType.Location = new Point(41, 195);
      lbType.Name = "lbType";
      lbType.Size = new Size(31, 15);
      lbType.TabIndex = 5;
      lbType.Text = "Type";
      // 
      // lbItemName
      // 
      lbItemName.AutoSize = true;
      lbItemName.Location = new Point(33, 166);
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
      edItemData.Size = new Size(447, 163);
      edItemData.TabIndex = 3;
      edItemData.TextChanged += edItemName_TextChanged;
      // 
      // edItemDesc
      // 
      edItemDesc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edItemDesc.Location = new Point(76, 222);
      edItemDesc.Multiline = true;
      edItemDesc.Name = "edItemDesc";
      edItemDesc.ScrollBars = ScrollBars.Both;
      edItemDesc.Size = new Size(447, 113);
      edItemDesc.TabIndex = 2;
      edItemDesc.TextChanged += edItemName_TextChanged;
      // 
      // edItemType
      // 
      edItemType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemType.FormattingEnabled = true;
      edItemType.Location = new Point(77, 192);
      edItemType.Name = "edItemType";
      edItemType.Size = new Size(447, 23);
      edItemType.TabIndex = 1;
      edItemType.TextChanged += edItemName_TextChanged;
      // 
      // edItemName
      // 
      edItemName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      edItemName.Location = new Point(77, 163);
      edItemName.Name = "edItemName";
      edItemName.Size = new Size(447, 23);
      edItemName.TabIndex = 0;
      edItemName.TextChanged += edItemName_TextChanged;
      // 
      // tpSchedule
      // 
      tpSchedule.Controls.Add(btnDeleteQueueItem);
      tpSchedule.Controls.Add(edRunLogOut);
      tpSchedule.Controls.Add(lbRunItemName);
      tpSchedule.Controls.Add(lbWorkingStatus);
      tpSchedule.Controls.Add(btnRunNextScheduled);
      tpSchedule.Controls.Add(btnStartStop);
      tpSchedule.Controls.Add(btnAddToSchedule);
      tpSchedule.Controls.Add(lbAgentQueue);
      tpSchedule.Controls.Add(label5);
      tpSchedule.Controls.Add(tabControl2);
      tpSchedule.Location = new Point(4, 24);
      tpSchedule.Name = "tpSchedule";
      tpSchedule.Size = new Size(531, 510);
      tpSchedule.TabIndex = 3;
      tpSchedule.Text = "Schedule";
      tpSchedule.UseVisualStyleBackColor = true;
      // 
      // btnDeleteQueueItem
      // 
      btnDeleteQueueItem.Location = new Point(122, 140);
      btnDeleteQueueItem.Name = "btnDeleteQueueItem";
      btnDeleteQueueItem.Size = new Size(61, 23);
      btnDeleteQueueItem.TabIndex = 9;
      btnDeleteQueueItem.Text = "Delete";
      btnDeleteQueueItem.UseVisualStyleBackColor = true;
      btnDeleteQueueItem.Click += btnDeleteQueueItem_Click;
      // 
      // edRunLogOut
      // 
      edRunLogOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      edRunLogOut.Location = new Point(219, 169);
      edRunLogOut.Multiline = true;
      edRunLogOut.Name = "edRunLogOut";
      edRunLogOut.Size = new Size(300, 334);
      edRunLogOut.TabIndex = 8;
      // 
      // lbRunItemName
      // 
      lbRunItemName.AutoSize = true;
      lbRunItemName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbRunItemName.Location = new Point(204, 116);
      lbRunItemName.Name = "lbRunItemName";
      lbRunItemName.Size = new Size(98, 21);
      lbRunItemName.TabIndex = 7;
      lbRunItemName.Text = "Running: NA";
      // 
      // lbWorkingStatus
      // 
      lbWorkingStatus.AutoSize = true;
      lbWorkingStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbWorkingStatus.Location = new Point(221, 140);
      lbWorkingStatus.Name = "lbWorkingStatus";
      lbWorkingStatus.Size = new Size(112, 21);
      lbWorkingStatus.TabIndex = 6;
      lbWorkingStatus.Text = "Status: Waiting";
      // 
      // btnRunNextScheduled
      // 
      btnRunNextScheduled.Location = new Point(55, 140);
      btnRunNextScheduled.Name = "btnRunNextScheduled";
      btnRunNextScheduled.Size = new Size(61, 23);
      btnRunNextScheduled.TabIndex = 5;
      btnRunNextScheduled.Text = "Do Next";
      btnRunNextScheduled.UseVisualStyleBackColor = true;
      btnRunNextScheduled.Click += btnRunNextScheduled_Click;
      // 
      // btnStartStop
      // 
      btnStartStop.Location = new Point(6, 140);
      btnStartStop.Name = "btnStartStop";
      btnStartStop.Size = new Size(43, 23);
      btnStartStop.TabIndex = 4;
      btnStartStop.Text = "Start";
      btnStartStop.UseVisualStyleBackColor = true;
      btnStartStop.Click += btnStartStop_Click;
      // 
      // btnAddToSchedule
      // 
      btnAddToSchedule.Location = new Point(13, 44);
      btnAddToSchedule.Name = "btnAddToSchedule";
      btnAddToSchedule.Size = new Size(75, 23);
      btnAddToSchedule.TabIndex = 3;
      btnAddToSchedule.Text = "Add";
      btnAddToSchedule.UseVisualStyleBackColor = true;
      btnAddToSchedule.Click += btnAddToSchedule_Click;
      // 
      // lbAgentQueue
      // 
      lbAgentQueue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      lbAgentQueue.FormattingEnabled = true;
      lbAgentQueue.Location = new Point(6, 169);
      lbAgentQueue.Name = "lbAgentQueue";
      lbAgentQueue.Size = new Size(209, 334);
      lbAgentQueue.TabIndex = 2;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Font = new Font("Segoe UI", 12F);
      label5.Location = new Point(15, 14);
      label5.Name = "label5";
      label5.Size = new Size(73, 21);
      label5.TabIndex = 1;
      label5.Text = "Schedule";
      // 
      // tabControl2
      // 
      tabControl2.Alignment = TabAlignment.Bottom;
      tabControl2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      tabControl2.Controls.Add(tpScheduleJob);
      tabControl2.Location = new Point(94, 3);
      tabControl2.Name = "tabControl2";
      tabControl2.SelectedIndex = 0;
      tabControl2.Size = new Size(431, 115);
      tabControl2.TabIndex = 0;
      // 
      // tpScheduleJob
      // 
      tpScheduleJob.Controls.Add(rbDeliverable);
      tpScheduleJob.Controls.Add(rbPerformance);
      tpScheduleJob.Controls.Add(rbCallSheet);
      tpScheduleJob.Controls.Add(rbBeat);
      tpScheduleJob.Controls.Add(rbScene);
      tpScheduleJob.Controls.Add(rbStory);
      tpScheduleJob.Controls.Add(lbScheduleItem);
      tpScheduleJob.Location = new Point(4, 4);
      tpScheduleJob.Name = "tpScheduleJob";
      tpScheduleJob.Padding = new Padding(3);
      tpScheduleJob.Size = new Size(423, 87);
      tpScheduleJob.TabIndex = 0;
      tpScheduleJob.Text = "Generation";
      tpScheduleJob.UseVisualStyleBackColor = true;
      // 
      // rbDeliverable
      // 
      rbDeliverable.AutoSize = true;
      rbDeliverable.Location = new Point(223, 62);
      rbDeliverable.Name = "rbDeliverable";
      rbDeliverable.Size = new Size(83, 19);
      rbDeliverable.TabIndex = 6;
      rbDeliverable.TabStop = true;
      rbDeliverable.Tag = "shedule";
      rbDeliverable.Text = "Deliverable";
      rbDeliverable.UseVisualStyleBackColor = true;
      // 
      // rbPerformance
      // 
      rbPerformance.AutoSize = true;
      rbPerformance.Location = new Point(174, 41);
      rbPerformance.Name = "rbPerformance";
      rbPerformance.Size = new Size(93, 19);
      rbPerformance.TabIndex = 5;
      rbPerformance.TabStop = true;
      rbPerformance.Tag = "shedule";
      rbPerformance.Text = "Performance";
      rbPerformance.UseVisualStyleBackColor = true;
      // 
      // rbCallSheet
      // 
      rbCallSheet.AutoSize = true;
      rbCallSheet.Location = new Point(123, 62);
      rbCallSheet.Name = "rbCallSheet";
      rbCallSheet.Size = new Size(74, 19);
      rbCallSheet.TabIndex = 4;
      rbCallSheet.TabStop = true;
      rbCallSheet.Tag = "shedule";
      rbCallSheet.Text = "CallSheet";
      rbCallSheet.UseVisualStyleBackColor = true;
      // 
      // rbBeat
      // 
      rbBeat.AutoSize = true;
      rbBeat.Location = new Point(86, 41);
      rbBeat.Name = "rbBeat";
      rbBeat.Size = new Size(48, 19);
      rbBeat.TabIndex = 3;
      rbBeat.TabStop = true;
      rbBeat.Tag = "shedule";
      rbBeat.Text = "Beat";
      rbBeat.UseVisualStyleBackColor = true;
      // 
      // rbScene
      // 
      rbScene.AutoSize = true;
      rbScene.Location = new Point(42, 62);
      rbScene.Name = "rbScene";
      rbScene.Size = new Size(56, 19);
      rbScene.TabIndex = 2;
      rbScene.TabStop = true;
      rbScene.Tag = "shedule";
      rbScene.Text = "Scene";
      rbScene.UseVisualStyleBackColor = true;
      // 
      // rbStory
      // 
      rbStory.AutoSize = true;
      rbStory.Location = new Point(8, 41);
      rbStory.Name = "rbStory";
      rbStory.Size = new Size(52, 19);
      rbStory.TabIndex = 1;
      rbStory.TabStop = true;
      rbStory.Tag = "shedule";
      rbStory.Text = "Story";
      rbStory.UseVisualStyleBackColor = true;
      // 
      // lbScheduleItem
      // 
      lbScheduleItem.AutoSize = true;
      lbScheduleItem.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbScheduleItem.Location = new Point(8, 7);
      lbScheduleItem.Name = "lbScheduleItem";
      lbScheduleItem.Size = new Size(126, 21);
      lbScheduleItem.TabIndex = 0;
      lbScheduleItem.Text = "Item to Schedule";
      // 
      // tpExport
      // 
      tpExport.Controls.Add(btnAbortExport);
      tpExport.Controls.Add(btnUpdateExport);
      tpExport.Controls.Add(lbExportItemName);
      tpExport.Controls.Add(lbExportFilePath);
      tpExport.Controls.Add(lbLinkExport);
      tpExport.Controls.Add(cbExportRecurse);
      tpExport.Controls.Add(btnExport);
      tpExport.Controls.Add(fclbDescription);
      tpExport.Location = new Point(4, 24);
      tpExport.Name = "tpExport";
      tpExport.Padding = new Padding(3);
      tpExport.Size = new Size(531, 510);
      tpExport.TabIndex = 1;
      tpExport.Text = "Export";
      tpExport.UseVisualStyleBackColor = true;
      // 
      // btnAbortExport
      // 
      btnAbortExport.Location = new Point(86, 6);
      btnAbortExport.Name = "btnAbortExport";
      btnAbortExport.Size = new Size(75, 23);
      btnAbortExport.TabIndex = 32;
      btnAbortExport.Text = "Abort";
      btnAbortExport.UseVisualStyleBackColor = true;
      btnAbortExport.Click += btnAbortItem_Click;
      // 
      // btnUpdateExport
      // 
      btnUpdateExport.Location = new Point(5, 6);
      btnUpdateExport.Name = "btnUpdateExport";
      btnUpdateExport.Size = new Size(75, 23);
      btnUpdateExport.TabIndex = 31;
      btnUpdateExport.Text = "Update";
      btnUpdateExport.UseVisualStyleBackColor = true;
      btnUpdateExport.Click += btnUpdateItem_Click;
      // 
      // lbExportItemName
      // 
      lbExportItemName.AutoSize = true;
      lbExportItemName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      lbExportItemName.Location = new Point(28, 61);
      lbExportItemName.Name = "lbExportItemName";
      lbExportItemName.Size = new Size(52, 21);
      lbExportItemName.TabIndex = 30;
      lbExportItemName.Text = "label9";
      // 
      // lbExportFilePath
      // 
      lbExportFilePath.AutoSize = true;
      lbExportFilePath.Location = new Point(7, 32);
      lbExportFilePath.Name = "lbExportFilePath";
      lbExportFilePath.Size = new Size(93, 15);
      lbExportFilePath.TabIndex = 29;
      lbExportFilePath.Text = "lbExportFilePath";
      // 
      // lbLinkExport
      // 
      lbLinkExport.AutoSize = true;
      lbLinkExport.Location = new Point(375, 10);
      lbLinkExport.Name = "lbLinkExport";
      lbLinkExport.Size = new Size(60, 15);
      lbLinkExport.TabIndex = 28;
      lbLinkExport.TabStop = true;
      lbLinkExport.Text = "linkLabel1";
      lbLinkExport.LinkClicked += lbLinkExport_LinkClicked;
      // 
      // cbExportRecurse
      // 
      cbExportRecurse.AutoSize = true;
      cbExportRecurse.Location = new Point(261, 9);
      cbExportRecurse.Name = "cbExportRecurse";
      cbExportRecurse.Size = new Size(108, 19);
      cbExportRecurse.TabIndex = 27;
      cbExportRecurse.Text = "Children items?";
      cbExportRecurse.UseVisualStyleBackColor = true;
      // 
      // btnExport
      // 
      btnExport.Location = new Point(167, 6);
      btnExport.Name = "btnExport";
      btnExport.Size = new Size(88, 23);
      btnExport.TabIndex = 26;
      btnExport.Text = "Export Item";
      btnExport.UseVisualStyleBackColor = true;
      btnExport.Click += btnExport_Click;
      // 
      // fclbDescription
      // 
      fclbDescription.AutoCompleteBracketsList = new char[]
  {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
  };
      fclbDescription.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
      fclbDescription.AutoScrollMinSize = new Size(0, 14);
      fclbDescription.BackBrush = null;
      fclbDescription.BorderStyle = BorderStyle.FixedSingle;
      fclbDescription.CharHeight = 14;
      fclbDescription.CharWidth = 8;
      fclbDescription.DefaultMarkerSize = 8;
      fclbDescription.DisabledColor = Color.FromArgb(100, 180, 180, 180);
      fclbDescription.Dock = DockStyle.Bottom;
      fclbDescription.FindForm = null;
      fclbDescription.Font = new Font("Courier New", 9.75F);
      fclbDescription.GoToForm = null;
      fclbDescription.Hotkeys = resources.GetString("fclbDescription.Hotkeys");
      fclbDescription.IsReplaceMode = false;
      fclbDescription.Location = new Point(3, 89);
      fclbDescription.Name = "fclbDescription";
      fclbDescription.Paddings = new Padding(0);
      fclbDescription.ReplaceForm = null;
      fclbDescription.SelectionColor = Color.FromArgb(60, 0, 0, 255);
      fclbDescription.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("fclbDescription.ServiceColors");
      fclbDescription.Size = new Size(525, 418);
      fclbDescription.TabIndex = 0;
      fclbDescription.WordWrap = true;
      fclbDescription.Zoom = 100;
      fclbDescription.TextChanged += fclbDescription_TextChanged;
      // 
      // runTimer
      // 
      runTimer.Interval = 860;
      runTimer.Tick += runTimer_Tick;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(813, 538);
      Controls.Add(splitContainer1);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "Form1";
      Text = "Storytime";
      Shown += Form1_Shown;
      Resize += Form1_Resize;
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
      tpSchedule.ResumeLayout(false);
      tpSchedule.PerformLayout();
      tabControl2.ResumeLayout(false);
      tpScheduleJob.ResumeLayout(false);
      tpScheduleJob.PerformLayout();
      tpExport.ResumeLayout(false);
      tpExport.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)fclbDescription).EndInit();
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
    private TabPage tpExport;
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
    private Label label4;
    private ComboBox cbRelRelation;
    private Label lbRelationId;
    private Label lbRelItemName;
    private Label label1;
    private Button btnCancelRelation;
    private Button btnUpdateRelation;
    private ImageList treeList;
    private ToolStripMenuItem miDuplicateItem;
    private FastColoredTextBoxNS.FastColoredTextBox fclbDescription;
    private LinkLabel lbLinkExport;
    private CheckBox cbExportRecurse;
    private Button btnExport;
    private Label lbExportItemName;
    private Label lbExportFilePath;
    private Button btnAbortExport;
    private Button btnUpdateExport;
    private LinkLabel lbLaunchCmd;
    private Button btnArchive;
    private TabPage tpSchedule;
    private TabControl tabControl2;
    private TabPage tpScheduleJob;
    private Label label5;
    private RadioButton rbPerformance;
    private RadioButton rbCallSheet;
    private RadioButton rbBeat;
    private RadioButton rbScene;
    private RadioButton rbStory;
    private Label lbScheduleItem;
    private ListBox lbAgentQueue;
    private Button btnRunNextScheduled;
    private Button btnStartStop;
    private Button btnAddToSchedule;
    private TextBox edRunLogOut;
    private Label lbRunItemName;
    private Label lbWorkingStatus;
    private RadioButton rbDeliverable;
    private System.Windows.Forms.Timer runTimer;
    private Button btnDeleteQueueItem;
  }
}
