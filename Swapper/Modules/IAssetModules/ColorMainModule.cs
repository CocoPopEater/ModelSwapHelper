using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class ColorMainModule : IAssetModule
{
    public string AssetPath { get; set; }

    public ColorMainModule(string assetPath)
    {
    }
    
    public ColorMainModule()
    {
        
    }

    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D colorMainTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (colorMainTexture == null)
        {
            Melon<Core>.Logger.Error($"Failed to load Color Main Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_ColorMainLookup")) mat.SetTexture("_ColorMainLookup", colorMainTexture);
        }
    }
}