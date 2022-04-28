using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldLoader : MonoBehaviour {
	public SkinnedMeshRenderer shieldObject;
	public ParticleSystemRenderer shieldParticles;
	public ShieldData currentShieldData;
	public void EquipShield (ShieldData data) {
		//Debug.Log ("Equipping " + data.name);
		ShowShield (data);
		currentShieldData = data;
	}
	public void ShowShield (ShieldData data) {
		if (data != null) {
			shieldObject.material.SetColor ("_BaseColor", data.shieldColor);
			shieldParticles.material.SetColor ("_BaseColor", data.shieldColor);
		}
	}
	public void SetShieldStrength (float intensity) {
		//Debug.Log (intensity);
		shieldObject.material.SetFloat ("_Transparency", intensity);
		shieldObject.material.SetFloat ("_InverseIntensity", 10f - intensity * 8f);
	}
}