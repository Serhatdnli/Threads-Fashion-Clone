using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class RayManager : Singleton<RayManager>
{
	private Camera mainCamera;
	private void Awake()
	{
		mainCamera = Camera.main;
	}



	/// <summary>
	/// Throw multi raycast with jobs and if you found type return true and out you need T object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="origins">origins vector3 array</param>
	/// <param name="directions">directions vector3 array !MUST BE EQUAL ORIGINS LENGTH</param>
	/// <param name="returnObject"></param>
	/// <param name="range">hit range</param>
	/// <param name="maxHit">hit count</param>
	/// <returns></returns>
	public bool GetComponentWithMultiRaycast<T>(Vector3[] origins, Vector3[] directions, out T returnObject, float range = 1000, int maxHit = 10)
	{
		returnObject = GetComponentRaycastAllWithJobs<T>(origins, directions, range, maxHit);

		return returnObject != null;
	}

	/// <summary>
	/// Throw multi raycast with jobs and get what you need type if you found.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="origins">origins vector3 array</param>
	/// <param name="directions">directions vector3 array !MUST BE EQUAL ORIGINS LENGTH</param>
	/// <param name="range">hit range</param>
	/// <param name="maxHit">hit count</param>
	/// <returns></returns>
	public T GetComponentRaycastAllWithJobs<T>(Vector3[] origins, Vector3[] directions, float range = 1000, int maxHit = 10)
	{
		T returnObject = default(T);

		if (MultiRaycast(origins, directions, out RaycastHit[] results, range, maxHit))
			// Copy the result. If batchedHit.collider is null there was no hit
			foreach (var hitted in results)
			{
				Collider c = hitted.collider;
				if (c != null && c.TryGetComponent(out T tObj))
				{
					returnObject = tObj;
					break;
				}
			}

		return returnObject;
	}


	/// <summary>
	/// Throw multi raycast with jobs and get RaycastHit[] results.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="origins">origins vector3 array</param>
	/// <param name="directions">directions vector3 array !MUST BE EQUAL ORIGINS LENGTH</param>
	/// <param name="range">hit range</param>
	/// <param name="maxHit">hit count</param>
	/// <returns></returns>
	public bool MultiRaycast(Vector3[] origins, Vector3[] directions, out RaycastHit[] hits, float range = 1000, int maxHit = 10)
	{
		//Ray ray = mainCamera.ViewportPointToRay(new Vector2(.5f, .5f));

		// Perform a single raycast using RaycastCommand and wait for it to complete
		// Setup the command and result buffers
		NativeArray<RaycastHit> results = new NativeArray<RaycastHit>(maxHit, Allocator.TempJob);


		if (origins.Length != directions.Length)
		{
			hits = null;
			return false;
		}

		NativeArray<RaycastCommand> commands = new NativeArray<RaycastCommand>(origins.Length, Allocator.TempJob);

		for (int i = 0; i < origins.Length; i++)
		{
			commands[i] = new RaycastCommand(origins[i], directions[i], range, maxHits: maxHit);
			DrawRayDebug(origins[i], directions[i], range);
		}

		// Schedule the batch of raycasts.
		JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1);

		// Wait for the batch processing job to complete
		handle.Complete();

		//// Copy the result. If batchedHit.collider is null there was no hit
		//foreach (var hitted in results)
		//{
		//    if (hitted.collider != null)
		//    {
		//        // If hit.collider is not null means there was a hit
		//    }
		//}

		hits = results.ToArray();
		bool isHitted = hits.Any(x => x.collider);
		// Dispose the buffers
		results.Dispose();
		commands.Dispose();

		return isHitted;

	}



	public bool GetObjectFromRaycast<T>(Vector3 origin, Vector3 direction, out T tOjb, float range = 1000)
	{
		tOjb = GetObjectFromRaycast<T>(origin, direction, range);

		return tOjb != null;
	}

	public T GetObjectFromRaycast<T>(Vector3 origin, Vector3 direction, float range = 1000)
	{
		T tOjb = default(T);


		if (RayCast(origin, direction, out RaycastHit hit, range))
		{
			tOjb = hit.collider.GetComponent<T>();
		}
		return tOjb;
	}



	public bool RayFromCameraScreen(out RaycastHit hit, Vector3 screenPosition, float range = 1000)
	{
		Ray ray = mainCamera.ScreenPointToRay(screenPosition);
		DrawRayDebug(ray.origin, ray.direction, range);
		return Physics.Raycast(ray.origin, ray.direction, out hit, range);
	}


	public bool RayFromCameraViewport(out RaycastHit hit, Vector2 viewportPoint, float range = 1000)
	{
		Ray ray = mainCamera.ViewportPointToRay(viewportPoint);
		DrawRayDebug(ray.origin, ray.direction, range);

		return Physics.Raycast(ray, out hit, range);
	}

	public bool RayCast(Vector3 origin, Vector3 direction, out RaycastHit hit, float range = 1000)
	{
		DrawRayDebug(origin, direction, range);
		return Physics.Raycast(origin, direction, out hit, range);
	}


	public bool GetObjectFromRaycastAll<T>(Vector3 origin, Vector3 direction, out T[] tList, float range = 1000)
	{
		tList = GetObjectFromRaycastAll<T>(origin, direction, range);

		return tList.Any();

	}


	public T[] GetObjectFromRaycastAll<T>(Vector3 origin, Vector3 direction, float range = 1000)
	{
		List<T> list = new List<T>();
		DrawRayDebug(origin, direction, range);

		if (RaycastAll(origin, direction, out RaycastHit[] hits, range))
		{
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].collider.TryGetComponent(out T t))
				{
					list.Add(t);
				}
			}
		}

		return list.ToArray();
	}

	public bool RaycastAll(Vector3 origin, Vector3 direction, out RaycastHit[] hits, float range = 1000)
	{
		hits = Physics.RaycastAll(origin, direction, range);
		DrawRayDebug(origin, direction, range);
		return hits.Any(x => x.collider);
	}


	private void DrawRayDebug(Vector3 origin, Vector3 direction, float range = 1000)
	{
#if UNITY_EDITOR
		Debug.DrawRay(origin, direction * range, Color.green, 1);
#endif
	}


}
