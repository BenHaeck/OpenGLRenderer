public static class LightingSystem {
	// number of floats per light
	public const int LIGHT_SIZE = 7;
	
	// maximum number of lights
	public const int LIGHT_NUMBER_MAX = 10;

	public static readonly string LIGHTING_CODE = @"
uniform float[LIGHT_SIZE*LIGHT_NUMBER] lightVals;
uniform int lightNumber;
uniform vec3 ambientLight;

vec3 getSimpleLight (vec3 pos) {
	vec3 lightChange = vec3(0,0,0);
	for (int i = 0; i < lightNumber; i++) {
		int si = i * LIGHT_SIZE;
		vec3 lightPos = vec3(lightVals[si+0], lightVals[si+1], lightVals[si+2]);
		float radius = lightVals[si + 3];
		vec3 lightColor = vec3(lightVals[si+4], lightVals[si+5], lightVals[si+6]);

		vec3 distSqr3 = lightPos - pos;
		distSqr3 *= distSqr3;
		float dist = distSqr3.x + distSqr3.y + distSqr3.z;
		if (dist > radius * radius || radius <= 0.01) {
			continue;
		}

		dist = sqrt(dist);

		lightChange += vec3(lightVals[si+4],lightVals[si+5],lightVals[si+6]) *
		(1 - dist / radius);
	}
	return lightChange * (1 - ambientLight) + ambientLight;
}

vec3 getLight (vec3 pos, vec3 normal) {
	vec3 lightChange = vec3(0,0,0);
	for (int i = 0; i < lightNumber; i++) {
		int si = i * LIGHT_SIZE;
		vec3 lightPos = vec3(lightVals[si+0], lightVals[si+1], lightVals[si+2]);
		float radius = lightVals[si + 3];
		vec3 lightColor = vec3(lightVals[si+4], lightVals[si+5], lightVals[si+6]);

		vec3 distSqr3 = lightPos - pos;
		distSqr3 *= distSqr3;
		float dist = distSqr3.x + distSqr3.y + distSqr3.z;
		if (dist > radius * radius || radius <= 0.01) {
			continue;
		}

		dist = sqrt(dist);

		lightChange += vec3(lightVals[si+4],lightVals[si+5],lightVals[si+6]) *
		max(dot(normalize(pos-lightPos),-normal),0.0) *
		(1 - dist / radius);

	}
	return lightChange * (1 - ambientLight) + ambientLight;
}
	".Replace("LIGHT_SIZE", ""+LIGHT_SIZE).Replace("LIGHT_NUMBER", ""+LIGHT_NUMBER_MAX);
	public static readonly ShaderLib lights = new ShaderLib("light", LIGHTING_CODE);

	static float [] lightVals = new float [LIGHT_SIZE*LIGHT_NUMBER_MAX];
	static int numberOfLights = 0;

	public static void SetAmbientLight (Shader shader, float r, float g, float b) {
		shader.SetUniformV3("ambientLight", r, g, b);
	}
	public static void SendLightInfo (Shader shader) {
		shader.SetUniformArray("lightVals", lightVals);
		shader.SetUniform("lightNumber", numberOfLights);
		//Console.WriteLine(numberOfLights);
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
	public static void Add (float posX, float posY, float posZ, float radius, float r, float g, float b) {
		if (numberOfLights >= LIGHT_NUMBER_MAX)
			return;
		
		int lightIndex = numberOfLights * LIGHT_SIZE;
		//Console.WriteLine(lightIndex);
		lightVals[lightIndex + 0] = posX;
		lightVals[lightIndex + 1] = posY;
		lightVals[lightIndex + 2] = posZ;

		lightVals[lightIndex + 3] = radius;

		lightVals[lightIndex + 4] = r;
		lightVals[lightIndex + 5] = g;
		lightVals[lightIndex + 6] = b;

		numberOfLights++;
	}

	public static void ClearLights () {
		numberOfLights = 0;
	}
}