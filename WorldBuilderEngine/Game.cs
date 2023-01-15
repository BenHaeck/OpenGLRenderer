namespace MBEngine {
	public class BaseGame {
		public virtual void Start () {}

		public virtual void OnResize (int screenWidth, int screenHeight) {}

		public virtual void Update (float dt) {}

		public virtual void OnRender () {}

		public virtual void OnEnd(){  }
	}

	public struct Level {
		public short [,] tiles;
		EntityMarker[] entityMarkers;
	}

	public struct EntityMarker {
		public EntityMarker (byte x, byte y, byte z, char entType = ' ', LookDir dir = LookDir.NoDirection) {
			posX = x;
			posY = y;
			posZ = z;
			entityType = entType;
			this.dir = dir;
			
		}

		public byte posX, posY, posZ;
		public LookDir dir;

		public char entityType = ' ';
	}

	public enum LookDir {
		NoDirection = 0,
		Left = 1<<1, Right = 1,
		Up = 1<<3, Down = 1<<4
	}
}