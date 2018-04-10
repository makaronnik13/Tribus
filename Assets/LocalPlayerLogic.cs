using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerLogic : Singleton<LocalPlayerLogic> {

    public GamePlayer player;
    private LocalPlayerVisual visual;

    private void Start()
    {
        visual = GetComponent<LocalPlayerVisual>();
    }

    public void OnEndTurnPush()
    {
        player.EndTurn();
    }

    public void EndTurn()
    {
        Debug.Log("local logic turn end");
        visual.EndTurn();
    }

    public void StartTurn()
    {
        Debug.Log("local logic turn start");
        visual.StartTurn();
    }

    public void SetPlayerList(GamePlayer[] players)
    {
        visual.SetPlayersList(players);
    }
}
