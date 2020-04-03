using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using System;

public class InitiativeTimeline : Singleton<InitiativeTimeline> {

	public Gradient timeLineColorV;
	public Gradient timeLineColorH;
	public Image timeLineImage;
	private float textureHeight = 100;
	private float textureWidth = 25;

	public Action OnTick = ()=>{};
	private float timeToTick = 0;

	private GameObject token
	{
		get
		{
			return (GameObject)Resources.Load<GameObject> ("InitiativeTimeline/Token");
		}
	}

	// Use this for initialization
	public void Start () 
	{
		timeLineImage.sprite = GetGradientSprite (timeLineColorV, timeLineColorH);
	}

	void Update()
	{
		timeToTick+=InitiativeToken.speedCoef*Time.deltaTime;
		if(timeToTick>=1)
		{
			OnTick();
			timeToTick = 0;
		}	
	}

	public void StartBattle(WarriorObject[] warriors)
	{
		Clear ();
		foreach(WarriorObject bw in warriors)
			{
				AddWarrior (bw);
			}
			Show ();
	}



	private void Show()
	{
		GetComponent<Animator> ().SetBool ("Active", true);
		Invoke ("StartTimeline", 3f);
	}

	public void Hide()
	{
		StopTimeline ();
		GetComponent<Animator> ().SetBool ("Active", false);
	}

	public void AddWarrior(WarriorObject bw)
	{
        //Lean.Pool.LeanPool.Spawn(token, transform.GetChild(1).position, Quaternion.identity, transform.GetChild(1)).GetComponent<InitiativeToken>().Init(bw);
        Instantiate (token, transform.GetChild(1).position, Quaternion.identity ,transform.GetChild(1)).GetComponent<InitiativeToken>().Init(bw);
	}

	private void Clear()
	{
		foreach(Transform t in transform.GetChild(1))
		{
            Destroy(t.gameObject);
		}
	}

	public void StartTimeline()
	{
		InitiativeToken.speedCoef = 1;
	}

	private void StopTimeline()
	{
		InitiativeToken.speedCoef = 0;
	}

	private Sprite GetGradientSprite(Gradient gradientV, Gradient gradientH)
	{
		Texture2D texture = new Texture2D ((int)textureWidth, (int)textureHeight);
		for(int i = 0; i< textureHeight; i++)
		{
			for(int j = 0;j<textureWidth;j++)
			{
				Color c = gradientV.Evaluate(i/textureHeight);
				float x = gradientH.Evaluate (j/textureWidth).a;
				c.a *= x;
				texture.SetPixel (j, i, c);
			}
		}
		texture.Apply ();
		return Sprite.Create (texture, new Rect(0,0,texture.width, texture.height), Vector2.one/2);
	}

	public void StartTurn(WarriorObject warrior)
	{
		StopTimeline ();
		BattleField.Instance.GiveTurn (warrior);
	}

    public void RemoveWarrior(WarriorObject warriorObject)
    {
        foreach (InitiativeToken token in GetComponentsInChildren<InitiativeToken>())
        {
            if (token.warrior == warriorObject)
            {
                Destroy(token.gameObject);
            }
        }
    }
}
