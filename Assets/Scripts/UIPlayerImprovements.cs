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

	GamePlayer getPlayer() {
		return GameManager.LocalPlayer.GetComponent<GamePlayer> ();
	}

	public void ClickImproveTrade() {
		PlayerImprovement newImprov = getImprovement ();
		int nextLevel = (int)newImprov.CurrentTradeImprovement + 1;
		bool craneApplied = false;
		if (getPlayer ().craneProgressCardDiscount) {
			nextLevel--;
			craneApplied = true;
		}

		Dictionary<StealableType, int> reqRes = new Dictionary<StealableType, int> () {
			{StealableType.Commodity_Cloth, nextLevel}
		};

		if (!GameManager.LocalPlayer.GetComponent<GamePlayer> ().HasEnoughResources (reqRes)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You don't have enought cloth to upgrade to level " + nextLevel));
			return;
		}

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdConsumeResources (SerializationUtils.ObjectToByteArray (reqRes));

		if (craneApplied) {
			GameManager.GUI.ShowMessage ("Crane progress card applied.");
			getPlayer ().craneProgressCardDiscount = false;
		}

		newImprov.ImproveTrade ();
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (getPlayerName() + " improved his trade.");
	}

	public void ClickImprovePolitics() {
		PlayerImprovement newImprov = getImprovement ();
		int nextLevel = (int)newImprov.CurrentPoliticsImprovement + 1;
		bool craneApplied = false;
		if (getPlayer ().craneProgressCardDiscount) {
			nextLevel--;
			craneApplied = true;
		}

		Dictionary<StealableType, int> reqRes = new Dictionary<StealableType, int> () {
			{StealableType.Commodity_Coin, nextLevel}
		};

		if (!GameManager.LocalPlayer.GetComponent<GamePlayer> ().HasEnoughResources (reqRes)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You don't have enought coin to upgrade to level " + nextLevel));
			return;
		}

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdConsumeResources (SerializationUtils.ObjectToByteArray (reqRes));

		if (craneApplied) {
			GameManager.GUI.ShowMessage ("Crane progress card applied.");
			getPlayer ().craneProgressCardDiscount = false;
		}

		newImprov.ImprovePolitics ();
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (getPlayerName() + " improved his politic.");
	}

	public void ClickImproveScience() {
		PlayerImprovement newImprov = getImprovement ();
		int nextLevel = (int)newImprov.CurrentScienceImprovement + 1;
		bool craneApplied = false;
		if (getPlayer ().craneProgressCardDiscount) {
			nextLevel--;
			craneApplied = true;
		}

		Dictionary<StealableType, int> reqRes = new Dictionary<StealableType, int> () {
			{StealableType.Commodity_Paper, nextLevel}
		};

		if (!GameManager.LocalPlayer.GetComponent<GamePlayer> ().HasEnoughResources (reqRes)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You don't have enough paper to upgrade to level " + nextLevel));
			return;
		}

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdConsumeResources (SerializationUtils.ObjectToByteArray (reqRes));

		if (craneApplied) {
			GameManager.GUI.ShowMessage ("Crane progress card applied.");
			getPlayer ().craneProgressCardDiscount = false;
		}

		newImprov.ImproveScience ();
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdChangeImprovement (newImprov);
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (getPlayerName() + " improved his science.");
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
