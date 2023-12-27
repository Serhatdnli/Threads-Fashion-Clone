using System;
using UnityEngine;

[CreateAssetMenu]
public class MaterialManagerScriptable : ScriptableObject
{
	[SerializeField] private MaterialData[] materialData;

	public Material[] GetMaterials(PoolType poolType, MaterialType materialType) => materialData.Find(x => x.PoolType == poolType).Materials.Find(x => x.MaterialType == materialType).materials;
}

[Serializable]
public struct MaterialData
{
	public PoolType PoolType;
	public Materials[] Materials;
}

[Serializable]
public struct Materials
{
	public MaterialType MaterialType;
	public Material[] materials;
}



public enum MaterialType
{
	Default = 0,
	Outlined = 1,
}
