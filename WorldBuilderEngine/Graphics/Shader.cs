using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MBEngine {

public struct ShaderLib {
	public ShaderLib (string name, string code, bool collapse = true) {
		this.name = name;

		//if (collapse)
		//	code = code.Replace('\n', ' ');

		this.code = code;
	}
	string name, code;
	public string AddToShader (string shaderCode) {
		Console.WriteLine ("Replace " + @"//$"+name);
		return shaderCode.Replace(@"//$"+name, code);
	}

	public static string AddToShader (string shaderCode, ShaderLib[] libs) {
		for (int i = 0; i < libs.Length; i++) {
			shaderCode = libs[i].AddToShader(shaderCode);
		}
		return shaderCode;
	}
}

public class Shader : IDisposable {
	public readonly int handle = 0;
	bool disposed = false;

	public bool supressLocationError = false;

	Dictionary<string, int> validUniform = new Dictionary<string, int>();

	public static Shader? lastShaderRecived = null;

	public static readonly ShaderLib /*fragVals = new ShaderLib ("fragVals", @"
in vec2 texCoord;
in vec4 color;

uniform sampler2D texture0;

out vec4 FragColor;"),*/
	vertVals = new ShaderLib("vertVals", @"
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec2 aTexCoord;
out vec2 texCoord;
out float depth;
out vec4 color;
uniform float aspectRatio = 1;

vec3 fixDepth (vec3 pos) {
	pos.z-=0.05;
	return pos;
}

vec2 fixAspectRatio(vec2 pos) {
	if (aspectRatio == 0)
		return pos;
	return pos * vec2(aspectRatio, 1);
}
");

/*
FRAG_TEMPLATE
# version 330 core

//$fragvals$

void main () {
	FragColor = fragFunc ();
}
VERT_TEMPLATE
# version 330 core

//$vertVals

void main () {
	vertFunc();
}*/


	public const string DEF_VERT = @"
# version 330 core

//$vertVals
//$camTr

out vec3 normal;

out float deep;

out vec3 fragPos;
void main () {
	fragPos = aPosition;
	vec3 tempPos = cameraTr(aPosition);
	tempPos.xy = fixAspectRatio(tempPos.xy) * fovMult;
	gl_Position = vec4(fixDepth(tempPos), tempPos.z);
	color = vec4(aColor, 1);
	
	normal = aNormal;
	//if (fixDepth(tempPos).z > 1 || fixDepth(tempPos).z < 0)color = vec4 (1,0,0,1);
	texCoord = aTexCoord;
}", DEF_FRAG = @"
# version 330 core
in vec2 texCoord;
in vec4 color;

uniform sampler2D texture0;

out vec4 FragColor;
//$light
in vec3 normal;
in vec3 fragPos;
uniform vec3 clearColor;
void main () {

	vec4 col = color * texture (texture0, texCoord) * vec4(getLight(fragPos, normal),1);
	FragColor = col;
	if (col.rgb == clearColor)
		FragColor = vec4(col.rgb,0);
}";

	public Shader (string? fragmentCode = null, string? vertexCode = null) {
		if (vertexCode == null)
			vertexCode = DEF_VERT;

		
		vertexCode = vertVals.AddToShader(vertexCode);

		vertexCode = Camera.vertexCamTr.AddToShader(vertexCode);
		
		if (fragmentCode == null || fragmentCode.ToUpper() == "DEFF")
			fragmentCode = DEF_FRAG;	

	//	fragmentCode = fragVals.AddToShader(fragmentCode);
		fragmentCode = LightingSystem.lights.AddToShader(fragmentCode);


		// vertex shader
		int vertexShader = GL.CreateShader(ShaderType.VertexShader); // creates the shader
		GL.ShaderSource(vertexShader, vertexCode); // sends the source code to openGL

		GL.CompileShader(vertexShader); // compiles the source code

		// Prints errors
		int success = 0;
		GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
		if (success == 0) {
			Console.WriteLine ($"VERTEX\n{vertexCode}\n{GL.GetShaderInfoLog(vertexShader)}");
		}
		
		// fragment shader
		int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragmentCode); // sends over the source code

		GL.CompileShader(fragmentShader); // compiles the shader

		// prints errors 
		GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
		if (success == 0) {
			Console.WriteLine ($"FRAG\n{fragmentCode}\n{GL.GetShaderInfoLog(fragmentShader)}");
		}

		handle = GL.CreateProgram();
		GL.AttachShader(handle, vertexShader);
		GL.AttachShader(handle, fragmentShader);

		GL.LinkProgram(handle);

		GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
		if (success == 0) {
			Console.WriteLine($"ERROR: {GL.GetProgramInfoLog(handle)}");
		}

		//SetUniformV4("tint", 1f, 1f, 1f, 1f);

		//App.PrintErrors();
	}

	public void Use () {
		if (disposed == false)
			GL.UseProgram(handle);

		lastShaderRecived = this;
	}

	void UseToSend() {
		if (lastShaderRecived != this) {
			//Console.WriteLine("Used");
			Use();
			lastShaderRecived = this;
		}
	}

	public int GetLocation (string name) {
		int loc;
		if (validUniform.ContainsKey(name))
			loc = validUniform[name];

		else {
			loc = GL.GetUniformLocation(handle, name);
			validUniform.Add(name, loc);

			if (loc == -1) {
				if (!supressLocationError)
					Console.WriteLine($"ERROR: location for {name} does not exist");
				return -1;
			}
		}

		//App.PrintErrors("GetLocationFunc");

		//Console.WriteLine($"name: {name}, location {loc}");
		
		return loc;
	}

	public void SetUniformV4 (string name, float v1, float v2, float v3, float v4) {
		int loc = GetLocation (name);

		if (loc <= -1) 
			return;
		

		UseToSend();
		GL.Uniform4(loc, v1, v2, v3, v4);
		//App.PrintErrors("UniformV4");
	}

	public void SetUniformV4 (string name, int v1, int v2, int v3, int v4) {
		int loc = GetLocation (name);

		if (loc == -1) 
			return;
		

		UseToSend();
		GL.Uniform4(loc, v1, v2, v3, v4);

		//App.PrintErrors("UniformIV4");
	}

	public void SetUniformV3 (string name, float v1, float v2, float v3) {
		int loc = GetLocation (name);

		if (loc == -1) 
			return;
		
		UseToSend();
		GL.Uniform3(loc, v1, v2, v3);

		//App.PrintErrors("UniformV3");
	}

	public void SetUniformV3 (string name, int v1, int v2, int v3) {
		int loc = GetLocation (name);

		if (loc == -1) 
			return;
		
		UseToSend();
		GL.Uniform3(loc, v1, v2, v3);

		//App.PrintErrors("UniformV3");
	}

	public void SetUniformV2 (string name, float v1, float v2) {
		int loc = GetLocation (name);

		if (loc == -1)
			return;

		UseToSend();
		GL.Uniform2(loc, v1, v2);

		//App.PrintErrors("UniformV2");
	}

	public void SetUniformV2 (string name, int v1, int v2) {
		int loc = GetLocation (name);

		if (loc == -1)
			return;

		UseToSend();
		GL.Uniform2(loc, v1, v2);

		//App.PrintErrors("UniformV2");
	}

	public void SetUniform (string name, float v) {
		int loc = GetLocation (name);

		if (loc == -1)
			return;

		UseToSend();
		GL.Uniform1(loc, v);

		//App.PrintErrors("Uniform");
	}

	public void SetUniform (string name, int v) {
		int loc = GetLocation (name);
		
		if (loc == -1)
			return;

		UseToSend();
		GL.Uniform1(loc, v);

		//App.PrintErrors("Uniform");
	}

	public void SetUniformArray (string name, float[] array) {
		int loc = GetLocation (name);

		if (loc <= -1)
			return;
		
		UseToSend();
		GL.Uniform1(loc, array.Length, array);
	}

	public void SetUniformArray (string name, int[] array) {
		int loc = GetLocation (name);

		if (loc <= -1)
			return;
		
		UseToSend();
		GL.Uniform1(loc, array.Length, array);
	}

	public void SetUniformM4(string name, OpenTK.Mathematics.Matrix3 matrix){
		int loc = GetLocation(name);
		UseToSend();
		GL.UniformMatrix3(loc, false, ref matrix);
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
}