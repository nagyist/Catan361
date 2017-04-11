using System;

[Serializable]
public abstract class AbstractProgressCard
{
	public enum ProgressCardType {
		Science,
		Politic,
		Trade
	}

	public ProgressCardType CardType;

	public AbstractProgressCard ()
	{
	}

	public abstract void ExecuteCardEffect();

	public virtual string GetTitle() {
		return "No Title";
	}

	public virtual string GetDescription() {
		return "No Description";
	}
}