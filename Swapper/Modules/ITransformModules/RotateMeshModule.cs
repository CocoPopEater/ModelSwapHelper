using UnityEngine;

namespace ModelSwapLib.Swapper.Modules.ITransformModules;

public class RotateMeshModule : ITransformModule
{
    Vector3 vector;
    public RotateMeshModule(float xRot, float yRot, float zRot)
    {
        vector = new Vector3(xRot, yRot, zRot);
    }

    public RotateMeshModule(Vector3 vector)
    {
        this.vector = vector;
    }

    public void Apply(GameObject gameObject)
    {
        
        var smr  = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;
        
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh); // Apparently turns mesh into the objects current mesh?
        if (mesh == null) return;
        
        mesh = MeshUtils.RotateMesh(mesh, vector.x, vector.y, vector.z);
        smr.sharedMesh = mesh; 
    }
}