using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SliceSettings
{
	public Piece PieceTemplate;
	public Sprite ConnectionSprite;
	public Vector2Int PieceTextureSize;

	public Dictionary<int, Texture2D> Connections;
	public Dictionary<int, Sprite> CachedPieces;
}