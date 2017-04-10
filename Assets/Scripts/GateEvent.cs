using System;

[Serializable]
public class GateEvent
{
	public RollDiceScript.EventDiceOutcome GateOutcome;
	public AbstractProgressCard.ProgressCardType CardType;

	public GateEvent(RollDiceScript.EventDiceOutcome outcome) {
		GateOutcome = outcome;
		if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Green) {
			CardType = AbstractProgressCard.ProgressCardType.Science;
		} else if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Blue) {
			CardType = AbstractProgressCard.ProgressCardType.Politic;
		} else if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Red) {
			CardType = AbstractProgressCard.ProgressCardType.Trade;
		}
	}
}
