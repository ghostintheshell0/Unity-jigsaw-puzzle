using UnityEngine;

public class PieceDragger : MonoBehaviour
{
	public PieceEvent Dropped;

	[SerializeField] private Camera mainCamera = default;
	[SerializeField] private int dragSortingOrder = default;
	private Vector3 offset;
	private Piece currentPiece;

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			Raycast(pos);
		}

		if(currentPiece != null)
		{
			Drag();
		}

		if (Input.GetMouseButtonUp(0))
		{
			Drop();
		}

	}

	private void Raycast(Vector3 pos)
	{
		var hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit.collider == null) return;

		var piece = hit.collider.GetComponent<Piece>();
		if (piece == null) return;

		StartDrag(piece);
	}

	private void StartDrag(Piece piece)
	{

		var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		offset = piece.transform.position - pos;

		currentPiece = piece;
		dragSortingOrder++;
		currentPiece.SortingOrder = dragSortingOrder;
	}

	private void Drag()
	{
		if (currentPiece == null) return;

		var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		pos += offset;
		pos.z = 0;
		currentPiece.transform.position = pos;

	}

	private void Drop()
	{
		if (currentPiece == null) return;

		var piece = currentPiece;
		currentPiece = null;
		Dropped.Invoke(piece);
	}

	public int LastSortingOrder
	{
		get => dragSortingOrder;
		set => dragSortingOrder = value;
	}

}