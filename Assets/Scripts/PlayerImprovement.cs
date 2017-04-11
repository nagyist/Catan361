using System;

[Serializable]
public class PlayerImprovement
{
	public enum TradeImprovement {
		None = 0, Level1 = 1, Level2 = 2, Level3_TradingHouse = 3, Level4_Metropolis = 4, Level5 = 5
	}

	public enum PoliticsImprovement {
		None = 0, Level1 = 1, Level2 = 2, Level3_Fortress = 3, Level4_Metropolis = 4, Level5 = 5
	}

	public enum ScienceImprovement {
		None = 0, Level1 = 1, Level2 = 2, Level3_Aqueduct = 3, Level4_Metropolis = 4, Level5 = 5
	}

	public TradeImprovement CurrentTradeImprovement = TradeImprovement.None;
	public PoliticsImprovement CurrentPoliticsImprovement = PoliticsImprovement.None;
	public ScienceImprovement CurrentScienceImprovement = ScienceImprovement.None;

	public void ImproveTrade() {
		if ((int)(CurrentTradeImprovement + 1) > 5) {
			return;
		}

		CurrentTradeImprovement++;
	}

	public void ImprovePolitics() {
		if ((int)(CurrentPoliticsImprovement + 1) > 5) {
			return;
		}

		CurrentPoliticsImprovement++;
	}

	public void ImproveScience() {
		if ((int)(CurrentScienceImprovement + 1) > 5) {
			return;
		}

		CurrentScienceImprovement++;
	}
}
