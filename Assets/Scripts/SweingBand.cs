using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweingBand : BandBase
{
	[SerializeField] private float spawnControlSpeed;

	private List<ProductHandler> productHandlers;

	protected override void Awake()
	{
		base.Awake();
		productHandlers = new List<ProductHandler>();
		ActionManager.AddProductHandler += AddProductHandler;

		ActionManager.OnInstantiateProcess += UsedProduct;
	}


	protected override void Start()
	{
		ActionManager.AddCameraView?.Invoke(AreaType.SweingArea, cameraPoint);
		ActionManager.ChangeArea(AreaType.SweingArea);
		StartCoroutine(SpawnControl());
	}

	protected void UsedProduct(Product product)
	{
		handleds.Remove(product);
		Sort();
	}

	private void AddProductHandler(ProductHandler handler) => productHandlers.Add(handler);


	private IEnumerator SpawnControl()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(spawnControlSpeed);
		yield return new WaitForFixedUpdate();
		while (true)
		{

			if (productHandlers.Exists(x => x.IsEmpty) && handleds.Count <= productHandlers.Count)
			{
				Product product = PoolManager.GetObjectFromPool(PoolType.Rope, parent: transform).GetComponent<Product>();
				handleds.Add(product);
				Sort();
			}
			yield return waitForSeconds;

		}
	}
}

public static partial class ActionManager
{
	public static Action<ProductHandler> AddProductHandler { get; set; }
	public static Action<Product> OnInstantiateProcess { get; set; }
}
