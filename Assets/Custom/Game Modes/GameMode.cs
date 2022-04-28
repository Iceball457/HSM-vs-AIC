using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour {
	public static GameMode activeMode;
	public bool gameRunning;
	private void Awake () {
		SetSelfAsActive ();
	}
	private void Start () {
		StartGame ();
	}
	public void SetSelfAsActive () {
		activeMode = this;
	}
	public abstract void RequestSpawn (Character player);
	public abstract void RequestRespawn (Character player);
	public abstract void AwardPoint (Character player);
	public abstract void RemovePlayerFromLeaderboard (Character player);
	public abstract void OnKill (Character assailant, Character victim);
	public abstract string ScoreboardToString ();
	public abstract void StartGame ();
	public abstract void EndGame ();
}