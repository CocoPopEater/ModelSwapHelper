using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class ColorDetailModule : BaseModule
{
    public ColorDetailModule(string assetPath) : base(assetPath)
    {
    }
    
    public ColorDetailModule() : base(string.Empty)
    {
        
    }

    public override void Apply(GameObject obj, AssetBundle bundle)
    {
        Texture2D colorDetailTexture = bundle.LoadAsset<Texture2D>(this.AssetPath);
        if (colorDetailTexture == null)
        {
            MelonLogger.Error($"Failed to load Color Detail Texture2D: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        var mats = smr.materials;
        if(mats == null || mats.Length == 0) return;
        foreach (var mat in mats)
        {
            if(mat.HasProperty("_ColorDetailLookup")) mat.SetTexture("_ColorDetailLookup", colorDetailTexture);
        }
    }
}