using System;

[Serializable]
public class BarbarianEvent
{
	public int BarbarianCounter = 7;
	public Vec3 BarbarianPosition;
	public int BarbarianInvasionCounter = 0;

	public bool BarbarianInvasionTriggered() {
		return BarbarianCounter == 0;
	}

	public void Reset() {
		BarbarianCounter = 7;
		BarbarianInvasionCounter++;
	}
}

