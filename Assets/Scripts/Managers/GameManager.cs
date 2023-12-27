using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField, ReadOnly] PoolContainer container;
	private bool gameIsRunning { get; set; }


	private void Awake()
	{
#if UNITY_EDITOR
		Application.targetFrameRate = -1;
#endif
		Application.targetFrameRate = 60;
		container = Resources.Load<PoolContainer>(Constants.POOLCONTAINER_PATH);
		container.Init();
		DataManager.Init();
	}
	private void Start()
	{
		GameStart();
	}

	private void GameStart()
	{
		gameIsRunning = true;
	}


	private void GameRestart()
	{
		ResetAllSetup();
	}

	private void ResetAllSetup()
	{
		StopAllCoroutines();
		DOTween.KillAll();
		ActionManager.ResetAllStaticsVariables(typeof(ActionManager));
		ActionManager.ResetAllStaticsVariables(typeof(PoolManager));
		ActionManager.ResetAllStaticsVariables(typeof(DataManager));
	}

	private void OnDestroy()
	{
		ResetAllSetup();
	}


}

public static partial class ActionManager
{

}

public static partial class Constants
{
	public const string POOLCONTAINER_PATH = "Pool Container";
}