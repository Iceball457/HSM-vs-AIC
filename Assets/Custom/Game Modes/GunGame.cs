using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunGame : GameMode {
	public int pointsToWin;
	Dictionary<Character, int> scoreboard = new Dictionary<Character, int> ();
	public float respawnDelay;
	public float postGameTime;
	public float targetDistanceToSpawn;
	public List<GameObject> respawnPoints;
	public List<GunData> powerWeapons;
	public List<GunData> primaries;
	public List<GunData> sidearms;
	public List<ArmorData> lightArmors;
	public List<ArmorData> midArmors;
	public List<ArmorData> heavyArmors;
	public List<ShieldData> shields;
	public List<Equipment> grenades;
	public override void RequestSpawn (Character player) {
		Respawn (player);
	}
	public override void RequestRespawn (Character player) {
		StartCoroutine (RespawnTimer (player));
	}

	public override void OnKill (Character assailant, Character victim) {
		if (gameRunning) {
			if (assailant != victim) {
				AwardPoint (assailant);
			}
		}
	}
	public override void AwardPoint (Character player) {
		if (!scoreboard.ContainsKey (player)) {
			scoreboard [player] = 0;
		}
		scoreboard [player]++;
		foreach (Character character in ArenaNetworkManager.master.connectedPlayers.Values) {
			PlayerController playerToCastTo = character as PlayerController;
			if (playerToCastTo != null) {
				playerToCastTo.RpcCastScore (ScoreboardToString ());
			}
		}
		if (scoreboard [player] >= pointsToWin) {
			EndGame ();
		}
	}
	public override void RemovePlayerFromLeaderboard (Character player) {
		scoreboard.Remove (player);
	}
	public override string ScoreboardToString () {
		string output = "";
		List<int> scores = scoreboard.Values.ToList ();
		List<Character> players = scoreboard.Keys.ToList ();
		scores.Sort ();
		scores.Reverse ();
		foreach (int score in scores.Distinct ()) {
			foreach (Character player in players) {
				if (scoreboard [player] == score) {
					if (score == pointsToWin) {
						output += player.gameObject.name + " wins!\n";
					} else {
						output += player.gameObject.name + ": " + score.ToString () + "\n";
					}
				}
			}
		}
		return output;
	}
	public override void StartGame () {
		scoreboard = new Dictionary<Character, int> ();
		gameRunning = true;
		// Each player needs to request a spawn
		foreach (Character character in ArenaNetworkManager.master.connectedPlayers.Values) {
			PlayerController player = character as PlayerController;
			if (player != null) {
				player.RpcForceScoreboard (false);
			}
			RequestSpawn (character);
		}
	}

	public override void EndGame () {
		gameRunning = false;
		StartCoroutine (PostGameDelay ());
		foreach (Character character in ArenaNetworkManager.master.connectedPlayers.Values) {
			PlayerController player = character as PlayerController;
			if (player != null) {
				player.RpcForceScoreboard (true);
			}
		}
	}
	IEnumerator PostGameDelay () {
		yield return new WaitForSeconds (postGameTime);
		StartGame ();
	}
	public int CheckScore (Character player) {
		if (!scoreboard.ContainsKey (player)) {
			scoreboard [player] = 0;
		}
		return scoreboard [player];
	}
	IEnumerator RespawnTimer (Character player) {
		yield return new WaitForSeconds (respawnDelay);
		Respawn (player);
	}
	void Respawn (Character player) {
		player.RemoveAllGuns ();
		foreach (GunData gun in RandomLoadout (player)) {
			player.AddGun (new ActiveGunData (gun), false);
		}
		ArmorData newArmor = RandomArmor (player);
		Debug.Log (player.name + " got " + newArmor.name);
		player.RemoveArmor ();
		if (newArmor != null) {
			player.EquipArmor (newArmor);
		}
		player.EquipShield (RandomShield ());
		player.equipmentInventory.Clear ();
		for (int i = 0; i < 2; i++) {
			player.equipmentInventory.Add (RandomGrenade ());
		}
		player.Respawn (SelectBestSpawn (targetDistanceToSpawn));
	}
	IEnumerable<GunData> RandomLoadout (Character player) {
		float playerPercentToWin = CheckScore (player) / (float)pointsToWin;
		float powerWeaponChance = 0.5f - playerPercentToWin / 2f;
		float tripleSideArmChance = playerPercentToWin / 2f;
		if (Random.Range (0f, 1f) < powerWeaponChance) {
			// Player gets a power weapon
			yield return powerWeapons [Random.Range (0, powerWeapons.Count)];
		} else if (Random.Range (0f, 1f) < tripleSideArmChance) {
			// Player gets three sidearms
			yield return sidearms [Random.Range (0, sidearms.Count)];
			yield return sidearms [Random.Range (0, sidearms.Count)];
			yield return sidearms [Random.Range (0, sidearms.Count)];
		} else {
			// Player gets one primary and one sidearm
			yield return primaries [Random.Range (0, primaries.Count)];
			yield return sidearms [Random.Range (0, sidearms.Count)];
		}
	}
	ArmorData RandomArmor (Character player) {
		float playerPercentToWin = CheckScore (player) / (float)pointsToWin;
		float armorTier = Random.Range (0f, 0.5f) + (0.5f - playerPercentToWin / 2f);
		if (armorTier < 0.25f) {
			return null;
		} else if (armorTier < 0.5f) {
			return lightArmors [Random.Range (0, lightArmors.Count)];
		} else if (armorTier < 0.75f) {
			return midArmors [Random.Range (0, midArmors.Count)];
		} else {
			return heavyArmors [Random.Range (0, heavyArmors.Count)];
		}
	}
	ShieldData RandomShield () {
		return shields [Random.Range (0, shields.Count)];
	}

	Equipment RandomGrenade () {
		return grenades [Random.Range (0, grenades.Count)];
	}
	Vector3 SelectBestSpawn (float targetDistance) {
		//float bestDistance = -1;
		Vector3 bestSpawn = respawnPoints [Random.Range (0, respawnPoints.Count)].transform.position;
		/*
		foreach (GameObject spawn in respawnPoints) {
			float worstDistanceForSpawn = -1;
			foreach (Character player in Character.allCharacters) {
				float currentDistance = Vector3.Distance (player.transform.position, spawn.transform.position);
				if (worstDistanceForSpawn == -1 || Mathf.Abs (targetDistance - currentDistance) > worstDistanceForSpawn) {
					worstDistanceForSpawn = currentDistance;
				}
			}
			if (bestDistance == -1 || Mathf.Abs (targetDistance - worstDistanceForSpawn) < bestDistance) {
				bestSpawn = spawn.transform.position;
				bestDistance = worstDistanceForSpawn;
			}
		}
		*/
		return bestSpawn;
	}


}