using System;
using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.Ui.Library.Inspectors;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UiToolkit.Library;

namespace NotepadMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class NotepadWindow : Window
{
  private TextField noteField;
  private readonly NotepadDataManager _dataManager;
  public NotepadWindow(NotepadDataManager dataManager) : base(new LocStrFormatted("Notepad"), false)
  {
    _dataManager = dataManager;
    WindowSize(350.px(), 500.px());
    MakeMovable();
    EnablePinning();

    BuildUI();
    LoadNotes();
  }

  private void BuildUI()
  {
    noteField = new TextField()
      .Multiline(doNotScroll: false, labelOnTop: false)
      .OnValueChanged(_dataManager.Save, isDelayed: true)
      .FocusOnShow()
      .SetTextAreaHeight(380.px())
      .Fill();

    var col = new ScrollColumn();
    col.Width(330.px());

    col.Add(noteField);
    var panel = new PanelWithHeader("Notes".AsLoc());
    panel.BodyAdd(col);
    Body.Add(panel);
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
