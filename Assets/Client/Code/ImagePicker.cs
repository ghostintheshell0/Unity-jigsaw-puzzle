using UnityEngine;

public class ImagePicker : MonoBehaviour
{
	[SerializeField] private Sprite[] values = default;
	[SerializeField] private Transform container = default;
	[SerializeField] private PuzzlePreview template = default;

	[SerializeField] private Color defaultBackgroundColor = default;
	[SerializeField] private Color pickedColor = default;

	private PuzzlePreview[] views = default;
	private PuzzlePreview pickedItem = default;

	private void Start()
	{
		Show();
		PickItem(0);
	}

	private void Show()
	{
		views = new PuzzlePreview[values.Length];
		for(int i = 0; i < values.Length; ++i)
		{
			var view = Instantiate(template, container);
			view.Sprite = values[i];
			view.Index = i;
			view.Clicked += PickView;
			view.BackgroundColor = defaultBackgroundColor;
			views[i] = view;
		}
	}

	private void PickView(PuzzlePreview preview)
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

	public Sprite Value => values[pickedItem.Index];
}