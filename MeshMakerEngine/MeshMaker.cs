using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
public class MeshMaker: IDisposable {
	public bool useIdices = false;

	// the size of the vertex
	// editable only by things that inharit from the meshMaker
	protected int VERTEX_SIZE = 8;

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

	bool disposed = false;	

	public MeshMaker (int numberOfVerteces, int numberOfIndices) {
		verteces = new float[numberOfVerteces * VERTEX_SIZE];
		indices = new uint[numberOfIndices];
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
	}

	public void PushVertex (float x, float y, float z, float r = 1, float g = 1, float b = 1, float tpX = 0, float tpY = 0) {
		if (vertexNumber * VERTEX_SIZE >= verteces.Length)
			return;
		
		int ind = vertexNumber * VERTEX_SIZE;
		verteces[ind] = x;
		verteces[ind + 1] = y;
		verteces[ind + 2] = z;

		verteces[ind + 3] = r;
		verteces[ind + 4] = g;
		verteces[ind + 5] = b;

		verteces[ind + 6] = tpX;
		verteces[ind + 7] = tpY;

		vertexNumber++;
	}

	public void PushIndex (uint i) {
		if (indexNumber >= indices.Length)
			return;
		
		indices[indexNumber] = i;
		indexNumber++;
	}

	public void Apply () {
		GL.BindVertexArray(vao);

		GL.BindBuffer(BufferTarget.ArrayBuffer,vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (useIdices) {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elm);
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StreamDraw);
		}

		GL.BindVertexArray(0);
		GL.BindBuffer(BufferTarget.ArrayBuffer,0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
	}

	public void Draw () {
		GL.BindVertexArray(vao);
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		//GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * verteces.Length, verteces, BufferUsageHint.StreamDraw);

		if (useIdices){
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
}