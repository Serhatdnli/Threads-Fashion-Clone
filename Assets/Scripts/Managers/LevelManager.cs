using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[Title("Requirement")]
	[SerializeField] private float requiredExpForLevel;


	void Start()
	{
		ActionManager.LevelChanged += UpdateUI;
		ActionManager.GainExp += GainExp;

		UpdateUI();


	}

	private void UpdateUI() => ActionManager.UpdateLevelSlider?.Invoke(requiredExpForLevel);

	private void GainExp(float _exp)
	{
		DOTween.Kill(GetHashCode());
		PrefManager.CurrentExp += _exp;

		ActionManager.UpdateLevelSlider?.Invoke(requiredExpForLevel);

		if (PrefManager.CurrentExp >= requiredExpForLevel)
		{
			PrefManager.CurrentExp = PrefManager.CurrentExp % 100;
			PrefManager.LevelUp();

		}

	}




}

public static partial class ActionManager
{
	public static Action<float> GainExp { get; set; }
	public static Action<float> UpdateLevelSlider { get; set; }

}
