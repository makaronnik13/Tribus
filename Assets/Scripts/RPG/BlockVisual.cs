using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockVisual : MonoBehaviour {


	public Image BlockImg;
	public Image BlockFill;

    public TextMeshProUGUI BlockText;

    private int _block;
    public int Block
    {
        get
        {
            return _block;
        }
        set
        {
			GetComponentInParent<WarriorObject> ().EmmitParticle (_block - value, true);
            _block = value;
			SetBlock (_block);
        }
    }

    public void Init()
    {
		_block = 0;
		SetBlock (_block);
    }

	private void SetBlock(int value)
    {
		BlockText.text = value.ToString ();
		BlockText.enabled = value>0;
		BlockImg.enabled = value>0;
		BlockFill.enabled = value>0;
    }
}
