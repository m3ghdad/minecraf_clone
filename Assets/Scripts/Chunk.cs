using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextureType {
	air, grass, rock
}

public class Chunk : MonoBehaviour {

	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List <int>();
	private List<Vector2> newUV = new List <Vector2>();

	private Mesh mesh;
	private MeshCollider chunkCollider;
	private float textureWidth = 0.083f;
	private int faceCount;
	private World world;

	[SerializeField] GameObject worldGO;
	[SerializeField] int chunkSize = 16;

	//Textures
	private Vector2 grassTop = new Vector2 (1,11);
	private Vector2 grassSide = new Vector2 (0,10);
	private Vector2 rock = new Vector2 (7,8);

	// Use this for initialization
	void Start () {

		world = worldGO.GetComponent("World") as World;
		mesh = GetComponent<MeshFilter>().mesh;
		chunkCollider = GetComponent<MeshCollider>();

		GenerateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateMesh() {
		for (int x = 0; x < chunkSize; x ++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					if (world.Block (x, y, z) != (byte)TextureType.air.GetHashCode()) {
						//Blcok above is air
						if (world.Block(x, y + 1 , z) == (byte)TextureType.air.GetHashCode()) {
							CubeTop(x, y, z, world.Block (x, y, z));
						}
						if (world.Block(x, y - 1 , z) == (byte)TextureType.air.GetHashCode()) {
							CubeBot(x, y, z, world.Block (x, y, z));
						}
						if (world.Block(x + 1, y , z) == (byte)TextureType.air.GetHashCode()) {
							CubeEast(x, y, z, world.Block (x, y, z));
						}
						if (world.Block(x - 1, y , z) == (byte)TextureType.air.GetHashCode()) {
							CubeWest(x, y, z, world.Block (x, y, z));
						}
						if (world.Block(x, y , z + 1) == (byte)TextureType.air.GetHashCode()) {
							CubeNorth(x, y, z, world.Block (x, y, z));
						}
						if (world.Block(x, y , z - 1) == (byte)TextureType.air.GetHashCode()) {
							CubeSouth(x, y, z, world.Block (x, y, z));
						}
					}
				}
			}
		}

		UpdateMesh();
	}

	void UpdateMesh () {
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.RecalculateNormals();
		chunkCollider.sharedMesh = null;
		chunkCollider.sharedMesh = mesh;

		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		faceCount = 0;

	}

	void CubeTop(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x, y, z));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}

	void CubeNorth(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x , y, z + 1));
		newVertices.Add(new Vector3(x, y - 1, z + 1));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}

	void CubeEast(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x + 1, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x + 1, y, z + 1));
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}

	void CubeSouth(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x, y - 1, z));
		newVertices.Add(new Vector3(x, y, z));
		newVertices.Add(new Vector3(x + 1, y, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}

	void CubeWest(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x, y - 1, z + 1));
		newVertices.Add(new Vector3(x, y, z + 1));
		newVertices.Add(new Vector3(x, y, z));
		newVertices.Add(new Vector3(x, y - 1, z));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}

	void CubeBot(int x, int y, int z, byte Block) {
		newVertices.Add(new Vector3(x, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z));
		newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
		newVertices.Add(new Vector3(x, y - 1, z + 1));

		Vector2 texturePos;
		texturePos = rock;

		Cube (texturePos);
	}


	void Cube (Vector2 texturePos) {

		newTriangles.Add (faceCount * 4); //1
		newTriangles.Add (faceCount * 4 + 1); //2
		newTriangles.Add (faceCount * 4 + 2); //3
		newTriangles.Add (faceCount * 4); //4
		newTriangles.Add (faceCount * 4 + 2); //5
		newTriangles.Add (faceCount * 4 + 3); //6


		newUV.Add (new Vector2 (textureWidth * texturePos.x + textureWidth, textureWidth * texturePos.y));
		newUV.Add (new Vector2 (textureWidth * texturePos.x + textureWidth, textureWidth * texturePos.y + textureWidth));
		newUV.Add (new Vector2 (textureWidth * texturePos.x, textureWidth * texturePos.y + textureWidth));
		newUV.Add (new Vector2 (textureWidth * texturePos.x, textureWidth * texturePos.y));

		faceCount ++;
	}

}
