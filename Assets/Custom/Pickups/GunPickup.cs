using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : NetworkBehaviour {
	public GunData editorRepresents;
	public ActiveGunData represents;
	public bool shouldDespawn = false;
	public float lifeTime = 15f;
	float despawnTimer;
	private void Start () {
		if (editorRepresents != null && represents == null) {
			represents = new ActiveGunData (editorRepresents);
		}
	}
	private void FixedUpdate () {
		despawnTimer += Time.fixedDeltaTime;
		if (despawnTimer > lifeTime && shouldDespawn) {
			Destroy (gameObject);
		}
	}
	public ActiveGunData Pickup () {
		if (isServer) {
			Destroy (gameObject);
			return represents;
		} else {
			throw new System.Exception ("Cannot call Pickup() on client.");
		}
	}
}