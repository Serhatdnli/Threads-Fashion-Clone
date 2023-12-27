using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu]
public class SweingScriptableObject : ScriptableObject
{
	[SerializeField] private MachineInfo[] machinesInfo;

	public MachineInfo GetMachineInfo(PoolType type) => machinesInfo.Find(x => x.Type == type);
}


[Serializable]
public struct MachineInfo
{
	[PreviewField] public Sprite ProductIcon;
	public PoolType Type;
	public float ProcessTime;

}