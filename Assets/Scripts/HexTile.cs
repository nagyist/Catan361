using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class HexTile
{
	public int SelectedNum { get; set; }
	public int SelectedNum2 { get; set; }
	public int SelectedNum3 { get; set; }
	public int SelectedNum4 { get; set; }
	public int SelectedNum5 { get; set; }

	public StealableType Resource { get; set; }

	public bool IsWater { get; set; }
	public bool IsFishingGround { get; set; }
	public bool IsLakeTile { get; set; }
	public int FishingNum { get; set; }
	public int FishingReturnNum { get; set; }


	public HexTile() {
		
	}
}
