using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
	private Dictionary<AreaType, Transform> cameraViewDict;
	[SerializeField] private AreaType currentArea;
	private void AddCameraView(AreaType areaType, Transform target) => cameraViewDict.Add(areaType, target);
	private void Awake()
	{
		cameraViewDict = new Dictionary<AreaType, Transform>();

		ActionManager.AddCameraView += AddCameraView;
		ActionManager.ChangeArea += ChangeArea;
		ActionManager.GetCurrentArea = () => currentArea;
	}	

	private void ChangeArea(AreaType type)
	{
		currentArea = type;
		Transform target = cameraViewDict[type];

		transform.position = target.position;
		transform.eulerAngles = target.eulerAngles;

	}
}



public delegate void AddCameraViewDelegate(AreaType type, Transform target);
public static partial class ActionManager
{

	public static Action<AreaType> ChangeArea { get; set; }
	public static Func<AreaType> GetCurrentArea { get; set; }

	public static AddCameraViewDelegate AddCameraView { get; set; }
}

public enum AreaType
{
	SweingArea = 0,
	PaintingArea = 1,

}