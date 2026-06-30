using System;
using System.IO;
using Mafi;

namespace NotepadMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class NotepadDataManager
{
  private string SaveFilePath => Path.Combine(Environment.CurrentDirectory, "notes.txt");

  public void Save(NotepadData data)
  {
    try
    {
      File.WriteAllText(SaveFilePath, data.TextNote);
    } 
    catch(Exception e)
    {
      Log.Warning($"[NotepadMod] Failed to save data: {e.Message}");  
    }
  }

  public NotepadData Load()
  {
    if(!File.Exists(SaveFilePath)) return new NotepadData("");

    try
    {
      NotepadData data = new(File.ReadAllText(SaveFilePath));
      return data;
    }
    catch(Exception e)
    {
      Log.Warning($"[NotepadMod] Failed to load data: {e.Message}");
      return new NotepadData("");
    }
  }
}

