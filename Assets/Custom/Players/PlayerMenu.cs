using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour {
	public PlayerController pc;
	GameInput input;
	private void Awake () {
		input = new GameInput ();
	}
	private void Start () {
		CloseMenu ();
	}
	public void Update () {
		if (input.Settings.Lock.triggered) {
			CloseMenu ();
		}
		if (input.Settings.Unlock.triggered) {
			transform.localScale = Vector3.one;
		}
	}
	public void CloseMenu () {
		transform.localScale = Vector3.zero;
	}
	private void OnEnable () {
		input.Enable ();
	}
	private void OnDisable () {
		input.Disable ();
	}
}