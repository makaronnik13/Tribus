using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FakeController : Singleton<FakeController> {

    private bool _myTurn = false;
    public bool MyTurn
    {
        get
        {
            return _myTurn;
        }
    }

	public Card[] fakePile;

    public void EndTurn()
    {
        ResourcesManager.Instance.EndTurn();
        GetComponentInChildren<DropSlot>().ResetDrop();
        _myTurn = false;
    }

    public void StartTurn()
    {
        _myTurn = true;
        CardsManager.Instance.GetCard();
        for (int i = CardsManager.Instance.CardsCount; i < 5; i++)
        {
            CardsManager.Instance.GetCard();
        }
        ResourcesManager.Instance.StartTurn();
    }

	public void SkipTurn()
	{
        if (MyTurn)
        {
            EndTurn();
        }
        else
        {
            StartTurn();
        }
	}

	void Start()
	{
        CardsManager.Instance.GeneratePile(fakePile.ToList());
        StartTurn();
	}
}
