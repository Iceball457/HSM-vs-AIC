using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour {
	public TMP_InputField usernameField;
	public Slider FFAMainRed;
	public Slider FFAMainGreen;
	public Slider FFAMainBlue;
	public Slider FFAArmorRed;
	public Slider FFAArmorGreen;
	public Slider FFAArmorBlue;
	public Slider GoldTeamMain;
	public Slider GoldTeamArmor;
	public Slider BlueTeamMain;
	public Slider BlueTeamArmor;
	public List<SkinnedMeshRenderer> bodyObjects;
	public List<MeshRenderer> armorObjects;
	private void Start () {
		LoadDataToSliders ();
		UpdateUsername ();
	}
	void LoadDataToSliders () {
		PlayerData data = PlayerDataSingle.localPlayerData.playerData;
		usernameField.text = data.username;
		FFAMainRed.SetValueWithoutNotify (data.FFAMainColor.r);
		FFAMainGreen.SetValueWithoutNotify (data.FFAMainColor.g);
		FFAMainBlue.SetValueWithoutNotify (data.FFAMainColor.b);

		FFAArmorRed.SetValueWithoutNotify (data.FFAArmorColor.r);
		FFAArmorGreen.SetValueWithoutNotify (data.FFAArmorColor.g);
		FFAArmorBlue.SetValueWithoutNotify (data.FFAArmorColor.b);

		GoldTeamMain.SetValueWithoutNotify (data.GoldTeamMainColor);
		GoldTeamArmor.SetValueWithoutNotify (data.GoldTeamArmorColor);

		BlueTeamMain.SetValueWithoutNotify (data.BlueTeamMainColor);
		BlueTeamArmor.SetValueWithoutNotify (data.BlueTeamArmorColor);
		UpdateDisplay (0);
	}
	public void UpdateFFAColors () {
		PlayerDataSingle.localPlayerData.playerData.FFAMainColor = new Color (FFAMainRed.value, FFAMainGreen.value, FFAMainBlue.value);
		PlayerDataSingle.localPlayerData.playerData.FFAArmorColor = new Color (FFAArmorRed.value, FFAArmorGreen.value, FFAArmorBlue.value);
		UpdateDisplay (0);
	}
	public void UpdateDisplay (int team) {
		if (team == 0) {
			foreach (SkinnedMeshRenderer current in bodyObjects) {
				current.material.SetColor ("_BaseColor", new Color (FFAMainRed.value, FFAMainGreen.value, FFAMainBlue.value));
			}
			foreach (MeshRenderer current in armorObjects) {
				current.material.SetColor ("_BaseColor", new Color (FFAArmorRed.value, FFAArmorGreen.value, FFAArmorBlue.value));
			}
		} else if (team == 1) {
			foreach (SkinnedMeshRenderer current in bodyObjects) {
				current.material.SetColor ("_BaseColor", PlayerDataSingle.localPlayerData.playerData.GoldTeamColor (PlayerDataSingle.localPlayerData.playerData.GoldTeamMainColor));
			}
			foreach (MeshRenderer current in armorObjects) {
				current.material.SetColor ("_BaseColor", PlayerDataSingle.localPlayerData.playerData.GoldTeamColor (PlayerDataSingle.localPlayerData.playerData.GoldTeamArmorColor));
			}
		} else if (team == 2) {
			foreach (SkinnedMeshRenderer current in bodyObjects) {
				current.material.SetColor ("_BaseColor", PlayerDataSingle.localPlayerData.playerData.BlueTeamColor (PlayerDataSingle.localPlayerData.playerData.BlueTeamMainColor));
			}
			foreach (MeshRenderer current in armorObjects) {
				current.material.SetColor ("_BaseColor", PlayerDataSingle.localPlayerData.playerData.BlueTeamColor (PlayerDataSingle.localPlayerData.playerData.BlueTeamArmorColor));
			}
		}
		
	}
	public void UpdateGoldTeamColors () {
		PlayerDataSingle.localPlayerData.playerData.GoldTeamMainColor = GoldTeamMain.value;
		PlayerDataSingle.localPlayerData.playerData.GoldTeamArmorColor = GoldTeamArmor.value;
		UpdateDisplay (1);
	}
	public void UpdateBlueTeamColors () {
		PlayerDataSingle.localPlayerData.playerData.BlueTeamMainColor = BlueTeamMain.value;
		PlayerDataSingle.localPlayerData.playerData.BlueTeamArmorColor = BlueTeamArmor.value;
		UpdateDisplay (2);
	}
	public void UpdateUsername () {
		if (usernameField.text.Length == 0) {
			PlayerDataSingle.localPlayerData.playerData.username = "Player Playerson";
		} else {
			PlayerDataSingle.localPlayerData.playerData.username = usernameField.text;
		}
	}
}