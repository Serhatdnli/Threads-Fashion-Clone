
using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class ProductScriptableObject : ScriptableObject
{
	[SerializeField] private ProductInfo[] productInfos;

	public ProductInfo GetProductInfo(PoolType type) => productInfos.Find(x => x.ProductType == type);
}

[Serializable]
public struct ProductInfo
{
	public PoolType ProductType;
	public float Processtime;
	public int EarnMoneyAmount;
	public float EarnExpAmount;
}
