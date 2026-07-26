using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class MeshFilterModule : IAssetModule
{
    public string AssetPath { get; set; }

    public MeshFilterModule(string assetPath)
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
        
        var mf = obj.GetComponent<MeshFilter>();
        if (mf == null) return;
        mf.mesh = mesh;
    }
}