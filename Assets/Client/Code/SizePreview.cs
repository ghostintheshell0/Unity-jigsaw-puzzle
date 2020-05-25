using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SizePreview : MonoBehaviour, IPointerDownHandler
{
	public event Action<SizePreview> Clicked;

	public int Index;
	[SerializeField] private TMP_Text output = default;
	[SerializeField] private Image background = default;

	private Vector2Int value;

	public Vector2Int Value
	{
		get => value;
		set
		{
			this.value = value;
			output.text = $"{value.x}x{value.y}";
		}
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