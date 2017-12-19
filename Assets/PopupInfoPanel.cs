using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoPanel : MonoBehaviour {

    public float speed;
    private Image[] images;
    private Text[] texts;
    private bool moving;

    public void Emmit()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
        moving = true;
        Invoke("Hide", speed * 2);
    }
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
            foreach (Image img in images)
            {
                img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, 0), Time.deltaTime / speed * 2);
            }

            foreach (Text img in texts)
            {
                img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, 0), Time.deltaTime / speed * 2);
            }
        }
    }

    void Hide()
    {
        transform.localPosition = Vector3.zero;
    }
}
