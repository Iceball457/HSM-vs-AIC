using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
	public string username;
	public Color FFAMainColor = Color.white;
	public Color FFAArmorColor = Color.white;
	public float GoldTeamMainColor = 0.75f;
	public float GoldTeamArmorColor = 0.5f;
	public float BlueTeamMainColor = 0.75f;
	public float BlueTeamArmorColor = 0.5f;
	
	public Color GoldTeamColor (float brightness) {
		return new Color (1f, 0.1f + brightness * 0.8f, 0f);
	}
	public Color BlueTeamColor (float brightness) {
		return new Color (0f, 0.1f + brightness * 0.8f, 1f);
	}
}