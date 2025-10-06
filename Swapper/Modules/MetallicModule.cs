using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MetallicModule : BaseModule
{
    public MetallicModule(string assetPath) : base(assetPath)
    {
    }
    
    public MetallicModule() : base(string.Empty)
    {
        
    }

    public override void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D metallicTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (metallicTexture == null)
        {
            MelonLogger.Error($"Failed to load Metallic Texture2D: {this.AssetPath}");
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