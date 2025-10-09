using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public interface IAssetModule : IModule
{
    string AssetPath { get; set; }
    
    public void Apply(GameObject obj, AssetBundle bundle);
    public void  ApplyAll(IEnumerable<GameObject> objects, AssetBundle assetBundle)
    {
        foreach (GameObject go in objects)
        {
            Apply(go, assetBundle);
        }
    }
}