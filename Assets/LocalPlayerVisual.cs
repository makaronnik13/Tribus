using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerVisual : MonoBehaviour {

    public Button endTurnButton;
    public PlayersVisualizer playersVisualiser;

    public void EndTurn()
    {
        endTurnButton.interactable = false;
    }

    public void StartTurn()
    {
        endTurnButton.interactable = true;
    }
}
