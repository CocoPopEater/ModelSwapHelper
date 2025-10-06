using UnityEngine;

namespace ModelSwapHelper.Swapper.Modules;

public interface IModule
{
    public string AssetPath { get; set; }
    public void Apply(GameObject obj, AssetBundle bundle);
}