using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFlyController : Singleton<MoneyFlyController>
{
	[SerializeField] private RectTransform targetRect;
	[SerializeField] private Transform canvasParent;

	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}


	public void MoneyEffect(Vector3 worldPos, int earnMoney)
	{
		Vector3 uýPos = mainCamera.WorldToScreenPoint(worldPos);
		uýPos.z = 0;

		int spawnCount = 5;
		List<RectTransform> createdObjects = new List<RectTransform>();

		for (int i = 0; i < spawnCount; i++)
		{
			RectTransform rect = PoolManager.GetObjectFromPool(PoolType.UIMoney, parent: canvasParent).transform as RectTransform;
			rect.position = uýPos;
			createdObjects.Add(rect);
		}
		Tween lastTween = null;
		for (int i = 0; i < spawnCount; i++)
		{
			RectTransform rect = createdObjects[i];

			lastTween = rect.DOJump(targetRect.position, -5f, 1, 1f)
				.SetEase(Ease.InQuad)
				.SetDelay(i * .05f)
				.OnComplete(() =>
				{
					PoolManager.GetAudioFromPool(PoolType.MoneyUISound);
				});

		}

		lastTween?.OnComplete(() =>
		{
			createdObjects.ForEach(x => PoolManager.ReturnToPool(PoolType.UIMoney, x.gameObject));
			PrefManager.ChangeMoney(earnMoney);
		});

	}


}
