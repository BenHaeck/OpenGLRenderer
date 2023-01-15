using OpenTK.Graphics.OpenGL4;
using StbImageSharp;


namespace MBEngine {

public class Texture {
	readonly int handle;

	public readonly int width;
	public readonly int height;
	//byte[] data;

	public Texture (string path) {
		//StbImage.stbi_set_flip_vertically_on_load(1);
		var image = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

		handle = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2D, handle);

		width = image.Width;
		height = image.Height;

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

		GL.BindTexture(TextureTarget.Texture2D, 0);
		App.PrintErrors();

	}

	public Texture (byte[] data, int width) {
		handle = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2D, handle);
		//this.data = data;
		var ndata = new byte[(4 * data.Length)/ 3];
		//Console.WriteLine($"data {this.data.Length / 4}, {data.Length / 3}");
		for (int i = 0; i < data.Length / 3; i++) {
			int ndi = i * 4, odi = i * 3;
			ndata[ndi  ] = data[odi  ];
			ndata[ndi+1] = data[odi+1];
			ndata[ndi+2] = data[odi+2];
			ndata[ndi+3] = 255;
		}

		this.width = width;
		this.height = data.Length / (3*width);

		if (ndata.Length / 4 % width != 0) {
			Console.WriteLine("ERROR: wrong width or number of color channels");
			this.width  = 0;
			this.height = 0;
		}

		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ndata);
		App.PrintErrors();
		//Console.WriteLine(ToString());
	}

	public override string ToString(){
		return $"[{width}, {height}]";
	}

	public void Use () {
		//GL.BindTexture(TextureTarget.Texture2D, handle);
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, handle);
	}

	public void SetWrapMode (TextureWrapMode wrap) {
		Use ();
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrap);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrap);
	}

	public void SetFilter (TextureMagFilter magFilter, TextureMinFilter minFilter) {
		Use();
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
	}
}
}