using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SweingMachine : ProductHandler
{
	[SerializeField] protected PoolType type;

	[Title("Requirements")]
	[SerializeField] private SweingScriptableObject sweingScriptableObject;

	[Title("Child Requirements")]
	[SerializeField] private Transform needleTransform;
	[SerializeField] private Transform productCollectPoint;
	[SerializeField] private Transform productCreatePoint;
	[SerializeField] private Transform productEndPoint;
	[SerializeField] private SpriteRenderer productIconSR;
	[SerializeField] private SpriteRenderer productIconBGSR;

	[Title("Settings")]
	[SerializeField] private Color emptyColor;
	[SerializeField] private Color completedColor;
	[SerializeField] private Vector2 needleLocalYBounds;

	public override PoolType ProductType => type;

	private MachineInfo machineInfo;



	private void Awake()
	{
		machineInfo = sweingScriptableObject.GetMachineInfo(type);
		productIconSR.sprite = machineInfo.ProductIcon;
	}
	private void Start()
	{
		ActionManager.AddProductHandler?.Invoke(this);
	}

	private void UpdateVisuals()
	{
		productIconBGSR.color = isCompleted ? completedColor : emptyColor;
	}


	public override void HandleProduct(Product product)
	{
		if (!IsEmpty || product is null)
			return;

		isEmpty = false;

		product.SetParentAndResetTransform(productCollectPoint);
		product.InstantiateProcess(productCreatePoint.position, productEndPoint.position, machineInfo.ProcessTime);


		Product created = PoolManager.GetObjectFromPool(type, parent: productCreatePoint).GetComponent<Product>();
		created.InstantiateProcess(productCreatePoint.position, productEndPoint.position, machineInfo.ProcessTime);

		PoolManager.GetAudioFromPool(PoolType.SweingMachineSound, machineInfo.ProcessTime);

		float needleTime = .05f;
		int count = Mathf.RoundToInt(machineInfo.ProcessTime / needleTime);

		needleTransform.DOLocalMoveY(needleLocalYBounds.y, needleTime)
			.From(needleLocalYBounds.x)
			.SetEase(Ease.InFlash)
			.SetId(GetHashCode())
			.SetLoops(count, LoopType.Yoyo)
			.OnComplete(() =>
			{
				myProduct = created;
				isCompleted = true;

				Vector3 pos = created.Transform.position + Vector3.back;
				PoolManager.GetParticleFromPool(PoolType.AnyProcCompletedFirstParticle, position: pos);
				PoolManager.GetParticleFromPool(PoolType.AnyProcCompletedLastParticle, position: Vector3.up, parent: created.Transform);
				UpdateVisuals();
			});

	}

	public override void ReleaseIfComplete()
	{
		if (isEmpty || !IsCompleted)
			return;
		isEmpty = true;
		isCompleted = false;
		UpdateVisuals();

		myProduct.InstantiateRelease();
		myProduct = null;

		PoolManager.GetAudioFromPool(PoolType.ProductProcessReleaseSound);

	}




}
