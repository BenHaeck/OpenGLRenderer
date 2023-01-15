using OpenTK.Windowing;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Input;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
namespace MBEngine {

	public static class KAndMInput {
		static KeyboardState keyboard;
		public static ref KeyboardState Keyboard => ref keyboard;

		static Vector2 mousePosition;
		public static Vector2 MousePosition => mousePosition;
		
		static Vector2 scroll;
		public static Vector2 Scroll => Scroll;

		static bool rightMouse, middleMouse, leftMouse;

		public static bool RightMouse => rightMouse;
		public static bool MiddleMouse => middleMouse;
		public static bool LeftMouse => leftMouse;

		public static void Update () {
			if (App.mainApp != null){
				keyboard = App.mainApp.KeyboardState;
				var ms = App.mainApp.MouseState;
				mousePosition = App.mainApp.MousePosition;
				scroll = ms.ScrollDelta;
				leftMouse = ms.IsButtonDown(MouseButton.Left);
				middleMouse = ms.IsButtonDown(MouseButton.Middle);
				rightMouse = ms.IsButtonDown(MouseButton.Right);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]	
		public static bool GetKey (Keys key) {
			return keyboard.IsKeyDown(key);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetKeyDown (Keys key) {
			return keyboard.IsKeyPressed(key);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetKeyUp (Keys key) {
			return keyboard.IsKeyReleased(key);
		}
	}
}