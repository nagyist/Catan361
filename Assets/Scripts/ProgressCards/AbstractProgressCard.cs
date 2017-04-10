using System;

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
}