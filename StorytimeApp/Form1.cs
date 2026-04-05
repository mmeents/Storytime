using FastColoredTextBoxNS.Types;
using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Storytime.Core;
using Storytime.Core.Agents;
using Storytime.Core.Constants;
using Storytime.Core.Entities;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Handlers.LmStudio;
using Storytime.Core.Service;
using StorytimeAr.Models;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;


namespace StorytimeAr {
  public partial class Form1 : Form {
    private readonly IServiceScopeFactory _scopeFactory;
    private ILogger<Form1> _logger;
    private readonly IAppDataModuleService _appDataModuleService;
    private Dictionary<int, ItemTypeDto> _itemTypeCache = new Dictionary<int, ItemTypeDto>();
    private Dictionary<int, ItemDto> _itemCache = new Dictionary<int, ItemDto>();

    public Form1(IServiceScopeFactory scopeFactory) {
      _scopeFactory = scopeFactory;
      using var scope = _scopeFactory.CreateScope();
      _logger = scope.ServiceProvider.GetRequiredService<ILogger<Form1>>();
      _appDataModuleService = scope.ServiceProvider.GetRequiredService<IAppDataModuleService>();
      InitializeComponent();
      _logger.LogInformation("Form1 initialized.");
      lbClaudeLaunch.Text = Cx.ClaudeExecutablePath;
      lbLinkExport.Text = Cx.ExportPath;
    }

    #region TreeView Loading and Setup
    private async void Form1_Shown(object sender, EventArgs e) {
      await LoadProjectItems();
      await ReloadSchedule();
    }

