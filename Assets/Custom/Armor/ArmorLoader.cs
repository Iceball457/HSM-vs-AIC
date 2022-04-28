using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLoader : MonoBehaviour {
	public GameObject head;
	public GameObject chest;
	public GameObject waist;
	public GameObject leftUpperArm;
	public GameObject rightUpperArm;
	public GameObject leftForearm;
	public GameObject rightForearm;
	public GameObject leftThigh;
	public GameObject rightThigh;
	public GameObject leftShin;
	public GameObject rightShin;
	GameObject headArmor;
	GameObject chestArmor;
	GameObject waistArmor;
	GameObject leftUpperArmArmor;
	GameObject rightUpperArmArmor;
	GameObject leftForearmArmor;
	GameObject rightForearmArmor;
	GameObject leftThighArmor;
	GameObject rightThighArmor;
	GameObject leftShinArmor;
	GameObject rightShinArmor;

	public ArmorData currentArmor;

	public void EquipArmorVisuals (ArmorData armorData, PlayerData data, int team, bool islocal) {
		if (currentArmor != null) {
			if (currentArmor.fullUnequipThis) {
				RemoveArmorVisuals ();
			}
		}
		if (armorData.fullUnequipOther) {
			RemoveArmorVisuals ();
		}
		headArmor = CreateArmorVisual (armorData.helmetPrefab, head, headArmor, data, team, islocal);
		chestArmor = CreateArmorVisual (armorData.chestPrefab, chest, chestArmor, data, team);
		waistArmor = CreateArmorVisual (armorData.waistPrefab, waist, waistArmor, data, team);
		leftUpperArmArmor = CreateArmorVisual (armorData.leftUpperArmPrefab, leftUpperArm, leftUpperArmArmor, data, team);
		rightUpperArmArmor = CreateArmorVisual (armorData.rightUpperArmPrefab, rightUpperArm, rightUpperArmArmor, data, team);
		leftForearmArmor = CreateArmorVisual (armorData.leftForearmPrefab, leftForearm, leftForearmArmor, data, team);
		rightForearmArmor = CreateArmorVisual (armorData.rightForearmPrefab, rightForearm, rightForearmArmor, data, team);
		leftThighArmor = CreateArmorVisual (armorData.leftThighPrefab, leftThigh, leftThighArmor, data, team);
		rightThighArmor = CreateArmorVisual (armorData.rightThighPrefab, rightThigh, rightThighArmor, data, team);
		leftShinArmor = CreateArmorVisual (armorData.leftShinPrefab, leftShin, leftShinArmor, data, team);
		rightShinArmor = CreateArmorVisual (armorData.rightShinPrefab, rightShin, rightShinArmor, data, team);

		currentArmor = armorData;
	}
	GameObject CreateArmorVisual (GameObject prefab, GameObject parent, GameObject oldArmor, PlayerData data, int team, bool islocal = false) {
		if (prefab != null) {
			if (oldArmor != null) {
				Destroy (oldArmor);
			}
			GameObject newArmor = Instantiate (prefab, parent.transform.position, parent.transform.rotation, parent.transform);
			foreach (MeshRenderer mesh in newArmor.GetComponentsInChildren<MeshRenderer> ()) {
				mesh.material.SetColor ("_BaseColor", ArmorColor (data, team));
				if (islocal) {
					mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				}
			}
			return newArmor;
		}
		return null;
	}
	public void UpdateAllArmorColors (PlayerData data, int team) {
		UpdateArmorColor (headArmor, data, team);
		UpdateArmorColor (chestArmor, data, team);
		UpdateArmorColor (waistArmor, data, team);
		UpdateArmorColor (leftUpperArmArmor, data, team);
		UpdateArmorColor (rightUpperArmArmor, data, team);
		UpdateArmorColor (leftForearmArmor, data, team);
		UpdateArmorColor (rightForearmArmor, data, team);
		UpdateArmorColor (leftThighArmor, data, team);
		UpdateArmorColor (rightThighArmor, data, team);
		UpdateArmorColor (leftShinArmor, data, team);
		UpdateArmorColor (rightShinArmor, data, team);
	}
	void UpdateArmorColor (GameObject armorPiece, PlayerData data, int team) {
		if (armorPiece == null) {
			return;
		}
		foreach (MeshRenderer mesh in armorPiece.GetComponentsInChildren<MeshRenderer> ()) {
			mesh.material.SetColor ("_BaseColor", ArmorColor (data, team));
		}
	}
	private Color ArmorColor (PlayerData data, int team) {
		if (data == null) {
			return new Color ();
		}
		if (team == 1) {
			return data.GoldTeamColor (data.GoldTeamArmorColor);
		} else if (team == 2) {
			return data.BlueTeamColor (data.BlueTeamArmorColor);
		} else {
			return data.FFAArmorColor;
		}
	}
	public void RemoveArmorVisuals () {
		Destroy (headArmor);
		Destroy (chestArmor);
		Destroy (waistArmor);
		Destroy (leftUpperArmArmor);
		Destroy (rightUpperArmArmor);
		Destroy (leftForearmArmor);
		Destroy (rightForearmArmor);
		Destroy (leftThighArmor);
		Destroy (rightThighArmor);
		Destroy (leftShinArmor);
		Destroy (rightShinArmor);
	}
}