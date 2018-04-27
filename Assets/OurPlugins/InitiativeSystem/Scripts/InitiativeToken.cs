using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeToken : MonoBehaviour 
{
	
	private static float raundTime = 10;
	public static float speedCoef = 0;
	private RectTransform parentTransform;
	private float value = 0;
	private float visualValue = 0;
	private static float startValue = 0f;

	private Color playerColor = Color.gray;

    private bool selected = false;
	private float randomInitiativeModificator = 0;

	public WarriorObject warrior;

	private RectTransform line
	{
		get
		{
			return transform.GetChild (0).GetComponent<RectTransform> ();
		}
	}

	private Image circle
	{
		get
		{
			return transform.GetChild (1).GetComponent<Image> ();
		}
	}

	private Image portrait
	{
		get
		{
			return transform.GetChild (2).GetChild(0).GetComponent<Image> ();
		}
	}

	private RectTransform portraitTransform
	{
		get
		{
			return transform.GetChild (2).GetComponent<RectTransform> ();
		}
	}

	private float speed
	{
		get
		{
			return Mathf.Clamp(warrior.WarriorAsset.initiative+randomInitiativeModificator,1,10) /10;
		}
	}

	private RectTransform rectTransform
	{
		get
		{
			return GetComponent<RectTransform> ();
		}
	}

	//public BattleWarrior warrior;

	public void ChangeValue(float changeAmount)
	{
		SetValue (value+changeAmount);
	}

	public void SetValue(float v)
	{
		value = Mathf.Clamp (v, 0, 1);
	}

	public void Init(WarriorObject warrior)
	{
		if(warrior.Player!=null)
		{
			playerColor = RPGCardGameManager.sInstance.GetPlayerColor (warrior.Player);
            Debug.Log("set color " + playerColor +" "+warrior.warriorName);
        }
		this.warrior = warrior;
		value = startValue;
		parentTransform = transform.parent.GetComponent<RectTransform> ();
		InvokeRepeating ("MoveForward", 0, 0.01f);



			line.GetComponent<Image>().color = playerColor;
			circle.color = playerColor;

		portrait.sprite = warrior.WarriorAsset.sprite;
		randomInitiativeModificator = Random.Range (-0.5f, 0.5f);
		visualValue = (value - 0.5f) * parentTransform.rect.height;

		transform.localPosition = new Vector3(transform.localPosition.x, visualValue ,transform.localPosition.z);
	}

	void Update () 
	{
        float yPos = 0;

		switch (warrior.IsEnemy)
        {
		case false:
			yPos = -rectTransform.rect.height;
                break;
		case true:
			yPos = rectTransform.rect.height;
                break;
        }

		line.GetComponent<Image>().color = Color.Lerp(line.GetComponent<Image>().color, playerColor, Time.deltaTime);
		circle.color = Color.Lerp(circle.color, playerColor, Time.deltaTime);

        rectTransform.localPosition = new Vector2(Mathf.Lerp(rectTransform.localPosition.x, yPos*0.8f, Time.deltaTime), rectTransform.localPosition.y);
		line.localPosition = new Vector2(line.localPosition.x, Mathf.Lerp(line.localPosition.y, -yPos / 2, Time.deltaTime));

     
		visualValue = Mathf.Lerp (visualValue, (value-0.5f)*parentTransform.rect.height, Time.deltaTime*8*speedCoef);
		transform.localPosition = new Vector3(transform.localPosition.x, visualValue,transform.localPosition.z);
		if (selected) {
			portraitTransform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one * 1.5f, Time.deltaTime*5);
			circle.transform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one * 1.5f, Time.deltaTime*5);
		} else {
			portraitTransform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one, Time.deltaTime*5);
			circle.transform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one, Time.deltaTime*5);
		}
	}

	private void MoveForward()
	{
		value += (speedCoef* speed * 0.01f / raundTime);
		if(value>=1)
		{
			speedCoef = 0;
			value = startValue;
			randomInitiativeModificator = Random.Range (-0.5f, 0.5f);
			InitiativeTimeline.Instance.StartTurn(warrior);
		}
	}

	public void ResumeInitiative()
	{
		speedCoef = 1;
	}

	public void ShowInfo()
	{
		transform.SetSiblingIndex (transform.parent.childCount-1);
		//GameController.Instance.HighlightedWarrior = warrior;
		selected = true;
	}

	public void HideInfo()
	{
		//GameController.Instance.HighlightedWarrior = null;
		selected = false;
	}
}
