using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSingle : MonoBehaviour {
	public static PlayerDataSingle localPlayerData;
	public PlayerData playerData = new PlayerData ();
	private void Awake () {
		DontDestroyOnLoad (gameObject);
		if (localPlayerData == null) {
			localPlayerData = this;
			Debug.Log ("LocalData Set");
		} else {
			Destroy (gameObject);
		}
	}
}
