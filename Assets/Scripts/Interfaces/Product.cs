using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public abstract class Product : MonoBehaviour, IPoolableObjects
{
	[Title("Requirements")]
	[SerializeField] protected PoolType type;
	[SerializeField] protected ProductScriptableObject productScriptableObject;


	protected SpriteRenderer spriteRenderer;
	protected Collider _collider;
	protected Renderer _renderer;
	protected MaterialPropertyBlock materialPropertyBlock;
	protected ProductInfo productInfo;
	protected MoneyFlyController moneyFlyController;


	public Transform Transform => transform;
	public PoolType ProductType => type;

	public int EarnAmount => productInfo.EarnMoneyAmount;


	public abstract void InstantiateProcess(Vector3 startPos, Vector3 endPos, float time);
	public abstract void PaintProcess(Color color, float time);


	private void Start()
	{
		materialPropertyBlock = new MaterialPropertyBlock();
		moneyFlyController = MoneyFlyController.Instance;

	}

	public virtual void SetParentAndResetTransform(Transform parent, Action onStart = null, Action onComplete = null)
	{
		transform.SetParent(parent);
		transform.localRotation = Quaternion.identity;
		transform.DOLocalJump(Vector3.zero, 1, 1, .5f)
			.SetEase(Ease.OutQuad)
			.OnStart(() =>
			{
				onStart?.Invoke();
				spriteRenderer.enabled = false;
			})
			.OnComplete(() =>
			{
				onComplete?.Invoke();
			});
	}


	public virtual void PaintRelease()
	{
		PoolManager.GetAudioFromPool(PoolType.ProductProcessReleaseSound);

		moneyFlyController.MoneyEffect(transform.position, EarnAmount);

		ActionManager.GainExp?.Invoke(productInfo.EarnExpAmount);

		DOTween.Kill(GetHashCode());

		ReturnToPool();
	}

	public virtual void InstantiateRelease()
	{
		spriteRenderer.enabled = true;
		_collider.enabled = true;

		ActionManager.InstantiateReleased?.Invoke(this);
		PoolManager.GetAudioFromPool(PoolType.ProductProcessReleaseSound);

	}

	public virtual void Selected()
	{
		DOTween.Complete(GetHashCode());
		PoolManager.GetAudioFromPool(PoolType.ProductSelectedSound);
		transform.DOScale(transform.localScale.x, .2f).From(transform.localScale.x * 1.2f).SetEase(Ease.InBack).SetId(GetHashCode());
		spriteRenderer.color = Color.green;
	}

	public virtual void DeSelected()
	{
		spriteRenderer.color = Color.white;
	}

	public virtual void ReturnToPool()
	{
		DOTween.Complete(GetHashCode());
		materialPropertyBlock.Clear();
		_renderer.SetPropertyBlock(materialPropertyBlock);
		PoolManager.ReturnToPool(type, gameObject);

	}

	public void OnInstantiateObject()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_collider = GetComponent<Collider>();
		_renderer = GetComponentInChildren<Renderer>();
		productInfo = productScriptableObject.GetProductInfo(type);
	}

	public void OnGetObjectFromPool()
	{
		spriteRenderer.enabled = true;
	}

	public void OnReturnToPool()
	{

	}

}
