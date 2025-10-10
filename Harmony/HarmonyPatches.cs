using HarmonyLib;
using Il2CppLudiq;
using MelonLoader;
using ModelSwapLib.Swapper;
using UnityEngine;

namespace ModelSwapLib.Harmony;

[HarmonyPatch]
public class HarmonyPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Transform), typeof(bool) })]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion) })]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform) })]
    static void InstantiatePostfix(ref UnityEngine.Object __result)
    {
        if (__result == null) return;
        
        var gameObject = __result.GameObject();
        if(gameObject == null) return;
        
        MelonCoroutines.Start(ObjectActionManager.GetInstance().HandleObject(gameObject));
    }
}