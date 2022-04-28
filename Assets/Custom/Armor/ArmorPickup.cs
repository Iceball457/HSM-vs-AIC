using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : NetworkBehaviour {
	public ArmorData represents;

	public ArmorData Pickup () {
		if (isServer) {
			Destroy (gameObject);
			return represents;
		} else {
			throw new System.Exception ("Cannot call Pickup() on client.");
		}
	}
}