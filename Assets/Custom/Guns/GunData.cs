using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "New Gun")]
public class GunData : ScriptableObject {
	public string nameID;
	public GameObject pickup;
	public GameObject held;
	public int inventoryCost;
	public AudioClip equipSound;
	public AudioClip reloadSound;
	public AudioClip chamberSound;
	public AudioClip dryFireSound;
	public enum FireType {
		BoltAction,
		SemiAuto,
		FullAuto
	}
	public FireType firetype;
	public int magSize;
	public float chargeTime;
	public float reloadTime;
	public float burstTime;
	public List<ShotData> burst;
}