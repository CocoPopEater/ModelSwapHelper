using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class Texture2DModule : IAssetModule
{
    public string AssetPath { get; set; }
    public Texture2DModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }
    
    public Texture2DModule()
    {
        
    }

    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D mainTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (mainTexture == null)
        {
            Melon<Core>.Logger.Error($"Failed to load Main Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats[0].HasProperty("_T1")) mats[0].SetTexture("_T1", mainTexture);
    }
}