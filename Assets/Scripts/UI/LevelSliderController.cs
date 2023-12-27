using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSliderController : MonoBehaviour
{
	[Title("Requirements")]
	[SerializeField] private TextMeshProUGUI fromLevelText;
	[SerializeField] private TextMeshProUGUI toLevelText;
	[SerializeField] private Slider levelSlider;
	[SerializeField] private Color expColor;

	private void Awake()
	{
		ActionManager.UpdateLevelSlider += UpdateUI;
	}

	private void UpdateUI(float requiredExp)
	{
		DOTween.Kill(GetHashCode());
		int fromLevel = PrefManager.Level;
		fromLevelText.text = fromLevel.ToString();
		toLevelText.text = (fromLevel + 1).ToString();
		float ratio = PrefManager.CurrentExp / requiredExp;
		levelSlider.DOValue(ratio, .2f).SetEase(Ease.OutQuad).SetId(GetHashCode());
	}
}
