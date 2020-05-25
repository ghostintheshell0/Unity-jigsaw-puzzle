using System.Collections.Generic;
using UnityEngine;

public class PiecesGenerator : MonoBehaviour
{
	private static Color pieceColor = Color.white;
	private static Color alphaColor = Color.clear;
	private static TextureFormat textureFormat = TextureFormat.RGBA32;
	private static Piece template;

	[SerializeField] private Piece pieceTemplate = default;

	private void Awake()
	{
		template = pieceTemplate;
	}

	public static Piece[] SliceImage(Sprite image, int[,] connectionsMap, Sprite connection)
	{

		var puzzleWidth = connectionsMap.GetLength(0);
		var puzzleHeight = connectionsMap.GetLength(1);

		var pieces = new Piece[puzzleWidth * puzzleHeight];

		var order = 0;
		Vector3 imagePivot = new Vector3(image.pivot.x / image.rect.width, image.pivot.y / image.rect.height);
		var imageWorldSize = new Vector3(image.rect.width, image.rect.height) / image.pixelsPerUnit;
		var pieceWorldSize = new Vector3(imageWorldSize.x / puzzleWidth, imageWorldSize.y / puzzleHeight);
		var offset = Vector3.Scale(imageWorldSize, imagePivot);
	
		var halfPieceSize = pieceWorldSize * 0.5f;
		var puzzleSize = new Vector2Int((int)image.rect.width / puzzleWidth, (int)image.rect.height / puzzleHeight);

		var sliceSettings = new SliceSettings()
		{
			ConnectionSprite = connection,
			Connections = GenerateConnections(connection),
			PieceTextureSize = puzzleSize,
			PieceTemplate = template,
			CachedPieces = new Dictionary<int, Sprite>()
		};

		for (int ix = 0; ix < puzzleWidth; ++ix)
		{
			for (int iy = 0; iy < puzzleHeight; ++iy)
			{
				var piece = Object.Instantiate(template);
				piece.SpriteRenderer.sprite = image;
				piece.Collider.size = pieceWorldSize;
				piece.Position = new Vector2Int(ix, iy);
				piece.Mask.sprite = GetPiece(connectionsMap[ix, iy], sliceSettings);

				piece.SpriteRenderer.transform.localPosition = offset - new Vector3(ix * pieceWorldSize.x, iy * pieceWorldSize.y) - halfPieceSize;// + offset;// - halfPieceSize;

				piece.SortingOrder = order;
				pieces[iy * puzzleWidth + ix] = piece;
				++order;
			}
		}

		return pieces;
	}

	public static Sprite GetPiece(int value, SliceSettings settings)
	{
		if (settings.CachedPieces.ContainsKey(value))
		{
			return settings.CachedPieces[value];
		}

		var piece = GeneratePiece(value, settings);
		settings.CachedPieces.Add(value, piece);
		return piece;
	}

