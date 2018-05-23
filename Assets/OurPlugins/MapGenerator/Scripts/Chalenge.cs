
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/Chellenge/Chellenge")]
public class Chalenge : ScriptableObject
{
    public string ChalengeName;
    public float HardLvl;

	public CellengeState StartState;
}