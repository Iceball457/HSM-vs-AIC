using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaNetworkManager : NetworkManager {
	public static ArenaNetworkManager master;
	//public GameObject gameModePrefab;
	public Dictionary<NetworkConnection, Character> connectedPlayers = new Dictionary<NetworkConnection, Character> ();
	public List<Transform> spawnPoints;

	public override void Awake () {
		base.Awake ();
		ForceSingleton ();
	}
	public override void OnStartServer () {
		base.OnStartServer ();
		//If the server has multiple GameMode prefabs, remove any that are not the current mode
		if (GameMode.activeMode != null) {
			//Destroy (GameMode.activeMode.gameObject);
		}
		//Pull the data from the GameModeSettings supplied by the NetLink

	}

	private void ForceSingleton () {
		if (master == null) {
			master = this;
		} else {
			Destroy (gameObject);
		}
	}
	
	public override void OnServerAddPlayer (NetworkConnection conn) {
		GameObject player = Instantiate (playerPrefab);
		Character pc = player.GetComponent<Character> ();
		connectedPlayers.Add (conn, pc);
		NetworkServer.AddPlayerForConnection (conn, player);
		GameMode.activeMode.RequestSpawn (pc);
	}
	public override void OnServerDisconnect (NetworkConnection conn) {
		base.OnServerDisconnect (conn);
		if (connectedPlayers.ContainsKey (conn)) {
			GameMode.activeMode.RemovePlayerFromLeaderboard (connectedPlayers [conn]);
		}
		connectedPlayers.Remove (conn);
	}
}