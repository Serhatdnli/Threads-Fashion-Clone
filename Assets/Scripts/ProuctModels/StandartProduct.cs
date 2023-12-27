using DG.Tweening;
using System;
using UnityEngine;

public class StandartProduct : Product
{
	public override void InstantiateProcess(Vector3 startPos, Vector3 endPos, float time)
	{
		PoolManager.GetAudioFromPool(PoolType.ItemGoingToMachineSound);

		spriteRenderer.enabled = false;
		_collider.enabled = false;

		transform.DOMove(endPos, time).From(startPos).SetEase(Ease.Linear).SetId(GetHashCode()).OnComplete(() =>
		{
			PoolManager.GetAudioFromPool(PoolType.ProductProcessCompleteSound);
		});
	}

	public override void PaintProcess(Color color, float time)
	{
		_collider.enabled = false;

		DeSelected();
		ActionManager.OnPaintProccess?.Invoke(this);


		Color from = _renderer.sharedMaterial.color;

		PoolManager.GetParticleFromPool(PoolType.BubbleTrailParticle, time, position: transform.position + Vector3.up);
		PoolManager.GetAudioFromPool(PoolType.ItemPaintingSound, time);


		_renderer.SetPropertyBlockExt(materialPropertyBlock, color, from, time, Constants.SHADERKEY_BASE_COLOR)
			.SetId(GetHashCode())
			.SetEase(Ease.Linear).OnComplete(() =>
			{
				PoolManager.GetAudioFromPool(PoolType.ProductProcessCompleteSound);

			});			

	}


	public override void SetParentAndResetTransform(Transform parent, Action onStart = null, Action onComplete = null)
	{
		onComplete += () => { spriteRenderer.enabled = true; };
		base.SetParentAndResetTransform(parent, onStart, onComplete);
	}


}
