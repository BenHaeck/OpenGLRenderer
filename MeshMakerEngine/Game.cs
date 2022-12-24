using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

public class Game: GameWindow{
	Shader shader;
	Texture texture;
	float[] verteces = {
		// position			color
		-0.5f, -0.5f, 0.0f,		1, 0, 0,	// bottom left
		 0.5f, -0.5f, 0.0f,		0, 1, 0,	// bottom right
		-0.5f,  0.5f, 0.0f,		0, 0, 1,	// top left
		 0.5f,  0.5f, 0.0f,		0, 1, 1, 	// top right
	};

	uint[] indices = {
		0, 1, 2, // first	triangle
		2, 3, 1  // second	triangle
	};

	int VertexBufferObject;
	int vertexArrayObject;
	int elementBufferObject;

	MeshMaker mm;

	public Game (int width, int height, string name = "Window!") :
	base (GameWindowSettings.Default, new NativeWindowSettings() {Size = (width, height), Title = name}) {
		shader = new Shader ("deff");
	}

	protected override void OnLoad(){
		base.OnLoad();

		GL.ClearColor(0.2f, 0.3f, 0.2f, 1.0f);

		texture = new Texture ("images\\Brick.png");

		mm = new MeshMaker(6, 6);
		mm.useIdices = true;
		mm.PushVertex(-0.6f, -0.6f, 0f,  1, 0, 0,  0.0f, 0.0f);
		mm.PushVertex( -0.6f,  0.6f, 0f,  0, 0, 1,  0f, 1.0f);
		mm.PushVertex( 0.6f, -0.6f, 0f,  0, 1, 0,  1.0f, 0.0f);
		mm.PushVertex( 0.6f,  0.6f, 0f, 1, 1, 1, 1, 1);

		mm.PushIndex(0);
		mm.PushIndex(1);
		mm.PushIndex(2);

		mm.PushIndex(1);
		mm.PushIndex(2);
		mm.PushIndex(3);


		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

		//mm.PushIndex(1);
		//mm.PushIndex(2);
		//mm.PushIndex(3);
		Console.WriteLine(mm.IndexNumber + ", " + mm.VertexNumber);
		mm.Apply();


		// vertex array
		vertexArrayObject = GL.GenVertexArray(); // creates the array object
		GL.BindVertexArray(vertexArrayObject); // binds it

		// vertex Buffer
		VertexBufferObject = GL.GenBuffer(); // creates the vertex buffer in gpu memory
		GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); // binds it making it current
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StaticDraw); // sends the buffer data

		// element buffer
		elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

		// defines the position atribute and binds it to the buffer object
		GL.VertexAttribPointer(0,3, VertexAttribPointerType.Float, false, 6*sizeof(float), 0);
		GL.EnableVertexAttribArray(0); // enables the attribute

		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
		GL.EnableVertexAttribArray(1);
		//GL.VertexAttribPointer(1, 3);
		// GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbinds the buffer
		GL.BindVertexArray(0);

		PrintErrors();
	}

	protected override void OnUpdateFrame(FrameEventArgs args){
		base.OnUpdateFrame(args);

		var input = KeyboardState;
		if (input.IsKeyDown(Keys.Escape)) {
			Close();
		}
		//verteces [12] += 0.5f * ((float)args.Time);
		//GL.BindVertexArray(vertexArrayObject);
		//GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
		//GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);
		//GL.BindVertexArray(0);
	}

	protected override void OnResize(ResizeEventArgs e){
		base.OnResize(e);
		GL.Viewport(0,0, e.Width, e.Height);
	}

	protected override void OnRenderFrame(FrameEventArgs args){
		base.OnRenderFrame(args);
		GL.Clear(ClearBufferMask.ColorBufferBit);// clears the buffer

		//sshader.Use();
		//sGL.BindVertexArray(vertexArrayObject);
		//sGL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
		//sGL.BindVertexArray(0);
		shader.Use();
		texture.Use();
		mm.Draw();
		SwapBuffers(); // swaps the buffers to avoid screen taring
		Game.PrintErrors();
	}

	public static void PrintErrors () {
		var ec = GL.GetError();;
			
		while (ec != OpenTK.Graphics.OpenGL4.ErrorCode.NoError) {
			Console.WriteLine(ec);
			ec = GL.GetError();
		}
	}
}

