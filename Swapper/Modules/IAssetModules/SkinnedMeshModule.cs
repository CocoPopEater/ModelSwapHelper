using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class SkinnedMeshModule : IAssetModule
{
    public string AssetPath { get; set; }

    public SkinnedMeshModule(string assetPath)
    {
        this.AssetPath = assetPath;
    }
    public void Apply(GameObject obj, Il2CppAssetBundle bundle)
    {
        Mesh mesh = bundle.LoadAsset<Mesh>(this.AssetPath);
        if (mesh == null)
        {
            ConsoleUtils.Error($"Failed to load Mesh: {this.AssetPath}");
            return;
        }
        
        var smr = obj.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        smr.sharedMesh = mesh;
    }
}