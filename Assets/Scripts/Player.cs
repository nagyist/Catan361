using System;

// Written by Alex B
using System.Collections.Generic;



	public class Player
	{
		// private username and password
		private string username;
		private string password;
		// status, vps, gold, and longestRoad are simple c# attributes
		private PlayerStatus status { get; set; }
		private int defenderOfCatanVPs { get; set; }
		private int gold { get; set; }
		private bool longestRoad { get; set; }
		// using dictionary for holding resources and commodities
		public Dictionary<StealableType, int> ownedResources= new Dictionary<StealableType, int>();
		// using list for holding owned progress cards
		List<ProgressCardType> ownedProgressCards = new List<ProgressCardType>();
		List<OwnableUnit> ownedUnits = new List<OwnableUnit>();

		// NOTE: we can use the built-in methods for the list and dictionary to edit the values

		// constructor
		public Player (string u, string p)
		{
			this.username = u;
			this.password = p;
			this.status = PlayerStatus.Available;
			this.defenderOfCatanVPs = 0;
			this.defenderOfCatanVPs = 0;
			this.gold = 0;
			this.longestRoad = false;
		}

	}


