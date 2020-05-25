using UnityEngine;

public class Piece : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer = default;
	[SerializeField] private BoxCollider2D boxCollider = default;
	[SerializeField] private SpriteMask mask = default;

	public SpriteRenderer SpriteRenderer => spriteRenderer;
	public BoxCollider2D Collider => boxCollider;
	public SpriteMask Mask => mask;

	[HideInInspector] public Vector2Int Position;
	[HideInInspector] public bool IsCorrect;

	public int SortingOrder
	{
		get => spriteRenderer.sortingOrder;
		set
		{
			spriteRenderer.sortingOrder = value;
			mask.frontSortingOrder = value;
			mask.backSortingOrder = value - 1;
		}
	}
}
