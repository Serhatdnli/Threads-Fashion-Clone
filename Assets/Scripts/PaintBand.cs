using System;
using System.Collections.Generic;

public class PaintBand : BandBase
{

	protected override void Start() => ActionManager.AddCameraView?.Invoke(AreaType.PaintingArea, cameraPoint);

	protected override void Awake()
	{
		base.Awake();
		ActionManager.InstantiateReleased += CompleteInstatiateProcess;
		ActionManager.OnPaintProccess += UsedProduct;
	}

	private void CompleteInstatiateProcess(Product product)
	{
		handleds.Add(product);
		product.SetParentAndResetTransform(transform, onComplete: () => Sort());
	}

	private void UsedProduct(Product product)
	{
		handleds.Remove(product);
		Sort();
	}




}

public static partial class ActionManager
{
	public static Action<Product> InstantiateReleased { get; set; }
	public static Action<Product> OnPaintProccess { get; set; }
}
