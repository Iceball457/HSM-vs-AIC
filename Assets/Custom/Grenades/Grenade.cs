using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Grenade", menuName = "New Grenade")]
public class Grenade : Equipment {
	public GameObject activePrefab;
	public GameObject pickupPrefab;

	public override void Drop (Vector3 position, Vector3 forward) {
		throw new System.NotImplementedException ();
	}

	public override bool Use (Character owner, Vector3 position, Vector3 forward, Vector3 velocity, float force) {
		// Instantiate the active prefab
		GameObject newGrenade = GameObject.Instantiate (activePrefab, position + forward, Quaternion.identity);
		newGrenade.GetComponent<Rigidbody> ().velocity = velocity + forward * force;
		newGrenade.GetComponent<ActiveGrenade> ().SetOwner (owner);
		NetworkServer.Spawn (newGrenade);
		// Return true since grenades are always consumed on use
		return true;
	}
}