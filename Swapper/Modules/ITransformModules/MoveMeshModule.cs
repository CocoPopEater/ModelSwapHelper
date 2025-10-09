using UnityEngine;

namespace ModelSwapLib.Swapper.Modules.ITransformModules;

public class MoveMeshModule : ITransformModule
{
    Vector3 vector;
    public MoveMeshModule(float xDist, float yDist, float zDist)
    {
        vector = new Vector3(xDist, yDist, zDist);
    }

    public MoveMeshModule(Vector3 vector)
    {
        this.vector = vector;
    }

    public void Apply(GameObject gameObject)
    {
        var smr  = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh); // Turns mesh into the objects current mesh
        if (mesh == null) return;
        
        mesh = MeshUtils.MoveMesh(mesh, vector);
        smr.sharedMesh = mesh;
    }
}