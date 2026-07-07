using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mafi;

namespace NotepadMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class NotepadDataManager
{
  private const string delimiter = "\n---NOTE_TAB_SPLIT---\n";
  private string SaveDir => Path.Combine(Environment.CurrentDirectory, "NotepadMod");
  private string SaveFilePath => Path.Combine(SaveDir, "notes.txt");

  public void Save(List<NotepadData> notes, int activeTab)
  {
    string data = "";
    data += activeTab + delimiter;

    foreach (NotepadData note in notes)
    {
      data += note.TextNote + delimiter;
    }

    try
    {
      if (!Directory.Exists(SaveDir)) 
        Directory.CreateDirectory(SaveDir);

      File.WriteAllText(SaveFilePath, data);
    } 
    catch(Exception e)
    {
      Log.Error($"[NotepadMod] Failed to save data: {e.Message}");  
    }
  }

  public (List<NotepadData>, int activeIndex) Load()
  {
    int active = 0;
    if(!File.Exists(SaveFilePath)) return ([], active);

    try
    {
      List<NotepadData> notes = [];
      string data = File.ReadAllText(SaveFilePath);
      string[] split = data.Split([delimiter], StringSplitOptions.None);

      active = int.TryParse(split[0], out int idx) ? idx : 0;
      split[0] = "";
      foreach (string s in split)
      {
        if(s != "")
        {
          notes.Add(new NotepadData(s));
        }
      }
      return (notes, active);
    }
    catch(Exception e)
    {
      Log.Error($"[NotepadMod] Failed to load data: {e.Message}");
      return ([], active);
    }
  }
}

