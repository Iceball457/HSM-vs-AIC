using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GunDataSerializer {
	public static void WriteGun (this NetworkWriter writer, GunData gun) {
		writer.WriteString ("Guns/" + gun.name);
	}
	public static GunData ReadGun (this NetworkReader reader) {
		return Resources.Load<GunData> (reader.ReadString ());
	}
}