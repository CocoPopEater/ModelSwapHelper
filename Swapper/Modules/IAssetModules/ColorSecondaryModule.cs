using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class ColorSecondaryModule  : IAssetModule
{
    public string AssetPath { get; set; }
    public ColorSecondaryModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }

    public ColorSecondaryModule()
    {
        
    }

    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D colorSecondaryTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (colorSecondaryTexture == null)
        {
            Melon<Core>.Logger.Error($"Failed to load Color Secondary Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_ColorSecondaryLookup")) mat.SetTexture("_ColorSecondaryLookup", colorSecondaryTexture);
        }
    }
}