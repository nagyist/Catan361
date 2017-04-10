using System;

public class PrinterCard : AbstractProgressCard
{
	public PrinterCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Printer";
	}

	public override string GetDescription ()
	{
		return "1 Victory Point! Reveal this card immediately when you draw it. This card cannot be stolen by a spy.";
	}
}
