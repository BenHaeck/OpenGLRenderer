
public class HeightPlane {
	public MeshMaker mm;

	public int[,] tiles;

	public readonly int defaultTile = 0;

	//float maxHeight = 0, minShade = 1;
	
	public float tileHeight = 0.5f;

	public HeightPlane (int[,] tiles, int defaultTile = 0, float tileHeight = 1, float minShade = 1, int numberOfTextures = 1) {
		mm = new MeshMaker(tiles.Length * 4 * 4, tiles.Length * 6 * 4);
		this.tiles = tiles;
		this.defaultTile = defaultTile;
		this.tileHeight = tileHeight;
		float inverseTexNum = 1f/numberOfTextures;
		mm.useindices = true;

		for (int z = -1; z < tiles.GetLength(0); z++) {
			for (int x = -1; x < tiles.GetLength(1); x++){
				//Console.WriteLine($"{x}, {z}");
				var smaller = Math.Min(GetTile(x,z), GetTile(x + 1, z));
				var larger = Math.Max(GetTile(x,z), GetTile(x + 1, z));
				float largerH = GetTileHeight(larger), smallerH = GetTileHeight(smaller);

				float dir = 0;
				if (smallerH != largerH){
					dir = GetTile(x,z) > GetTile(x + 1,z)? 1: -1;
					int tileType = GetTileType(larger);
					float left = inverseTexNum * tileType;
					float right = inverseTexNum * (tileType + 1);
					int ind = 
					mm.PushVertex(x+1, largerH, z+1,  1, 1, 1,  left, -largerH,  dir, 0, 0);
					mm.PushVertex(x+1, largerH, z+0,  1, 1, 1,  right, -largerH,  dir, 0, 0);

					mm.PushVertex(x+1, smallerH, z+1,  minShade, minShade, minShade,  left, -smallerH,  dir, 0, 0);
					mm.PushVertex(x+1, smallerH, z+0,  minShade, minShade, minShade,  right, -smallerH,  dir, 0, 0);

					//if (dir < 0) ind += 3;
					if (dir>0) {
						mm.PushIndex((uint)(ind+0));
						mm.PushIndex((uint)(ind+1));
						mm.PushIndex((uint)(ind+2));

						mm.PushIndex((uint)(ind+3));
						mm.PushIndex((uint)(ind+2));
						mm.PushIndex((uint)(ind+1));
					} else {
						mm.PushIndex((uint)(ind+1));
						mm.PushIndex((uint)(ind+0));
						mm.PushIndex((uint)(ind+2));

						mm.PushIndex((uint)(ind+2));
						mm.PushIndex((uint)(ind+3));
						mm.PushIndex((uint)(ind+1));
					}
				}
				smaller = Math.Min(GetTile(x,z), GetTile(x, z + 1));
				larger = Math.Max(GetTile(x,z), GetTile(x, z + 1));
				largerH = GetTileHeight(larger);
				smallerH = GetTileHeight(smaller);

				if (smallerH != largerH) {
					dir = GetTile(x,z) > GetTile(x,z + 1)? 1: -1;
					
					int tileType = GetTileType(larger);
					float left = inverseTexNum * tileType;
					float right = inverseTexNum * (tileType + 1);
					int ind = 
					mm.PushVertex(x+1, largerH, z+1,  1, 1, 1,  left, -largerH,  0, 0, dir);
					mm.PushVertex(x+0, largerH, z+1,  1, 1, 1,  right, -largerH,  0, 0, dir);

					mm.PushVertex(x+1, smallerH, z+1,  minShade, minShade, minShade,  left, -smallerH,  0, 0, dir);
					mm.PushVertex(x+0, smallerH, z+1,  minShade, minShade, minShade,  right, -smallerH,  0, 0, dir);

					if (dir<0) {
						mm.PushIndex((uint)(ind+0));
						mm.PushIndex((uint)(ind+1));
						mm.PushIndex((uint)(ind+2));

						mm.PushIndex((uint)(ind+3));
						mm.PushIndex((uint)(ind+2));
						mm.PushIndex((uint)(ind+1));
					} else {
						mm.PushIndex((uint)(ind+1));
						mm.PushIndex((uint)(ind+0));
						mm.PushIndex((uint)(ind+2));

						mm.PushIndex((uint)(ind+2));
						mm.PushIndex((uint)(ind+3));
						mm.PushIndex((uint)(ind+1));
					}
				}
				//Console.Write(ind + " ");
			}
		}

		for (int i = 0; i < tiles.Length; i++) {
			int z = i % tiles.GetLength(0), x = i / tiles.GetLength(0);
			//Console.WriteLine($"{x}, {z}");
			int tileType = GetTileType(tiles[z,x]);
			float left = inverseTexNum * tileType;
			float right = inverseTexNum * (tileType + 1);
			int ind = 
			mm.PushVertex(x  , GetHeight(x,z), z  ,  1, 1, 1,  left, 0,  0, 1, 0);
			mm.PushVertex(x+1, GetHeight(x,z), z  ,  1, 1, 1,  right, 0,  0, 1, 0);
			mm.PushVertex(x  , GetHeight(x,z), z+1,  1, 1, 1,  left, 1,  0, 1, 0);
			mm.PushVertex(x+1, GetHeight(x,z), z+1,  1, 1, 1,  right, 1,  0, 1, 0);

			mm.PushIndex((uint)(ind+0));
			mm.PushIndex((uint)(ind+1));
			mm.PushIndex((uint)(ind+2));

			mm.PushIndex((uint)(ind+3));
			mm.PushIndex((uint)(ind+2));
			mm.PushIndex((uint)(ind+1));

			//Console.Write(ind + " ");
		}
		Console.WriteLine($"{mm.VertexNumber / 4}, {tiles.Length * 4}");

	}

	public static int GetTileType (int tile) {
		return Math.Abs(tile % 10);
	}

	public float GetTileHeight (int tile) {
		return (tile/10) * tileHeight;
	}

	public float GetHeight (int x, int z) {
		return GetTileHeight(GetTile(x, z));
	}

	public int GetTile (int x, int z) {
		if (x < 0 || x >= tiles.GetLength(1) || z < 0|| z >= tiles.GetLength(0))
			return defaultTile;
		
		return tiles[z,x];
	}
}