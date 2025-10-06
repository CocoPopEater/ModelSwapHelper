using MelonLoader;
using UnityEngine;

namespace ModelSwapHelper.Swapper.Modules;

public class NormalsModule : BaseModule
{
    public NormalsModule(string assetPath) : base(assetPath)
    {
    }
    
    public NormalsModule() : base(string.Empty)
    {
        
    }

    public override void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D normalTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (normalTexture == null)
        {
            MelonLogger.Error($"Failed to load Normals Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_T2")) mat.SetTexture("_T2", normalTexture);
        }
    }
}