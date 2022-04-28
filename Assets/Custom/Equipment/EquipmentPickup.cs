using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickup : NetworkBehaviour {
	public Equipment represents;

	public Equipment Pickup () {
		if (isServer) {
			Destroy (gameObject);
			return represents;
		} else {
			throw new System.Exception ("Cannot call Pickup() on client.");
		}
	}
	private void OnTriggerEnter (Collider other) {
		AutoPickup ap = other.GetComponentInParent<AutoPickup> ();
		if (ap != null) {
			ap.owner.ArbitraryPickup (this);
		}
	}
}