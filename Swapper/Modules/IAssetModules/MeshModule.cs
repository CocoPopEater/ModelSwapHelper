using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MeshModule : IAssetModule
{

    public string AssetPath { get; set; }

    public MeshModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }
    public void Apply(GameObject obj, AssetBundle bundle)
    {
        Mesh mesh = bundle.LoadAsset<Mesh>(this.AssetPath);
        if (mesh == null)
        {
            Melon<Core>.Logger.Error($"Failed to load Mesh: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        smr.sharedMesh = mesh;
    }
}