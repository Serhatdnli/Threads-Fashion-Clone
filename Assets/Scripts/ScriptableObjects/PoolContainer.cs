using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PoolContainer : ScriptableObject
{
	public AudioClipData[] audioClipData;
	public List<Pool> pools;

	internal void Init()
	{
		Debug.Log("pool inited");
		PoolManager.Init(pools, audioClipData);
	}
}
