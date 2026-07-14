using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NotepadMod
{
  internal static class NotepadKeybinds
  {
    private const string MOD_ID = "NotepadMod";
    private const string DISPLAY = "Notepad Mod";
    private const string SEP = "~|~";

    // id, label, type, default combo, gesture hint, group, tooltip
    private static readonly string[][] Descriptors =
    [
      ["NotepadMod_ToggleWindow", "Toggle notepad window", "Discrete", "LeftControl + N", "", "Notepad", "Opens and closes notepad window"]
    ];

    private static MethodInfo s_getCombo;
    private static bool s_initialized;

    public static void Register()
    {
      if (s_initialized) return;
      s_initialized = true;
      try
      {
        Type apiType = null;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
          apiType = asm.GetType("KeybindFramework.KeybindFrameworkApi");
          if (apiType != null) break;
        }
        if (apiType == null) return;

        s_getCombo = apiType.GetMethod("GetCombo", new[] { typeof(string), typeof(string) });
        var reg = apiType.GetMethod("RegisterRaw", new[] { typeof(string), typeof(string), typeof(string[]) });
        if (reg == null) return;

        var rows = new List& lt; string> ();
        foreach (var d in Descriptors)
          rows.Add(string.Join(SEP, new[] { d[0], d[1], d[2], d[3], d[4], d[5], "", d[6] }));
        reg.Invoke(null, new object[] { MOD_ID, DISPLAY, rows.ToArray() });
      }
      catch { }
    }

    private static string ComboFor(string id, string fallbackDefault)
    {
      try
      {
        if (s_getCombo != null)
        {
          var c = s_getCombo.Invoke(null, new object[] { MOD_ID, id }) as string;
          if (!string.IsNullOrEmpty(c)) return c;
        }
      }
      catch { }
      return fallbackDefault;
    }

    private static bool IsModifierKey(KeyCode k)
    {
      return k switch
      {
        KeyCode.LeftControl or KeyCode.RightControl or KeyCode.LeftShift or KeyCode.RightShift or KeyCode.LeftAlt or KeyCode.RightAlt => true,
        _ => false,
      };
    }

    private static bool ModifiersHeldExactly(List&lt; KeyCode> mods)
        {
            bool wantCtrl = false, wantShift = false, wantAlt = false;
            foreach (var m in mods)
            {
                if (m == KeyCode.LeftControl || m == KeyCode.RightControl) wantCtrl = true;
                if (m == KeyCode.LeftShift || m == KeyCode.RightShift) wantShift = true;
                if (m == KeyCode.LeftAlt || m == KeyCode.RightAlt) wantAlt = true;
            }
            bool hasCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    bool hasShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    bool hasAlt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            return wantCtrl == hasCtrl && wantShift == hasShift && wantAlt == hasAlt;
        }

public static bool IsPressed(string id, string defaultCombo)
{
  if (AppDomain.CurrentDomain.GetData("MoriPP_KeybindCapturingFrame") is int cf && Time.frameCount - cf <= 1)
    return false;

  if (!s_initialized) Register();

  string combo = ComboFor(id, defaultCombo);
  KeyCode mainKey = KeyCode.None;
  var mods = new List& lt; KeyCode > ();
  foreach (var token in combo.Split('+'))
  {
    var t = token.Trim();
    if (t.Length == 0 || t == "None") continue;
    if (!Enum.TryParse & lt; KeyCode > (t, out var key)) continue;
    if (IsModifierKey(key)) mods.Add(key); else mainKey = key;
  }
  if (mainKey == KeyCode.None) return false;
  if (!ModifiersHeldExactly(mods)) return false;
  return Input.GetKeyDown(mainKey);
}
    }
}