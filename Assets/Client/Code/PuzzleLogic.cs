using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleLogic : MonoBehaviour
{
	public UnityEvent Completed;

	[SerializeField] private SettingsPicker settings = default;
	[SerializeField] private PieceDragger pieceDragger = default;
	[SerializeField] private LineRenderer lineRenderer = default;
	[SerializeField] private Transform MinPosition = default;
	[SerializeField] private Transform MaxPosition = default;
	[SerializeField] private Transform StartPosition = default;

	[SerializeField] private SpriteRenderer OriginalOutput = default;
	[SerializeField] private float snapDistance = default;
	[SerializeField] private bool snapping = true;

	private Puzzle currentPuzzle;

    public void StartGame()
    {
		Init(settings.Image, settings.Size, settings.Connection);
		HideOriginal();
	}

	public void Init(Sprite image, Vector2Int size, Sprite connection)
	{
		currentPuzzle = new Puzzle(image, size, connection);
		Shuffle();

		var offset = currentPuzzle.ImageSize * -0.5f + StartPosition.position;
		DrawBorders(new Rect(offset.x, offset.y, currentPuzzle.ImageSize.x, currentPuzzle.ImageSize.y));
		pieceDragger.LastSortingOrder = currentPuzzle.Pieces.Count;
	}
	
	public void Shuffle()
	{
		if(currentPuzzle == null) return;
		
		foreach(var piece in currentPuzzle.Pieces)
		{
			if (piece.IsCorrect) continue;
			piece.transform.position = GetRandomPoint();
		}
	}

	private Vector3 GetRandomPoint()
	{
		return new Vector3(
			Random.Range(MinPosition.position.x, MaxPosition.position.x),
			Random.Range(MinPosition.position.y, MaxPosition.position.y),
			Random.Range(MinPosition.position.z, MaxPosition.position.z)
			);
	}

	
	public  void CheckCorrect(Piece piece)
	{
		if (currentPuzzle == null) return;

		var pivot = piece.Mask.sprite.pivot;
		var offset = StartPosition.position - (currentPuzzle.ImageSize - currentPuzzle.PieceSize) * 0.5f;
		var isCorrect = currentPuzzle.CheckCorrect(piece, offset, snapDistance);
		if(isCorrect && snapping)
		{
			piece.transform.position = currentPuzzle.GetCorrectPosition(piece) + offset;
		}
	}

	private void DrawBorders(Rect rect)
	{
		var p1 = new Vector3(rect.xMin, rect.yMin);
		var p2 = new Vector3(rect.xMin, rect.yMax);
		var p3 = new Vector3(rect.xMax, rect.yMax);
		var p4 = new Vector3(rect.xMax, rect.yMin);

		lineRenderer.positionCount = 4;
		lineRenderer.SetPosition(0, p1);
		lineRenderer.SetPosition(1, p2);
		lineRenderer.SetPosition(2, p3);
		lineRenderer.SetPosition(3, p4);
	}

	public void Clear()
	{
		if (currentPuzzle == null) return;

		foreach(var piece in currentPuzzle.Pieces)
		{
			Destroy(piece.gameObject);
		}

		currentPuzzle = null;
	}

	public void ShowOriginal()
	{
		OriginalOutput.transform.position = StartPosition.position;
		OriginalOutput.gameObject.SetActive(true);
		OriginalOutput.sprite = currentPuzzle.Image;
	}

	public void HideOriginal()
	{
		OriginalOutput.gameObject.SetActive(false);
	}


	public void IsComplete()
	{
		if(currentPuzzle != null && currentPuzzle.IsComplete())
		Completed.Invoke();
	}

	public void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}