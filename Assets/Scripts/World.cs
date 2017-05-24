using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	[SerializeField] GameObject chunk;
	[SerializeField] int worldX = 16;
	[SerializeField] int worldY = 16;
	[SerializeField] int worldZ = 16;
	[SerializeField] int chunkSize = 16;

	private byte[,,] worldData;
	private Chunk[,,] chunks;


	// Use this for initialization
	void Start () {
		worldData = new byte[worldX, worldY, worldZ];

		for(int x = 0; x < worldX; x++) {
			for (int y = 0; y < worldY; y++) {
				for (int z = 0; z < worldZ; z++) {
					if (y <= 8) {
						worldData [x, y, z] = (byte)TextureType.rock.GetHashCode();
					}
				}
			}
		}

		chunks = new Chunk [Mathf.FloorToInt(worldX/chunkSize), Mathf.FloorToInt(worldY/chunkSize), Mathf.FloorToInt(worldZ/chunkSize)];

		for(int x = 0; x < chunks.GetLength(0); x ++) {
			for (int y = 0; y < chunks.GetLength(1); y++) {
				for (int z = 0; z < chunks.GetLength(2); z++) {
					GameObject newChunk = Instantiate(chunk, new Vector3(x * chunkSize, y * chunkSize, z * chunkSize), new Quaternion (0, 0, 0, 0)) as GameObject;
					chunks [x, y, z] = newChunk.GetComponent("Chunk") as Chunk;
					chunks [x, y, z].WorldGo = gameObject;
					chunks [x, y, z].ChunkSize = chunkSize;
					chunks [x, y, z].ChunkX = x * chunkSize;
					chunks [x, y, z].ChunkY = y * chunkSize;
					chunks [x, y, z].ChunkZ = z * chunkSize;

				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public byte Block(int x, int y, int z) {
		if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0) {
			return (byte)TextureType.rock.GetHashCode();
		}

		return worldData[x, y, z];
	}
}

