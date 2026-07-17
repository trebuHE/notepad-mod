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
  private List<NotepadData> notes = [];
  private int activeTabIndex = 0;
  private const int numberOfTabs = 5;

  private List<ButtonText> tabButtons = [];
  public NotepadWindow(NotepadDataManager dataManager) : base(new LocStrFormatted("Notepad"), false)
  {
    _dataManager = dataManager;
    WindowSize(350.px(), 540.px());
    MakeMovable();
    EnablePinning();

    LoadNotes();
    BuildUI();
    SwitchTab(activeTabIndex);
  }

  private void BuildUI()
  {
    var tabsRow = new Row(2.pt()).AlignItemsCenterMiddle().PaddingBottom(2.pt());
    var tabsLabel = new Label("Tabs".AsLoc());
    for (int i = 0; i < numberOfTabs; i++)
    {
      int index = i;
      var btn = new ButtonText($"{i + 1}".AsLoc(), () => SwitchTab(index)).Compact();
      tabButtons.Add(btn);
      tabsRow.Add(btn);
    }

    noteField = new TextField()
      .Multiline(doNotScroll: false, labelOnTop: false)
      .OnValueChanged(newText => { notes[activeTabIndex].TextNote = newText; SaveNotes(); }, isDelayed: true)
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

  private void SaveNotes()
  {
    _dataManager.Save(notes, activeTabIndex);
  }

  private void SwitchTab(int index)
  {
    activeTabIndex = index;

    for (int i = 0; i < tabButtons.Count; i++)
    {
      tabButtons[i].Color(i == index ? Theme.PositiveColor : null);
    }

    noteField.Text(notes[activeTabIndex].TextNote);
  }
  private void LoadNotes()
  {
    (notes, activeTabIndex) = _dataManager.Load();

    notes ??= [];

    while (notes.Count < numberOfTabs)
    {
      notes.Add(new NotepadData(""));
    }
  }

  [GlobalDependency(RegistrationMode.AsEverything)]
  public class Controller : WindowController<NotepadWindow>
  {
    public static Controller Instance { get; private set; }
    public Controller(ControllerContext controllerContext) : base(controllerContext)
    {
      controllerContext.UiRoot.AddDependency(this);
      Instance = this;
    }

    public void ToggleWindow()
    {
      if (IsActive)
      {
        DeactivateSelf();
      }
      else
      {
        ActivateSelf();
      }
    }
  }
}

