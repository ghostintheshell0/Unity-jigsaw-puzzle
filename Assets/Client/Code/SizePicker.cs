using UnityEngine;

public class SizePicker : MonoBehaviour
{
	[SerializeField] private Vector2Int[] values = default;
	[SerializeField] private Transform container = default;
	[SerializeField] private SizePreview template = default;

	[SerializeField] private Color defaultBackgroundColor = default;
	[SerializeField] private Color pickedColor = default;

	private SizePreview pickedItem = default;
	private SizePreview[] views = default;

	private void Start()
	{
		Show();
		PickItem(0);
	}

	private void Show()
	{
		views = new SizePreview[values.Length];

		for (int i = 0; i < values.Length; ++i)
		{
			var view = Instantiate(template, container);
			view.Value = values[i];
			view.Index = i;
			view.Clicked += PickView;
			view.BackgroundColor = defaultBackgroundColor;
			views[i] = view;
		}
	}

	private void PickView(SizePreview preview)
	{
		if (pickedItem != null)
		{
			pickedItem.BackgroundColor = defaultBackgroundColor;
		}

		pickedItem = preview;

		if (pickedItem != null)
		{
			pickedItem.BackgroundColor = pickedColor;
		}
	}

	public void PickItem(int index)
	{
		PickView(views[index]);
	}

	public Vector2Int Value => values[pickedItem.Index];
}