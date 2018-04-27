using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockVisual : MonoBehaviour {


    private Slider _blockSlider;
    private Slider BlockSlider
    {
        get
        {
            if (!_blockSlider)
            {
                _blockSlider = GetComponent<Slider>();
            }
            return _blockSlider;
        }
    }

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
            _block = value;
            BlockSlider.value = value;
            BlockText.text = value+"";
        }
    }

    public void Init(int max)
    {
        BlockSlider.maxValue = max;
        Block = 0;
    }

    public void ValueChanged()
    {
        float v = GetComponent<Slider>().value;

            foreach (Image img in GetComponentsInChildren<Image>())
            {
                img.enabled = v > 0;
            }
        GetComponentInChildren<TextMeshProUGUI>().enabled = v > 0;
    }
}
