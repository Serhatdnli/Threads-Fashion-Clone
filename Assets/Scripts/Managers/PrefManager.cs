using System;
using UnityEngine;

public class PrefManager : MonoBehaviour
{

	public static int Money { get { return PlayerPrefs.GetInt(Constants.MONEY, 0); } set { PlayerPrefs.SetInt(Constants.MONEY, value); } }
	public static int Level { get { return PlayerPrefs.GetInt(Constants.LEVEL, 1); } set { PlayerPrefs.SetInt(Constants.LEVEL, value); } }
	public static float CurrentExp { get { return PlayerPrefs.GetFloat(Constants.CURRENT_LEVEL_EXP, 0); } set { PlayerPrefs.SetFloat(Constants.CURRENT_LEVEL_EXP, value); } }

	public static void ChangeMoney(int value)
	{
		Money += value;
		ActionManager.MoneyChanged?.Invoke();
	}

	public static void SetMoney(int value)
	{
		Money = value;
		ActionManager.MoneyChanged?.Invoke();
	}

	public static void LevelUp()
	{
		Level++;
		ActionManager.LevelChanged?.Invoke();
	}


}


public static partial class ActionManager
{
	public static Action MoneyChanged { get; set; }
	public static Action LevelChanged { get; set; }
}