	public static Sprite GeneratePiece(int value, SliceSettings settings)
	{
		var connections = settings.Connections;

		var x = value.Contains(PieceValues.Left) ? connections[PieceValues.Left].width : 0;
		var y = value.Contains(PieceValues.Bottom) ? connections[PieceValues.Bottom].height : 0;
		var middleX = x + settings.PieceTextureSize.x / 2;
		var middleY = y + settings.PieceTextureSize.y / 2;

		var texSize = GetTextureSize(value, settings);

		var tex = new Texture2D(texSize.x, texSize.y, textureFormat, false);
		tex.filterMode = FilterMode.Point;
		tex.Fill(alphaColor);
		tex.Fill(new Rect(x, y, settings.PieceTextureSize.x, settings.PieceTextureSize.y), pieceColor);

		if (value.Contains(PieceValues.Right))
		{
			var startX = x + settings.PieceTextureSize.x;
			var startY = middleY - connections[PieceValues.Right].height / 2;

			tex.DrawTexture(connections[PieceValues.Right], new Vector2Int(startX, startY));
		}
		else if(value.Contains(PieceValues.RightNeighbord))
		{
			var startX = texSize.x - connections[PieceValues.Left].width;
			var startY = middleY - connections[PieceValues.Left].height / 2;

			tex.Cut(connections[PieceValues.Left], new Vector2Int(startX, startY));
		}

		if (value.Contains(PieceValues.Left))
		{
			var startX = 0;
			var startY = middleY - connections[PieceValues.Left].height / 2;

			tex.DrawTexture(connections[PieceValues.Left], new Vector2Int(startX, startY));
		}
		else if(value.Contains(PieceValues.LeftNeighbord))
		{
			var startX = 0;
			var startY = middleY - connections[PieceValues.Right].height / 2;

			tex.Cut(connections[PieceValues.Right], new Vector2Int(startX, startY));
		}

		if (value.Contains(PieceValues.Top))
		{
			var startX = middleX - connections[PieceValues.Top].width / 2;
			var startY = y + settings.PieceTextureSize.y;

			tex.DrawTexture(connections[PieceValues.Top], new Vector2Int(startX, startY));
		}
		else if(value.Contains(PieceValues.TopNeighbord))
		{
			var startX = middleX - connections[PieceValues.Bottom].width / 2;
			var startY = texSize.y - connections[PieceValues.Bottom].height;

			tex.Cut(connections[PieceValues.Bottom], new Vector2Int(startX, startY));
		}


		if (value.Contains(PieceValues.Bottom))
		{
			var startX = middleX - connections[PieceValues.Bottom].width / 2;
			var startY = 0;

			tex.DrawTexture(connections[PieceValues.Bottom], new Vector2Int(startX, startY));
		}
		else if(value.Contains(PieceValues.BottomNeighbord))
		{
			var startX = middleX - connections[PieceValues.Top].width / 2;
			var startY = 0;

			tex.Cut(connections[PieceValues.Top], new Vector2Int(startX, startY));
		}


		tex.Apply();

		var pivot = GetPivot(value, settings.PieceTextureSize, texSize);
		var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot);
		return sprite;
	}

	private static Vector2Int GetTextureSize(int value, SliceSettings settings)
	{
		var result = settings.PieceTextureSize;

		if (value.Contains(PieceValues.Left))
		{
			result.x += settings.Connections[PieceValues.Left].width;
		}
		if (value.Contains(PieceValues.Right))
		{
			result.x += settings.Connections[PieceValues.Right].width;
		}
		if (value.Contains(PieceValues.Top))
		{
			result.y += settings.Connections[PieceValues.Top].height;
		}
		if (value.Contains(PieceValues.Bottom))
		{
			result.y += settings.Connections[PieceValues.Bottom].height;
		}

		return result;
	}

	private static Vector2 GetPivot(int value, Vector2Int pieceSize, Vector2Int texSize)
	{
		var pivot = new Vector2(0.5f, 0.5f);

		if (value.Contains(PieceValues.Left))
		{

			pivot.x += 0.5f - pieceSize.x * 0.5f / texSize.x;;
		}
		if (value.Contains(PieceValues.Right))
		{
			pivot.x -= 0.5f - pieceSize.x * 0.5f / texSize.x;
		}
		if (value.Contains(PieceValues.Bottom))
		{
			pivot.y += 0.5f - pieceSize.y * 0.5f / texSize.y;
		}
		if (value.Contains(PieceValues.Top))
		{
			pivot.y -= 0.5f - pieceSize.y * 0.5f / texSize.y;
		}

		return pivot;
	}

	private static Dictionary<int, Texture2D> GenerateConnections(Sprite rightTemplateSpite)
	{
		
		var textureRect = new RectInt((int)rightTemplateSpite.rect.x, (int)rightTemplateSpite.rect.y, (int)rightTemplateSpite.rect.width, (int)rightTemplateSpite.rect.height);
		var rightTemplate = rightTemplateSpite.texture.Copy(textureRect);
		rightTemplate.Apply();
		var leftTemplate = rightTemplate.GetHorizontalMirror();
		leftTemplate.Apply();
		var topTemplate = rightTemplate.GetClockwiseRotate();
		topTemplate.Apply();
		var bottomTemplate = topTemplate.GetVerticalMirror();

		bottomTemplate.Apply();

		var connections = new Dictionary<int, Texture2D>(4);

		connections.Add(PieceValues.Right, rightTemplate);
		connections.Add(PieceValues.Left, leftTemplate);
		connections.Add(PieceValues.Top, topTemplate);
		connections.Add(PieceValues.Bottom, bottomTemplate);

		return connections;

	}
}