using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.IO;

public static class Ext
{
	public static T Find<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		foreach (var current in enumerable)
		{
			if (predicate(current))
			{
				return current;
			}
		}
		return default(T);
	}

	public static string FormatNumber(this int amount, string percentSeperator = ",", bool removeDies = false)
	{
		const string letters = "KMBTEFGHIJKLMNOPQRSTUVWXYZ";

		if (amount < 1000)
		{
			return amount.ToString();
		}

		int letterIndex = 0;
		int divisor = 1;

		while (amount >= divisor * 1000)
		{
			letterIndex++;

			divisor *= 1000;
		}

		string suffix = "";

		if (letterIndex > 0)
		{
			int suffixCount = (letterIndex / letters.Length) + (letterIndex % letters.Length == 0 ? 0 : 1);
			for (int i = 0; i < suffixCount; i++)
			{
				suffix += letters[(letterIndex - 1) % letters.Length].ToString();
			}
		}

		int scaledAmount = amount / divisor;
		char first = (amount - (scaledAmount * divisor)).ToString().First();

		return $"{scaledAmount}{percentSeperator}{first}{suffix}";
	}


	public static Tween SetPropertyBlockExt(this Renderer renderer, MaterialPropertyBlock mtb, Color to, Color from, float time, string customShaderKey)
	{
		return DOTween.To(() => Color.white, x =>
		{
			mtb.SetColor(customShaderKey, x);
			renderer.SetPropertyBlock(mtb);
		}, to, time)
		.From(from);
	}

	public static void SetPropertyBlockExt(this Renderer renderer, MaterialPropertyBlock mtb, Color to, string customShaderKey, int materialIndex = 0)
	{
		mtb.SetColor(customShaderKey, to);
		renderer.SetPropertyBlock(mtb, materialIndex);

	}


	#region FileReadWriteStuff
	public static void ReadJson<T>(ref T data)
	{
		string fileName = nameof(data);
		string dataPath = Application.persistentDataPath;

#if UNITY_EDITOR
		dataPath = Application.dataPath;
#endif

		string readedText = File.ReadAllText(dataPath + $"/{fileName}.json");
		data = JsonUtility.FromJson<T>(readedText);
	}

	public static void WriteJson<T>(T data)
	{
		string fileName = nameof(data);
		string jsonValue = JsonUtility.ToJson(data, true);
		string dataPath = Application.persistentDataPath;


		dataPath = Application.persistentDataPath;

#if UNITY_EDITOR
		AssetDatabase.Refresh();
		dataPath = Application.dataPath;
#endif

		File.WriteAllText(dataPath + $"/{fileName}.json", jsonValue);
	}

	public static bool HasJson<T>(T data)
	{
		string fileName = nameof(data);
		string dataPath = Application.persistentDataPath;

#if UNITY_EDITOR
		dataPath = Application.dataPath;
#endif
		return File.Exists(dataPath + $"/{fileName}.json");
	}


	#endregion

}
