using System;
using System.Collections.Generic;
using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UiToolkit;

namespace NotepadMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class NotepadWindow : Window
{
  private TextField noteField;
  private readonly NotepadDataManager _dataManager;
  private string[] tabsText = new string[5];
  private int activeTabIndex = 0;
  private const int numberOfTabs = 5;

  private List<ButtonText> tabButtons = [];
  public NotepadWindow(NotepadDataManager dataManager) : base(new LocStrFormatted("Notepad"), false)
  {
    _dataManager = dataManager;
    WindowSize(350.px(), 540.px());
    MakeMovable();
    EnablePinning();

    BuildUI();
    LoadNotes();
  }

  private void BuildUI()
  {
    var tabsRow = new Row(2.pt()).AlignItemsCenterMiddle().PaddingBottom(2.pt());
    var tabsLabel = new Label("Tabs".AsLoc());
    for (int i = 0; i < numberOfTabs; i++)
    {
      int index = i;
      var btn = new ButtonText($"{i+1}".AsLoc(), () => SwitchTab(index)).Compact();
      tabButtons.Add(btn);
      tabsRow.Add(btn);
    }

    noteField = new TextField()
      .Multiline(doNotScroll: false, labelOnTop: false)
      .OnValueChanged(_dataManager.Save, isDelayed: true)
      .FocusOnShow()
      .SetTextAreaHeight(320.px())
      .Fill();

    var col = new ScrollColumn();
    col.WidthAuto();
    col.Add(noteField);
    var notesPanel = new PanelWithHeader("Notes".AsLoc()).Margin(1.pt());
    var tabsPanel = new PanelWithHeader("Tabs".AsLoc()).Margin(1.pt());
    tabsPanel.Add(tabsRow);
    notesPanel.BodyAdd(col);
    Body.Add(tabsPanel);
    Body.Add(notesPanel);
  }

  private void SwitchTab(int index)
  {
    for (int i = 0; i < tabButtons.Count; i++)
    {
      tabButtons[i].Color(i == index ? Theme.PositiveColor : null);
    }
  }
  private void LoadNotes()
  {
    string notes  = _dataManager.Load().TextNote;

    if (!string.IsNullOrEmpty(notes))
    {
      noteField.Text(notes.AsLoc());
    }
  }

  [GlobalDependency(RegistrationMode.AsEverything)]
  public class Controller : WindowController<NotepadWindow>
  {
    public Controller(ControllerContext controllerContext) : base(controllerContext)
    {
      controllerContext.UiRoot.AddDependency(this);
      controllerContext.InputManager.RegisterGlobalShortcut(_ => ShortcutMap.Instance.OpenNotepad, this);
    }

    public void Open()
    {
      ActivateSelf();
    } 

    public class ShortcutMap
    {
      public static ShortcutMap Instance {get; } = new();

      [Kb(KbCategory.Tools, "open_notepad", "Open notepad")]
      public KeyBindings OpenNotepad {get; set; } = KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, UnityEngine.KeyCode.LeftControl, UnityEngine.KeyCode.N);
    }
  }
}
