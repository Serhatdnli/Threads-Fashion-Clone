using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{


	public void StartCustomCoroutine(Action callBack, float time)
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSeconds(time);
			callBack?.Invoke();

		}
	}



}
