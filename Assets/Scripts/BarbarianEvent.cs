using System;

[Serializable]
public class BarbarianEvent
{
	public int BarbarianCounter = 7;
	public Vec3 BarbarianPosition;

	public bool BarbarianInvasionTriggered() {
		return BarbarianCounter == 0;
	}
}

