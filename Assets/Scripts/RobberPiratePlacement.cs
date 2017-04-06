using System;

[Serializable]
public class RobberPiratePlacement
{
	public string EntityType;
	public Vec3 PlacementPos;
	public bool IsPlaced = false;

	public RobberPiratePlacement(string type, Vec3 pos) {
		this.EntityType = type;
		this.PlacementPos = pos;
		this.IsPlaced = true;
	}
}

