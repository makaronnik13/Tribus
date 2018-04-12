using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CardGameVisual : NetworkBehaviour {

    public PlayersVisualizer playersVisualiser;

    [Server]
    public void SetPlayersList(GamePlayer[] players)
    {
        Debug.Log(playersVisualiser);
        Debug.Log(players);
        playersVisualiser.Init(players.ToList().Select(gp => gp.player).ToList());
    }
}
