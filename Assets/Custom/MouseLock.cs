using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour {
	GameInput input;
	private void Start () {
		input = new GameInput ();
		input.Enable ();
	}
	private void Update () {
		if (input.Settings.Unlock.triggered) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (input.Settings.Lock.triggered) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	private void OnDisable () {
		input.Disable ();
	}
}