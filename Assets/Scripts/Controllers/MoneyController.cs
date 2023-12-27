using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
	private TextMeshProUGUI moneyText;
	private void Awake()
	{
		moneyText = GetComponentInChildren<TextMeshProUGUI>();
		ActionManager.MoneyChanged += SetMoney;
		SetMoney();
	}

	private void SetMoney()
	{
		moneyText.text = PrefManager.Money.FormatNumber();
	}
}
