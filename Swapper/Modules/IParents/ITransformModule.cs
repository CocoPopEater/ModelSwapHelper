using UnityEngine;

namespace ModelSwapLib.Swapper.Modules;

public interface ITransformModule : IModule
{
    public void Apply(GameObject obj);
    public void  ApplyAll(IEnumerable<GameObject> objects)
    {
        foreach (GameObject go in objects)
        {
            Apply(go);
        }
    }
}