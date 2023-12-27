using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductModelBase : MonoBehaviour
{
	protected Renderer _renderer;
	protected void Awake()
	{
		_renderer = GetComponentInChildren<Renderer>();
	}


	public abstract void ReturnToPool();

}
