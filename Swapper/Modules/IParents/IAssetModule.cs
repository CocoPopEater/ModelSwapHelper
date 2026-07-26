using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public interface IAssetModule : IModule
{
    string AssetPath { get; set; }
    
    public void Apply(GameObject obj, Il2CppAssetBundle bundle);
    public void  ApplyAll(IEnumerable<GameObject> objects, Il2CppAssetBundle assetBundle)
    {
        foreach (GameObject go in objects)
        {
            Apply(go, assetBundle);
        }
    }
}