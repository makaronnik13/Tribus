using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CounterPanel : Singleton<CounterPanel> {

	public TextMeshProUGUI counter, text;
	public Image avatar, border;
	private float timer;
	private Action calback;

	public void RunCounter(Player nextPlayer, int time, Action calback)
	{
		EventSystem.current.SetSelectedGameObject (gameObject);
		this.calback = calback;
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (true);
		}
		avatar.sprite = nextPlayer.PlayerAvatar;
		border.color = nextPlayer.PlayerColor;
		text.text = nextPlayer.PlayerName + " turn in";
		StartCoroutine (StartCount(time));
	}

	public void Skip()
	{
		if (timer > 4) 
		{
			return;
		}
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive (false);
		}	
		if (calback!= null) 
		{
			timer = 0;
			EventSystem.current.SetSelectedGameObject (null);
			calback.Invoke ();
		}
		calback = null;
	}

	private IEnumerator StartCount(float time)
	{
		timer = time;
		while(timer>0)
		{
			counter.text = Mathf.CeilToInt (timer)+"";
			timer -= Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		Skip ();
	}
}
