using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerImprovements : MonoBehaviour {
	
	GameObject tradeImprovement;
	GameObject politicsImprovement;
	GameObject scienceImprovement;

	void Start () {
		tradeImprovement = transform.FindChild ("Trade").gameObject;
		politicsImprovement = transform.FindChild ("Politics").gameObject;
		scienceImprovement = transform.FindChild ("Science").gameObject;
	}

	string getPlayerName() {
		return GetComponentInParent<PlayerResourcePanel> ().PlayerName;
	}

	public void ClickImproveTrade() {
		PlayerImprovement newImprov = getImprovement ();
		newImprov.ImproveTrade ();

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
	}

	public void ClickImprovePolitics() {
		PlayerImprovement newImprov = getImprovement ();
		newImprov.ImprovePolitics ();

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
	}

	public void ClickImproveScience() {
		PlayerImprovement newImprov = getImprovement ();
		newImprov.ImproveScience ();

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
	}

	PlayerImprovement getImprovement() {
		return GameManager.Instance.GetCurrentGameState ().CurrentPlayerImprovements.GetImprovementForPlayer (getPlayerName ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponentInParent<UIWindow> ().IsVisible) {
			return;
		}

		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		PlayerImprovement currentImprovements = GameManager.Instance.GetCurrentGameState ().CurrentPlayerImprovements.GetImprovementForPlayer (getPlayerName ());
		tradeImprovement.GetComponent<UIProgressBar> ().fillAmount = ((int) currentImprovements.CurrentTradeImprovement / 5.0f);
		politicsImprovement.GetComponent<UIProgressBar> ().fillAmount = ((int)currentImprovements.CurrentPoliticsImprovement / 5.0f);
		scienceImprovement.GetComponent<UIProgressBar> ().fillAmount = ((int)currentImprovements.CurrentScienceImprovement / 5.0f);

		if (getPlayerName () != GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			foreach (Button btn in GetComponentsInChildren<Button>()) {
				btn.gameObject.SetActive (false);
			}
		}
	}
}
