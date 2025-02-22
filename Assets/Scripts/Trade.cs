﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Trade {
	public enum TradePlayerOfferStatus
	{
		PlayerAccepted,
		PlayerRejected,
		Undecided
	}

	public string Player1;
	public string Player2;

	public TradePlayerOfferStatus Player1Accepted = TradePlayerOfferStatus.Undecided;
	public TradePlayerOfferStatus Player2Accepted = TradePlayerOfferStatus.Undecided;

	public Dictionary<StealableType, int> Player1Offer = new Dictionary<StealableType, int>();
	public Dictionary<StealableType, int> Player2Offer = new Dictionary<StealableType, int>();

	public int GetPlayer1OfferingFor(StealableType type) {
		if (!Player1Offer.ContainsKey (type)) {
			return 0;
		}

		return Player1Offer [type];
	}

	public int GetPlayer2OfferingFor(StealableType type) {
		if (!Player2Offer.ContainsKey (type)) {
			return 0;
		}

		return Player2Offer [type];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
