using MelonLoader;
using MelonLoader.Utils;
using System.Collections;
using System.IO;
using System.Linq;
using System.Numerics;
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
            //Texture2D normalTex = bundle.LoadAsset<Texture2D>("Assets/NormalMap.png");

            bundle.Unload(false);

            if (customMesh == null)
            {
                //debugging - MelonLogger.Error("Custom mesh not found in bundle.");
                return;
            }

            var targets = GameObject.FindObjectsOfType<GameObject>()
                .Where(go => go.name == "ObjectName")
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

                    /*
                    if (normalTex != null && mats[1].HasProperty("_T2"))
                        mats[0].SetTexture("_T2", normalTex);
                    */

                    
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