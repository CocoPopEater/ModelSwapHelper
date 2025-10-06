using MelonLoader;
using UnityEngine;

namespace ModelSwapHelper.Swapper.Modules;

public class EmmisionsModule : BaseModule
{
    public EmmisionsModule(string assetPath) : base(assetPath)
    {
    }
    
    public EmmisionsModule() : base(string.Empty)
    {
        
    }

    public override void Apply(GameObject obj, AssetBundle bundle)
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