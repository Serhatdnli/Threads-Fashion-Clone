using UnityEngine;
using UnityEngine.UI;

public abstract class UIButtonBase : MonoBehaviour
{
	protected Button button;
	protected virtual void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnButtonClicked);
	}


	protected virtual void OnButtonClicked()
	{
		PoolManager.GetAudioFromPool(PoolType.ButtonSound);
	}

}
