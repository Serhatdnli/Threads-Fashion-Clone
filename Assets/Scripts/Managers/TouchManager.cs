using System;
using UnityEngine;

public class TouchManager : Singleton<TouchManager>
{

	public Action<Vector3> OnMouseDown { get; set; }
	public Action<Vector3> OnMouseHold { get; set; }
	public Action<Vector3> OnMouseUp { get; set; }


	private void Update()
	{
		TouchListener();
	}

	private void TouchListener()
	{
		if (Input.GetMouseButtonDown(0))
			MouseDown();
		if (Input.GetMouseButton(0))
			MouseHold();
		if (Input.GetMouseButtonUp(0))
			MouseUp();
	}

	private void MouseDown() => OnMouseDown?.Invoke(Input.mousePosition);

	private void MouseHold() => OnMouseHold?.Invoke(Input.mousePosition);

	private void MouseUp() => OnMouseUp?.Invoke(Input.mousePosition);



}
