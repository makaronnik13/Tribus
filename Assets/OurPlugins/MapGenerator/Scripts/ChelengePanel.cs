using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace Tribus
{
	public class ChelengePanel : MonoBehaviour
	{

		public float choosingTime = 30;
		public Chalenge testChallenge;
		public Slider timeSlider;
		public GameObject Visual;
		public TextMeshProUGUI StateText;
		public Transform VariantsHab;
		public GameObject VariantPrefab;
		public Image StateImg;

		[Button ("test")]
		public void Test ()
		{
			ShowPanel (testChallenge);
		}

		// Use this for initialization
		void Start ()
		{
			FindObjectOfType<RoomActivator> ().OnChellengeIn += ShowPanel;
		}

		private void ShowPanel (Chalenge chellenge)
		{
			timeSlider.maxValue = choosingTime;
			timeSlider.value = choosingTime;
			Visual.SetActive (true);
			StartChellenge (chellenge);
			StartCoroutine (Tick (choosingTime));
		}

		public void HidePanel ()
		{
			StopAllCoroutines ();
			Visual.SetActive (false);
		}

		private IEnumerator Tick(float choosingTime)
		{
			float t = 0;
			while (t <= choosingTime)
			{
				t += Time.deltaTime;
				timeSlider.value = timeSlider.maxValue - t;
				yield return new WaitForSeconds(Time.deltaTime);
			}

			HidePanel();
		}

		private void StartChellenge(Chalenge chellenge)
		{
			ComeToState (chellenge.StartState);
		}

		private void ChooseVariant(ChellengeVariant variant)
		{
			if (!variant.aimState) 
			{
				HidePanel ();
			} else 
			{
				ComeToState (variant.aimState);
			}
		}

		private void ComeToState(CellengeState state)
		{
			StateText.text = state.Text;
			StateImg.sprite = state.Img;

			foreach(Transform t in VariantsHab)
			{
				Destroy (t.gameObject);
			}

			foreach(ChellengeVariant cv in state.Variants)
			{
				GameObject newVariant = Instantiate (VariantPrefab);
				newVariant.transform.SetParent (VariantsHab);
				newVariant.transform.localScale = Vector3.one;
				newVariant.transform.localPosition = Vector3.zero;
				newVariant.GetComponent<VariantButton> ().Init (cv, (ChellengeVariant variant)=>{ChooseVariant(variant);});
			}
		}

	}
}
