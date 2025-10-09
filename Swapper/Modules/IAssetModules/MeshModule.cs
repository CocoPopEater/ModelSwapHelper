using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MeshModule : IAssetModule
{

    public string AssetPath { get; set; }
    public Vector3? Rotation { get; set; } = null;
    public Vector3? Movement { get; set; } = null;
    public MeshModule(string assetPath, Vector3? rotation = null, Vector3? movement = null)
    {
        this.AssetPath = assetPath;
        this.Rotation = rotation;
        this.Movement = movement;
    }

    public void Apply(GameObject obj, AssetBundle bundle)
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