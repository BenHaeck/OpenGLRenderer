using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using MBEngine.Physics;

namespace MBEngine.World {
public class HeightPlane {
	public MeshMaker mm;

	public int[,] tiles;

	public readonly int defaultTile = 0;

	//float maxHeight = 0, minShade = 1;
	
	public float tileHeight = 0.5f;

	Collider collider = new Collider(){width = 0.5f, height = 100, length = 0.5f};

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public HeightPlane (int[,] tiles, int defaultTile = 0, float tileHeight = 1, float minShade = 1, int numberOfTextures = 1) {
		mm = new MeshMaker(tiles.Length * 4 * 4, tiles.Length * 6 * 4, BufferUsageHint.StaticDraw);
		this.tiles = tiles;
		this.defaultTile = defaultTile;
		this.tileHeight = tileHeight;
		float inverseTexNum = 1f/numberOfTextures;
		mm.useindices = true;

		for (int i = 0; i < tiles.Length; i++) {
			int z = i % tiles.GetLength(0), x = i / tiles.GetLength(0);
			//Console.WriteLine($"{x}, {z}");
			int tileType = GetTileType(tiles[z,x]);
			float left = inverseTexNum * tileType;
			float right = inverseTexNum * (tileType + 1);
			int ind = 
			mm.PushVertex(x+0, GetHeight(x,z), z+0,  1, 1, 1,  left, 0,  0, 1, 0);
			mm.PushVertex(x+1, GetHeight(x,z), z+0,  1, 1, 1,  right, 0,  0, 1, 0);
			mm.PushVertex(x+0, GetHeight(x,z), z+1,  1, 1, 1,  left, 1,  0, 1, 0);
			mm.PushVertex(x+1, GetHeight(x,z), z+1,  1, 1, 1,  right, 1,  0, 1, 0);

			mm.PushIndex((uint)(ind+0));
			mm.PushIndex((uint)(ind+1));
			mm.PushIndex((uint)(ind+2));

			mm.PushIndex((uint)(ind+3));
			mm.PushIndex((uint)(ind+2));
			mm.PushIndex((uint)(ind+1));

			//Console.Write(ind + " ");
		}

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

					float grR = GetSmallistTile(x,z, 1, 1);
					float grL = GetSmallistTile(x,z, 1,-1);
					float grUR = GetLargestTile(x,z, 1, 1);
					float grUL = GetLargestTile(x,z, 1,-1);

					grR = GetGradient(largerH,grR,smallerH);
					grR = OpenTK.Mathematics.MathHelper.Lerp(minShade, 1, grR);

					grL = GetGradient(largerH,grL,smallerH);
					grL = OpenTK.Mathematics.MathHelper.Lerp(minShade, 1, grL);

					grUR = GetGradient(grUR, smallerH, largerH);
					grUR = MathHelper.Lerp(minShade, 1, grUR);
					
					grUL = GetGradient(grUL, smallerH, largerH);
					grUL = MathHelper.Lerp(minShade, 1, grUL);

					int ind = 
					mm.PushVertex(x+1, largerH, z+1,  grUR, grUR, grUR,  right, -largerH,  dir, 0, 0);
					mm.PushVertex(x+1, largerH, z+0,  grUL, grUL, grUL,  left, -largerH,  dir, 0, 0);

					mm.PushVertex(x+1, smallerH, z+1,  grR, grR, grR,  right, -smallerH,  dir, 0, 0);
					mm.PushVertex(x+1, smallerH, z+0,  grL, grL, grL,  left, -smallerH,  dir, 0, 0);


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

					float grR = GetSmallistTile(x,z,1,1);
					float grL = GetSmallistTile(x,z,-1,1);
					float grUR = GetLargestTile(x,z,1,1);
					float grUL = GetLargestTile(x,z,-1,1);


					grR = GetGradient(largerH,grR,smallerH);
					grR = MathHelper.Lerp(minShade, 1, grR);

					grL = GetGradient(largerH,grL,smallerH);
					grL = MathHelper.Lerp(minShade, 1, grL);

					grUR = GetGradient(grUR, smallerH, largerH);
					grUR = MathHelper.Lerp(minShade, 1, grUR);
					
					grUL = GetGradient(grUL, smallerH, largerH);
					grUL = MathHelper.Lerp(minShade, 1, grUL);



					

					int ind = 
					mm.PushVertex(x+1, largerH, z+1,  grUR, grUR, grUR,  right, -largerH,  0, 0, dir);
					mm.PushVertex(x+0, largerH, z+1,  grUL, grUL, grUL,  left, -largerH,  0, 0, dir);

					mm.PushVertex(x+1, smallerH, z+1,  grR, grR, grR,  right, -smallerH,  0, 0, dir);
					mm.PushVertex(x+0, smallerH, z+1,  grL, grL, grL,  left, -smallerH,  0, 0, dir);

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

		
		Console.WriteLine($"{mm.VertexNumber / 4}, {tiles.Length * 4}");

	}
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	static float GetGradient (float top, float bottom, float med) {
		if (top == bottom)
			return 0;
		return (med-bottom) / (top-bottom);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	float GetSmallistTile (int x, int z, int offsetX, int offsetZ) {
		return MathF.Min(
			MathF.Min(GetHeight(x, z), GetHeight(x + offsetX, z)),
			MathF.Min(GetHeight(x, z+offsetZ), GetHeight(x+offsetX, z+offsetZ))
		);
	}
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	float GetLargestTile (int x, int z, int offsetX, int offsetZ) {
		return MathF.Max(
			MathF.Max(GetHeight(x, z), GetHeight(x + offsetX, z)),
			MathF.Max(GetHeight(x, z+offsetZ), GetHeight(x+offsetX, z+offsetZ))
		);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetTileType (int tile) {
		return Math.Abs(tile % 10);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetTileHeight (int tile) {
		return (tile/10) * tileHeight;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public float GetHeight (int x, int z) {
		return GetTileHeight(GetTile(x, z));
	}

	public int GetTile (int x, int z) {
		if (x < 0 || x >= tiles.GetLength(1) || z < 0|| z >= tiles.GetLength(0))
			return defaultTile;
		
		return tiles[z,x];
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public void HandleCollision (Collider coll, int sampleSize = 1) {
		for (int i = 0; i < 3; i++){
			CollisionData data = new CollisionData (coll);
			for (int z = -sampleSize; z <= sampleSize; z++)
				for (int x = -sampleSize; x <= sampleSize; x++) {
					collider.posX = x + MathF.Floor(coll.posX) + 0.5f;
					collider.posZ = z + MathF.Floor(coll.posZ) + 0.5f;
					collider.posY = GetHeight(x + (int)coll.posX,z + (int)coll.posZ) - collider.height;
					data.Merge(coll.GetCollision(collider));
				}
			if (!data.overlapped)return;
			data.Bump(tileHeight);
		}
	}
}
}