using OpenTK.Windowing.Desktop;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;


namespace MBEngine {
public static class Renderer {
	//static App? App;
	
	static Shader? defaultShader;
	public static Shader? DefaultShader => defaultShader;
	static Shader? objectShader;
	public static Shader? ObjectShader => objectShader;

	static int width = 800, height = 600;

	public static int Width => width;
	public static int Height => height;

	public static Color4 clearColor;

	public static float AspectRatio => height / (float)width;

	//public static GameWindow app = null;
	
	public static Camera? mainCamera = null;

	public static void Init (App App) {
		//Renderer.App = App;
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

	public static void UpdateShaderComplete (Shader shader) {
		UpdateShader(shader);
		LightingSystem.SendLightInfo(shader);
		if (mainCamera != null)
			mainCamera.SendToShader(shader);
	}

	public static bool ShouldBeCulled (float x, float y, float z, float r) {
		if (mainCamera == null)
			return false;
		
		return mainCamera.ShouldBeCulled(x, y, z, r);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static void EndDraw () {
		if (App.mainApp != null)
			App.mainApp.SwapBuffers(); // swaps the buffers to avoid screen taring
	}
}

}