using Godot;
using System;
using System.Collections.Generic;

namespace Sandbox
{
	[Tool]
	public class MeshInstance : Godot.MeshInstance
	{
		private List<Vector3> QuadVertices { get; set; }
		private List<int> QuadIndices { get; set; }
		private Godot.Collections.Dictionary<Vector3,int> QuadOrderDic { get; set; }
		private const float CUBE_SIZE = 0.5f;
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
		private bool GenMeshPress(bool val)
		{
			MakeCube();
			return false;
		}

		private void MakeCube()
		{
			QuadVertices  = new List<Vector3>();
			QuadIndices  = new List<int>();
			QuadOrderDic = new Godot.Collections.Dictionary<Vector3, int>();
			var surfaceTool = new SurfaceTool();
			surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
			var vertNorthTopRight = new Vector3(-CUBE_SIZE, CUBE_SIZE, CUBE_SIZE);
			var vertNorthTopLeft = new Vector3(CUBE_SIZE, CUBE_SIZE, CUBE_SIZE);
			var vertNorthBottomLeft = new Vector3(CUBE_SIZE, CUBE_SIZE, -CUBE_SIZE);
			var vertNorthBottomRight = new Vector3(-CUBE_SIZE, CUBE_SIZE, -CUBE_SIZE);
			var vertSouthTopRight = new Vector3(-CUBE_SIZE, -CUBE_SIZE, CUBE_SIZE);
			var vertSouthTopLeft = new Vector3(CUBE_SIZE, -CUBE_SIZE, CUBE_SIZE);
			var vertSouthBottomLeft = new Vector3(CUBE_SIZE, -CUBE_SIZE, -CUBE_SIZE);
			var vertSouthBottomRight = new Vector3(-CUBE_SIZE, -CUBE_SIZE, -CUBE_SIZE);

			AddQuad(vertSouthTopRight, vertSouthTopLeft, vertSouthBottomLeft, vertSouthBottomRight);
			AddQuad(vertNorthTopRight, vertNorthBottomRight, vertNorthBottomLeft, vertNorthTopLeft);

			AddQuad(vertNorthBottomLeft, vertNorthBottomRight, vertSouthBottomRight, vertSouthBottomLeft);
			AddQuad(vertNorthTopLeft, vertSouthTopLeft, vertSouthTopRight, vertNorthTopRight);

			AddQuad(vertNorthTopRight, vertSouthTopRight, vertSouthBottomRight, vertNorthBottomRight);
			AddQuad(vertNorthTopLeft, vertNorthBottomLeft, vertSouthBottomLeft, vertSouthTopLeft);

			foreach (var vertex in QuadVertices)
			{
				surfaceTool.AddVertex(vertex);
			}

			foreach (var index in QuadIndices)
			{
				surfaceTool.AddIndex(index);
			}
			surfaceTool.GenerateNormals();
			var result_mesh = surfaceTool.Commit();
			this.Mesh = result_mesh;
		}

		private void AddQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
		{
			var vertex_index_one = -1;
			var vertex_index_two = -1;
			var vertex_index_three = -1;
			var vertex_index_four = -1;

			vertex_index_one = AddOrGetVertex(p1);
			vertex_index_two = AddOrGetVertex(p2);
			vertex_index_three = AddOrGetVertex(p3);
			vertex_index_four = AddOrGetVertex(p4);

			QuadIndices.Add(vertex_index_one);
			QuadIndices.Add(vertex_index_two);
			QuadIndices.Add(vertex_index_three);

			QuadIndices.Add(vertex_index_one);
			QuadIndices.Add(vertex_index_three);
			QuadIndices.Add(vertex_index_four);
		}

		private int AddOrGetVertex(Vector3 ver)
		{
			if (QuadOrderDic.ContainsKey(ver))
			{
				return QuadOrderDic[ver];
			}
			else
			{
				QuadVertices.Add(ver);
				var index = QuadVertices.Count - 1;
				QuadOrderDic.Add(ver,index);
				return index;
			}
		}
		public override void _Ready()
		{
			
		}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	}

}
