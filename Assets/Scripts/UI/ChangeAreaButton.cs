using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAreaButton : UIButtonBase
{
	[SerializeField] private AreaType areaType;

	protected override void Awake()
	{
		base.Awake();
		ActionManager.ChangeArea += ChangedArea;
	}

	protected override void OnButtonClicked()
	{
		base.OnButtonClicked();
		ActionManager.ChangeArea?.Invoke(areaType);

	}

	private void ChangedArea(AreaType _areaType)
	{
		gameObject.SetActive(areaType != _areaType);
	}


}
