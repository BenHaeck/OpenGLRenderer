using static System.MathF;
using System.Runtime.CompilerServices;

namespace MBEngine {
	public static class Helpers {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance2 (float x, float y, float z) {
			return x * x + y * y + z * z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance (float x, float y, float z) {
			return Sqrt(Distance2(x,y,z));
		}

		public static void Normalize (ref float x, ref float y, ref float z) {
			float mult = Distance2(x,y,z);
			if (mult <= 0) {
				x = 0;
				y = 0;
				z = 0;
			}

			mult = 1/Sqrt(mult);

			x *= mult;
			y *= mult;
			z *= mult;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Normalize (float x) {
			if (x > 0) return 1;
			if (x < 0) return -1;
			return 0;
		}
	}
}