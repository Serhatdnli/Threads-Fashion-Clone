using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
	private static AudioClipData[] audioClipData;
	private static Dictionary<PoolType, Pool> poolDictionary;

	public static void Init(List<Pool> pools, AudioClipData[] _audioClipData)
	{

		audioClipData = _audioClipData;
		poolDictionary = new Dictionary<PoolType, Pool>();
		for (int i = 0; i < pools.Count; i++)
		{
			poolDictionary.Add(pools[i].type, pools[i]);
		}
	}


	public static GameObject CreateObject(GameObject prefab)
	{
		GameObject createdObject = UnityEngine.Object.Instantiate(prefab);
		createdObject.SetActive(false);
		createdObject.GetComponent<IPoolableObjects>()?.OnInstantiateObject();
		return createdObject;
	}

	public static GameObject GetObjectFromPool(PoolType tag, Vector3 position = default, Transform parent = null, Quaternion rotation = default)
	{
		if (poolDictionary.ContainsKey(tag))
		{

			GameObject objectToSpawn = poolDictionary[tag].GetObject(position, rotation);

			objectToSpawn.GetComponent<IPoolableObjects>()?.OnGetObjectFromPool();

			objectToSpawn.SetActive(true);
			objectToSpawn.transform.SetParent(parent);
			objectToSpawn.transform.localPosition = position;
			objectToSpawn.transform.localRotation = rotation;

			return objectToSpawn;
		}
		else
		{
			Debug.LogError("Pool wit tag " + tag + " doesn't exist.");
			return null;
		}
	}

	public static GameObject GetParticleFromPool(PoolType tag, float totalTime = -1, Color color = default, Vector3 position = default, Transform parent = null, Quaternion rotation = default)
	{
		if (poolDictionary.ContainsKey(tag))
		{

			GameObject objectToSpawn = poolDictionary[tag].GetObject(position, rotation);

			objectToSpawn.GetComponent<IPoolableObjects>()?.OnGetObjectFromPool();

			objectToSpawn.SetActive(true);
			objectToSpawn.transform.SetParent(parent);
			objectToSpawn.transform.localPosition = position;
			objectToSpawn.transform.localRotation = rotation;
			ParticleSystem ps = objectToSpawn.GetComponentInChildren<ParticleSystem>();
			ParticleSystemRenderer psr = objectToSpawn.GetComponentInChildren<ParticleSystemRenderer>();

			float duration = totalTime > 0 ? totalTime : ps.main.duration;
			ParticleSystem.MainModule main = ps.main;
			if (color != default)
				main.startColor = color;

			Action act = () => ReturnToPool(tag, objectToSpawn);
			CoroutineManager.Instance.StartCustomCoroutine(act, duration);

			return objectToSpawn;

		}
		else
		{
			Debug.LogError("Pool wit tag " + tag + " doesn't exist.");
			return null;
		}
	}


	public static GameObject GetAudioFromPool(PoolType tag, float totalTime = -1, bool autoPlay = true)
	{
		if (poolDictionary.ContainsKey(PoolType.AudioParent))
		{

			GameObject objectToSpawn = poolDictionary[PoolType.AudioParent].GetObject(Vector3.zero, Quaternion.identity);

			objectToSpawn.GetComponent<IPoolableObjects>()?.OnGetObjectFromPool();

			objectToSpawn.SetActive(true);

			AudioSource auS = objectToSpawn.GetComponentInChildren<AudioSource>();
			AudioClip clip = audioClipData.Find(x => x.PoolType == tag).Clip;

			auS.clip = clip;


			float duration = totalTime > 0 ? totalTime : clip.length;

			if (autoPlay)
				auS.Play();


			Action act = () => ReturnToPool(PoolType.AudioParent, objectToSpawn);
			CoroutineManager.Instance.StartCustomCoroutine(act, duration);




			return objectToSpawn;
		}
		else
		{
			Debug.LogError("Pool wit tag " + tag + " doesn't exist.");
			return null;
		}
	}


	public static void ReturnToPool(PoolType tag, GameObject obj)
	{
		if (poolDictionary.ContainsKey(tag))
		{
			obj.GetComponent<IPoolableObjects>()?.OnReturnToPool();
			poolDictionary[tag].ReturnToPool(obj);
		}
		else
		{
			Debug.LogError("Pool wit tag " + tag + " doesn't exist.");
		}
	}


}

[System.Serializable]
public class Pool
{
	public PoolType type;
	public GameObject prefab;

	private List<GameObject> items = new List<GameObject>();
	public List<GameObject> Items { get => items; }

	public GameObject GetObject(Vector3 position, Quaternion rotation)
	{

		GameObject obj;
		if (items.Count > 0)
		{
			obj = items[0];
			items.RemoveAt(0);
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.SetActive(true);
			return obj;
		}
		else
		{
			items.Add(PoolManager.CreateObject(prefab));
			return GetObject(position, rotation);
		}

	}

	public void ReturnToPool(GameObject obj)
	{
		if (!items.Contains(obj))
		{
			obj.SetActive(false);
			items.Add(obj);
		}

	}


}


public enum PoolType
{
	Default = 0,
	Rope = 1,
	ShortSocks = 2,
	Short = 3,
	Bra = 4,
	Slip = 5,
	UIMoney = 6,


	//audios
	AudioParent = 200,
	ButtonSound = 201,
	ItemPaintingSound = 202,
	MoneyUISound = 203,
	ProductProcessReleaseSound = 204,
	ProductSelectedSound = 205,
	ProductProcessCompleteSound = 206,
	ItemGoingToMachineSound = 207,
	SweingMachineSound = 208,

	//trails
	BubbleTrailParticle = 308,
	AnyProcCompletedFirstParticle = 309,
	AnyProcCompletedLastParticle = 310,
}

public interface IPoolableObjects
{
	void OnInstantiateObject();
	void OnGetObjectFromPool();
	void OnReturnToPool();
}

[Serializable]
public struct AudioClipData
{
	public PoolType PoolType;
	public AudioClip Clip;
}