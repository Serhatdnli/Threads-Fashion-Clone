using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

public class Rope : Product
{
	[SerializeField] private GameObject[] parts;
	public override void InstantiateProcess(Vector3 startPos, Vector3 endPos, float time)
	{
		spriteRenderer.enabled = false;

		PoolManager.GetAudioFromPool(PoolType.ItemGoingToMachineSound);

		DeSelected();
		ActionManager.OnInstantiateProcess?.Invoke(this);

		float perEffectTime = time / parts.Length;
		StartCoroutine(DecreaseEffect());



		IEnumerator DecreaseEffect()
		{
			WaitForSeconds waitForSeconds = new WaitForSeconds(perEffectTime);
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].SetActive(false);
				yield return waitForSeconds;
			}

			parts.ForEach(x => x.SetActive(true));
			ReturnToPool();
		}


	}

	public override void PaintProcess(Color color, float time)
	{
	}

}
