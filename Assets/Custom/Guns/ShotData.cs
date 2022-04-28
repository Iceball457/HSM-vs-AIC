using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shot", menuName = "New Shot")]
public class ShotData : ScriptableObject {
	public Vector3 muzzle;
	public int shots;
	public int damagePerShot;
	public int maxBonusDamagePerShot;
	public float armorPenetration;
	public float shieldPenetration;
	public AudioClip shotSound;
	public Color shotColor;
	public float shotSpeed;
	public Vector2 recoil;
	public Vector2 recoilRandomness;
	public float inaccuracy;
	public float bloom;
}