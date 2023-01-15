using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

using MBEngine.World;

namespace MBEngine {

public class ObjectRenderer {
	public MeshMaker meshMaker;
	// the maximum number of objects the renderer can make
	public readonly int maxObjects;
	public ObjectRenderer (int maxObjects) {
		meshMaker = new MeshMaker(maxObjects * 4, maxObjects * 6, BufferUsageHint.StreamDraw);
		meshMaker.useindices = true;;
		this.maxObjects = maxObjects;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public void AddObject (
		float posX, float posY, float posZ, float width, float height, float rotH = 0, TextureSlice? ts = null
	) {
		if (Renderer.ShouldBeCulled(posX, posY, posZ, MathF.Max(height, width) * 1.5f))
			return;
		float dirX =-MathF.Sin(rotH);
		float dirZ = MathF.Cos(rotH);

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
}