using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : NetworkBehaviour {
	public List<GameObject> gunsSelection;
	public Vector3 RandomPosition (Vector3 center, Vector3 size) {
		return new Vector3 (Random.Range (center.x - size.x, center.x + size.x), Random.Range (center.y - size.y, center.y + size.y), Random.Range (center.z - size.z, center.z + size.z));
	}
	public List<Vector3> spawnAreas;
	public List<Vector3> spawnSizes;
	public float spawnFrequency;
	float timer;
	public float totalGunsToSpawn;
	float gunsSpawned;

	private void Update () {
		if (isServer) {
			timer += Time.deltaTime;
			if (timer >= spawnFrequency && gunsSpawned < totalGunsToSpawn) {
				int spawnArea = Random.Range (0, spawnAreas.Count - 1);
				GameObject newGun = Instantiate (gunsSelection [Random.Range (0, gunsSelection.Count)], RandomPosition (spawnAreas [spawnArea], spawnSizes [spawnArea]), Quaternion.Euler (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360)));
				NetworkServer.Spawn (newGun);
				//RpcOnSpawnGun (gunsSelection [Random.Range (0, gunsSelection.Count -1)], spawnAreas [Random.Range (0, spawnAreas.Count - 1)].RandomPosition ());
				timer = 0;
				gunsSpawned++;
			}
		}
	}
}