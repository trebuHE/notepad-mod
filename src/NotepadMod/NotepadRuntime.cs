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
  }
}