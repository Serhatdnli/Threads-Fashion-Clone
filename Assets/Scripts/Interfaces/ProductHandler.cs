using UnityEngine;

public abstract class ProductHandler:MonoBehaviour
{

	protected Product myProduct;

	protected bool isCompleted = false;
	protected bool isEmpty = true;

	public virtual PoolType ProductType => default;
	public bool IsCompleted => isCompleted;
	public bool IsEmpty => isEmpty;
	public abstract void HandleProduct(Product product);
	public abstract void ReleaseIfComplete();

}
