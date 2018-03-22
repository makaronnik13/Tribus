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
    public void EndTurn()
    {
		GameLobby.Instance.EndTurn ();
        GetComponentInChildren<DropSlot>().ResetDrop();
        _myTurn = false;
    }

	public void StartTurn(Player player)
    {
        _myTurn = true;
		CardsManager.Instance.SelectPlayer (player);
        ResourcesManager.Instance.StartTurn();
    }

	public void SkipTurn()
	{
        EndTurn();
	}
}
