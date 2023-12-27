using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cauldron : ProductHandler
{
	[Title("Requirements")]
	[SerializeField] private CauldronColorType cauldronType;
	[SerializeField] private CauldronScriptableObject cauldronScriptableObject;
	[SerializeField] private Transform turnerPivot;
	[SerializeField] private Transform turnerPoint;

	[Title("Settings")]
	[SerializeField] private float rotationAngle;

	private CauldronInfo cauldronData;
	private Renderer _renderer;
	private MaterialPropertyBlock materialPropertyBlock;
	private ProgressBarController processBarController;


	private void Awake()
	{
		processBarController = GetComponentInChildren<ProgressBarController>();
		_renderer = GetComponentInChildren<Renderer>();
		cauldronData = cauldronScriptableObject.GetCauldronData(cauldronType);
		SetVisuals();
	}


	public override void HandleProduct(Product product)
	{
		if (!IsEmpty || product is null)
			return;

		isEmpty = false;

		float time = cauldronData.ProcessTime;

		processBarController.StartProgressBar(time);

		product.SetParentAndResetTransform(turnerPoint, onComplete: () =>
		{
			product.PaintProcess(cauldronData.Color, time);
		});

		float angle = rotationAngle * time;

		turnerPivot.DORotate(Vector3.up * angle, time)
			.SetRelative()
			.SetEase(Ease.Linear)
			.SetId(GetHashCode())
			.OnComplete(() =>
			{
				isCompleted = true;
				myProduct = product;

				Vector3 pos = product.Transform.position + Vector3.up;
				PoolManager.GetParticleFromPool(PoolType.AnyProcCompletedFirstParticle, position: pos);
				PoolManager.GetParticleFromPool(PoolType.AnyProcCompletedLastParticle, position: Vector3.up, parent: product.Transform);
			});

	}

	public override void ReleaseIfComplete()
	{
		if (isEmpty || !IsCompleted)
			return;

		isEmpty = true;
		isCompleted = false;

		processBarController.ResetProgressBar();

		myProduct.PaintRelease();
		myProduct = null;

		PoolManager.GetAudioFromPool(PoolType.ProductProcessReleaseSound);



	}


	private void SetVisuals()
	{
		int liquidIndex = 1;
		materialPropertyBlock = new MaterialPropertyBlock();

		_renderer.SetPropertyBlockExt(materialPropertyBlock, cauldronData.Color, Constants.SHADERKEY_BASE_COLOR, liquidIndex);

	}
}


public static partial class Constants
{
	public const string SHADERKEY_BASE_COLOR = "_BaseColor";
}