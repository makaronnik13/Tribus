using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tribus
{
public class ChestPanel : MonoBehaviour {

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
        FindObjectOfType<RoomActivator>().OnChestIn += ShowPanel;
    }

    private void ShowPanel()
    {
        visual.SetActive(true);
		CreateItems ();
    }

    public void HidePanel()
    {
        visual.SetActive(false);
    }

	private void CreateItems()
	{
			//List<Item> avaliableItems = 
	}
	}
}
