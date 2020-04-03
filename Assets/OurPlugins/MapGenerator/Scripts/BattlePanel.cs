﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tribus
{
public class BattlePanel : MonoBehaviour {

    private GameObject _visual;
    private GameObject visual
    {
        get
        {
            if (!_visual)
            {
                _visual = transform.GetChild(0).gameObject;
            }
            return _visual;
        }
    }

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<RoomActivator>().OnBattleIn += ShowPanel;
    }

    private void ShowPanel(Battle battle)
    {
	   //PhotonNetwork.LoadLevelAsync(3);
        visual.SetActive(true);
    }

    public void HidePanel()
    {
        visual.SetActive(false);
    }
}
}
