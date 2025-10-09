using UnityEngine;

namespace ModelSwapLib.Swapper.Modules.ITransformModules;

public class ScaleMeshModule : ITransformModule
{
    Vector3 vector;
    public ScaleMeshModule(float xScale, float yScale, float zScale)
    {
        vector = new Vector3(xScale, yScale, zScale);
    }

    public ScaleMeshModule(Vector3 vector)
    {
        this.vector = vector;
    }

    public void Apply(GameObject gameObject)
    {
        var smr  = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh); // Turns mesh into the objects current mesh
        if(mesh == null) return;
        
        mesh = MeshUtils.ScaleMesh(mesh, vector);
        smr.sharedMesh = mesh;
    }
}