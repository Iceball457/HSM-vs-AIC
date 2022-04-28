using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArmorDataSerializer {
	public static void WriteArmor (this NetworkWriter writer, ArmorData armor) {
		if (armor != null) {
			writer.WriteString ("Armor/" + armor.name);
		} else {
			writer.WriteString ("null");
		}
	}
	public static ArmorData ReadArmor (this NetworkReader reader) {
		string path = reader.ReadString ();
		if (path != "null") {
			return Resources.Load<ArmorData> (path);
		} else {
			return null;
		}
	}
}