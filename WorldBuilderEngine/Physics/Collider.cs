using System.Runtime.CompilerServices;
using static System.MathF;
namespace MBEngine.Physics {
	public class Collider {
		
		public float posX = 0, posY = 0, posZ = 0;
		public float width = 1, height = 1, length = 1;
		public int hitDirX = 0, hitDirY = 0, hitDirZ = 0;

		public Collider(){}
		public Collider(float posX, float posY, float posZ, float width, float height, float length){
			this.posX = posX;
			this.posY = posY;
			this.posZ = posZ;
			this.width = width;
			this.height = height;
			this.length = length;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public CollisionData GetCollision(Collider coll) {
			float dx = Abs(posX - coll.posX), dy = Abs(posY - coll.posY), dz = Abs(posZ - coll.posZ);
			float cw = this.width + coll.width, ch = this.height + coll.height, cl = this.length + coll.length;
			return new CollisionData(cw - dx, ch - dy, cl - dz, this, coll);
		}
	}


	public struct CollisionData {
		public float overlapX, overlapY, overlapZ;
		public sbyte distX, distY, distZ;
		public float minOverlap;
		public bool overlapped;
		public Collider mainCollider;
		public Collider? otherCollider;

		public CollisionData (Collider mc) {
			overlapX = -1;
			overlapY = -1;
			overlapZ = -1;
			distX = -1;
			distY = -1;
			distZ = -1;
			minOverlap = -1;
			overlapped = false;
			mainCollider = mc;
			otherCollider = null;
		}

		//[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
		public CollisionData (float ox, float oy, float oz, Collider c1, Collider c2) {
			overlapX = ox;
			overlapY = oy;
			overlapZ = oz;
			distX = (sbyte)Helpers.Normalize(c2.posX - c1.posX);
			distY = (sbyte)Helpers.Normalize(c2.posY - c1.posY);
			distZ = (sbyte)Helpers.Normalize(c2.posZ - c1.posZ);
			minOverlap = Min(ox, Min(oy, oz));
			overlapped = minOverlap > 0;
			mainCollider = c1;
			otherCollider = c2;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
		public void Merge (CollisionData collDat) {
			if (mainCollider != collDat.mainCollider || !collDat.overlapped)
				return;

			if (collDat.minOverlap > minOverlap){
				this = collDat;
				//Console.WriteLine(minOverlap);
			}
		}

		public void Bump (float biasY = 0) {
			if (!overlapped || otherCollider == null)
				return;

			if (overlapY - biasY < overlapX && overlapY - biasY  < overlapZ) {
				mainCollider.hitDirY = distY;
				mainCollider.posY -= overlapY * distY;
			}
			else if (overlapX < overlapZ) {
				mainCollider.hitDirX = distX;

				mainCollider.posX -= overlapX * distX;
			} else {
				mainCollider.hitDirZ = distZ;

				mainCollider.posZ -= overlapZ * distZ;
			}

		}
	}
}