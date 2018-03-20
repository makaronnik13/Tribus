using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FakeController : MonoBehaviour {

	public Card[] fakePile;

	public void SkipTurn()
	{
		GetComponentInChildren<DropSlot> ().ResetDrop ();

		CardsManager.Instance.GetCard ();

		for (int i = CardsManager.Instance.CardsCount; i < 5; i++) 
		{
			CardsManager.Instance.GetCard ();
		}
	}

	void Start()
	{
		CardsManager.Instance.GeneratePile (fakePile.ToList());
		for(int i = 0;i<5;i++)
		{
			CardsManager.Instance.GetCard ();
		}
	}
}
