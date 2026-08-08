using HarmonyLib;
using Il2CppKeepsake;
using Il2CppKeepsake.Pickupables.GenericWeapon;
using Il2CppLudiq;
using MelonLoader;
using ModelSwapLib.ObjectTracking;
using ModelSwapLib.Swapper;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModelSwapLib.Harmony;

[HarmonyPatch]
public class HarmonyPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Transform), typeof(bool) })]
    static void InstantiatePostfix1(ref UnityEngine.Object __result, ref Transform parent, ref bool instantiateInWorldSpace)
    {
        ConsoleUtils.Msg($"Postfix1: {__result.name}/{__result.GetInstanceID()} : {parent.name}/{parent.GetInstanceID()} : {instantiateInWorldSpace}");
        HandleObject(ref __result);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion) })]
    static void InstantiatePostfix2(ref UnityEngine.Object __result, ref Vector3 position, ref Quaternion rotation)
    {
        ConsoleUtils.Msg($"Postfix1: {__result.name}/{__result.GetInstanceID()} : {position.ToString()} : {rotation.ToString()}");
        HandleObject(ref __result);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform) })]
    static void InstantiatePostfix3(ref UnityEngine.Object __result, ref Vector3 position, ref Quaternion rotation, ref Transform parent)
    {
        ConsoleUtils.Msg($"Postfix1: {__result.name}/{__result.GetInstanceID()} : {position.ToString()} : {rotation.ToString()} : {parent.name}/{parent.GetInstanceID()}");
        HandleObject(ref __result);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Instantiate), new Type[] { typeof(UnityEngine.Object) })]
    static void InstantiatePostfix4(ref UnityEngine.Object __result, ref UnityEngine.Object original)
    {
        ConsoleUtils.Msg($"Postfix1: {__result.name}/{__result.GetInstanceID()} : {original.name}/{original.GetInstanceID()}");
        HandleObject(ref __result);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.MonoBehaviour), "Awake", new[] { typeof(UnityEngine.Object) })]
    static void Awake(ref MonoBehaviour __instance)
    {
        if(!__instance) return;
        Object go = __instance.gameObject as Object;
        ConsoleUtils.Msg($"MonoBehaviour Awake: {__instance.name}/{__instance.GetInstanceID()}");
        HandleObject(ref go);
    }

    static void HandleObject(ref UnityEngine.Object obj)
    {
        if (obj == null) return;
        
        var gameObject = obj.GameObject();
        if(gameObject == null) return;
        
        TrackingManager.AddTrackingDetails(gameObject);
        
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject == gameObject) continue;
            TrackingManager.AddTrackingDetails(child.gameObject);
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), "Destroy", new[] { typeof(UnityEngine.Object) })]
    static void Prefix_Object(UnityEngine.Object obj)
    {
        if(!obj) return;
        if (obj is GameObject go)
        {
            TrackingManager.RemoveTrackingDetails(go);
        }
    }
}