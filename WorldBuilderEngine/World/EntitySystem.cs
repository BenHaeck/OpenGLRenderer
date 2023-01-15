using System.Runtime.CompilerServices;
using MBEngine.Physics;
namespace MBEngine.World {
	public interface IEntity {
		public Collider GetCollider ();
		public bool Dead ();
		public void Start ();
		public void Update (float dt);

		public void Draw (ObjectRenderer renderer);

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		public static void UpdateAll <E> (List<E> entities, float dt) where E: IEntity {
			for (int i = entities.Count - 1; i >= 0; i--) {
				if (entities[i].Dead())
					entities.RemoveAt(i);
				else
					entities[i].Update(dt);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		public static void DrawAll <E> (List<E> entities, ObjectRenderer renderer) where E: IEntity {
			for (int i = 0; i < entities.Count; i--) {
				entities[i].Draw(renderer);
			}
		}
	}
}