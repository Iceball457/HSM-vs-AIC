using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerController : Character {

	//Management Variables
	GameInput input;
	public Camera cam;
	public Canvas UI;
	public TMP_Text scoreboard;
	public AudioListener listener;
	public RectTransform firingError;
	public GameObject crosshair;
	public float minOffset;
	public float offsetScale;

	bool inGame = true;
	bool forceScoreboard;
	bool aimingBuffer; //Prevents the need to network the aiming variable every frame;
	bool crouchingBuffer; //Prevents the need to network the crouching variable every frame;

	bool jumpFlag; //Input is taken in the update loop but jumping needs to be performed in the fixed update loop

	// Start is called before the first frame update
	protected override void Awake () {
		base.Awake ();
		input = new GameInput ();
	}

	protected override void Start () {
		base.Start ();
		if (isLocalPlayer) {
			cam.enabled = true;
			UI.enabled = true;
			listener.enabled = true;
		} else {
			cam.enabled = false;
			UI.enabled = false;
			listener.enabled = false;
			scoreboard.gameObject.SetActive (false);
		}
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	override protected void FixedUpdate () {
		base.FixedUpdate ();
		if (jumpFlag) {
			Jump ();
			jumpFlag = false;
		}
	}
	override protected void Update () {
		base.Update ();
		if (isLocalPlayer && inGame && alive) {
			Vector2 moveInput = input.Master.Movement.ReadValue<Vector2> ();
			if (moveInput != movementInputBuffer) {
				CmdMovementUpdate (moveInput);
				movementInputBuffer = moveInput;
			}
			//Debug.Log (input.Master.Look.ReadValue<Vector2> ().x.ToString () + ", " + input.Master.Look.ReadValue<Vector2> ().y.ToString ());
			Look (input.Master.Look.ReadValue<Vector2> ());
			//If you are not chambering or reloading, run this block:
			if (gunInventory.Count > 0) {
				if (!(gunInventory [currentGun].chambering > 0 || gunInventory [currentGun].reloading > 0)) {
					if (input.Master.Select.ReadValue<float> () > 0) {
						CycleGun (1);
					} else if (input.Master.Select.ReadValue<float> () < 0) {
						CycleGun (-1);
					}
					if (input.Master.Primary.ReadValue<float> () > 0) {
						Fire (input.Master.Primary.triggered);
					}
				}
				if (input.Master.Reload.triggered) {
					Reload ();
				}
			}
			if (input.Master.Jump.triggered) {
				jumpFlag = true;
			}
			if (input.Master.Flashlight.triggered) {
				ToggleFlashlight ();
			}
			if (input.Master.Secondary.ReadValue<float> () > 0 && !aimingBuffer) {
				SetAim (true);
				aimingBuffer = true;
				//crosshair.SetActive (false);
			}
			if (input.Master.Secondary.ReadValue<float> () == 0 && aimingBuffer) {
				SetAim (false);
				aimingBuffer = false;
				crosshair.SetActive (true);
			}
			if (input.Master.Crouch.ReadValue<float> () > 0 && !crouchingBuffer) {
				SetCrouch (true);
				crouchingBuffer = true;
			}
			if (input.Master.Crouch.ReadValue<float> () == 0 && crouchingBuffer) {
				SetCrouch (false);
				crouchingBuffer = false;
			}
			if (input.Master.Grenade.triggered) {
				UseFirst<Grenade> ();
			}
			if (input.Master.Interact.triggered) {
				Debug.Log ("Interact Input");
				ManualPickup ();
			}
			if (input.Master.Drop.triggered) {
				DropGun (currentGun);
			}
		}
		if (isLocalPlayer) {
			// UI updates
			
			//helmet.LoadVitals (armor, health, shield);
			firingError.sizeDelta = Vector2.one * (minOffset + (bloom * offsetScale));
			if (input.Settings.Lock.triggered) {
				CloseMenu ();
			}
			if (input.Settings.Unlock.triggered) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				inGame = false;
			}
			if (input.Settings.Scoreboard.ReadValue<float> () > 0 || forceScoreboard) {
				if (input.Settings.Scoreboard.triggered) {
					RequestScoreFromServer ();
				}
				scoreboard.gameObject.SetActive (true);
			} else {
				scoreboard.gameObject.SetActive (false);
			}
		}
	}
	[Command]
	void CmdMovementUpdate (Vector2 movementInput) {
		movementInputBuffer = movementInput;
	}
	public void CloseMenu () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		inGame = true;
	}
	[Command]
	void RequestScoreFromServer () {
		OnScoreRecieved (connectionToClient, GameMode.activeMode.ScoreboardToString ());
	}
	[TargetRpc]
	void OnScoreRecieved (NetworkConnection conn, string scoreboard) {
		this.scoreboard.text = scoreboard;
	}
	[ClientRpc]
	public void RpcCastScore (string scoreboard) {
		this.scoreboard.text = scoreboard;
	}
	[ClientRpc]
	public void RpcForceScoreboard (bool show) {
		crosshair.SetActive (!show);
		forceScoreboard = show;
	}
	protected override void OnEnable () {
		base.OnEnable ();
		input.Enable ();
	}
	protected override void OnDisable () {
		base.OnDisable ();
		input.Disable ();
	}
	public void Disconnect () {
		if (isLocalPlayer && !isServer) {
			//CmdOnDisconnect ();
			ArenaNetworkManager.master.StopClient ();
		} else if (isLocalPlayer && isServer) {
			RpcDisconnectAllPlayers ();
			ArenaNetworkManager.master.StopHost ();
		}
	}
	[ClientRpc]
	void RpcDisconnectAllPlayers () {
		if (!isServer) {
			Disconnect ();
		}
	}
}