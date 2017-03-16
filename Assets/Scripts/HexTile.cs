using System;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class HexTile
{
	public int SelectedNum { get; set; }
	public StealableType Resource { get; set; }
	public bool IsWater { get; set; }

	public HexTile() {
		
	}
}
