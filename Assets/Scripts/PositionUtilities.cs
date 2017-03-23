using System;

[Serializable]
public struct Vec3 {
	public int x;
	public int y; 
	public int z;

	public Vec3(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public override bool Equals(System.Object obj) {
		if (obj == null || GetType () != obj.GetType ())
			return false;

		Vec3 v = (Vec3) obj;
		return v.x == this.x &&
		v.y == this.y &&
		v.z == this.z;
	}

	public override int GetHashCode() {
		return new UnityEngine.Vector3 (x, y, z).GetHashCode ();
	}
}

public static class PositionUtilities {
	public static byte[] PosToByte(Vec3 pos) {
		byte[] xBytes = BitConverter.GetBytes ((int)pos.x);
		byte[] yBytes = BitConverter.GetBytes ((int)pos.y);
		byte[] zBytes = BitConverter.GetBytes ((int)pos.z);

		byte[] result = new byte[64];

		for (int i = 0; i < xBytes.Length; i++) {
			result [i] = xBytes [i];
		}

		for (int j = 0; j < yBytes.Length; j++) {
			result [j + 3 - 1] = yBytes [j];
		}

		for (int k = 0; k < zBytes.Length; k++) {
			result [k + 6 - 1] = zBytes [k];
		}

		return result;
	}
}