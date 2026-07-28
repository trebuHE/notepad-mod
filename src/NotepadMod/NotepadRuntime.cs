using UnityEngine;

namespace NotepadMod;

internal static class NotepadRuntime
{
  private static GameObject s_gameObject;
  private static NotepadKeybindPoller s_behaviour;

  public static void Start()
  {
    if (s_gameObject != null) return;

    s_gameObject = new GameObject("NotepadMod_Runtime");
    Object.DontDestroyOnLoad(s_gameObject);

    s_behaviour = s_gameObject.AddComponent<NotepadKeybindPoller>();
  }
}

public class NotepadKeybindPoller : MonoBehaviour
{
  private void Update()
  {
    if (NotepadKeybinds.IsPressed("NotepadMod_ToggleWindow", "LeftControl + N"))
    {
      NotepadWindow.Controller.Instance?.ToggleWindow();
    }

    if (NotepadWindow.Controller.Instance?.IsActive == true)
    {
      if (NotepadKeybinds.IsPressed("NotepadMod_Tab1", "LeftControl + Alpha1")) NotepadWindow.Instance.SwitchTab(0);
      if (NotepadKeybinds.IsPressed("NotepadMod_Tab2", "LeftControl + Alpha2")) NotepadWindow.Instance.SwitchTab(1);
      if (NotepadKeybinds.IsPressed("NotepadMod_Tab3", "LeftControl + Alpha3")) NotepadWindow.Instance.SwitchTab(2);
      if (NotepadKeybinds.IsPressed("NotepadMod_Tab4", "LeftControl + Alpha4")) NotepadWindow.Instance.SwitchTab(3);
      if (NotepadKeybinds.IsPressed("NotepadMod_Tab5", "LeftControl + Alpha5")) NotepadWindow.Instance.SwitchTab(4);
    }
  }
}