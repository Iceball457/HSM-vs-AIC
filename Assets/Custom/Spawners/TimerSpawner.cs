using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSpawner : NetworkBehaviour {
	public GameObject prefab;
	GameObject activeItem;
	public float timer;
	float cTimer;

	// Start is called before the first frame update
	void Start () {
		cTimer = float.MaxValue;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isServer) {
			if (activeItem == null) {
				cTimer += Time.fixedDeltaTime;
				if (cTimer >= timer) {
					GameObject newObject = Instantiate (prefab, transform.position, transform.rotation);
					activeItem = newObject;
					NetworkServer.Spawn (newObject);
					cTimer = 0;
				}
			} else {
				cTimer = 0;
			}
		}
	}
}