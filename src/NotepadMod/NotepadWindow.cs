using System;
using Mafi;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiToolkit.Library;

namespace NotepadMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class NotepadWindow : Window
{
  public NotepadWindow() : base(new LocStrFormatted("Notepad"), false)
  {
    
  }

  [GlobalDependency(RegistrationMode.AsEverything)]
  public class Controller : WindowController<NotepadWindow>
  {
    public Controller(ControllerContext controllerContext) : base(controllerContext)
    {
      controllerContext.UiRoot.AddDependency(this);
      controllerContext.InputManager.RegisterGlobalShortcut(_ => ShortcutMap.Instance.OpenNotepad, this);
    }

    public void Open() => this.ActivateSelf();

    public class ShortcutMap
    {
      public static ShortcutMap Instance {get; } = new();

      [Kb(KbCategory.Tools, "open_notepad", "Open notepad")]
      public KeyBindings OpenNotepad {get; set; } = KeyBindings.FromPrimaryKeys(KbCategory.Tools, ShortcutMode.Game, UnityEngine.KeyCode.LeftControl, UnityEngine.KeyCode.N);
    }
  }
}
