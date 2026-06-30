using System;
using Mafi;
using Mafi.Collections;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Core.Mods;

namespace NotepadMod;

public sealed class NotepadMod : IMod
{
  public NotepadMod(ModManifest manifest)
  {
    Manifest = manifest;
    Log.Info("[NotepadMod] Contructed!");
  }
  public string Name => "Notepad Mod";
  public int Version => 1;
  public bool IsUiOnly => true;
  public ModManifest Manifest {get; }
  public ModJsonConfig JsonConfig => new ModJsonConfig(this);
  
  [Obsolete]
  public Option<IConfig> ModConfig => default;

  public void RegisterPrototypes(ProtoRegistrator registrator)
  {

  }
  public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded)
  {

  }

  public void EarlyInit(DependencyResolver resolver) 
  {
    
  }

  public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
  {
    
  }

  public void MigrateJsonConfig(VersionSlim savedVersion, Dict<string, object> savedValues) 
  {

  }

  public void Dispose() 
  {

  }
}
