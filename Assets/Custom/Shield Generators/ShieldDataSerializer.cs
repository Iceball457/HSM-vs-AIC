using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShieldDataSerializer {
	public static void WriteShield (this NetworkWriter writer, ShieldData armor) {
		if (armor != null) {
			writer.WriteString ("Shields/" + armor.name);
		} else {
			writer.WriteString ("null");
		}
	}
	public static ShieldData ReadShield (this NetworkReader reader) {
		string path = reader.ReadString ();
		if (path != "null") {
			return Resources.Load<ShieldData> (path);
		} else {
			return null;
		}
	}
}