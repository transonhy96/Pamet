using Godot;
using System;
[Tool]
public class Planet : MeshInstance
{
    Face[] faces;
    [Export]
    public int resolution = 16;
    [Export] Material mat;
    private bool CreatMesh = false;
    [Export]
    private bool _CreatMesh
    {
        get => CreatMesh;
        set
        {
            CreatMesh = value;
            GenMeshPress(value);
        }

    }
    ArrayMesh tmpMesh = new ArrayMesh();
    private bool GenMeshPress(bool val)
    {
        Initialize();
        GenerateMesh();
        return false;
    }
    public override void _Ready()
    {
        Initialize();
        GenerateMesh();
    }
    public void Initialize()
    {
        faces = new Face[6];
        Vector3[] direction = { Vector3.Up, Vector3.Down, Vector3.Left, Vector3.Right, Vector3.Forward, Vector3.Back };
        for (int i = 0; i < 6; i++)
        {
            faces[i] = new Face(resolution, direction[i]);
        }
    }
    public void GenerateMesh()
    {
        SurfaceTool st = new SurfaceTool();
        st.Begin(Mesh.PrimitiveType.Triangles);
        st.SetMaterial(mat);
        foreach (var face in faces)
        {
            tmpMesh = face.ContructMesh();
            st.GenerateNormals();
            st.Commit(tmpMesh);
            Mesh = tmpMesh;
        }
        
    }
}
