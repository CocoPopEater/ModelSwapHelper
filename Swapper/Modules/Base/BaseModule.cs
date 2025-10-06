using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public class BaseModule : IModule
{
    public string AssetPath { get; set; }

    public BaseModule(string assetPath)
    {
        AssetPath = assetPath;
    }

    public virtual void Apply(GameObject obj, AssetBundle bundle)
    {
        throw new NotImplementedException();
    }
}