using UnityEngine;

namespace ModelSwapLib;

public class MeshUtils
{
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