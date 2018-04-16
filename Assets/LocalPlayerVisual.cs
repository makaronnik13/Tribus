using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerVisual : MonoBehaviour {

    public Button endTurnButton;
    public PlayersVisualizer playersVisualiser;
    public CardsLayout hand;

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        foreach (Transform cv in hand.transform)
        {
            cv.GetComponent<CardVisual>().UpdateAvaliablility();
        }
    }

    public void StartTurn()
    {
        endTurnButton.interactable = true;
        FindObjectOfType<DropSlot>().ResetDrop();
        foreach (Transform cv in hand.transform)
        {
            cv.GetComponent<CardVisual>().UpdateAvaliablility();
        }
    }

    public void GetCard(Card card)
    {
        CardsManager.Instance.GetCard(card);
    }
}
