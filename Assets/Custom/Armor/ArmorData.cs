using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "New Armor")]
public class ArmorData : ScriptableObject {
	[Range(0, 100)] public int armorRating;
	public float moveSpeed;
	public float sneakSpeed;
	public bool fullUnequipOther;
	public bool fullUnequipThis;
	public GameObject helmetPrefab;
	public GameObject chestPrefab;
	public GameObject waistPrefab;
	public GameObject leftUpperArmPrefab;
	public GameObject rightUpperArmPrefab;
	public GameObject leftForearmPrefab;
	public GameObject rightForearmPrefab;
	public GameObject leftThighPrefab;
	public GameObject rightThighPrefab;
	public GameObject leftShinPrefab;
	public GameObject rightShinPrefab;
}