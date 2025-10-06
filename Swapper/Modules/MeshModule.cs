using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MeshModule : BaseModule
{
    public MeshModule(string assetPath) : base(assetPath)
    {
    }

    public MeshModule() : base(string.Empty)
    {
        
    }

    public override void Apply(GameObject obj, AssetBundle bundle)
    {
        Mesh mesh = bundle.LoadAsset<Mesh>(this.AssetPath);
        if (mesh == null)
        {
            MelonLogger.Error($"Failed to load Mesh: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        smr.sharedMesh = mesh;
    }
}