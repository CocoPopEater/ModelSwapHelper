using MelonLoader;
using MelonLoader.Utils;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(ModelSwapTemplate_JumpSpace.Core), "ModName", "Version", "Author", null)]
[assembly: MelonGame("Keepsake Games", "Jump Space")]

namespace ModelSwapTemplate_JumpSpace
{
    public class Core : MelonMod
    {

        private static float reloadMessageStart = -1f;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Mod initialized.");
        }

        public override void OnLateUpdate()
        {

            if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame)
            {
                MelonLogger.Msg("Manual reload triggered.");
                MelonCoroutines.Start(DelayedActivate());
                reloadMessageStart = Time.time;
                MelonEvents.OnGUI.Subscribe(DrawReloadText, 100);
            }
        }

        public static void DrawReloadText()
        {
            if (reloadMessageStart > 0 && Time.time - reloadMessageStart < 3f)
            {
                GUI.Label(new Rect(20, 20, 500, 50),
                    "<b><color=black><size=30>Reloading...</size></color></b>");
            }
            else
            {
                MelonEvents.OnGUI.Unsubscribe(DrawReloadText);
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            //debugging - MelonLogger.Msg($"Scene initialized: {sceneName} ({buildIndex})");
            MelonCoroutines.Start(DelayedActivate());
        }

        private IEnumerator DelayedActivate()
        {
            yield return new WaitForSeconds(3f);
            MeshSwap.SwapMesh();
        }
    }

    public static class MeshSwap
    {
        public static void SwapMesh()
        {
            string bundlePath = Path.Combine(
                MelonEnvironment.ModsDirectory,
                "BundleName.bundle"
            );

            if (!File.Exists(bundlePath))
            {
                //debugging - MelonLogger.Warning("Bundle not found: " + bundlePath);
                return;
            }

            var bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null)
            {
                //debugging - MelonLogger.Error("Failed to load AssetBundle.");
                return;
            }

            Mesh customMesh = bundle.LoadAsset<Mesh>("Assets/Mesh.fbx");
            Texture2D mainTex = bundle.LoadAsset<Texture2D>("Assets/Texture.png");

            bundle.Unload(false);

            if (customMesh == null)
            {
                //debugging - MelonLogger.Error("Custom mesh not found in bundle.");
                return;
            }

            var targets = GameObject.FindObjectsOfType<GameObject>()
                .Where(go => go.name == "BuddyBot")
                .ToList();

            MelonLogger.Msg($"Found {targets.Count} target object(s).");

            foreach (var targetObj in targets)
            {
                var smr = targetObj.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    smr.sharedMesh = customMesh;
                    var mats = smr.materials;

                    if (mainTex != null && mats[0].HasProperty("_T1"))
                        mats[0].SetTexture("_T1", mainTex);


                    /*foreach (var mat in mats)
                    {
                        if (mat.HasProperty("_T2")) mat.SetTexture("_T2", null); //Normal map
                        if (mat.HasProperty("_T3")) mat.SetTexture("_T3", null); //Emission map
                        if (mat.HasProperty("_M")) mat.SetTexture("_M", null); //Metallic map

                        if (mat.HasProperty("_ColorMainLookup")) mat.SetTexture("_ColorMainLookup", null);
                        if (mat.HasProperty("_ColorSecondaryLookup")) mat.SetTexture("_ColorSecondaryLookup", null);
                        if (mat.HasProperty("_ColorDetailLookup")) mat.SetTexture("_ColorDetailLookup", null);
                    }

                    //This is a temporary measure, certain models may be ultra reflective with their default shader:
                    Shader unlitShader = Shader.Find("Unlit/Texture");
                    if (unlitShader != null)
                    {
                        foreach (var mat in mats)
                        {
                            mat.shader = unlitShader;
                            if (bodyTex != null)
                                mat.SetTexture("_MainTex", bodyTex);
                        }
                    }*/

                    smr.materials = mats;
                }
            }
        }
    }
}