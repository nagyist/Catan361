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
	public StealableType Resource { get; set; }
	public bool IsWater { get; set; }

	public HexTile() {
		
	}
}
