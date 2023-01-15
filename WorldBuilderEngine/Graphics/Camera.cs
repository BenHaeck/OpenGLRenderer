using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using OpenTK.Input;
using static System.MathF;

namespace MBEngine {

public class Camera {
	// public variables
	public float posX = 0, posY = 0, posZ = 0; // position
	public float rotationH = 0, rotationV = 0;// rotation

	float cullPosX = 0;
	float cullPosY = 0;
	float cullPosZ = 0;
	float cullRadius = 12;

	public float CullPosX => cullPosX;
	public float CullPosZ => cullPosZ;
	
	// fov setter
	public float FOV {
		set {fovMult = 1/MathF.Tan(value * 0.5f * MathF.PI / 180);}
	}

	// fovMult, the depth multiplier of the camera
	float fovMult = 1;


	// transform world to local
	float camHCos = 1, camHSin = 0;
	float camVCos = 1, camVSin = 0;
	
	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public void GenTransforms () {
		rotationH %= PI * 2;
		rotationV = Min(PI / 2, Max(-PI/2, rotationV));
		//Console.WriteLine (rotationV);


		camHCos = Cos(rotationH);
		camHSin = Sin(rotationH);

		camVCos = Cos(rotationV);
		camVSin = Sin(rotationV);

		cullPosX = camHSin * cullRadius * camVCos + posX;
		cullPosY = camVSin * cullRadius + posY;
		cullPosZ = camHCos * cullRadius * camVCos + posZ;
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public void SendToShader (Shader shader) {

		shader.supressLocationError = true;
		shader.SetUniformV3 ("camPos", posX, posY, posZ);
		shader.SetUniformV4 ("camTr", camVCos, camVSin, camHCos, camHSin);
		//shader.SetUniformV2 ("camTrV", camVCos, camVSin);
		shader.SetUniform ("fovMult", fovMult);
		shader.supressLocationError = false;

	}

	//[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public bool ShouldBeCulled (float posX, float posY, float posZ, float radius) {
		float distX = cullPosX - posX;
		float distY = cullPosY - posY;
		float distZ = cullPosZ - posZ;
		float cRadius = radius + cullRadius;
		return distX * distX + distY * distY + distZ * distZ > cRadius * cRadius; 
	}

	public static readonly ShaderLib vertexCamTr = new ShaderLib ("camTr", @"
uniform vec3 camPos;
uniform vec4 camTr;
uniform float fovMult;

vec2 rotate (vec2 v, vec2 tr) {
	return v.x * tr + v.y * tr.yx * vec2(-1, 1);
}

vec3 rotateWithCamera (vec3 v) {
	vec3 v2 = v;
	v2.xz = rotate(v2.xz, camTr.zw);
	v2.yz = rotate(v2.yz, camTr.xy);
	return v2;
}

vec3 cameraTr (vec3 pos) {
	vec3 nPos = pos - camPos;
	//nPos.z += 1;
	
	return rotateWithCamera(nPos);
}

", false);
	

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public void ControlCamera (float speedH, float speedV, float mouseSpeed, float dt){
		
		var win = App.mainApp;
		//var input = win.KeyboardState;
		var mm = win.MousePosition;
		var halfSize = new Vector2(Renderer.Width, Renderer.Height) * 0.5f;

		halfSize = new Vector2(MathF.Floor(halfSize.X), MathF.Floor(halfSize.Y));
		
		var dir = Vector3.Zero;
		if (!KAndMInput.GetKey(Keys.O)){
			rotationH += ((mm.X - halfSize.X) * mouseSpeed * 0.25f) * dt;
			rotationV -= ((mm.Y - halfSize.Y) * mouseSpeed * 0.25f) * dt;
			win.MousePosition = halfSize;
		}


		if (KAndMInput.GetKey(Keys.D))
			dir.X += 1;

		if (KAndMInput.GetKey(Keys.A))
			dir.X -= 1;

		if (KAndMInput.GetKey(Keys.Space))
			dir.Y += 1;

		if (KAndMInput.GetKey(Keys.LeftControl))
			dir.Y -= 1;

		if (KAndMInput.GetKey(Keys.W))
			dir.Z += 1;

		if (KAndMInput.GetKey(Keys.S))
			dir.Z -= 1;
		var moveTr = new Vector2(MathF.Cos(rotationH), MathF.Sin(rotationH));
		posX += (dir.X * moveTr.X + dir.Z * moveTr.Y) * dt * speedH;
		posY += dir.Y * dt * speedV;
		posZ += (-dir.X * moveTr.Y + dir.Z * moveTr.X) * dt * speedH;
	}
}

}