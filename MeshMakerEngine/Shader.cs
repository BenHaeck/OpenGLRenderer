using OpenTK;
using OpenTK.Graphics.OpenGL4;
public class Shader : IDisposable {
	public readonly int handle = 0;
	bool disposed = false;

	Dictionary<string, int> validUniform = new Dictionary<string, int>();

	public const string DEF_VERT = @"
# version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 texCo;

out vec2 texCoord;
out vec4 color;

void main () {
	gl_Position = vec4 (aPosition, 1.0);
	color = vec4(aColor, 1);
	texCoord = texCo;
}
	", DEF_FRAG = @"
# version 330 core

in vec2 texCoord;
in vec4 color;

uniform sampler2D texture0;

out vec4 FragColor;
void main () {
	FragColor = color * texture(texture0, texCoord);
}
";

	public Shader (string? fragmentCode = null, string? vertexCode = null) {
		if (vertexCode == null) vertexCode = DEF_VERT;
		
		if (fragmentCode == null || fragmentCode.ToUpper() == "DEFF") fragmentCode = DEF_FRAG;
		// vertex shader
		int vertexShader = GL.CreateShader(ShaderType.VertexShader); // creates the shader
		GL.ShaderSource(vertexShader, vertexCode); // sends the source code to openGL

		GL.CompileShader(vertexShader); // compiles the source code

		// Prints errors
		int success = 0;
		GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
		if (success == 0) {
			Console.WriteLine ("VERTEX " + GL.GetShaderInfoLog(vertexShader));
		}
		
		// fragment shader
		int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragmentCode); // sends over the source code

		GL.CompileShader(fragmentShader); // compiles the shader

		// prints errors 
		GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
		if (success == 0) {
			Console.WriteLine ("FRAG " + GL.GetShaderInfoLog(fragmentShader));
		}

		handle = GL.CreateProgram();
		GL.AttachShader(handle, vertexShader);
		GL.AttachShader(handle, fragmentShader);

		GL.LinkProgram(handle);

		GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
		if (success == 0) {
			Console.WriteLine("ERROR: " + GL.GetProgramInfoLog(handle));
		}

		Game.PrintErrors();
	}

	public void Use () {
		if (disposed == false)
			GL.UseProgram(handle);

		//Game.PrintErrors();
	}

	public void EditUniformV4 (string name, float v1, float v2, float v3, float v4) {
		int loc;
		if (validUniform.ContainsKey(name))
			loc = validUniform[name];
		else {
			loc = GL.GetUniformLocation(handle, name);
			if (loc == -1) {
				Console.WriteLine("ERROR: location does not exist");
				return;
			}
			validUniform.Add(name, loc);
		}

		GL.Uniform4(loc, v1, v2, v3, v4);

		Game.PrintErrors();
	}

	public void Dispose () {
		if (disposed == false)
			GL.DeleteProgram(handle);
		
		disposed = true;
	}

	~Shader () {
		if (!disposed) {
			GL.DeleteProgram(handle);
		}
	}
}