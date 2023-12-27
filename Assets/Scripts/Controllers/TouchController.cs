using UnityEngine;

public class TouchController : MonoBehaviour
{
	private TouchManager touchManager;
	private RayManager rayManager;

	private Product selectedProduct;
	private ProductHandler selectedProductHandler;

	void Start()
	{
		rayManager = RayManager.Instance;
		touchManager = TouchManager.Instance;

		touchManager.OnMouseDown += MouseDown;
		touchManager.OnMouseHold += MouseHold;
		touchManager.OnMouseUp += Mouseup;

	}

	private void MouseDown(Vector3 mousePos)
	{
		if (rayManager.RayFromCameraScreen(out RaycastHit hit, mousePos))
		{

			if (hit.collider.TryGetComponent(out IClickable clickable))
			{
				clickable.OnClick();
				return;
			}

			if (hit.collider.TryGetComponent(out Product product))
			{
				selectedProduct?.DeSelected();

				selectedProduct = product;
				selectedProduct.Selected();
				return;
			}
			selectedProductHandler = hit.collider.GetComponent<ProductHandler>();

			HandleProduct();

		}
		else
		{
			selectedProduct?.DeSelected();
			selectedProduct = null;
		}

	}
	private void MouseHold(Vector3 mousePos)
	{
		if (rayManager.RayFromCameraScreen(out RaycastHit hit, mousePos))
		{

			if (hit.collider.TryGetComponent(out ProductHandler productHandler))
			{
				selectedProductHandler = productHandler;
				HandleProduct();
			}

		}
	}

	private void HandleProduct()
	{

		selectedProductHandler?.ReleaseIfComplete();
		selectedProductHandler?.HandleProduct(selectedProduct);

		selectedProduct?.DeSelected();
		selectedProduct = null;
	}

	private void Mouseup(Vector3 mousePos)
	{
		selectedProductHandler = null;
	}



}
