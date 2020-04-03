using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tribus/Chellenge/State")]
public class CellengeState : ScriptableObject 
{
	public string Text;
	public Sprite Img;
	public List<ChellengeVariant> Variants;
}
