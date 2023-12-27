using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
	[SerializeField] private SpriteRenderer fillImage;
	[SerializeField] private Image img;

	//private MaterialPropertyBlock materialPropertyBlock;

	//private void Start()
	//{
	//	materialPropertyBlock = new MaterialPropertyBlock();
	//}

	public void StartProgressBar(float processTime)
	{
		//float startValue = 360;

		//DOTween.To(() => startValue, x =>
		//{
		//	materialPropertyBlock.SetFloat(Constants.SHADERKEY_ARC2, x);
		//	fillImage.SetPropertyBlock(materialPropertyBlock);
		//}, 0, processTime).SetEase(Ease.Linear);



		float startValue2 = 0;
		img.DOFillAmount(1, processTime).From(0).SetEase(Ease.Linear);





	}

	public void ResetProgressBar()
	{
		//materialPropertyBlock.Clear();
		//fillImage.SetPropertyBlock(materialPropertyBlock);
		img.fillAmount = 0;

	}



}


public static partial class Constants
{
	public const string SHADERKEY_ARC2 = "_Arc2";
}