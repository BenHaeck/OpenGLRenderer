
namespace MBEngine {

public struct TextureSlice {
	public readonly float x1, y1, x2, y2;

	public TextureSlice (float x, float y, float width, float height, Texture texture) {
		float texWidth = texture.width;
		float texHeight = texture.height;
		x /= texWidth;
		y /= texHeight;
		width /= texWidth;
		height /= texHeight;
		this.x1 = x;
		this.y1 = y;
		this.x2 = x + width;
		this.y2 = y + height;
	}

	public TextureSlice (float x, float y, float width, float height, float texWidth = 1, float texHeight = 1) {
		x /= texWidth;
		y /= texHeight;
		width /= texWidth;
		height /= texHeight;
		this.x1 = x;
		this.y1 = y;
		this.x2 = x + width;
		this.y2 = y + height;
	}
}
}