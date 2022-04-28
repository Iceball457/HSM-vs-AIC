using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : NetworkBehaviour {
	public ShieldData represents;
	public MeshRenderer visual;
	private void Start () {
		visual.material.SetColor ("_BaseColor", represents.shieldColor);
	}
	public ShieldData Pickup () {
		if (isServer) {
			Destroy (gameObject);
			return represents;
		} else {
			throw new System.Exception ("Cannot call Pickup() on client.");
		}
	}
}