    private async Task LoadProjectItems() {

      btnAbortItem.Visible = false;
      btnUpdateItem.Visible = false;
      btnCancelRelation.Visible = false;
      btnUpdateRelation.Visible = false;
      btnAbortExport.Visible = false;
      btnUpdateExport.Visible = false;
      if (btnReloadTree.Visible) btnReloadTree.Visible = false;

      try {
        int? selectedItemId = null;
        bool isSelectedNodeRelation = false;
        if (tvKb.SelectedNode != null) {
          isSelectedNodeRelation = (tvKb.SelectedNode as ItemNode)?.IsRelationNode ?? false;
          selectedItemId = isSelectedNodeRelation ? (tvKb.SelectedNode as ItemNode)?.Relation?.Id : (tvKb.SelectedNode as ItemNode)?.Item?.Id;
        }

        tvKb.Nodes.Clear();
        _itemCache.Clear();

        var items = await _appDataModuleService.GetAllProjectItems();
        foreach (var item in items) {
          _itemCache[item.Id] = item;
          ItemNode itemNode = item.ToItemNode();
          var tnItem = tvKb.Nodes.Add(itemNode);
          if (item.Relations.Count() > 0) {
            foreach (var rel in item.Relations) {
              var relatedItem = await AddNodeById(itemNode, rel.RelatedItemId, rel);
            }

          }
        }


        await LoadItemTypesCache();

        if (selectedItemId.HasValue) {
          TreeNode[] foundNodes = tvKb.Nodes.Find(selectedItemId.Value.ToString(), true);
          if (foundNodes.Length > 0) {
            tvKb.SelectedNode = foundNodes[0];
            tvKb.SelectedNode.Expand();
          }
        }
        ItemTabDirty = false;

        _logger.LogInformation("Loaded {Count} project items.", items.Count);

      } catch (Exception ex) {
        _logger.LogError(ex, "Error loading project items.");
        MessageBox.Show("An error occurred while loading project items. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async Task<ItemNode?> AddNodeById(ItemNode parent, int? itemId, ItemRelationDto? relation = null) {
      if (itemId == null) return null;
      var item = await _appDataModuleService.GetItemById(itemId);
      if (item != null) {
        _itemCache[item.Id] = item;
        ItemNode newNode = relation == null ? item.ToItemNode() : relation.ToItemNode(item);
        parent.Nodes.Add(newNode);
        if (item.Relations.Count() > 0) {
          foreach (var rel in item.Relations) {
            var relatedItem = await AddNodeById(newNode, rel.RelatedItemId, rel);
          }
        }

        return newNode;
      }
      return null;
    }


    private async Task LoadItemTypesCache() {
      try {
        _inSetupTpItems = true;
        var itemTypes = await _appDataModuleService.GetAllItemTypes();
        _itemTypeCache = itemTypes.ToDictionary(t => t.Id, t => t);

        edItemType.DataSource = _itemTypeCache.Values.ToList();
        edItemType.DisplayMember = "Name";
        edItemType.ValueMember = "Id";

      } catch (Exception ex) {
        _logger.LogError(ex, "Error loading item types.");
        MessageBox.Show("An error occurred while loading item types. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      try {
        var RelationTypes = await _appDataModuleService.GetAllRelationTypes();
        cbRelRelation.DataSource = RelationTypes;
        cbRelRelation.DisplayMember = "Relation";
        cbRelRelation.ValueMember = "Id";
      } catch (Exception ex) {
        _logger.LogError(ex, "Error loading relation types.");
        MessageBox.Show("An error occurred while loading relation types. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      _inSetupTpItems = false;

    }

    #endregion

    #region TreeView Selection and Tab Setup
    private ItemNode? _selectedNode = null;

    private void tvKb_AfterSelect(object sender, TreeViewEventArgs e) {
      if (e.Node != null) {
        _selectedNode = e.Node as ItemNode;
        var itemId = _selectedNode?.Item?.Id;
        if (itemId.HasValue && _selectedNode != null) {
          var parentRelation = _selectedNode?.Relation;
          if (parentRelation != null) {
            SetupTpRelations();
          }
          SetupTpItems();

        }
      }
    }

    private bool _ItemTabDirty = false;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ItemTabDirty {
      get { return _ItemTabDirty; }
      set {
        _ItemTabDirty = value;
        btnAbortItem.Visible = value;
        btnUpdateItem.Visible = value;
        btnAbortExport.Visible = value;
        btnUpdateExport.Visible = value;
      }
    }

    private bool _RelationTabDirty = false;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RelationTabDirty {
      get { return _RelationTabDirty; }
      set {
        _RelationTabDirty = value;
        btnCancelRelation.Visible = value;
        btnUpdateRelation.Visible = value;
      }
    }

    private ItemDto? _CurrentItemBackup = null;
    private ItemRelationDto? _CurrentRelationBackup = null;
    private bool _inSetupTpItems = false;
    private bool _inSetupTpRelations = false;
    private void SetupTpItems() {
      if (_selectedNode != null && _selectedNode.Item != null) {
        _inSetupTpItems = true;
        var item = _selectedNode.Item;

        _CurrentItemBackup = _selectedNode.Item.Clone();
        lbItemId.Text = "ItemId: " + _selectedNode.Item.Id.ToString();
        edItemType.DataBindings.Clear();
        edItemType.DataBindings.Add("SelectedValue", _selectedNode.Item, "ItemTypeId", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemName.DataBindings.Clear();
        edItemName.DataBindings.Add("Text", _selectedNode.Item, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemDesc.DataBindings.Clear();
        edItemDesc.DataBindings.Add("Text", _selectedNode.Item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemData.DataBindings.Clear();
        edItemData.DataBindings.Add("Text", _selectedNode.Item, "Data", true, DataSourceUpdateMode.OnPropertyChanged);

        lbExportFilePath.Text = Path.Combine(Cx.ExportPath, $"{item.ItemTypeName}{item.Id}-{item.Name.UrlSafe()}.md");
        lbExportItemName.Text = $"{item.Name}";
        lbScheduleItem.Text = item.Name;
        fclbDescription.DataBindings.Clear();
        fclbDescription.DataBindings.Add("Text", _selectedNode.Item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
        SetupRbItemData();
        _inSetupTpItems = false;
        ItemTabDirty = false;
      }
    }

    private void SetupRbItemData() {
      if (_selectedNode != null && _selectedNode.Item != null) {

        var selectedItemTypeId = _selectedNode.Item.ItemTypeId;
        bool isProperty = false;
        switch (selectedItemTypeId) {
          case (int)StItemType.Project:
            rbStory.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          case (int)StItemType.Story:
            rbScene.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          case (int)StItemType.Scene:
            rbBeat.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          case (int)StItemType.Beat:
            rbCallSheet.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          case (int)StItemType.CallSheet:
            rbPerformance.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          case (int)StItemType.Performance:
            rbDeliverable.Checked = true;
            btnAddToSchedule.Enabled = true;
            break;
          default:
            rbStory.Enabled = false;
            rbStory.Checked = false;
            rbScene.Enabled = false;
            rbScene.Checked = false;
            rbBeat.Enabled = false;
            rbBeat.Checked = false;
            rbCallSheet.Enabled = false;
            rbCallSheet.Checked = false;
            rbPerformance.Enabled = false;
            rbPerformance.Checked = false;
            rbDeliverable.Enabled = false;
            rbDeliverable.Checked = false;
            btnAddToSchedule.Enabled = false;
            isProperty = true;
            break;
        }
        if (!isProperty) {
          rbStory.Enabled = !(selectedItemTypeId >= (int)StItemType.Story);
          rbScene.Enabled = !(selectedItemTypeId >= (int)StItemType.Scene);
          rbBeat.Enabled = !(selectedItemTypeId >= (int)StItemType.Beat);
          rbCallSheet.Enabled = !(selectedItemTypeId >= (int)StItemType.CallSheet);
          rbPerformance.Enabled = !(selectedItemTypeId >= (int)StItemType.Performance);
          rbDeliverable.Enabled = !(selectedItemTypeId >= (int)StItemType.Deliverable);
        }
      } else {
        rbStory.Enabled = false;
        rbScene.Enabled = false;
        rbBeat.Enabled = false;
        rbCallSheet.Enabled = false;
        rbPerformance.Enabled = false;
        btnAddToSchedule.Enabled = false;
      }
    }

    private void SetupTpRelations() {
      _inSetupTpRelations = true;
      if (_selectedNode != null && _selectedNode.Relation != null) {

        _CurrentRelationBackup = _selectedNode.Relation.Clone();
        lbRelationId.Text = "RelationId: " + _selectedNode.Relation.Id.ToString();

        var rank = _selectedNode.Relation.Rank.HasValue ? _selectedNode.Relation.Rank.Value : 0;
        edRank.Value = rank;

        lbRelItemName.DataBindings.Clear();
        lbRelItemName.DataBindings.Add("Text", _selectedNode.Relation, "ItemName", true, DataSourceUpdateMode.OnPropertyChanged);

        if (!cbRelRelation.Enabled) cbRelRelation.Enabled = true;
        cbRelRelation.DataBindings.Clear();
        cbRelRelation.DataBindings.Add("SelectedValue", _selectedNode.Relation, "RelationTypeId", true, DataSourceUpdateMode.OnPropertyChanged);

      } else {
        lbRelationId.Text = "RelationId: N/A";
        lbRelItemName.DataBindings.Clear();

        cbRelRelation.DataBindings.Clear();
        cbRelRelation.Text = "";
        if (cbRelRelation.Enabled) cbRelRelation.Enabled = false;
      }
      _inSetupTpRelations = false;
      RelationTabDirty = false;
    }
    #endregion


    // Context Menu Setup
    private void cmsTreeview_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
      if (_selectedNode == null || _selectedNode.Item == null) {
        miAddProject.Visible = true;
        miAddStory.Visible = false;
        miAddScene.Visible = false;
        miAddBeat.Visible = false;
        miAddCharacter.Visible = false;
        miAddLocation.Visible = false;
        miAddRule.Visible = false;
        miDeleteItem.Visible = false;
        miGenerateStory.Visible = false;
        miGenerateScene.Visible = false;
        miGenerateBeat.Visible = false;
        miGenerateCallSheet.Visible = false;
        miGeneratePerformance.Visible = false;
        miGenerateDeliverable.Visible = false;
        miDuplicateItem.Visible = false;
      } else {
        var selectedItem = _selectedNode.Item;
        var itemTypeName = _itemTypeCache.ContainsKey(selectedItem.ItemTypeId) ? _itemTypeCache[selectedItem.ItemTypeId].Name : "";
        miDuplicateItem.Visible = true;
        miDuplicateItem.Text = $"Duplicate {itemTypeName}";
        miAddProject.Visible = itemTypeName == "Project";
        miAddStory.Visible = itemTypeName == "Project" || itemTypeName == "Story";
        miAddScene.Visible = itemTypeName == "Story";
        miAddBeat.Visible = itemTypeName == "Scene";
        miAddCharacter.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat";
        miAddLocation.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat";
        miAddRule.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat" || itemTypeName == "Character";
        miGenerateStory.Visible = itemTypeName == "Project";
        miGenerateScene.Visible = itemTypeName == "Story";
        miGenerateBeat.Visible = itemTypeName == "Scene";
        miGenerateCallSheet.Visible = itemTypeName == "Scene";
        miGeneratePerformance.Visible = itemTypeName == "CallSheet";
        miGenerateDeliverable.Visible = itemTypeName == "Performance";
        miDeleteItem.Visible = true;

      }
    }

    #region Context Menu Add Item Handlers
    private async void miAddProject_Click(object sender, EventArgs e) {
      if (_selectedNode != null) {
        if (_selectedNode.Item!.ItemTypeId == (int)StItemType.Project) {
          var newsubItem = await _appDataModuleService.CreateItem(new ItemDto {
            Name = "Sub Project",
            Description = "",
            Data = "{}",
            ItemTypeId = (int)StItemType.Project
          });
          if (newsubItem == null) return;
          var relation = await _appDataModuleService.CreateRelation(_selectedNode.Item.Id, newsubItem.Id, (int)StRelationType.Contains);
          if (relation == null) return;
          var relationNode = relation.ToItemNode(newsubItem);
          if (_selectedNode != null) {
            var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
            tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
            tvKb.Refresh();
          }
        } else {

          var newRItem = await _appDataModuleService.CreateItem(new ItemDto {
            Name = "New Project",
            Description = "",
            Data = "{}",
            ItemTypeId = (int)StItemType.Project
          });
          if (newRItem == null) return;
          var indxR = tvKb.Nodes.Add(newRItem.ToItemNode());
          tvKb.SelectedNode = tvKb.Nodes[indxR];
          tvKb.Refresh();
        }
      } else {

        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Project",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Project
        });
        if (newItem == null) return;
        var indx = tvKb.Nodes.Add(newItem.ToItemNode());
        tvKb.SelectedNode = tvKb.Nodes[indx];
        tvKb.Refresh();
      }
    }

    private async void miAddStory_Click(object sender, EventArgs e) {
      if (_selectedNode == null) return;
      var currentProject = _selectedNode?.Item;
      if (currentProject != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Story",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Story
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentProject.Id, newItem.Id, (int)StRelationType.Contains);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddScene_Click(object sender, EventArgs e) {
      var currentStory = _selectedNode?.Item;
      if (currentStory != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Scene",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Scene
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentStory.Id, newItem.Id, (int)StRelationType.Contains);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddBeat_Click(object sender, EventArgs e) {
      var currentScene = _selectedNode?.Item;
      if (currentScene != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Beat",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Beat
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentScene.Id, newItem.Id, (int)StRelationType.Contains);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddCharacter_Click(object sender, EventArgs e) {
      var currentItem = _selectedNode?.Item;
      if (currentItem != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Character",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Character
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentItem.Id, newItem.Id, (int)StRelationType.FeaturesCharacter);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddLocation_Click(object sender, EventArgs e) {
      var currentItem = _selectedNode?.Item;
      if (currentItem != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Location",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Location
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentItem.Id, newItem.Id, (int)StRelationType.TakesPlaceAt);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddRule_Click(object sender, EventArgs e) {
      var currentItem = _selectedNode?.Item;
      if (currentItem != null) {
        var newItem = await _appDataModuleService.CreateItem(new ItemDto {
          Name = "New Rule",
          Description = "",
          Data = "{}",
          ItemTypeId = (int)StItemType.Rule
        });
        if (newItem == null) return;
        var relation = await _appDataModuleService.CreateRelation(currentItem.Id, newItem.Id, (int)StRelationType.UsesRule);
        if (relation == null) return;
        var relationNode = relation.ToItemNode(newItem);
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miDeleteItem_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null) {
        var confirmResult = MessageBox.Show($"Are you sure you want to delete '{_selectedNode.Item.Name}' and all its relations?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult == DialogResult.Yes) {
          var nodeToDelete = _selectedNode;
          tvKb.SelectedNode = _selectedNode.Parent;
          await _appDataModuleService.DeleteItem(nodeToDelete.Item.Id);
          await LoadProjectItems();
        }
      }

    }
    #endregion 

    #region Context Menu Duplicate Item Handlers
    private async void miDuplicateItem_Click(object sender, EventArgs e) {
      try {

        if (_selectedNode != null && _selectedNode.Item != null) {
          var parentNode = _selectedNode.Parent as ItemNode;
          var parentRelation = _selectedNode.Relation;
          var parentNodeItem = parentNode?.Item;
          var nodeItem = _selectedNode.Item;

          if (parentNodeItem == null && nodeItem.ItemTypeId == (int)StItemType.Project) {
            var newProjectItem = await _appDataModuleService.CreateItem(new ItemDto {
              Name = nodeItem.Name,
              Description = nodeItem.Description,
              Data = nodeItem.Data,
              ItemTypeId = (int)StItemType.Project
            });
            if (newProjectItem == null) return;
            var indx = tvKb.Nodes.Add(newProjectItem.ToItemNode());
            tvKb.SelectedNode = tvKb.Nodes[indx];
            tvKb.Refresh();
            return;
          }

          if (parentNode != null && parentNodeItem != null && parentRelation != null) {
            var newOtherItem = await _appDataModuleService.CreateItem(new ItemDto {
              Name = nodeItem.Name,
              Description = nodeItem.Description,
              Data = nodeItem.Data,
              ItemTypeId = nodeItem.ItemTypeId
            });
            if (newOtherItem == null) return;
            var relation = await _appDataModuleService.CreateRelation(parentNodeItem.Id, newOtherItem.Id, parentRelation.RelationTypeId);
            if (relation == null) return;
            var relationNode = relation.ToItemNode(newOtherItem);
            var relNodeIndx = parentNode!.Nodes.Add(relationNode);
            tvKb.SelectedNode = parentNode.Nodes[relNodeIndx];
            tvKb.Refresh();
          }

        }

      } catch (Exception ex) {
        _logger.LogError(ex, "Error duplicating item.");
        MessageBox.Show("An error occurred while duplicating the item. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    #endregion

    #region Context Menu Generate Handlers
    private async void miGenerateStory_Click(object sender, EventArgs e) {

      if (_selectedNode != null && _selectedNode.Item != null) {
        miGenerateStory.Enabled = false;
        var existingcolor = splitContainer2.Panel1.BackColor;
        splitContainer2.Panel1.BackColor = System.Drawing.Color.LightYellow;
        try {
          await _appDataModuleService.GenerateStory(_selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating story.");
          MessageBox.Show("An error occurred while generating the story. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGenerateStory.Enabled = true;
          splitContainer2.Panel1.BackColor = existingcolor;
        }
      }

    }

    private async void miGenerateScene_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null) {
        miGenerateScene.Enabled = false;
        var existingcolor = splitContainer2.Panel1.BackColor;
        splitContainer2.Panel1.BackColor = System.Drawing.Color.LightYellow;
        try {
          await _appDataModuleService.GenerateSceneAndCharacterForStory(_selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating scene.");
          MessageBox.Show("An error occurred while generating the scene. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGenerateScene.Enabled = true;
          splitContainer2.Panel1.BackColor = existingcolor;
        }
      }
    }

    private async void miGenerateBeat_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null) {
        miGenerateBeat.Enabled = false;
        try {
          var storyNode = _selectedNode.FindAncestorOfType(StItemType.Story);
          var storyId = storyNode!.Item!.Id;
          await _appDataModuleService.GenerateBeatsForScene(storyId, _selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating beat.");
          MessageBox.Show("An error occurred while generating the beat. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGenerateBeat.Enabled = true;
        }
      }
    }

    private async void miGenerateCallSheet_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null && _selectedNode.Item.ItemTypeId == (int)StItemType.Scene) {
        miGenerateCallSheet.Enabled = false;
        try {
          var storyNode = _selectedNode.FindAncestorOfType(StItemType.Story);
          var storyId = storyNode!.Item!.Id;
          await _appDataModuleService.GenerateCallSheetForStoryScene(storyId, _selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating call sheet.");
          MessageBox.Show("An error occurred while generating the call sheet. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGenerateCallSheet.Enabled = true;
        }
      }
    }

    private async void miGeneratePerformance_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null && _selectedNode.Item.ItemTypeId == (int)StItemType.CallSheet) {
        miGeneratePerformance.Enabled = false;
        try {
          var storyNode = _selectedNode.FindAncestorOfType(StItemType.Story);
          var storyId = storyNode!.Item!.Id;
          await _appDataModuleService.GeneratePerformanceForCallSheet(storyId, _selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating performance.");
          MessageBox.Show("An error occurred while generating the performance. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGeneratePerformance.Enabled = true;
        }
      }
    }

    private async void miGenerateDeliverable_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null && _selectedNode.Item.ItemTypeId == (int)StItemType.Performance) {
        miGenerateDeliverable.Enabled = false;
        try {
          await _appDataModuleService.GenerateDeliverableForPerformance(_selectedNode.Item.Id);
          btnReloadTree.Visible = true;
        } catch (Exception ex) {
          _logger.LogError(ex, "Error generating deliverable.");
          MessageBox.Show("An error occurred while generating the deliverable. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
          miGenerateDeliverable.Enabled = true;
        }
      }
    }
    #endregion

    #region Settings Tab Handlers

    delegate void LogMessageDelegate(string message);
    private void DoLogMessage(string message) {
      if (this.InvokeRequired) {
        this.Invoke(new LogMessageDelegate(DoLogMessage), new object[] { message });
      } else {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        sb.Append(tbTestOut.Text);
        string finalMessage = sb.ToString();
        if (finalMessage.Length > 10000) {
          finalMessage = finalMessage.Substring(0, 10000); // Keep only the last 10,000 characters
        }
        tbTestOut.Text = finalMessage;
        _logger.LogInformation(message);
      }
    }
    private async void btnGetLmStudioModels_Click(object sender, EventArgs e) {
      var models = await _appDataModuleService.GetLmStudioModels();
      lbLMStudioModels.Items.Clear();
      lbLMStudioModels.Items.AddRange(models.ToArray());
    }

    private void lbLaunchCmd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      // Open cmd in folder containing the .mcp.json file
      try {
        var exportPath = Cx.ClaudeExecutablePath;

        if (exportPath != null) {
          System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
            FileName = "cmd.exe",
            Arguments = $"/K cd /d \"{exportPath}\"", // /d handles drive changes (e.g., C: to D:)
            WorkingDirectory = exportPath,
            UseShellExecute = true
          });
        } else {
          MessageBox.Show("Could not determine the folder path for the export location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

      } catch (Exception ex) {
        _logger.LogError(ex, "Error opening export folder.");
        MessageBox.Show("An error occurred while trying to open the export folder. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void lbClaudeLaunch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      // Open the folder containing the Claude executable settings.  Claude should be configured in your path. 
      try {
        var claudePath = Cx.ClaudeExecutablePath;

        if (claudePath != null) {
          System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
            FileName = claudePath,
            UseShellExecute = true,
            Verb = "open"
          });
        } else {
          MessageBox.Show("Could not determine the folder path for the Claude executable to start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

      } catch (Exception ex) {
        _logger.LogError(ex, "Error opening Claude executable folder.");
        MessageBox.Show("An error occurred while trying to open the Claude executable folder. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void rbLMStudio_CheckedChanged(object sender, EventArgs e) {
      _appDataModuleService.CurrentMode = AgentRunnerMode.LmStudio;
      _appDataModuleService.CurrentLMStudioModel = edLmStudioModel.Text;
      lbCurrentModel.Text = $"Current: LM Studio Model: {_appDataModuleService.CurrentLMStudioModel} ";
    }

    private void rbClaudeCode_CheckedChanged(object sender, EventArgs e) {
      _appDataModuleService.CurrentMode = AgentRunnerMode.ClaudeCode;
      _appDataModuleService.CurrentClaudeModel = cbClaudeModel.Text;
      lbCurrentModel.Text = $"Current: Claude Code; Model: {_appDataModuleService.CurrentClaudeModel};";
    }

    private void cbClaudeModel_TextChanged(object sender, EventArgs e) {
      _appDataModuleService.CurrentClaudeModel = cbClaudeModel.Text;
      lbCurrentModel.Text = $"Current: Claude Code; Model: {_appDataModuleService.CurrentClaudeModel};";
    }

    private void lbLMStudioModels_DoubleClick(object sender, EventArgs e) {
      if (lbLMStudioModels.SelectedItem != null) {
        _appDataModuleService.CurrentLMStudioModel = lbLMStudioModels.SelectedItem.ToString() ?? Cx.LMStudioDefaultModel;
        lbCurrentModel.Text = $"Current: LM Studio Model: {_appDataModuleService.CurrentLMStudioModel} ";
      }
    }

    #endregion

    #region Item Tab Handlers
    private async void btnReloadTree_Click(object sender, EventArgs e) {
      await LoadProjectItems();
    }

    private async void reloadTreeToolStripMenuItem_Click(object sender, EventArgs e) {
      await LoadProjectItems();
    }

    private void edItemName_TextChanged(object sender, EventArgs e) {
      if (!_inSetupTpItems && _selectedNode != null && _selectedNode.Item != null) {
        ItemTabDirty = true;
      }
    }
    private void cbRelRelation_SelectedIndexChanged(object sender, EventArgs e) {
      if (!_inSetupTpRelations && _selectedNode != null && _selectedNode.Relation != null) {
        RelationTabDirty = true;
      }
    }

    private void cbRelItem_SelectedIndexChanged(object sender, EventArgs e) {
      if (!_inSetupTpRelations && _selectedNode != null && _selectedNode.Relation != null) {
        RelationTabDirty = true;
      }
    }

    private void btnUpdateItem_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null) {
        _appDataModuleService.UpdateItem(_selectedNode.Item);
        _selectedNode.Text = _selectedNode.Item.Name;
        SetupTpItems();
      }
    }

    private void btnAbortItem_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null && _CurrentItemBackup != null) {
        _selectedNode.Item.Name = _CurrentItemBackup.Name;
        _selectedNode.Item.Description = _CurrentItemBackup.Description;
        _selectedNode.Item.Data = _CurrentItemBackup.Data;
        _selectedNode.Item.ItemTypeId = _CurrentItemBackup.ItemTypeId;
        SetupTpItems();
      }
    }

    private void btnUpdateRelation_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Relation != null) {
        _selectedNode.Relation.Rank = (int)edRank.Value;
        _appDataModuleService.UpdateItemRelation(_selectedNode.Relation);
        _selectedNode.Text = _selectedNode.Relation.RelationTypeName;
        SetupTpRelations();
      }
    }

    private void btnCancelRelation_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Relation != null && _CurrentRelationBackup != null) {
        _selectedNode.Relation.RelationTypeId = _CurrentRelationBackup.RelationTypeId;
        _selectedNode.Relation.RelatedItemId = _CurrentRelationBackup.RelatedItemId;
        _selectedNode.Relation.Rank = _CurrentRelationBackup.Rank;
        SetupTpRelations();
      }
    }

    private void btnArchive_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Item != null) {
        var confirmResult = MessageBox.Show($"Are you sure you want to archive '{_selectedNode.Item.Name}'? This will remove it from the tree but keep it in the database.", "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult == DialogResult.Yes) {
          _selectedNode.Item.IsActive = false;
          _appDataModuleService.UpdateItem(_selectedNode.Item);
          var newParent = _selectedNode.Parent;
          var archivedNode = _selectedNode;
          tvKb.SelectedNode = newParent;  // this sets the selected node.
          tvKb.Nodes.Remove(archivedNode);
        }
      }
    }

    #endregion

    #region Drag and Drop Handlers
    private void tvKb_ItemDrag(object sender, ItemDragEventArgs e) {
      if (e.Button == MouseButtons.Left && e.Item != null) {
        DoDragDrop(e.Item, DragDropEffects.Move);
      }
    }

    private bool IsNodeDescendant(ItemNode node, ItemNode potentialAncestor) {
      if (node == null || potentialAncestor == null) return false;
      if (node == potentialAncestor) return true;
      var parent = node.Parent as ItemNode;
      while (parent != null) {
        if (parent == potentialAncestor) return true;
        parent = parent.Parent as ItemNode;
      }
      return false;
    }

    private void tvKb_DragDrop(object sender, DragEventArgs e) {
      if (e == null || e.Data == null) return;
      try {
        Point targetPt = tvKb.PointToClient(new Point(e.X, e.Y));
        var targetAt = tvKb.GetNodeAt(targetPt);
        var draggedAt = e.Data.GetData(typeof(ItemNode));
        if (targetAt == null || draggedAt == null) {
          MessageBox.Show("Invalid drag and drop operation. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
        ItemNode? targetNode = (ItemNode)targetAt;
        ItemNode? draggedNode = (ItemNode)draggedAt;
        if (draggedNode != null) {
          if (targetNode != null && targetNode != draggedNode
            && !IsNodeDescendant(draggedNode, targetNode)) {
            if (draggedNode.Relation == null) {
              MessageBox.Show("Only nodes with relations can be moved. Please create a relation for this node before moving.", "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return;
            }
            var newRelation = draggedNode.Relation;
            if (newRelation != null) {
              var aRel = new ItemRelationDto {
                Id = newRelation.Id,
                ItemId = targetNode!.Item!.Id,
                RelatedItemId = newRelation.RelatedItemId,
                RelationTypeId = newRelation.RelationTypeId,
                RelationTypeName = newRelation.RelationTypeName
              };
              _appDataModuleService.UpdateItemRelation(aRel);
              draggedNode.Relation = aRel;
              draggedNode.Parent!.Nodes.Remove(draggedNode);
              var indx = targetNode.Nodes.Add(draggedNode);
              tvKb.SelectedNode = targetNode.Nodes[indx];
              targetNode.Expand();
              tvKb.Refresh();
            }
          } else {
            MessageBox.Show("Invalid move. You cannot move a node to itself or its descendant.", "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }
        }
      } catch (Exception ex) {
        _logger.LogError(ex, "Error during drag and drop operation.");
        MessageBox.Show("An error occurred during the drag and drop operation. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void tvKb_DragEnter(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.Move;
    }

    private void tvKb_DragOver(object sender, DragEventArgs e) {
      if (e.Data != null) {
        Point targetPt = tvKb.PointToClient(new Point(e.X, e.Y));
        var targetAt = tvKb.GetNodeAt(targetPt);
        var draggedAt = e.Data.GetData(typeof(ItemNode));
        if (targetAt != null && draggedAt != null) {
          ItemNode targetNode = (ItemNode)targetAt;
          ItemNode draggedNode = (ItemNode)draggedAt;
          if (targetNode == draggedNode || IsNodeDescendant(draggedNode, targetNode)) {
            e.Effect = DragDropEffects.None;
          } else {
            if (IsValidDrop(targetNode, draggedNode)) {
              e.Effect = DragDropEffects.Move;
            } else {
              e.Effect = DragDropEffects.None;
            }

          }
        } else {
          e.Effect = DragDropEffects.None;
        }
      }
    }

    private bool IsValidDrop(ItemNode targetNode, ItemNode draggedNode) {
      int targetItemTypeId = targetNode.Item?.ItemTypeId ?? 0;
      int draggedItemTypeId = draggedNode.Item?.ItemTypeId ?? 0;

      switch ((StItemType)draggedItemTypeId) {
        case StItemType.Project:
          return (StItemType)targetItemTypeId == StItemType.Project;
        case StItemType.Story:
          return (StItemType)targetItemTypeId == StItemType.Project;
        case StItemType.Scene:
          return (StItemType)targetItemTypeId == StItemType.Story;
        case StItemType.Beat:
          return (StItemType)targetItemTypeId == StItemType.Scene;
        case StItemType.Character:
          return (StItemType)targetItemTypeId == StItemType.Story || (StItemType)targetItemTypeId == StItemType.Scene || (StItemType)targetItemTypeId == StItemType.Beat;
        case StItemType.Location:
          return (StItemType)targetItemTypeId == StItemType.Story || (StItemType)targetItemTypeId == StItemType.Scene || (StItemType)targetItemTypeId == StItemType.Beat;
        case StItemType.Rule:
          return (StItemType)targetItemTypeId == StItemType.Story || (StItemType)targetItemTypeId == StItemType.Scene
            || (StItemType)targetItemTypeId == StItemType.Beat;
        case StItemType.Deliverable:
          return (StItemType)targetItemTypeId == StItemType.Project;
        default:
          return false;
      }
    }
    #endregion

    #region Export Tab and Handlers
    private async void btnExport_Click(object sender, EventArgs e) {
      if (_selectedNode?.Item == null) return;
      var exportPath = Cx.ExportPath;
      if (string.IsNullOrEmpty(exportPath)) {
        MessageBox.Show("Please set an export folder in Cx.cs file.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      var wasCursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        var result = await _appDataModuleService.ExportItem(_selectedNode.Item.Id, cbExportRecurse.Checked, exportPath);
        Cursor.Current = wasCursor;
        if (!result.Success) {
          MessageBox.Show($"Export failed: {result.Message}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        } else {
          MessageBox.Show("Export complete.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      } catch (Exception ex) {
        _logger.LogError(ex, "Export failed.");
        Cursor.Current = wasCursor;
        MessageBox.Show("Export failed. Check logs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }


    private void lbLinkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      // Open the folder containing the Claude executable configurations.  Claude code should be configured in your path.
      try {
        var exportPath = Cx.ExportPath;

        if (exportPath != null) {
          System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
            FileName = exportPath,
            UseShellExecute = true,
            Verb = "open"
          });
        } else {
          MessageBox.Show("Could not determine the folder path for the export location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

      } catch (Exception ex) {
        _logger.LogError(ex, "Error opening export folder.");
        MessageBox.Show("An error occurred while trying to open the export folder. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    // Define some styles
    TextStyle HeaderStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
    TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold);
    TextStyle ItalicStyle = new TextStyle(null, null, FontStyle.Italic);
    TextStyle LinkStyle = new TextStyle(Brushes.DarkCyan, null, FontStyle.Underline);
    TextStyle CodeStyle = new TextStyle(Brushes.Gray, Brushes.GhostWhite, FontStyle.Regular);

    private void fclbDescription_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e) {
      // 1. Clear existing custom styles in the changed range
      e.ChangedRange.ClearStyle(HeaderStyle, BoldStyle, ItalicStyle, LinkStyle, CodeStyle);

      // 2. Apply Header highlighting (e.g., # Header)
      e.ChangedRange.SetStyle(HeaderStyle, @"^#.*$", RegexOptions.Multiline);

      // 3. Apply Bold highlighting (**text**)
      e.ChangedRange.SetStyle(BoldStyle, @"\*\*.*?\*\*");

      // 4. Apply Italic highlighting (*text*)
      e.ChangedRange.SetStyle(ItalicStyle, @"\*.*?\*");

      // 5. Apply Link highlighting ([text](url))
      e.ChangedRange.SetStyle(LinkStyle, @"\[.*?\]\(.*?\)");

      // 6. Apply Inline Code highlighting (`code`)
      e.ChangedRange.SetStyle(CodeStyle, @"`.*?`|(?s)```.*?```");

      if (!_inSetupTpItems && _selectedNode != null && _selectedNode.Item != null) {
        ItemTabDirty = true;
      }
    }

    #endregion

    #region Schedule Tab Handlers

    private async void btnAddToSchedule_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode?.Item != null && rbDeliverable.Enabled) {
        var itemId = _selectedNode.Item.Id;
        var itemCurrentTypeId = _selectedNode.Item.ItemTypeId;
        var itemDestinationTypeId = StItemType.Project;

        if (rbStory.Checked) {
          itemDestinationTypeId = StItemType.Story;
        } else if (rbScene.Checked) {
          itemDestinationTypeId = StItemType.Scene;
        } else if (rbBeat.Checked) {
          itemDestinationTypeId = StItemType.Beat;
        } else if (rbCallSheet.Checked) {
          itemDestinationTypeId = StItemType.CallSheet;
        } else if (rbPerformance.Checked) {
          itemDestinationTypeId = StItemType.Performance;
        } else if (rbDeliverable.Checked) {
          itemDestinationTypeId = StItemType.Deliverable;
        }

        if (itemDestinationTypeId == 0) {
          MessageBox.Show("Faild to translate destination type.");
          return;
        }

        var newQueueItem = await _appDataModuleService.AddToSchedule(itemId, itemDestinationTypeId);
        await ReloadSchedule();

      }
      return;
    }

    private bool _inScheduleReload = false;
    private ConcurrentDictionary<int, AgentQueueItem> _todoCache = new ConcurrentDictionary<int, AgentQueueItem>();
    private async Task ReloadSchedule() {

      var schedule = await _appDataModuleService.GetAgentQueueQuery();
      if (schedule != null) {
        _inScheduleReload = true;
        try {

          foreach (var item in schedule) {
            string listing = "";
            var itemId = item.ItemId;
            if (itemId != 0) {
              listing = $"{item.Id}: {_itemCache[itemId].Name}";
              _todoCache[item.Id] = item;
              var existingId = lbAgentQueue.Items.IndexOf(listing);
              if (existingId == -1) {
                var indx = lbAgentQueue.Items.Add(listing);
              }
            }
          }

          if (schedule.Count > 0) { 
            lbAgentQueue.SelectedIndex = 0;
            btnDeleteQueueItem.Enabled = true;
          } else { 
            btnDeleteQueueItem.Enabled = false;
          }
        } finally {
          _inScheduleReload = false;
        }

      }
    }


    private bool _engineRunning = false;
    private bool _isStopping = false;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool EngineRunning {
      get { return _engineRunning; }
      set {        
        _engineRunning = value;
        if (_engineRunning) {
          lbWorkingStatus.Text = "Status: Pipeline Running";
          btnStartStop.Text = "Stop";
          btnRunNextScheduled.Enabled = false;
        } else {
          lbWorkingStatus.Text = "Status: Pipeline Idle";
          btnStartStop.Text = "Start";
          btnRunNextScheduled.Enabled = true;          
        }
      }
    }

    private void btnStartStop_Click(object sender, EventArgs e) {      
      if (btnStartStop.Text == "Start") {
        EngineRunning = true;
        runTimer.Enabled = true;
      } else {
        runTimer.Enabled = false;
        EngineRunning = false;
      }      
    }

    private bool _isRunningFromTimer = false;
    private async void runTimer_Tick(object sender, EventArgs e) {
      runTimer.Enabled = false;
      _isRunningFromTimer = true;
      try {
        if (btnStartStop.Text == "Stop" ) {
          await RunNextScheduledAsync();
        }
      } finally {
        _isRunningFromTimer = false;
        if (btnStartStop.Text == "Stop" ) {
          runTimer.Enabled = true;
          btnRunNextScheduled.Enabled = false;
        }
      }
    }

    private async void btnRunNextScheduled_Click(object sender, EventArgs e) {
      await RunNextScheduledAsync();
    }

    private async Task RunNextScheduledAsync() {

      if (lbAgentQueue.Items.Count == 0) {
        // this should stop it when queue is empty.
        DoUpdateWorkingMessage("Finished");
        return;
      }
      if (_inScheduleReload ) {
        MessageBox.Show("Schedule is loading, please try again.");
        return;
      }
      if (btnRunNextScheduled.Enabled) btnRunNextScheduled.Enabled = false;     

      string nextQueueItemString = "";
      int nextQueueItemId = 0;
      AgentQueueItem? anextItem = null;
      IMediator? _mediator = null;
      StItemType TargetDepth = StItemType.Project;
      var itemId = 0;
      using var _scope = _scopeFactory.CreateScope();
      _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
      try {

        var lbitem = lbAgentQueue.Items;
        nextQueueItemString = (string)lbitem[0];
        nextQueueItemId = nextQueueItemString.Split(':')[0].AsInt();
        anextItem = _todoCache.Keys.Contains(nextQueueItemId) ? _todoCache[nextQueueItemId] : null;
        if (anextItem == null) {
          MessageBox.Show("didn't find next todo in cache. Maybe restart?");
          return;
        }
        TargetDepth = (StItemType)anextItem.TargetDepth;
        itemId = anextItem.ItemId;
        await _appDataModuleService.UpateAgentQueueItemStatusCommand(anextItem.Id, AgentQueueStatus.Running);
        DoPopOnQueueMessage("");

      } catch (Exception ex1) {
        MessageBox.Show("Error failed to parse next items id.");
        _logger.LogError(ex1, "failed to parse next item");
        return;
      }


      try {
        DoUpdateWorkingMessage($"starting run {nextQueueItemId}");
        if (btnStartStop.Text == "Start" && !_isRunningFromTimer) {
          btnStartStop.Text = "Stop";
        }
        var item = await _mediator.Send(new GetItemByIdQuery(itemId, true));
        if (item == null) return;
        DoPublishProgress("Starting (" + Cx.AsString((StItemType)item.ItemTypeId) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));

        int storyId = 0;
        var Started = DateTime.UtcNow;
        var workingTypeId = item.ItemTypeId;
        var workingId = item.Id;
        var hasBeats = false;

        if (item.ItemTypeId == (int)StItemType.Scene) {
          hasBeats = item.Relations.Any(r => r.RelationTypeId == (int)StRelationType.Contains);
        }

        // Pre-resolve storyId for any item deeper than Story
        if (workingTypeId != (int)StItemType.Project && workingTypeId != (int)StItemType.Story) {
          var aStoryId = await _mediator.Send(new GetAncestorIdByTypeQuery(workingId, StItemType.Story));
          if (aStoryId == null) return;
          storyId = aStoryId.Value;
        }

        // ── Project → Story ──────────────────────────────────────────────────
        if (workingTypeId == (int)StItemType.Project && btnStartStop.Text == "Stop") {
          DoUpdateWorkingMessage($"Running Story for {item.Name}");
          await _mediator.Send(new GenerateStoryCommand(workingId));
          item = await _mediator.Send(new GetItemByIdQuery(itemId, true));
          if (item == null) return;
          var nextItem = item.Relations
            .Where(r => r.RelationTypeId == (int)StRelationType.Contains && r.Established > Started)
            .OrderByDescending(r => r.Established).ToList();
          if (nextItem.Count == 0) return;
          workingId = nextItem[0].RelatedItemId!.Value;
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          DoPublishProgress("Added (" + Cx.AsString((StItemType)item.ItemTypeId) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));
          if (item == null) return;
          workingTypeId = item.ItemTypeId;
        }

        // ── Story → Scene ─────────────────────────────────────────────────────
        if (workingTypeId < (int)TargetDepth && workingTypeId == (int)StItemType.Story && btnStartStop.Text == "Stop") {
          DoUpdateWorkingMessage($"Running Scene for {item.Name}");
          storyId = workingId;
          await _mediator.Send(new GenerateSceneAndCharacterForStoryCommand(workingId));
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          if (item == null) return;
          var nextItem = item.Relations
            .Where(r => r.RelationTypeId == (int)StRelationType.Contains && r.Established > Started)
            .OrderByDescending(r => r.Established).ToList();
          if (nextItem.Count == 0) return;
          workingId = nextItem[0].RelatedItemId!.Value;  // sceneId
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          DoPublishProgress("Added (" + Cx.AsString((StItemType)item.ItemTypeId) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));
          if (item == null) return;
          workingTypeId = item.ItemTypeId; // Scene        
        }

        // ── Scene → Beats ─────────────────────────────────────────────────────
        if (workingTypeId < (int)TargetDepth && workingTypeId == (int)StItemType.Scene && btnStartStop.Text == "Stop") {
          DoUpdateWorkingMessage($"Running Beats for {item.Name}");
          if (!hasBeats) {
            await _mediator.Send(new GenerateBeatsForSceneCommand(storyId, workingId));
          }
          // advance regardless — menu entry is always on scene, beats live under it
          workingTypeId = (int)StItemType.Beat;
        }

        // ── Beats → CallSheet ─────────────────────────────────────────────────
        if (workingTypeId < (int)TargetDepth && workingTypeId == (int)StItemType.Beat && btnStartStop.Text == "Stop") {
          workingTypeId = (int)StItemType.CallSheet;
          DoUpdateWorkingMessage($"Running Director for {item.Name}");
          await _mediator.Send(new GenerateCallSheetCommand(storyId, workingId));
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          if (item == null) return;
          var nextItem = item.Relations
            .Where(r => r.RelationTypeId == (int)StRelationType.DirectedAs && r.Established > Started)
            .OrderByDescending(r => r.Established).ToList();
          if (nextItem.Count == 0) return;
          workingId = nextItem[0].RelatedItemId!.Value; // callSheetId
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          if (item == null) return;
          DoPublishProgress("Added (" + Cx.AsString((StItemType)item.ItemTypeId) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));
          workingTypeId = item.ItemTypeId; // CallSheet
        }

        // ── CallSheet → Performance ───────────────────────────────────────────
        if (workingTypeId < (int)TargetDepth && workingTypeId == (int)StItemType.CallSheet && btnStartStop.Text == "Stop") {
          DoUpdateWorkingMessage($"Running Performance for {item.Name}");
          await _mediator.Send(new GeneratePerformanceForCallSheetCommand(storyId, workingId));
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          if (item == null) return;
          var nextItem = item.Relations
            .Where(r => r.RelationTypeId == (int)StRelationType.Produces && r.Established > Started)
            .OrderByDescending(r => r.Established).ToList();
          if (nextItem.Count == 0) return;
          workingId = nextItem[0].RelatedItemId!.Value; // performanceId
          item = await _mediator.Send(new GetItemByIdQuery(workingId, true));
          if (item == null) return;
          DoPublishProgress("Added (" + Cx.AsString((StItemType)item.ItemTypeId) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));
          workingTypeId = item.ItemTypeId; // Performance
        }

        // ── Performance → Deliverable ─────────────────────────────────────────
        if (workingTypeId < (int)TargetDepth && workingTypeId == (int)StItemType.Performance && btnStartStop.Text == "Stop") {
          DoUpdateWorkingMessage($"Running Deliverables for {item.Name}");
          await _mediator.Send(new GenerateDeliverableCommand(workingId));
          DoPublishProgress("Added (" + Cx.AsString(TargetDepth) + "):" + nextQueueItemString + " destination being " + Cx.AsString(TargetDepth));
        }

        await _appDataModuleService.UpateAgentQueueItemStatusCommand(anextItem.Id, AgentQueueStatus.Completed);
      } catch (Exception ex) {

        _logger.LogError(ex, "Error running scheduled item.");
        if (anextItem != null) {
          await _appDataModuleService.UpateAgentQueueItemStatusCommand(anextItem.Id, AgentQueueStatus.Failed);
        }

      } finally {       
        DoUpdateWorkingMessage("Finished");
        DoPublishProgress($"Run {nextQueueItemId} Completed.");
      }

    }


    private void DoUpdateWorkingMessage(string message) {
      if (this.InvokeRequired) {
        this.Invoke(new LogMessageDelegate(DoUpdateWorkingMessage), new object[] { message });
      } else {
        if (message == "Finished" && (!_isRunningFromTimer || lbAgentQueue.Items.Count == 0)) {
          EngineRunning = false;
        }        
        lbRunItemName.Text = message;
        if (!btnReloadTree.Visible) btnReloadTree.Visible = true;
      }
    }

    private void DoPublishProgress(string message) {
      if (this.InvokeRequired) {
        this.Invoke(new LogMessageDelegate(DoPublishProgress), new object[] { message });
      } else {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        sb.Append(edRunLogOut.Text);
        string finalMessage = sb.ToString();
        if (finalMessage.Length > 10000) {
          finalMessage = finalMessage.Substring(0, 10000); // Keep only the last 10,000 characters
        }
        edRunLogOut.Text = finalMessage;
      }
    }

    private void DoPopOnQueueMessage(string message) {
      if (this.InvokeRequired) {
        this.Invoke(new LogMessageDelegate(DoPopOnQueueMessage), new object[] { message });
      } else {
        lbAgentQueue.Items.RemoveAt(0);
      }
    }

    private async void btnDeleteQueueItem_Click(object sender, EventArgs e) {
      var indx = lbAgentQueue.SelectedIndex;
      if (indx == -1) {
        MessageBox.Show("Please select an item to delete.");
        return;
      }
      var lbitem = lbAgentQueue.Items;
      var nextQueueItemString = (string)lbitem[indx];
      var nextQueueItemId = nextQueueItemString.Split(':')[0].AsInt();
      var confirmResult = MessageBox.Show($"Are you sure you want to delete queue item '{nextQueueItemString}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
      if (confirmResult == DialogResult.Yes) { 
        await _appDataModuleService.UpateAgentQueueItemStatusCommand(nextQueueItemId, AgentQueueStatus.Cancelled);
        lbAgentQueue.Items.RemoveAt(indx);
      }
    }

    #endregion

    #region Form resize and layout fixes.
    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
      if (tabControl1.SelectedTab == tpExport) {
        Form1_Resize(sender, e);
      }
    }

    private bool _inResize = false;
    private void Form1_Resize(object sender, EventArgs e) {
      if (_inResize) return;
      _inResize = true;
      fclbDescription.Height = tabControl1.Height - lbExportItemName.Top - (lbExportItemName.Height * 3);

      var sumHorizontal = lbItemName.Width + lbItemName.Left + edItemName.Width + 8;
      if (sumHorizontal < tabControl1.Width - 10 || sumHorizontal > tabControl1.Width) {
        int newEditWidth = tabControl1.Width - lbItemName.Width - lbItemName.Left - 16;
        edItemName.Width = newEditWidth;
        edItemType.Width = newEditWidth;
        edItemDesc.Width = newEditWidth;
        edItemData.Width = newEditWidth;
      }
      _inResize = false;
    }
    #endregion


  }
}