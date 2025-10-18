using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MetallicModule : IAssetModule
{
    public string AssetPath { get; set; }
    public MetallicModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }
    
    public MetallicModule()
    {
        
    }

    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D metallicTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (metallicTexture == null)
        {
            Melon<Core>.Logger.Error($"Failed to load Metallic Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_M")) mat.SetTexture("_M", metallicTexture);
        }
    }
}