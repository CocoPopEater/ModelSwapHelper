using MelonLoader;
using MelonLoader.Utils;
using System.Collections;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Il2CppEpic.OnlineServices;
using Il2CppKeepsake.HyperSpace.GameMode.Logic;
using Il2CppNewtonsoft.Json.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[assembly: MelonInfo(typeof(ModelSwapHelper.Core), "ModelSwapHelper", "0.0.0", "CocoPopEater", null)]
[assembly: MelonGame("Keepsake Games", "Jump Space")]
[assembly: MelonColor(255,0,255,0)]

namespace ModelSwapHelper
{
    public class Core : MelonMod
    {

        private static float reloadMessageStart = -1f;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("ModelSwapHelper initialized.");
        }

        public override void OnLateUpdate()
        {

            if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame)
            {
                MelonLogger.Msg("Manual reload triggered.");
                Helper.GetInstance().StartActivate();
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
            Helper.GetInstance().StartActivate();
        }
    }

    

    public static class MeshSwap
    {
        public static void SwapMesh(SwapDetails details)
        {
            /*StringBuilder startLog = new StringBuilder();
            startLog.AppendLine(details.ModName);
            foreach (string objectName in details.ObjectNames)
            {
                startLog.AppendLine(objectName);
            }
            
            MelonLogger.Msg($"Swapping Mesh, Details: \n{startLog.ToString()}");*/

            string bundleFileName = details.BundleName;
            if (!bundleFileName.EndsWith(".bundle"))
            {
                bundleFileName = string.Concat(details.BundleName, ".bundle");
            }
            
            string bundlePath = Path.Combine(
                MelonEnvironment.ModsDirectory,
                bundleFileName
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

            Mesh customMesh = bundle.LoadAsset<Mesh>(details.MeshAddress);
            Texture2D mainTex = bundle.LoadAsset<Texture2D>(details.TextureAddress);
            Texture2D normalTex = null;
            if (details.NormalAddress != null)
            {
                normalTex = bundle.LoadAsset<Texture2D>(details.NormalAddress);
            }

            bundle.Unload(false);

            if (customMesh == null)
            {
                MelonLogger.Error($"Custom mesh not found in bundle.\nMod: {details.ModName}\nBundle: {details.BundleName}\nMeshAddress: {details.MeshAddress}");
                return;
            }
            
            List<GameObject> targets = new List<GameObject>();

            
            
            
            

            foreach (string objectName in details.ObjectNames)
            {
                targets.AddRange(GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                                .Where(go => go.name == objectName)
                                .ToList<GameObject>());
            }

            MelonLogger.Msg($"Found {targets.Count} target object(s).");

            if (details.DeactivateObjectNames != null && details.DeactivateObjectNames.Count > 0)
            {
                List<GameObject> deactivateTargets = new List<GameObject>();
                foreach (string deacName in details.DeactivateObjectNames)
                {
                    deactivateTargets.AddRange(GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                        .Where(go => go.name == deacName));
                }

                foreach (GameObject obj in deactivateTargets)
                {
                    if (obj.activeSelf)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            
            foreach (var targetObj in targets)
            {
                var smr = targetObj.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    smr.sharedMesh = customMesh;
                    var mats = smr.materials;

                    if (mainTex != null && mats[0].HasProperty("_T1"))
                        mats[0].SetTexture("_T1", mainTex);
                    
                    if (normalTex != null && mats[1].HasProperty("_T2"))
                        mats[0].SetTexture("_T2", normalTex);
                    
                    smr.materials = mats;
                }
            }
        }
        
        
        
        /// <summary>
        /// Moves the mesh without moving the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="x">Move along the X axis (Left/Right)</param>
        /// <param name="z">Move along the Z axis (Up/Down)</param>
        /// <param name="y">Move along the Y axis (Forward/Backward)</param>
        /// <returns>The moved mesh</returns>
        public static Mesh MoveMesh(Mesh mesh, float x, float z, float y)
        {
            return MoveMesh(mesh, new Vector3(x, y, z));
        }

        /// <summary>
        /// Moves the mesh without moving the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="pos">The relative position to move to</param>
        /// <returns>The moved mesh</returns>
        public static Mesh MoveMesh(Mesh mesh, Vector3 pos)
        {
            Vector3[] originalVerts = mesh.vertices;
            Vector3[] transformedVerts = new Vector3[mesh.vertices.Length];

            for (int vert = 0; vert < originalVerts.Length; vert++)
            {
                transformedVerts[vert] = pos + originalVerts[vert];
            }

            mesh.vertices = transformedVerts;
            return mesh;
        }

        /// <summary>
        /// Rotates the mesh without rotating the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="x">Rotate around the X axis (Left/Right)</param>
        /// <param name="z">Rotate around the Z axis (Up/Down)</param>
        /// <param name="y">Rotate around the Y axis (Forward/Backward)</param>
        /// <returns>The rotated mesh</returns>
        public static Mesh RotateMesh(Mesh mesh, float x, float z, float y)
        {
            Quaternion qAngle = Quaternion.Euler(x, y, z);
            return RotateMesh(mesh, qAngle);
        }

        /// <summary>
        /// Rotates the mesh without rotating the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="qAngle">The quaternion to use for rotation</param>
        /// <returns>The rotated mesh</returns>
        public static Mesh RotateMesh(Mesh mesh, Quaternion qAngle)
        {
            Vector3[] originalVerts = mesh.vertices;
            Vector3[] transformedVerts = new Vector3[mesh.vertices.Length];

            for (int vert = 0; vert < originalVerts.Length; vert++)
            {
                transformedVerts[vert] = qAngle * originalVerts[vert];
            }

            mesh.vertices = transformedVerts;
            return mesh;
        }

        /// <summary>
        /// Scales the mesh without rotating the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="x">Scale along the X axis </param>
        /// <param name="x">Scale along the X axis (Left/Right)</param>
        /// <param name="z">Scale along the Z axis (Up/Down)</param>
        /// <param name="y">Scale along the Y axis (Forward/Backward)</param>
        /// <returns>The scaled mesh</returns>
        public static Mesh ScaleMesh(Mesh mesh, float x, float z, float y)
        {
            return ScaleMesh(mesh, new Vector3(x, y, z));
        }

        /// <summary>
        /// Scales the mesh without rotating the game object
        /// </summary>
        /// <param name="mesh">The mesh to transform</param>
        /// <param name="scale">The vector used for scaling</param>
        /// <returns>The scaled mesh</returns>
        public static Mesh ScaleMesh(Mesh mesh, Vector3 scale)
        {
            Vector3[] originalVerts = mesh.vertices;
            Vector3[] transformedVerts = new Vector3[mesh.vertices.Length];

            for (int vert = 0; vert < originalVerts.Length; vert++)
            {
                Vector3 originalVertex = originalVerts[vert];
                transformedVerts[vert] = new Vector3(
                    originalVertex.x * scale.x,
                    originalVertex.y * scale.y,
                    originalVertex.z * scale.z
                    );
            }

            mesh.vertices = transformedVerts;
            return mesh;
        }
    }
}