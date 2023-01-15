using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using MBEngine.Physics;

using MBEngine.World;

namespace MBEngine {
	public class App: GameWindow{
		public static App? mainApp;

		public BaseGame? game = null;

		//Texture texture;
		//Texture character;

		//Collider coll = new Collider ();
		//Collider camColl = new Collider();

		//bool colOverlap = false;

		//MeshMaker mm;

		//HeightPlane hp;

		//ObjectRenderer objRend;

		//Camera camera = new Camera();
		//System.Diagnostics.Stopwatch collStopwatch = new System.Diagnostics.Stopwatch();

		public App (int width, int height, string name = "OpenTK Window!") :
		base (
			GameWindowSettings.Default,
			new NativeWindowSettings() {NumberOfSamples = 0, Size = (width, height), Title = name}
		) {
			//Renderer.app = this;
			
			mainApp = this;
		}

		protected override void OnLoad(){
			base.OnLoad();
			Renderer.Init(this);
			GL.Enable(EnableCap.DepthTest);
			GL.ClearColor(0.0f, 0.0f, 0.15f, 1.0f);
			//this.VSync = VSyncMode.On;
			

			if (game != null)
				game.Start();
			PrintErrors("Load");
		}

		
		protected override void OnUpdateFrame(FrameEventArgs args){
			base.OnUpdateFrame(args);
			KAndMInput.Update();

			//var input = KeyboardState;
			if (KAndMInput.GetKey(Keys.Delete)) {
				Close();
			}

			//Vector2 halfSize = new Vector2(Renderer.Width / 2, Renderer.Height / 2);
			
			

			//Renderer.DefaultShader.SetUniformArray("lightVals", new float[]{camera.posX, camera.posY - 0.5f, camera.posZ, 5, 1, 1f, 0.5f});
			

			if (game != null)
				game.Update((float)args.Time);
		}

		protected override void OnResize(ResizeEventArgs e){
			base.OnResize(e);
			GL.Viewport(0,0, e.Width, e.Height);
			Renderer.OnResize(e.Width,e.Height);
			if (game != null)
				game.OnResize(e.Width, e.Height);
			//Renderer.OnResize(e);
		}

		protected override void OnRenderFrame(FrameEventArgs args){
			base.OnRenderFrame(args);

			//sshader.Use();
			//sGL.BindVertexArray(vertexArrayObject);
			//sGL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
			//sGL.BindVertexArray(0);
			Renderer.BeginDraw();
			

			if (game != null)
				game.OnRender();
			Renderer.EndDraw();
			//collStopwatch.Stop();
			//App.PrintErrors("drawFrame");
			//Console.WriteLine(collStopwatch.ElapsedMilliseconds);
		}

		protected override void OnUnload(){
			base.OnUnload();
			if (game != null) 
				game.OnEnd();
		}

		public static void PrintErrors (string checkPos = "unknown") {
			var ec = GL.GetError();;
				
			while (ec != OpenTK.Graphics.OpenGL4.ErrorCode.NoError) {
				Console.WriteLine(checkPos +": " + ec);
				ec = GL.GetError();
			}
		}
	}


}