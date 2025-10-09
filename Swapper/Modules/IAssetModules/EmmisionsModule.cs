using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class EmmisionsModule  : IAssetModule
{
    public string AssetPath { get; set; }
    public EmmisionsModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }
    
    public EmmisionsModule()
    {
        
    }

    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D emissionsTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (emissionsTexture == null)
        {
            MelonLogger.Error($"Failed to load Emissions Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_T3")) mat.SetTexture("_T3", emissionsTexture);
        }
    }
}