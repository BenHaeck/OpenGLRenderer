using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;

using System.Runtime;
using System.Runtime.CompilerServices;
/*public class Mesh {
	public bool useindices = false;

	// the size of the vertex
	// editable only by things that inharit from the meshMaker
	protected int VERTEX_SIZE = 11;

	// VERTEX array
	protected float[] verteces;
	protected int vertexNumber = 0;
	public int VertexNumber => vertexNumber;

	// index array
	protected uint[] indices;
	protected int indexNumber = 0;
	public int IndexNumber => indexNumber;

	// The object handles used by the MeshMaker
	protected readonly int vao = 0, vbo = 0, elm = 0;

	public bool updateGPU = false;

	bool disposed = false;

	public Mesh (float [] verteces, uint[] indices) {
		this.verteces = verteces;
		this.indices = indices;


		vao = GL.GenVertexArray();
		GL.BindVertexArray(vao);

		vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.DynamicDraw);

		elm = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.DynamicDraw);

		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 0);
		GL.EnableVertexAttribArray(0);

		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 3*sizeof(float));
		GL.EnableVertexAttribArray(1);

		GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 6*sizeof(float));
		GL.EnableVertexAttribArray(2);

		Apply();

		GL.BindVertexArray(0);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	void Apply () {
		//GL.BindVertexArray(vao);

		//GL.BindBuffer(BufferTarget.ArrayBuffer,vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (useindices) {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StreamDraw);
		}

		// GL.BindVertexArray(0);
		// GL.BindBuffer(BufferTarget.ArrayBuffer,0);
		// GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	public void Draw () {
		GL.BindVertexArray(vao);
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

		
		//GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (updateGPU)
			Apply();

		if (useindices){
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
			GL.DrawElements(PrimitiveType.Triangles, indexNumber, DrawElementsType.UnsignedInt, 0);
		} else
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertexNumber);

		GL.BindVertexArray(0);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	// clean up
	public void Dispose () {
		if (disposed) return;
		GL.DeleteBuffer(vbo);
		GL.DeleteVertexArray(vao);
	}

	~Mesh () {
		if (disposed) return;

		GL.DeleteBuffer(vbo);
		GL.DeleteVertexArray(vao);
	}
} */

public class MeshMaker: IDisposable {
	/*public MeshMaker (int vertexNumber, int indexNumber): base(new float [vertexNumber * 11], new uint[indexNumber]) {
		
	}*/

	public bool useindices = false;

	// the size of the vertex
	// editable only by things that inharit from the meshMaker
	protected int VERTEX_SIZE = 11;

	// VERTEX array
	protected float[] verteces;
	protected int vertexNumber = 0;
	public int VertexNumber => vertexNumber;

	// index array
	protected uint[] indices;
	protected int indexNumber = 0;
	public int IndexNumber => indexNumber;

	// The object handles used by the MeshMaker
	protected readonly int vao = 0, vbo = 0, elm = 0;

	public bool updateGPU = false;

	bool disposed = false;

	public MeshMaker (int vertexNum, int indexNumber) {
		this.verteces = new float[vertexNum * VERTEX_SIZE];
		this.indices = new uint [indexNumber];


		vao = GL.GenVertexArray();
		GL.BindVertexArray(vao);

		vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.DynamicDraw);

		elm = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.DynamicDraw);

		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 0);
		GL.EnableVertexAttribArray(0);

		GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 3*sizeof(float));
		GL.EnableVertexAttribArray(1);

		GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 6*sizeof(float));
		GL.EnableVertexAttribArray(2);

		GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, sizeof(float) * VERTEX_SIZE, 9*sizeof(float));
		GL.EnableVertexAttribArray(3);

		Apply();

		GL.BindVertexArray(0);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	void Apply () {
		//GL.BindVertexArray(vao);

		//GL.BindBuffer(BufferTarget.ArrayBuffer,vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (useindices) {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StreamDraw);
		}

		// GL.BindVertexArray(0);
		// GL.BindBuffer(BufferTarget.ArrayBuffer,0);
		// GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	public void Draw (Texture? tex = null) {
		GL.BindVertexArray(vao);
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

		
		//GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (updateGPU)
			Apply();

		updateGPU = false;
		if (tex != null) {
			tex.Use();
		}

		if (useindices){
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
			GL.DrawElements(PrimitiveType.Triangles, indexNumber, DrawElementsType.UnsignedInt, 0);
		} else
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertexNumber);

		GL.BindVertexArray(0);
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	// clean up
	public void Dispose () {
		if (disposed) return;
		GL.DeleteBuffer(vbo);
		GL.DeleteVertexArray(vao);
	}

	~MeshMaker () {
		if (disposed) return;

		GL.DeleteBuffer(vbo);
		GL.DeleteVertexArray(vao);
	}

	public void Reset () {
		vertexNumber = 0;
		indexNumber = 0;
	}

	public int PushVertex (float x, float y, float z, float r = 1, float g = 1, float b = 1, float tpX = 0, float tpY = 0, float normX = 1, float normY = 1, float normZ = 1) {
		if (vertexNumber * VERTEX_SIZE >= verteces.Length)
			return vertexNumber;
		
		updateGPU = true;
		
		int ind = vertexNumber * VERTEX_SIZE;
		verteces[ind] = x;
		verteces[ind + 1] = y;
		verteces[ind + 2] = z;

		verteces[ind + 3] = r;
		verteces[ind + 4] = g;
		verteces[ind + 5] = b;

		verteces[ind + 6] = normX;
		verteces[ind + 7] = normY;
		verteces[ind + 8] = normZ;

		verteces[ind + 9] = tpX;
		verteces[ind +10] = tpY;

		vertexNumber++;
		return vertexNumber-1;
	}

	public void PushIndex (int i) {
		if (i < 0) return;
		PushIndex((uint)i);
	}

	public void PushIndex (uint i) {
		if (indexNumber >= indices.Length)
			return;
		
		indices[indexNumber] = i;
		indexNumber++;
		updateGPU = true;
	}
}