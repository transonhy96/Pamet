using Godot;
using System;

public class Face 
{
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public Face(int resolution, Vector3 localUp)
    {
        this.resolution = resolution;
        this.localUp = localUp;
        axisA = new Vector3(localUp.y,localUp.z,localUp.x);
        axisB = localUp.Cross(axisA);
    }

    public ArrayMesh ContructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        int i = 0;
        int triIndex = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                vertices[i] = pointOnUnitCube;
                if (x != resolution -1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i  + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
                i++;
            }
        }
        var array_mesh = new ArrayMesh();
        var arrays = new Godot.Collections.Array();
        arrays.Resize(9);
        arrays[(int)ArrayMesh.ArrayType.Vertex] = vertices;
        arrays[(int)ArrayMesh.ArrayType.Index] = triangles;
        array_mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.TriangleStrip, arrays);
        return array_mesh;
    }
}
