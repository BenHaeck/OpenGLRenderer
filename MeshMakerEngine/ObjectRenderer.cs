using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class ObjectRenderer {
	public MeshMaker meshMaker;
	// the maximum number of objects the renderer can make
	public readonly int maxObjects;
	public ObjectRenderer (int maxObjects) {
		meshMaker = new MeshMaker(maxObjects * 4, maxObjects * 6);
		meshMaker.useindices = true;;
		this.maxObjects = maxObjects;
	}

	public void AddObject (
		float posX, float posY, float posZ, float width, float height, float rotH = 0, TextureSlice? ts = null
	) {
		float dirX = MathF.Cos(rotH);
		float dirZ = MathF.Sin(rotH);

		TextureSlice nts = new TextureSlice(0,0,1,1);
		if (ts.HasValue) {
			nts = ts.Value;
		}
		int i =
		meshMaker.PushVertex (posX + dirX * width, posY - height, posZ + dirZ * width, 1, 1, 1, nts.x2, nts.y2);
		meshMaker.PushVertex (posX + dirX * width, posY + height, posZ + dirZ * width, 1, 1, 1, nts.x2, nts.y1);

		meshMaker.PushVertex (posX - dirX * width, posY - height, posZ - dirZ * width, 1, 1, 1, nts.x1, nts.y2);
		meshMaker.PushVertex (posX - dirX * width, posY + height, posZ - dirZ * width, 1, 1, 1, nts.x1, nts.y1);

		meshMaker.PushIndex(i);
		meshMaker.PushIndex(i + 1);
		meshMaker.PushIndex(i + 2);

		
		meshMaker.PushIndex(i + 1);
		meshMaker.PushIndex(i + 2);
		meshMaker.PushIndex(i + 3);

	}
}
