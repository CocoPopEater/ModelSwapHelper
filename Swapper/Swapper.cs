using MelonLoader;
using ModelSwapLib.Swapper.Modules;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModelSwapLib.Swapper;

public class Swapper
{
    public string ModName { get; set; }
    public string SwapperName { get; set; } = "Default Name";
    public List<string> ObjectNames { get; set; }
    public string BundleName { get; set; }
    public List<IModule> Modules { get; set; } = new();
    
    public List<string> Deactivations { get; set; }
    
    public Guid SwapperGuid { get; internal set; }

    public void RunAllModules()
    {
        var bundle = BundleManager.GetInstance().GetBundle(BundleName);
        if (bundle == null)
        {
            MelonLogger.Error($"Failed to load AssetBundle from Mod: {ModName}\nSwapper Name: {SwapperName}");
            return;
        }
        List<GameObject> objects = new();
        foreach (string name in ObjectNames)
        {
            objects.AddRange(Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(go => go.name == name));
        }
        if(objects.Count == 0) return;
        
        foreach (IModule module in Modules)
        {
            switch (module)
            {
                case IAssetModule assetModule:
                    assetModule.ApplyAll(objects, bundle);
                    break;
                case ITransformModule transformModule:
                    transformModule.ApplyAll(objects);
                    break;
            }
        }

        // Handle Deactivations
        if (Deactivations == null || Deactivations.Count == 0) return; // There are no objects to deactivate, just return
        
        foreach (string name in Deactivations)
        {
            Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(go => go.name == name)
                .ToList()
                .ForEach(obj =>
                {
                    if (obj.activeSelf) obj.SetActive(false);
                });
        }
    }

    internal Guid GenerateSwapperGuid()
    {
        if (SwapperGuid == Guid.Empty)
        {
            SwapperGuid = Guid.NewGuid();
        }
        return SwapperGuid;
    }
    
    public bool Validate()
    {
        if(string.IsNullOrEmpty(ModName)) return false;
        if(ObjectNames == null || ObjectNames.Count == 0) return false;
        if(string.IsNullOrEmpty(BundleName)) return false;
        if(Modules == null || Modules.Count == 0) return false;
        
        if(!BundleName.EndsWith(".bundle")) BundleName = string.Concat(BundleName, ".bundle");
        
        GenerateSwapperGuid();
        return true;
    }
}