using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deathmatch : GameMode {
	public int pointsToWin;
	Dictionary<Character, int> scoreboard = new Dictionary<Character, int> ();
	public float respawnDelay;
	public float postGameTime;
	public float targetDistanceToSpawn;
	public List<GameObject> respawnPoints;

	public List<GunData> startingLoadout;
	public ArmorData startingArmor;
	public ShieldData startingShield;
	public Grenade startingGrenade;

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

	public override void OnKill (Character assailant, Character victim) {
		if (gameRunning) {
			if (assailant != victim) {
				AwardPoint (assailant);
			}
		}
	}

	public override void RemovePlayerFromLeaderboard (Character player) {
		scoreboard.Remove (player);
	}

	public override void RequestRespawn (Character player) {
		StartCoroutine (RespawnTimer (player));
	}

	IEnumerator RespawnTimer (Character player) {
		yield return new WaitForSeconds (respawnDelay);
		Respawn (player);
	}

	void Respawn (Character player) {
		player.RemoveAllGuns ();
		foreach (GunData gun in startingLoadout) {
			player.AddGun (new ActiveGunData (gun), false);
		}
		player.RemoveArmor ();
		if (startingArmor != null) {
			player.EquipArmor (startingArmor);
		}
		player.RemoveShield ();
		if (startingShield != null) {
			player.EquipShield (startingShield);
		}
		player.equipmentInventory.Clear ();
		for (int i = 0; i < 2; i++) {
			player.equipmentInventory.Add (startingGrenade);
		}
		player.Respawn (SelectBestSpawn (targetDistanceToSpawn));
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

	public override void RequestSpawn (Character player) {
		Respawn (player);
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
}