using UnityEngine;

public static class Texture2DExtensions
{
	private static TextureFormat textureFormat = TextureFormat.Alpha8;
	
	public static void DrawTexture(this Texture2D tex, Texture2D tex2, Vector2Int pos)
	{
		var w = tex2.width;
		var h = tex2.height;
		var colors = new Color[w * h];

		for (int ix = 0; ix < w; ix++)
		{
			for (int iy = 0; iy < h; iy++)
			{
				var c = tex2.GetPixel(ix, iy);
				if (c.a == 0) continue;
				colors[iy * w + ix] = c;
			}

		}
		tex.SetPixels(pos.x, pos.y, w, h, colors);
	}

	public static void Fill(this Texture2D tex, Rect rect, Color color)
	{
		var colors = new Color[(int)rect.width * (int)rect.height];
		for (int i = 0; i < colors.Length; ++i)
		{
			colors[i] = color;
		}

		tex.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, colors);
	}

	public static void Fill(this Texture2D tex, Color color)
	{
		tex.Fill(new Rect(0, 0, tex.width, tex.height), color);
	}

	public static Texture2D GetClockwiseRotate(this Texture2D tex)
	{
		var newWidth = tex.height;
		var newHeight = tex.width;
		var newTex = new Texture2D(newWidth, newHeight, textureFormat, false);


		for (int ix = 0; ix < newWidth; ++ix)
		{
			for (int iy = 0; iy < newHeight; ++iy)
			{
				var c = tex.GetPixel(iy, ix);
				newTex.SetPixel(ix, iy, c);
			}
		}

		return newTex;
	}

	public static Texture2D GetHorizontalMirror(this Texture2D tex)
	{
		var newWidth = tex.width;
		var newHeight = tex.height;
		var newTex = new Texture2D(newWidth, newHeight, textureFormat, false);


		for (int ix = 0; ix < newWidth; ++ix)
		{
			for (int iy = 0; iy < newHeight; ++iy)
			{
				var c = tex.GetPixel(newWidth - 1 - ix, iy);
				newTex.SetPixel(ix, iy, c);
			}
		}

		return newTex;
	}

	public static Texture2D GetVerticalMirror(this Texture2D tex)
	{
		var newWidth = tex.width;
		var newHeight = tex.height;
		var newTex = new Texture2D(newWidth, newHeight, textureFormat, false);


		for (int ix = 0; ix < newWidth; ++ix)
		{
			for (int iy = 0; iy < newHeight; ++iy)
			{
				var c = tex.GetPixel(ix, newHeight - 1 - iy);
				newTex.SetPixel(ix, iy, c);
			}
		}

		return newTex;
	}


	public static Texture2D Copy(this Texture2D tex, RectInt rect)
	{
		var newTex = new Texture2D(rect.width, rect.height, textureFormat, false);


		for (int ix = 0; ix < rect.width; ++ix)
		{
			for (int iy = 0; iy < rect.height; ++iy)
			{
				var c = tex.GetPixel(ix + rect.x, iy + rect.y);
				newTex.SetPixel(ix, iy, c);
			}
		}

		return newTex;
	}

	public static void Cut(this Texture2D tex, Texture2D tex2, Vector2Int pos)
	{

		for (int ix = 0; ix < tex2.width; ++ix)
		{
			for (int iy = 0; iy < tex2.height; ++iy)
			{

				var c = tex2.GetPixel(ix, iy);
				if (c.a == 0) continue;
				tex.SetPixel(ix + pos.x, iy + pos.y, Color.clear);
			}
		}

	}
}