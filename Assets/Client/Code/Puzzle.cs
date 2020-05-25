using System.Collections.Generic;
using UnityEngine;

public class Puzzle
{
	private Piece[] pieces;
	private int[,] connectionsMap;

	private Sprite image;
	private int width;
	private int height;

	private Vector3 imageSize;
	private Vector3 pieceSize;

	public Puzzle(Sprite image, Vector2Int size, Sprite connection)
	{
		this.image = image;
		this.width = size.x;
		this.height = size.y;

		connectionsMap = GenerateConnectionsMap(width, height);
		pieces = PiecesGenerator.SliceImage(image, connectionsMap, connection);

		imageSize = new Vector3(image.rect.width, image.rect.height) / image.pixelsPerUnit;
		pieceSize = new Vector3(imageSize.x / width, imageSize.y / height);
	}

	public bool CheckCorrect(Piece piece, Vector3 offset, float snapDistance)
	{
		var correctPos = new Vector3(piece.Position.x * pieceSize.x, piece.Position.y * pieceSize.y) + offset;
		piece.IsCorrect = piece.transform.IsNear(correctPos, snapDistance);
		
		return piece.IsCorrect;
	}

	public Vector3 GetCorrectPosition(Piece piece)
	{
		return new Vector3(piece.Position.x * pieceSize.x, piece.Position.y * pieceSize.y);
	}

	public bool IsComplete()
	{
		foreach(var piece in pieces)
		{
			if (!piece.IsCorrect) return false;
		}

		return true;
	}

	public IReadOnlyCollection<Piece> Pieces => pieces;
	public Sprite Image => image;
	public Vector3 ImageSize => imageSize;
	public Vector3 PieceSize => pieceSize;


	public static int[,] GenerateConnectionsMap(int width, int height)
	{
		var piecesMap = new int[width, height];

		for (int ix = 0; ix < width; ++ix)
		{
			for (int iy = 0; iy < height; ++iy)
			{
				var value = 0;

				if (ix > 0)
				{
					value |= PieceValues.LeftNeighbord;
					if (!piecesMap[ix - 1, iy].Contains(PieceValues.Right))
					{
						value |= PieceValues.Left;
					}
				}

				if (iy > 0)
				{
					value |= PieceValues.BottomNeighbord;
					if (!piecesMap[ix, iy - 1].Contains(PieceValues.Top))
					{
						value |= PieceValues.Bottom;
					}
				}

				if (ix < width - 1)
				{
					value |= PieceValues.RightNeighbord;
					if (Random.Range(0f, 1f) < 0.5f)
					{
						value |= PieceValues.Right;
					}
				}

				if (iy < height - 1)
				{
					value |= PieceValues.TopNeighbord;
					if (Random.Range(0f, 1f) < 0.5f)
					{
						value |= PieceValues.Top;
					}
				}
				piecesMap[ix, iy] = value;
			}
		}

		return piecesMap;
	}
}