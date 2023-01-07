using OpenTK.Windowing.Desktop;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
public static class Renderer {
	static Game? game;
	
	static Shader? defaultShader;
	public static Shader? DefaultShader => defaultShader;
	static Shader? objectShader;
	public static Shader? ObjectShader => objectShader;

	static int width = 800, height = 600;

	public static int Width => width;
	public static int Height => height;

	public static Color4 clearColor;

	public static float AspectRatio => height / (float)width;

	public static GameWindow window = null;
	

	public static void Init (Game game) {
		Renderer.game = game;
		defaultShader = new Shader("deff");

		objectShader = new Shader(@"
			#version 330 core
			//$light

			in vec2 texCoord;
			in vec4 color;

			uniform sampler2D texture0;

			out vec4 FragColor;
			in vec3 normal;
			in vec3 fragPos;

			uniform vec3 clearColor;
			//uniform sampler2D texture0;

			void main() {
				vec4 col = texture(texture0, texCoord);
				if (col.a < 0.5)
					discard;
				else{
					col *= color * vec4(getSimpleLight(fragPos), 1);
					FragColor = col;
				}
			}
		");

		GL.CullFace(CullFaceMode.Back);
		GL.FrontFace(FrontFaceDirection.Ccw);
	}
	
	public static void OnResize(int width, int height) {
		Renderer.width = width;
		Renderer.height = height;
	}

	public static void UpdateShader (Shader shader) {
		shader.supressLocationError = true;
		shader.SetUniform("aspectRatio", AspectRatio);
		shader.SetUniformV3("clearColor", clearColor.R, clearColor.G, clearColor.B);
		shader.supressLocationError = false;

	}

	public static void BeginDraw () {
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);// clears the buffer
	}

	public static void Draw (MeshMaker mesh, Texture texture, Shader? shader = null) {
		if (shader != null)
			shader.Use();
		else if (defaultShader != null)
			defaultShader.Use();
		else
			return;

		texture.Use();
		mesh.Draw();
	}

	public static void EndDraw () {
		if (game != null)
			game.SwapBuffers(); // swaps the buffers to avoid screen taring
	}
}