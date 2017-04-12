using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardFront : MonoBehaviour {

	public bool Turned = false;
	public bool CardSelected = false;
	public AbstractProgressCard CurrentCard;

	// Use this for initialization
	void Start () {
		CurrentCard = new BishopCard(-1);
		CardSelected = false;
	}

	public void ClickOnCard() {
		Turned = !Turned;
	}

	// Update is called once per frame
	void Update () {
		if (CurrentCard == null) {
			return;
		}

		if (!CardSelected) {
			transform.FindChild ("Front").gameObject.SetActive (false);
			transform.FindChild ("Back").gameObject.SetActive (true);

			transform.FindChild ("Back").FindChild ("BackEmpty").gameObject.SetActive (true);
			transform.FindChild ("Back").FindChild ("BackScience").gameObject.SetActive (false);
			transform.FindChild ("Back").FindChild ("BackPolitics").gameObject.SetActive (false);
			transform.FindChild ("Back").FindChild ("BackTrade").gameObject.SetActive (false);
			return;
		}

		AbstractProgressCard.ProgressCardType currentType = CurrentCard.CardType;


		if (Turned) {
			transform.FindChild ("Front").gameObject.SetActive (true);
			transform.FindChild ("Back").gameObject.SetActive (false);

			// TODO : image
			transform.FindChild ("Front").FindChild ("CardTitle").GetComponent<Text> ().text = CurrentCard.GetTitle ();
			transform.FindChild ("Front").FindChild ("CardDescription").GetComponent<Text> ().text = CurrentCard.GetDescription ();

			if (currentType == AbstractProgressCard.ProgressCardType.Science) {
				transform.FindChild ("Front").FindChild ("CrystalScience").gameObject.SetActive (true);
				transform.FindChild ("Front").FindChild ("CrystalPolitics").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalTrade").gameObject.SetActive (false);
			} else if (currentType == AbstractProgressCard.ProgressCardType.Politic) {
				transform.FindChild ("Front").FindChild ("CrystalScience").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalPolitics").gameObject.SetActive (true);
				transform.FindChild ("Front").FindChild ("CrystalTrade").gameObject.SetActive (false);
			} else if (currentType == AbstractProgressCard.ProgressCardType.Trade) {
				transform.FindChild ("Front").FindChild ("CrystalScience").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalPolitics").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalTrade").gameObject.SetActive (true);
			} else {
				transform.FindChild ("Front").FindChild ("CrystalScience").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalPolitics").gameObject.SetActive (false);
				transform.FindChild ("Front").FindChild ("CrystalTrade").gameObject.SetActive (false);
			}
		} else {
			transform.FindChild ("Front").gameObject.SetActive (false);
			transform.FindChild ("Back").gameObject.SetActive (true);

			if (currentType == AbstractProgressCard.ProgressCardType.Science) {
				transform.FindChild ("Back").FindChild ("BackEmpty").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackScience").gameObject.SetActive (true);
				transform.FindChild ("Back").FindChild ("BackPolitics").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackTrade").gameObject.SetActive (false);
			} else if (currentType == AbstractProgressCard.ProgressCardType.Politic) {
				transform.FindChild ("Back").FindChild ("BackEmpty").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackScience").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackPolitics").gameObject.SetActive (true);
				transform.FindChild ("Back").FindChild ("BackTrade").gameObject.SetActive (false);
			} else if (currentType == AbstractProgressCard.ProgressCardType.Trade) {
				transform.FindChild ("Back").FindChild ("BackEmpty").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackScience").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackPolitics").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackTrade").gameObject.SetActive (true);
			} else {
				transform.FindChild ("Back").FindChild ("BackEmpty").gameObject.SetActive (true);
				transform.FindChild ("Back").FindChild ("BackScience").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackPolitics").gameObject.SetActive (false);
				transform.FindChild ("Back").FindChild ("BackTrade").gameObject.SetActive (false);
			}
		}
	}
}
