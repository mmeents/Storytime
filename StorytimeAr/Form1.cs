using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Storytime.Core;
using Storytime.Core.Service;
using StorytimeAr.Models;
using System.ComponentModel;
using System.ComponentModel.Design;
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
      if (tabControl1.TabPages.Contains(tpItems)) {
        tabControl1.TabPages.Remove(tpItems);
      }
      if (tabControl1.TabPages.Contains(tpRelations)) {
        tabControl1.TabPages.Remove(tpRelations);
      }
    }

    private async void LoadProjectItems() {

      btnAbortItem.Visible = false;
      btnUpdateItem.Visible = false;
      btnCancelRelation.Visible = false;
      btnUpdateRelation.Visible = false;

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
              ItemNode relationNode = new ItemNode {
                Name = rel.Id.ToString(),
                Text = rel.RelationTypeName,
                Relation = rel,
                IsRelationNode = true
              };
              var tnRelations = itemNode.Nodes.Add(relationNode);
              var relatedItem = await AddNodeById(relationNode, rel.RelatedItemId);
            }

          }
        }


        LoadItemTypesCache();

        if (selectedItemId.HasValue) {          
          TreeNode[] foundNodes = tvKb.Nodes.Find(selectedItemId.Value.ToString(), true);
          if (foundNodes.Length > 0) {
            tvKb.SelectedNode = foundNodes[0];
            tvKb.SelectedNode.Expand();
          }
        }

        _logger.LogInformation("Loaded {Count} project items.", items.Count);

      } catch (Exception ex) {
        _logger.LogError(ex, "Error loading project items.");
        MessageBox.Show("An error occurred while loading project items. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async void LoadItemTypesCache() {
      try {
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

      try {
        var items = _itemCache.Values.ToList();
        cbRelItem.DataSource = items;
        cbRelItem.DisplayMember = "Name";
        cbRelItem.ValueMember = "Id";
      } catch (Exception ex) {
        _logger.LogError(ex, "Error loading items for relation dropdown.");
        MessageBox.Show("An error occurred while loading items for relation dropdown. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    private void LoadItemsByRelationFromCache(StRelationType relationType) {
      List<StItemType> allowedItemTypes = relationType switch {
        StRelationType.Contains => new List<StItemType> { StItemType.Story, StItemType.Scene, StItemType.Beat },
        StRelationType.HasBeat => new List<StItemType> { StItemType.Beat },
        StRelationType.NextBeat => new List<StItemType> { StItemType.Beat },
        StRelationType.UsesRule => new List<StItemType> { StItemType.Rule },
        StRelationType.FeaturesCharacter => new List<StItemType> { StItemType.Character },
        StRelationType.TakesPlaceAt => new List<StItemType> { StItemType.Location },
        _ => new List<StItemType>()
      };
      var stories = _itemCache.Values.Where(i => allowedItemTypes.Contains((StItemType)i.ItemTypeId)).ToList();
      cbRelItem.DataSource = stories;
      cbRelItem.DisplayMember = "Name";
      cbRelItem.ValueMember = "Id";
    }

    private async Task<ItemNode?> AddNodeById(ItemNode parent, int? itemId) {
      if (itemId == null) return null;
      var item = await _appDataModuleService.GetItemById(itemId);
      if (item != null) {
        _itemCache[item.Id] = item;
        ItemNode newNode = item.ToItemNode();
        parent.Nodes.Add(newNode);

        if (item.Relations.Count() > 0) {
          foreach (var rel in item.Relations) {
            ItemNode relationNode = new ItemNode {
              Name = rel.Id.ToString(),
              Text = rel.RelationTypeName,
              Relation = rel,
              IsRelationNode = true
            };
            var tnRelations = newNode.Nodes.Add(relationNode);
            var relatedItem = await AddNodeById(relationNode, rel.RelatedItemId);
          }

        }

        return newNode;
      }
      return null;
    }


    private void Form1_Shown(object sender, EventArgs e) {
      LoadProjectItems();

    }

    private void cmsTreeview_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
      if (_selectedNode == null) {
        e.Cancel = true; // No node selected, cancel the context menu
        return;
      } else {
        if (_selectedNode.IsRelationNode) {
          miAddProject.Visible = false;
          miAddStory.Visible = false;
          miAddScene.Visible = false;
          miAddBeat.Visible = false;
          miAddCharacter.Visible = false;
          miAddLocation.Visible = false;
          miAddRule.Visible = false;
          miAddRefCharacter.Visible = false;
        } else {
          if (_selectedNode.Item != null) {
            var itemTypeName = _itemTypeCache.ContainsKey(_selectedNode.Item.ItemTypeId) ? _itemTypeCache[_selectedNode.Item.ItemTypeId].Name : "";
            miAddProject.Visible = itemTypeName == "Project";
            miAddStory.Visible = itemTypeName == "Project";
            miAddScene.Visible = itemTypeName == "Story";
            miAddBeat.Visible = itemTypeName == "Scene";
            miAddCharacter.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat";
            miAddRefCharacter.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat";
            miAddLocation.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat";
            miAddRule.Visible = itemTypeName == "Story" || itemTypeName == "Scene" || itemTypeName == "Beat" || itemTypeName == "Character";
          } else {
            miAddProject.Visible = true;
            miAddStory.Visible = false;
            miAddScene.Visible = false;
            miAddBeat.Visible = false;
            miAddCharacter.Visible = false;
            miAddLocation.Visible = false;
            miAddRule.Visible = false;
            miAddRefCharacter.Visible = false;
          }
        }

      }
    }

    private ItemNode? _selectedNode = null;

    private void tvKb_AfterSelect(object sender, TreeViewEventArgs e) {
      if (e.Node != null) {
        _selectedNode = e.Node as ItemNode;
        var itemId = (_selectedNode?.IsRelationNode ?? false) ? _selectedNode?.Relation?.Id : _selectedNode?.Item?.Id;
        if (itemId.HasValue && _selectedNode != null) {
          if (_selectedNode.IsRelationNode) {
            if (tabControl1.TabPages.Contains(tpItems)) {
              tabControl1.TabPages.Remove(tpItems);
            }
            if (!tabControl1.TabPages.Contains(tpRelations)) {
              tabControl1.TabPages.Add(tpRelations);
            }
            tabControl1.SelectedTab = tpRelations;
            StRelationType relationType = (StRelationType)(_selectedNode?.Relation?.RelationTypeId ?? 1);
            LoadItemsByRelationFromCache(relationType);
            SetupTpRelations();
          } else {
            if (!tabControl1.TabPages.Contains(tpItems)) {
              tabControl1.TabPages.Add(tpItems);
            }
            SetupTpItems();
            tabControl1.SelectedTab = tpItems;
            if (tabControl1.TabPages.Contains(tpRelations)) {
              tabControl1.TabPages.Remove(tpRelations);
            }
          }
        } else {
          if (tabControl1.TabPages.Contains(tpItems)) {
            tabControl1.TabPages.Remove(tpItems);
          }
          if (tabControl1.TabPages.Contains(tpRelations)) {
            tabControl1.TabPages.Remove(tpRelations);
          }
          tabControl1.SelectedTab = tpBrowse;
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
        _CurrentItemBackup = _selectedNode.Item.Clone();
        edItemType.DataBindings.Clear();
        edItemType.DataBindings.Add("SelectedValue", _selectedNode.Item, "ItemTypeId", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemName.DataBindings.Clear();
        edItemName.DataBindings.Add("Text", _selectedNode.Item, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemDesc.DataBindings.Clear();
        edItemDesc.DataBindings.Add("Text", _selectedNode.Item, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
        edItemData.DataBindings.Clear();
        edItemData.DataBindings.Add("Text", _selectedNode.Item, "Data", true, DataSourceUpdateMode.OnPropertyChanged);
        _inSetupTpItems = false;
        ItemTabDirty = false;
      }
    }

    private void SetupTpRelations() {
      if (_selectedNode != null && _selectedNode.Relation != null) {
        _inSetupTpRelations = true;
        _CurrentRelationBackup = _selectedNode.Relation.Clone();
        lbRelItemName.DataBindings.Clear();
        lbRelItemName.DataBindings.Add("Text", _selectedNode.Relation, "ItemName", true, DataSourceUpdateMode.OnPropertyChanged);

        cbRelRelation.DataBindings.Clear();
        cbRelRelation.DataBindings.Add("SelectedValue", _selectedNode.Relation, "RelationTypeId", true, DataSourceUpdateMode.OnPropertyChanged);

        cbRelItem.DataBindings.Clear();
        cbRelItem.DataBindings.Add("SelectedValue", _selectedNode.Relation, "RelatedItemId", true, DataSourceUpdateMode.OnPropertyChanged);

        _inSetupTpRelations = false;
        RelationTabDirty = false;
      }
    }

    private void reloadTreeToolStripMenuItem_Click(object sender, EventArgs e) {
      if (_selectedNode != null) {
        LoadProjectItems();

      } else {
        LoadProjectItems();
      }
    }

    private void edItemName_TextChanged(object sender, EventArgs e) {
      if (!_inSetupTpItems && _selectedNode != null && _selectedNode.Item != null) {
        ItemTabDirty = true;
      }
    }
    private void cbRelRelation_SelectedIndexChanged(object sender, EventArgs e) {
      if (!_inSetupTpRelations && _selectedNode != null && _selectedNode.Relation != null) {
        RelationTabDirty = true;
        LoadItemsByRelationFromCache((StRelationType)_selectedNode.Relation.RelationTypeId);
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
        _appDataModuleService.UpdateItemRelation(_selectedNode.Relation);
        _selectedNode.Text = _selectedNode.Relation.RelationTypeName;
        SetupTpRelations();
      }
    }

    private void btnCancelRelation_Click(object sender, EventArgs e) {
      if (_selectedNode != null && _selectedNode.Relation != null && _CurrentRelationBackup != null) {
        _selectedNode.Relation.RelationTypeId = _CurrentRelationBackup.RelationTypeId;
        _selectedNode.Relation.RelatedItemId = _CurrentRelationBackup.RelatedItemId;
        SetupTpRelations();
      }
    }

    private async void miAddProject_Click(object sender, EventArgs e) {
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var storyNode = newItem.ToItemNode();
          var storyIndx = relationNode.Nodes.Add(storyNode);
          tvKb.SelectedNode = relationNode.Nodes[storyIndx];
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var sceneNode = newItem.ToItemNode();
          var sceneIndx = relationNode.Nodes.Add(sceneNode);
          tvKb.SelectedNode = relationNode.Nodes[sceneIndx];
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var beatNode = newItem.ToItemNode();
          var beatIndx = relationNode.Nodes.Add(beatNode);
          tvKb.SelectedNode = relationNode.Nodes[beatIndx];
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var characterNode = newItem.ToItemNode();
          var characterIndx = relationNode.Nodes.Add(characterNode);
          tvKb.SelectedNode = relationNode.Nodes[characterIndx];
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var locationNode = newItem.ToItemNode();
          var locationIndx = relationNode.Nodes.Add(locationNode);
          tvKb.SelectedNode = relationNode.Nodes[locationIndx];
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
        var relationNode = relation.ToItemNode();
        if (_selectedNode != null) {
          var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
          var ruleNode = newItem.ToItemNode();
          var ruleIndx = relationNode.Nodes.Add(ruleNode);
          tvKb.SelectedNode = relationNode.Nodes[ruleIndx];
          tvKb.Refresh();
        }
      }
    }

    private async void miAddRefCharacter_Click(object sender, EventArgs e) {
      try {
        var currentItem = _selectedNode?.Item;
        if (currentItem != null) {
          var relation = await _appDataModuleService.CreateRelation(currentItem.Id, null, (int)StRelationType.FeaturesCharacter);
          if (relation == null) return;
          var relationNode = relation.ToItemNode();
          if (_selectedNode?.Item != null) {
            var relNodeIndx = _selectedNode.Nodes.Add(relationNode);
            tvKb.SelectedNode = _selectedNode.Nodes[relNodeIndx];
            tvKb.Refresh();
          }
        }
      } catch (Exception ex) {
        _logger.LogError(ex, "Error adding reference character relation.");
        MessageBox.Show("An error occurred while adding reference character relation. Please check the logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    
  }



}
