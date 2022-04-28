using Mirror;
using Mirror.Cloud.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataSerializer {
	public static void WriteArmor (this NetworkWriter writer, PlayerData data) {
		writer.WriteString (data.username);
		writer.WriteVector4 (data.FFAMainColor);
		writer.WriteVector4 (data.FFAArmorColor);
		writer.WriteDouble (data.GoldTeamMainColor);
		writer.WriteDouble (data.GoldTeamArmorColor);
		writer.WriteDouble (data.BlueTeamMainColor);
		writer.WriteDouble (data.BlueTeamArmorColor);
	}
	public static PlayerData ReadArmor (this NetworkReader reader) {
		PlayerData output = new PlayerData ();
		output.username = reader.ReadString ();
		output.FFAMainColor = reader.ReadVector4 ();
		output.FFAArmorColor = reader.ReadVector4 ();
		output.GoldTeamMainColor = (float)reader.ReadDouble ();
		output.GoldTeamArmorColor = (float)reader.ReadDouble ();
		output.BlueTeamMainColor = (float)reader.ReadDouble ();
		output.BlueTeamArmorColor = (float)reader.ReadDouble ();
		return output;
	}
}