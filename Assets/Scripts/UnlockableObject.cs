using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UnlockableObject : MonoBehaviour, IClickable
{
	[Title("UnlockableSettings")]
	[SerializeField] private int id;
	[SerializeField] private int price;
	[SerializeField] private int unlockedLevel;
	[SerializeField] private bool isFree;
	[SerializeField] private GameObject targetObject;

	[Title("Requirements")]
	[SerializeField] private TextMeshPro tmp;

	private UnlockedMachineData unlockedData;
	// Start is called before the first frame update
	void Start()
	{
		unlockedData = DataManager.GetUnlockedMachineData(id);
		if (unlockedData != null)
			LoadData();
		else if (isFree)
			Purchase();

		UpdateVisuals();
	}

	private void Purchase()
	{
		unlockedData = new UnlockedMachineData(id);
		DataManager.AddUnlockedMachineData(unlockedData);
		DataManager.SaveData();
		LoadData();
	}


	private void LoadData()
	{
		targetObject.SetActive(true);
		Destroy(gameObject);
	}


	public void OnClick()
	{
		bool isHaveMoney = PrefManager.Money >= price;
		bool isHaveLevel = unlockedLevel <= PrefManager.Level;
		if (isHaveMoney && isHaveLevel)
		{
			PrefManager.ChangeMoney(-price);
			Purchase();
			UpdateVisuals();
		}
		else
			Debug.Log(!isHaveLevel ? Constants.NOT_ENOUGHT_LEVEL : Constants.NOT_ENOUGHT_MONEY);
	}

	private void UpdateVisuals()
	{
		if (unlockedLevel > PrefManager.Level)
			tmp.text = "Unlocked";
		else
			tmp.text = $"{price}$";

	}
}




public static partial class Constants
{
	public const string NOT_ENOUGHT_MONEY = "Not Enough Money";
	public const string NOT_ENOUGHT_LEVEL = "Not Enough Level";
}