using UnityEngine;

public class SettingsPicker : MonoBehaviour
{
	[SerializeField] private ImagePicker puzzlePicker = default;
	[SerializeField] private SizePicker sizePicker = default;
	[SerializeField] private ImagePicker connectionPicker = default;

	public Sprite Image => puzzlePicker.Value;
	public Vector2Int Size => sizePicker.Value;
	public Sprite Connection => connectionPicker.Value;
}