using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePreview : MonoBehaviour, IPointerDownHandler
{
	public event Action<PuzzlePreview> Clicked;

	public int Index;
	[SerializeField] private Image image = default;
	[SerializeField] private Image background = default;

	public Sprite Sprite
	{
		get => image.sprite;
		set => image.sprite = value;
	}

	public Color BackgroundColor
	{
		get => background.color;
		set => background.color = value;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Clicked.Invoke(this);
	}
}