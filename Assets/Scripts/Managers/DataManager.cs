using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DataManager
{
	private static GameData gameData;

	public static void Init()
	{
		if (Ext.HasJson(gameData))
			Ext.ReadJson(ref gameData);
		else
			gameData = new GameData();
	}

	public static void SaveData()
	{
		Ext.WriteJson(gameData);
	}

	public static void AddUnlockedMachineData(UnlockedMachineData data) => gameData.UnlockedMachineData.Add(data);

	public static UnlockedMachineData GetUnlockedMachineData(int id) => gameData.UnlockedMachineData.Find(x => x.id == id);


}





[Serializable]
public class GameData
{
	public List<UnlockedMachineData> UnlockedMachineData;
	public GameData()
	{
		UnlockedMachineData = new List<UnlockedMachineData>();
	}
}


[Serializable]
public class UnlockedMachineData
{
	public int id;
	public UnlockedMachineData(int _id) => id = _id;
	//maybe add another info
}