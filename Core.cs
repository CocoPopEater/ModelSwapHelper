using MelonLoader;
using ModelSwapLib.Swapper;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(ModelSwapLib.Core), "ModelSwapLib", "0.0.0", "CocoPopEater", null)]
[assembly: MelonGame("Keepsake Games", "Jump Space")]
[assembly: MelonColor(255,0,255,0)]

namespace ModelSwapLib
{
    public class Core : MelonMod
    {

        private static float reloadMessageStart = -1f;

        public override void OnInitializeMelon()
        {
            SwapperManager.GetInstance(); // Ensure SwapHandler has been initialized
            BundleManager.GetInstance(); // Ensure BundleManager has been initialized
            MelonLogger.Msg("ModelSwapLib initialized.");
        }

        public override void OnDeinitializeMelon()
        {
            BundleManager.GetInstance().Shutdown(); // Ensure BundleManager unloads all cached bundles and clears the dict
        }

        public override void OnLateUpdate()
        {

            if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame)
            {
                MelonLogger.Msg("Manual reload triggered.");
                SwapperManager.GetInstance().StartActivate();
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
            SwapperManager.GetInstance().StartActivate();
        }
    }
}