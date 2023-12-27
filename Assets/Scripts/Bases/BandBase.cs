using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BandBase : MonoBehaviour
{
	[SerializeField] protected Transform spawnPoint;
	[SerializeField] protected Transform cameraPoint;
	[SerializeField] protected float spacing;

	protected int xDirection;
	protected int posIndex = 0;
	protected List<Product> handleds;

	protected virtual void Awake()
	{
		xDirection = spawnPoint.localPosition.x > 0 ? -1 : 1;
		handleds = new List<Product>();
	}

	protected abstract void Start();


	protected void Sort()
	{
		DOTween.Kill(GetHashCode());
		for (int i = 0; i < handleds.Count; i++)
			handleds[i].Transform.DOLocalMoveX(spawnPoint.localPosition.x + (spacing * i * xDirection), .2f).SetEase(Ease.Linear).SetId(GetHashCode());
	}

}
