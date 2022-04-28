using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaNetworkLink : MonoBehaviour {
	public TMP_InputField addressField;
	private void Start () {
		ClearOnlineScene ();
	}
	public void StartClient () {
		ArenaNetworkManager.master.networkAddress = addressField.text;
		ArenaNetworkManager.master.StartClient ();
	}
	public void ClearOnlineScene () {
		ArenaNetworkManager.master.onlineScene = "";
	}
	public void SetOnlineScene (string scene) {
		ArenaNetworkManager.master.onlineScene = scene;
	}
	public void SetGamemode (GameObject gameModePrefab) {
		//ArenaNetworkManager.master.gameModePrefab = gameModePrefab;
	}
	public void StartHost () {
		if (ArenaNetworkManager.master.onlineScene != "") {
			ArenaNetworkManager.master.StartHost ();
		}
	}
}