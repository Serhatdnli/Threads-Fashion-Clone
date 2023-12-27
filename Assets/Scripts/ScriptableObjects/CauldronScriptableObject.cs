using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CauldronScriptableObject : ScriptableObject
{
	[SerializeField] private CauldronInfo[] cauldronData;

	public CauldronInfo GetCauldronData(CauldronColorType type) => cauldronData.Find(x => x.Type == type);
}

[Serializable]
public struct CauldronInfo
{
	public CauldronColorType Type;
	public Color Color;
	public float ProcessTime;
}


public enum CauldronColorType
{
	Red,
	Green,
	Blue,
	Yellow,
	Purple
}
