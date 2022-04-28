using Mirror;
using Mirror.Cloud.Examples.Pong;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : NetworkBehaviour {

	//Singletons
	public static List<Character> allCharacters = new List<Character> (); //Non networked, just need a list of all characters loaded on the current device

	//Management Variables
	Rigidbody rb;
	Animator anim;
	public GameObject visuals; // Used to hide the player's visuals and collision while they are dead.
	public SkinnedMeshRenderer skin;
	public ArmorLoader armorLoader;
	public ShieldLoader shieldLoader;
	public AnimationCurve shieldSpinUp;
	public AudioSource audio3D;
	public AudioSource audio2D;
	public Light flashlight;
	public ParticleSystem shieldParticles;
	public GameObject head;
	public GameObject hand;
	public GameObject shieldObject;
	public GameObject serverProjectile;
	public GameObject clientProjectile;
	Collider [] hitReg;

	PlayerData playerData;

	public LayerMask interactMask;

	Vector3 positionBuffer;
	Vector3 velocityBuffer;
	PredictionDelta clientSidePrediction = new PredictionDelta ();
	PredictionDelta clientSidePredictionVelocity = new PredictionDelta ();
	float velocityTolerance = 0.2f;

	//Gameplay Variables
	public bool alive = true;
	public bool aliveFlagForBullets = true;

	public int team; // 0 FFA, 1 Gold, 2 Blue

	[SyncVar] public int health;
	[SyncVar] public int armor;
	[SyncVar] public int shield;
	public HelmetUI helmet;
	public int inventorySpace = 3; // sidearms cost 1, primary weapons cost 2, and power weapons cost 3. Default is 3

	//Settings Variables
	public float lookSensitivity;
	public AudioClip footstep;
	public AudioClip hitConfirm;
	public AudioClip critConfirm;
	public AudioClip killConfirm;
	public AudioClip shieldHitConfirm;
	public AudioClip shieldPop;
	public AudioClip ouchNoise;

	#region VIRTUALS
	protected virtual void Awake () {
		allCharacters.Add (this);

		rb = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator> ();
		hitReg = GetComponentsInChildren<Collider> ();
	}

	protected virtual void Start () {
		if (isLocalPlayer) {
			SetPlayerData (PlayerDataSingle.localPlayerData.playerData);
			RequestAllInventories (this);
			ShowGun ();
			ShowOtherGuns ();
		}
		if (!isLocalPlayer && !isServer) {
			rb.useGravity = false;
		}
	}

	protected virtual void FixedUpdate () {
		FullMovement (movementInputBuffer);
		if (isLocalPlayer && !isServer) {
			clientSidePrediction.AddDelta (transform.position - positionBuffer);
			positionBuffer = transform.position;
			clientSidePredictionVelocity.AddDelta (rb.velocity - velocityBuffer);
			velocityBuffer = rb.velocity;
		}
		if (isServer) {
			ShieldChargeStep (); // This is in fixed update to prevent low framerates from charging capacitor shields faster
		}
	}

	protected virtual void Update () {
		if (helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
		//Reduce all 
		if (isLocalPlayer) {
			Look (recoil * 60 * Time.deltaTime, false);
		}
		if (recoil.x < 0.1f && recoil.x > -0.1f) {
			//Lock the recoil adjustment
			recoil.x = 0f;
		} else {
			if (recoil.x > 0.1f) {
				recoil.x -= recoilDecay * Time.deltaTime;
			} else if (recoil.x < -0.1f) {
				recoil.x += recoilDecay * Time.deltaTime;
			}
		}
		if (recoil.y < 0.1f && recoil.y > -0.1f) {
			//Lock the recoil adjustment
			recoil.y = 0f;
		} else {
			if (recoil.y > 0.1f) {
				recoil.y -= recoilDecay * Time.deltaTime;
			} else if (recoil.y < -0.1f) {
				recoil.y += recoilDecay * Time.deltaTime;
			}
		}
		if (bloom > 0.001f) {
			bloom -= bloomDecay * Time.deltaTime;
		} else {
			bloom = 0;
		}
		//Reduce all cooldowns on the active gun:
		if (gunInventory.Count > 0) {
			gunInventory [currentGun].reloading -= Time.deltaTime;
			gunInventory [currentGun].firingCooldown -= Time.deltaTime;
			gunInventory [currentGun].chambering -= Time.deltaTime;
		}//Increase time since hit
		timeSinceShieldDamage += Time.deltaTime;
		ShieldGlow ();
		if (isServer) {
			if (health <= 0 && alive) {
				alive = false;
				Death ();
			}
		}
	}

	protected virtual void OnEnable () {
		allCharacters.Add (this);
	}

	protected virtual void OnDisable () {
		allCharacters.Remove (this);
	}

	protected virtual void OnDestroy () {
		allCharacters.Remove (this);
	}
	#endregion VIRTUALS

	#region DEATH
	public void Death () {
		//Debug.Log ("Death");
		if (!isServer) {
			CmdDeath ();
		} else {
			DeathLogic ();
			RpcOnDeath ();
		}
	}
	[Command]
	void CmdDeath () {
		DeathLogic ();
		RpcOnDeath ();
	}
	[ClientRpc]
	void RpcOnDeath () {
		if (!isServer) {
			DeathLogic ();
		}
	}
	void DeathLogic () {
		alive = false;
		StopCoroutine (ReloadDelay ());
		foreach (Collider col in hitReg) {
			if (col.gameObject.layer == 11) {
				col.enabled = false;
			}
		}
		SetDeathAnimation (true);
		if (isServer) {
			//Request a respawn from the game mode manager
			Debug.Log ("Requested Respawn");
			GameMode.activeMode.RequestRespawn (this);
		}
	}
	public void Respawn (Vector3 location) {
		if (isServer) {
			RespawnBehavior (location);
			RpcOnRespawn (location);
		}
	}
	[ClientRpc]
	void RpcOnRespawn (Vector3 location) {
		if (!isServer) {
			RespawnBehavior (location);
		}
	}
	void RespawnBehavior (Vector3 location) {
		health = 100;
		if (armorLoader.currentArmor != null) {
			armor = armorLoader.currentArmor.armorRating;
			currentGroundSpeed = armorLoader.currentArmor.moveSpeed;
			currentSneakSpeed = armorLoader.currentArmor.sneakSpeed;
			//Debug.Log (armor);
		} else {
			armor = 0;
			currentGroundSpeed = groundSpeed;
			currentSneakSpeed = sneakSpeed;
		}
		if (shieldLoader.currentShieldData != null) {
			shield = shieldLoader.currentShieldData.maxCharge;
		} else {
			shield = 0;
		}
		alive = true;
		aliveFlagForBullets = true;
		currentGun = 0;
		foreach (Collider col in hitReg) {
			if (col.gameObject.layer == 11) {
				col.enabled = true;
			}
		}
		transform.position = location;
		if (isLocalPlayer && helmet != null) {
			helmet.LoadVitals (armor, health, shield);
			if (gunInventory.Count > 0) {
				helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gunInventory [currentGun].data.magSize);
			}
		}
		ShowGun ();
		SetDeathAnimation (false);
	}
	#endregion DEATH

	#region MOVEMENT
	public float groundSpeed;
	public float sneakSpeed;
	public float groundAcceleration;
	public float airSpeed;
	public float airAcceleration;
	public bool sneaking;

	public float currentGroundSpeed;
	public float currentSneakSpeed;

	protected Vector2 movementInputBuffer; // If this is the client, this is the movement input on the last frame. If this is the server, this is the last input recieved from the client.
	float cSyncTimer;

	public float footstepDistance;
	float cFootstepDistance;

	void FullMovement (Vector2 localIntendedVector) {
		if (isLocalPlayer && !isServer) {
			MovementMath (localIntendedVector);
			//clientSidePrediction.AddDelta (rb.velocity);
		}
		if (isServer) {
			MovementMath (localIntendedVector);
			if (cSyncTimer < syncInterval) {
				cSyncTimer += Time.fixedDeltaTime;
			} else {
				RpcCastMovement (transform.position, rb.velocity, transform.rotation);
				if (!isLocalPlayer) {
					RpcMovementCorrection (connectionToClient, NetworkTime.time, transform.position, rb.velocity);
				}
				cSyncTimer = 0;
			}
		}
		Footsteps ();
		MovementAnimation ();
	}
	void MovementMath (Vector2 localIntendedVector) {
		RaycastHit hit;
		if (Physics.SphereCast (new Ray (transform.position + transform.up, Vector3.down), 0.2f, out hit, 1f, interactMask)) {
			float speedMod = currentGroundSpeed;
			if (sneaking) {
				speedMod = currentSneakSpeed;
			}
			Vector3 local3DVector = Quaternion.FromToRotation (Vector3.up, hit.normal) * transform.TransformVector (new Vector3 (localIntendedVector.x * speedMod, 0f, localIntendedVector.y * speedMod));
			Vector3 requiredChange = (local3DVector - rb.velocity).normalized;
			//UnityEngine.Debug.Log ("Move Math: " + local3DVector.magnitude);
			if (onGround) {
				Vector3 intendedPosition = new Vector3 (transform.position.x, hit.point.y, transform.position.z);
				transform.position = intendedPosition;
				//clientSidePrediction.AddDelta (changeRequired);
				rb.velocity = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);
			}
			if (Vector3.Distance (rb.velocity, local3DVector) > velocityTolerance) {
				rb.AddForce (requiredChange * groundAcceleration);
			} else {
				rb.velocity = local3DVector;
			}
			if (rb.velocity.y < -velocityTolerance) {
				onGround = true;
				rb.useGravity = false;
			}
		} else {
			onGround = false;
			rb.useGravity = true;
		}
	}
	[TargetRpc]
	void RpcMovementCorrection (NetworkConnection conn, double timeStamp, Vector3 serverPosition, Vector3 serverVelocity) {
		if (isLocalPlayer && !isServer) {
			transform.position = serverPosition + clientSidePrediction.Reconciliate (timeStamp, NetworkTime.rtt);
			rb.velocity = serverVelocity + clientSidePredictionVelocity.Reconciliate (timeStamp, NetworkTime.rtt);
			//UnityEngine.Debug.DrawRay (transform.position, remainingDelta);
		}
	}
	[ClientRpc]
	void RpcCastMovement (Vector3 serverPosition, Vector3 serverVelocity, Quaternion serverRotation) {
		//Debug.Log ("Rpc Cast");
		if (!isLocalPlayer && !isServer) {
			//DateTime currentTime = DateTime.UtcNow;
			//Vector3 latencyOffset = serverVelocity * (float)(currentTime - timeStamp).TotalSeconds;
			//UnityEngine.Debug.Log (currentTime + " - " + timeStamp + " = " + (currentTime - timeStamp).TotalSeconds);
			transform.position = serverPosition; // + latencyOffset;
			rb.velocity = serverVelocity;
			transform.rotation = serverRotation;
		}
	}
	void Footsteps () {
		if (rb.velocity.magnitude > currentSneakSpeed && onGround) {
			cFootstepDistance += rb.velocity.magnitude * Time.fixedDeltaTime;
		} else {
			cFootstepDistance = 0;
		}
		if (cFootstepDistance > footstepDistance) {
			//UnityEngine.Debug.Log ("Step");
			if (isLocalPlayer && !isServer) {
				PlaySound3D (footstep);
				CmdPlayFootsteps ();
			} else if (isLocalPlayer) {
				PlaySound3D (footstep);
				RpcOnPlayFootsteps ();
			}
			cFootstepDistance = 0;
		}
	}
	[Command]
	void CmdPlayFootsteps () {
		PlaySound3D (footstep);
		RpcOnPlayFootsteps ();
	}
	[ClientRpc]
	void RpcOnPlayFootsteps () {
		if (!isLocalPlayer) {
			PlaySound3D (footstep);
		}
	}
	#endregion MOVEMENT

	#region JUMPING
	public float jumpForce;
	bool onGround = true;
	protected void Jump () {
		if (isLocalPlayer && !isServer) {
			JumpLogic ();
			CmdJump ();
		}
		if (isLocalPlayer && isServer) {
			JumpLogic ();
		}
	}
	[Command]
	void CmdJump () {
		Debug.Log ("JUMP");
		JumpLogic ();
	}
	void JumpLogic () {
		Debug.Log ("onGourn = " + onGround.ToString ());
		if (onGround) {
			//clientSidePrediction.AddDelta (Vector3.up * 0.2f);
			rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			onGround = false;
			Debug.Log ("Y veloci = " + rb.velocity.y);
		}
	}
	#endregion JUMPING

	#region AIMING
	public float lookHeight;
	protected void Look (Vector2 localLookDelta, bool useSens = true) {
		float multiplier = 1;
		if (useSens) {
			multiplier = lookSensitivity;
		}
		//Debug.Log (localLookDelta.x.ToString () + ", " + localLookDelta.y.ToString ());
		transform.Rotate (Vector3.up, localLookDelta.x * multiplier, Space.Self);
		lookHeight = Mathf.Clamp (lookHeight + localLookDelta.y * multiplier, -90, 90);
		anim.SetFloat ("Look Height", lookHeight);
		CmdLookSync (transform.rotation, lookHeight);
	}
	[Command]
	void CmdLookSync (Quaternion rotation, float lookHeight) {
		if (isServer) {
			transform.rotation = rotation;
			anim.SetFloat ("Look Height", lookHeight);
			RpcOnLookSync (rotation, lookHeight);
		}
	}
	[ClientRpc]
	void RpcOnLookSync (Quaternion rotation, float lookHeight) {
		if (!isLocalPlayer) {
			transform.rotation = rotation;
			anim.SetFloat ("Look Height", lookHeight);
		}
	}
	#endregion AIMING

	#region DAMAGE
	public bool RecieveDamage (int damage, float shieldPenetration, float armorPenetration, bool critical) { // Returns true if the player dies from the shot
		if (isServer) {
			bool shieldsActive = shield > 0;
			int shieldDamage = Mathf.FloorToInt (damage * (1 - shieldPenetration));
			shield -= shieldDamage;
			if (shieldDamage > 0) {
				timeSinceShieldDamage = 0;
			}
			int shieldOverflow = Mathf.Max (-shield, 0) + Mathf.FloorToInt (damage * shieldPenetration);
			if (shield < 0) {
				if (shieldsActive) {
					PopShield ();
				}
				shield = 0;
			}
			if (shieldOverflow > 0) {
				int armorDamage = Mathf.FloorToInt (shieldOverflow * (1 - armorPenetration));
				int healthDamage = Mathf.FloorToInt (shieldOverflow * armorPenetration);
				armor -= armorDamage;
				health -= healthDamage;
				int armorOverflow = -armor;
				if (armorOverflow > 0) {
					armor = 0;
					health -= armorOverflow;
					if (critical) {
						//DO IT AGAIN
						health -= armorOverflow;
					}
				}
			}
			RpcOnRecieveDamage (armor, health, shield, shieldDamage > 0);
		}
		return health <= 0;
	}
	[ClientRpc]
	public void RpcOnRecieveDamage (int armor, int health, int shield, bool resetShieldTimer) {
		this.armor = armor;
		this.health = health;
		this.shield = shield;
		if (shield > 0) {
			PlaySound3D (shieldHitConfirm);
		}
		if (isLocalPlayer) {
			if (helmet != null) {
				helmet.LoadVitals (armor, health, shield);
			}
			PlaySound2D (ouchNoise);
		}
		if (resetShieldTimer) {
			timeSinceShieldDamage = 0;
		}
	}
	[TargetRpc]
	public void RpcPlayHitConfirm (NetworkConnection conn) {
		PlaySound2D (hitConfirm);
	}
	[TargetRpc]
	public void RpcPlayCritConfirm (NetworkConnection conn) {
		PlaySound2D (critConfirm);
	}
	[TargetRpc]
	public void RpcPlayKillConfirm (NetworkConnection conn) {
		PlaySound2D (killConfirm);
	}
	[ClientRpc]
	void RpcOnShieldChanged (int shield) {
		this.shield = shield;
		if (isLocalPlayer && helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
	}
	#endregion DAMAGE

	#region ANIMATION
	void MovementAnimation () {
		anim.SetFloat ("Run", transform.InverseTransformVector (rb.velocity).z);
		anim.SetFloat ("Strafe", transform.InverseTransformVector (rb.velocity).x);
	}
	void ReloadAnimation () {
		anim.SetFloat ("Reload Speed", 1.2f / gunInventory [currentGun].data.reloadTime);
		anim.SetTrigger ("Reload");
	}
	void ChargeAnimation () {
		anim.SetFloat ("Reload Speed", 0.4f / gunInventory [currentGun].data.chargeTime);
		anim.SetTrigger ("Charge");
	}
	void SetAimingAnimation (bool aiming) {
		anim.SetBool ("Aim", aiming);
	}
	void SetCrouchingAnimation (bool crouching) {
		anim.SetBool ("Crouching", crouching);
	}

	void SetDeathAnimation (bool dead) {
		if (dead) {
			anim.SetTrigger ("Death");
			anim.SetLayerWeight (3, 1f);
			anim.applyRootMotion = true;
		} else {
			anim.SetLayerWeight (3, 0f);
			visuals.transform.localPosition = new Vector3 ();
			visuals.transform.localRotation = Quaternion.identity;
			anim.applyRootMotion = false;
		}
	}
	#endregion ANIMATION

	#region SOUNDS
	void PlaySound3D (AudioClip sound) {
		if (sound == null) {
			return;
		}
		audio3D.PlayOneShot (sound);
	}
	void PlaySound2D (AudioClip sound) {
		if (sound == null) {
			return;
		}
		audio2D.Stop ();
		audio2D.PlayOneShot (sound);

	}
	#endregion SOUNDS

	#region INVENTORY
	public void AddGun (ActiveGunData data, bool showNewestGun) {
		//Debug.Log ("Successfully Entered AddGun ()");
		if (isLocalPlayer && !isServer) {
			CmdAddGun (data, showNewestGun);
			//Debug.Log ("Added GUN");
		} else if (isServer) {
			AddGunBehavior (data);
			gunInventory.RemoveAll (item => item == null);
			if (showNewestGun) {
				currentGun = gunInventory.Count - 1;
			}
			RpcCastInventory (gunInventory, showNewestGun);
		}
	}
	[Command]
	void CmdAddGun (ActiveGunData data, bool showNewestGun) {
		AddGunBehavior (data);
		gunInventory.RemoveAll (item => item == null);
		if (showNewestGun) {
			currentGun = gunInventory.Count - 1;
		}
		RpcCastInventory (gunInventory, showNewestGun);
	}
	void AddGunBehavior (ActiveGunData data) {
		gunInventory.Add (data);
		currentGun = gunInventory.Count - 1;
	}
	public void RemoveAllGuns () {
		int gunsToRemove = gunInventory.Count;
		for (int i = 0; i < gunsToRemove; i++) {
			RemoveGun (0);
		}
	}
	public void RemoveGun (int index) {
		if (isLocalPlayer && !isServer) {
			CmdRemoveGun (index);
		} else if (isServer) {
			RemoveGunBehavior (index);
			RpcCastInventory (gunInventory, false);
		}
	}
	[Command]
	void CmdRemoveGun (int index) {
		RemoveGunBehavior (index);
		RpcCastInventory (gunInventory, false);
	}
	void RemoveGunBehavior (int index) {
		if (gunInventory.Count == 0)
			return;
		if (index < 0) {
			gunInventory.RemoveAt (0);
		} else if (index >= gunInventory.Count) {
			gunInventory.RemoveAt (gunInventory.Count - 1);
		} else {
			gunInventory.RemoveAt (index);
		}
	}
	[ClientRpc]
	void RpcCastInventory (List<ActiveGunData> inventory, bool showNewestGun) {
		//Debug.Log ("Inventory: " + inventory.Count + "\nActive: " + activeInventory.Count);
		gunInventory = inventory;
		if (currentGun < 0) {
			currentGun = 0;
		} else if (currentGun >= inventory.Count) {
			currentGun = inventory.Count - 1;
		}
		if (showNewestGun) {
			currentGun = inventory.Count - 1;
			ShowGun (currentGun);
		}
	}
	#endregion INVENTORY

	#region EQUIPMENT
	protected int currentGun;
	protected List<ActiveGunData> gunInventory = new List<ActiveGunData> ();
	GameObject currentGunVisual;

	public List<Equipment> equipmentInventory = new List<Equipment> ();
	public float grenadeThrowForce;

	public int CurrentLoadoutCost () {
		int output = 0;
		foreach (ActiveGunData gunData in gunInventory) {
			output += gunData.data.inventoryCost;
		}
		return output;
	}

	protected void CycleGun (int change) {
		if (gunInventory.Count == 0)
			return;
		int clampedIndex = Mathf.Clamp (currentGun + change, 0, gunInventory.Count - 1);
		currentGun = clampedIndex;
		ShowGun ();
		CmdEquipGun (clampedIndex);
	}
	[Command]
	protected void CmdEquipGun (int inventoryIndex) {
		if (isServer) {
			currentGun = inventoryIndex;
			RpcOnEquipGun (inventoryIndex);
		}
	}
	[ClientRpc]
	void RpcOnEquipGun (int inventoryIndex) {
		if (!isLocalPlayer) {
			currentGun = inventoryIndex;
			ShowGun ();
		}
	}
	[TargetRpc]
	void RpcShowGun (NetworkConnection conn, int index) {
		ShowGun (index);
	}
	void ShowGun (int index = -1) {
		if (currentGunVisual != null) {
			Destroy (currentGunVisual);
		}
		if (gunInventory.Count == 0) {
			return;
		}
		currentGun = Mathf.Clamp (currentGun, 0, gunInventory.Count - 1);
		if (isLocalPlayer && helmet != null) {
			helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gunInventory [currentGun].data.magSize);
		}
		if (index == -1) {
			currentGunVisual = Instantiate (gunInventory [currentGun].data.held, hand.transform.position, hand.transform.rotation, hand.transform);
		} else {
			currentGunVisual = Instantiate (gunInventory [index].data.held, hand.transform.position, hand.transform.rotation, hand.transform);
		}
	}
	void RequestAllInventories (Character whosAsking) {
		foreach (Character player in allCharacters) {
			if (player != whosAsking) {
				player.CmdRequestInventory ();
			}
		}
	}
	[Command(ignoreAuthority = true)]
	void CmdRequestInventory (NetworkConnectionToClient conn = null) {
		RpcCastInventory (gunInventory, false);
		RpcShowGun (conn, currentGun);
		RpcDisplayArmor (conn, armorLoader.currentArmor);
		RpcShowShield (conn, shieldLoader.currentShieldData);
		RpcShowPlayerData (conn, playerData);
	}
	void ShowOtherGuns () {
		foreach (Character character in allCharacters) {
			if (!character.isLocalPlayer) {
				character.ShowGun ();
			}
		}
	}
	protected void UseFirst<T> () where T : class {
		if (equipmentInventory.Count > 0) {
			for (int i = 0; i < equipmentInventory.Count; i++) {
				T grenade = equipmentInventory [i] as T;
				if (grenade != null) {
					UseItem (i);
					break;
				}
			}
		}
	}
	protected void UseItem (int index) {
		if (isServer) {
			UseItemBehavior (index);
		} else {
			CmdUseItem (index);
		}
	}
	[Command]
	void CmdUseItem (int index) {
		UseItemBehavior (index);
	}
	void UseItemBehavior (int index) {
		if (isServer) {
			if (equipmentInventory.Count > 0) {
				bool removeFromInventory = equipmentInventory [index].Use (this, head.transform.position, head.transform.forward, rb.velocity, grenadeThrowForce);
				if (removeFromInventory) {
					equipmentInventory.RemoveAt (index);
				}
			}
		}
	}
	#endregion EQUIPMENT

	#region ARMORSHIELDS
	public void EquipArmor (ArmorData data) {
		if (isLocalPlayer && !isServer) {
			CmdEquipArmor (data);
		} else if (isServer) {
			RpcOnEquipArmor (data);
		}
	}
	[Command]
	void CmdEquipArmor (ArmorData data) {
		EquipArmorBehavior (data);
		RpcOnEquipArmor (data);
	}
	[ClientRpc]
	void RpcOnEquipArmor (ArmorData data) {
		//Debug.Log ("Loading a player's armor");
		//if (!isServer) {
			EquipArmorBehavior (data);
		//}
	}
	[TargetRpc]
	void RpcDisplayArmor (NetworkConnection conn, ArmorData data) {
		if (data != null) {
			armorLoader.EquipArmorVisuals (data, playerData, team, isLocalPlayer);
		}
	}
	void EquipArmorBehavior (ArmorData data) {
		// Debug.Log (name + " is equipping " + data.name);
		armorLoader.EquipArmorVisuals (data, playerData, team, isLocalPlayer);
		armor = data.armorRating;
		currentGroundSpeed = data.moveSpeed;
		currentSneakSpeed = data.sneakSpeed;
		if (helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
	}
	public void EquipShield (ShieldData data) {
		if (isLocalPlayer && !isServer) {
			CmdEquipShield (data);
		} else if (isServer) {
			EquipShieldBehavior (data);
			RpcOnEquipShield (data);
		}
	}
	[Command]
	void CmdEquipShield (ShieldData data) {
		EquipShieldBehavior (data);
		RpcOnEquipShield (data);
	}
	[ClientRpc]
	void RpcOnEquipShield (ShieldData data) {
		//Debug.Log ("Loading a player's shield");
		if (!isServer) {
			EquipShieldBehavior (data);
		}
	}
	[TargetRpc]
	void RpcShowShield (NetworkConnection conn, ShieldData shield) {
		if (!isServer) {
			shieldLoader.currentShieldData = shield;
			shieldLoader.ShowShield (shield);
		}
	}
	void EquipShieldBehavior (ShieldData data) {
		timeSinceShieldDamage = 0;
		shield = 0;
		shieldLoader.EquipShield (data);
		if (helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
	}
	public void RemoveArmor () {
		if (isLocalPlayer && !isServer) {
			RemoveArmorBehavior ();
			CmdRemoveArmor ();
		} else if (isServer) {
			RemoveArmorBehavior ();
			RpcOnRemoveArmor ();
		}
	}
	[Command]
	void CmdRemoveArmor () {
		RemoveArmorBehavior ();
		RpcOnRemoveArmor ();
	}
	[ClientRpc]
	void RpcOnRemoveArmor () {
		if (!isServer && !isLocalPlayer) {
			RemoveArmorBehavior ();
		}
	}
	void RemoveArmorBehavior () {
		armorLoader.RemoveArmorVisuals ();
		armorLoader.currentArmor = null;
		armor = 0;
		if (helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
	}
	public void RemoveShield () {
		if (isLocalPlayer && !isServer) {
			RemoveShieldBehavior ();
			CmdRemoveShield ();
		} else if (isServer) {
			RemoveShieldBehavior ();
			RpcOnRemoveShield ();
		}
	}
	[Command]
	void CmdRemoveShield () {
		RemoveShieldBehavior ();
		RpcOnRemoveShield ();
	}
	[ClientRpc]
	void RpcOnRemoveShield () {
		if (!isServer && !isLocalPlayer) {
			RemoveShieldBehavior ();
		}
	}
	void RemoveShieldBehavior () {
		shieldLoader.currentShieldData = null;
		shield = 0;
		currentGroundSpeed = groundSpeed;
		currentSneakSpeed = sneakSpeed;
		if (helmet != null) {
			helmet.LoadVitals (armor, health, shield);
		}
	}
	#endregion ARMORSHIELDS

	#region PICKUPS
	public float pickupRange;
	public LayerMask pickupMask;

	protected void ManualPickup () {
		if (isLocalPlayer && !isServer) {
			CmdManualPickup ();
		} else if (isLocalPlayer) {
			ManualPickupBehavior ();
		}
	}
	[Command]
	void CmdManualPickup () {
		ManualPickupBehavior ();
	}
	void ManualPickupBehavior () {
		//Debug.Log ("Manual Pickup");
		RaycastHit hit;
		if (Physics.Raycast (head.transform.position + head.transform.up * 0.15f, head.transform.forward, out hit, pickupRange, pickupMask)) {
			//Debug.Log ("Hit " + hit.collider.gameObject.name);
			EquipmentPickup equipment = hit.collider.gameObject.GetComponentInParent<EquipmentPickup> ();
			if (equipment != null) {
				//Debug.Log ("Success");
				if (equipmentInventory.Count < 8) {
					equipmentInventory.Add (equipment.Pickup ());
				}
			}
			//Repeat the former pattern for GunPickup
			GunPickup gun = hit.collider.gameObject.GetComponentInParent<GunPickup> ();
			if (gun != null) {
				//Debug.Log ("Success");
				if (CurrentLoadoutCost () + gun.represents.data.inventoryCost <= inventorySpace) {
					AddGun (gun.Pickup (), true);
				} else if (CurrentLoadoutCost () - gunInventory [currentGun].data.inventoryCost + gun.represents.data.inventoryCost <= inventorySpace) {
					// Replace the gun you are holding
					DropGun (currentGun);
					AddGun (gun.Pickup (), true);
				} else {
					// Display a message to the player that they cannot pickup the gun
					
				}
			}
			// And for shield pickups
			ShieldPickup shieldPickup = hit.collider.gameObject.GetComponentInParent<ShieldPickup> ();
			if (shieldPickup != null) {
				EquipShield (shieldPickup.Pickup ());
			}
			// Finally for armor pickups
			ArmorPickup armorPickup = hit.collider.gameObject.GetComponentInParent<ArmorPickup> ();
			if (armorPickup != null) {
				EquipArmor (armorPickup.Pickup ());
			}
		}
	}
	public void ArbitraryPickup (EquipmentPickup pickup) {
		if (isServer) {
			if (equipmentInventory.Count < 8) {
				equipmentInventory.Add (pickup.Pickup ());
			}
		}
	}
	public void DropGun (int index) {
		if (isLocalPlayer && !isServer) {
			CmdDropGun (index);
		} else if (isLocalPlayer && isServer) {
			DropGunBehavior (index);
		}
	}
	[Command]
	void CmdDropGun (int index) {
		DropGunBehavior (index);
	}
	void DropGunBehavior (int index) {
		if (isServer) {
			ActiveGunData droppedGun = gunInventory [index];
			GameObject newPickup = Instantiate (droppedGun.data.pickup, head.transform.position + head.transform.forward, Quaternion.Euler (Random.Range (-180f, 180f), Random.Range (-180f, 180f), Random.Range (-180f, 180f)));
			newPickup.GetComponent<Rigidbody> ().velocity = head.transform.forward;
			newPickup.GetComponent<GunPickup> ().represents = droppedGun;
			newPickup.GetComponent<GunPickup> ().shouldDespawn = true;
			NetworkServer.Spawn (newPickup);
			RemoveGun (index);
			ShowGun ();
		}
	}
	#endregion PICKUPS

	#region SHOOTING
	protected float bloom;
	public float bloomDecay;
	public float bloomCap;
	protected Vector2 recoil;
	public float recoilDecay;
	public float recoilCap;
	protected void Fire (bool justPressed) {
		//UnityEngine.Debug.Log (activeInventory [currentGun].chambered);
		if (gunInventory [currentGun].firingCooldown > 0 || gunInventory [currentGun].reloading > 0 || gunInventory [currentGun].chambering > 0)
			return;
		//If you have a non auto weapon and just pulled the trigger, or if you have an auto gun, don't return
		if (!((gunInventory [currentGun].data.firetype != GunData.FireType.FullAuto && justPressed) || (gunInventory [currentGun].data.firetype == GunData.FireType.FullAuto)))
			return;
		//UnityEngine.Debug.Log ("Ready to fire");
		// Display visual bullet(s) and play audio
		StartCoroutine (BurstLoop (hand.transform.position));
		// Send shot data to server by using the burst loop Enumerator
		if (!isServer) {
			CmdFire (hand.transform.position);
		}
	}
	[Command]
	void CmdFire (Vector3 handPosition) {
		//UnityEngine.Debug.Log ("Firing on server");
		StartCoroutine (BurstLoop (handPosition));
	}
	IEnumerator BurstLoop (Vector3 clientHandPosition) {
		if (alive == false) {
			//throw new System.Exception ("You are already dead.");
			yield break;
		}
		if (gunInventory.Count == 0) {
			//throw new System.Exception ("Cannot fire while not holding a gun.");
			yield break;
		}
		GunData gun = gunInventory [currentGun].data;
		for (int i = 0; i < gun.burst.Count; i++) {
			ShotData shot = gun.burst [i];
			if (!gunInventory [currentGun].chambered || gunInventory [currentGun].mag == 0) {
				//Dry fire
				PlaySound3D (gun.dryFireSound);
				if (isServer) {
					RpcOnDryFire ();
				}
			} else {
				Quaternion bloomMod = Quaternion.Euler (new Vector3 (Random.Range (-bloom, bloom), Random.Range (-bloom, bloom)));
				if (aiming) {
					for (int j = 0; j < shot.shots; j++) {
						ShotMath (i, clientHandPosition, Quaternion.identity);
					}
				} else {
					for (int j = 0; j < shot.shots; j++) {
						ShotMath (i, clientHandPosition, bloomMod);
					}
				}
				gunInventory [currentGun].mag--;
				if (isLocalPlayer) {
					//UI Update
					if (helmet != null) {
						helmet.LoadDelayInfo (gun.burstTime, -1f);
					}
					//Set the bloom client side so prediction tracers better reflect the inaccuracy of the weapons
					bloom += shot.bloom * ((bloomCap - bloom) / bloomCap);
					if (!aiming) {
						//.Debug.Log ("Bloom: " + bloom);
						//Set the recoil on the client, since the Look() function only works client side
						recoil += (shot.recoil + new Vector2 (Random.Range (-shot.recoilRandomness.x, shot.recoilRandomness.x), Random.Range (-shot.recoilRandomness.y, shot.recoilRandomness.y))) * ((recoilCap - recoil.magnitude) / recoilCap);
					} else {
						float bloomNoiseScale = 1f;
						float bloomNoiseIntensity = 0.25f;
						Vector2 scopedBloom = new Vector2 (RandomPerlin (0.5f) * bloomNoiseIntensity * bloom, 0f);
						recoil += (shot.recoil + new Vector2 (Random.Range (-shot.recoilRandomness.x, shot.recoilRandomness.x), Random.Range (-shot.recoilRandomness.y, shot.recoilRandomness.y)) + scopedBloom) * ((recoilCap - recoil.magnitude) / recoilCap);
						float RandomPerlin (float seed) {
							return (-0.5f + Mathf.PerlinNoise ((float)NetworkTime.time * bloomNoiseScale + seed, (float)NetworkTime.time * bloomNoiseScale + seed)) * 2f;
						}
					}
				} else if (isServer && !isLocalPlayer) {
					//Set the bloom serverside as well
					bloom += shot.bloom * ((bloomCap - bloom) / bloomCap);
				}
				//Play the audio, too
				if (isLocalPlayer) {
					PlaySound3D (shot.shotSound);
				}
			}
			if (gun.firetype != GunData.FireType.BoltAction) {
				gunInventory [currentGun].firingCooldown = gun.burstTime;
				if (gunInventory [currentGun].mag == 0) {
					gunInventory [currentGun].chambered = false;
				}
				if (helmet != null) {
					helmet.LoadDelayInfo (gun.burstTime, -1f);
				}
			}
			if (helmet != null) {
				helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gun.magSize);
			}
			yield return new WaitForSeconds (gun.burstTime);
		}
		if (gun.firetype == GunData.FireType.BoltAction) {
			gunInventory [currentGun].chambered = false;
			if (helmet != null) {
				helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gun.magSize);
			}
		}

		yield return null;
	}
	[ClientRpc]
	void RpcOnFire (Vector3 shotOrigin, Vector3 shotDirection, int shotIndex) {
		if (gunInventory.Count == 0) {
			return;
		}
		if (!isLocalPlayer) { //This will show the local player both the local and server side versions of their bullet. Remove isLocalPlayer after testing.
			ClientBullet newBullet = Instantiate (clientProjectile, shotOrigin, Quaternion.identity).GetComponent<ClientBullet> ();
			newBullet.Initialize (shotDirection, gunInventory [currentGun].data.burst [shotIndex]);
		}
		if (!isLocalPlayer) {
			PlaySound3D (gunInventory [currentGun].data.burst [shotIndex].shotSound);
		}
	}
	[ClientRpc]
	void RpcOnDryFire () {
		if (!isServer && !isLocalPlayer) {
			PlaySound3D (gunInventory [currentGun].data.dryFireSound);
		}
	}
	void ShotMath (int shotIndex, Vector3 clientHandPosition, Quaternion bloomMod) {
		ShotData shot = gunInventory [currentGun].data.burst [shotIndex];
		Vector3 activeHand = hand.transform.position;
		if (clientHandPosition != null) {
			activeHand = clientHandPosition;
		}
		//Generate inaccuracies
		Quaternion inaccuracyMod = Quaternion.Euler (new Vector3 (Random.Range (-shot.inaccuracy, shot.inaccuracy), Random.Range (-shot.inaccuracy, shot.inaccuracy)));
		//Quaternion bloomMod = Quaternion.Euler (new Vector3 (Random.Range (-bloom, bloom), Random.Range (-bloom, bloom)));
		Quaternion inaccuracy = inaccuracyMod * bloomMod;
		Vector3 shotOrigin;
		Vector3 shotDirection = head.transform.TransformDirection (inaccuracy * Vector3.forward);
		if (isServer) {
			// Server bullets should come from the face
			shotOrigin = head.transform.position + (head.transform.up * 0.15f) + (head.transform.forward * 0.75f);
			//Use the math to create an actual projectile (no visuals)
			ServerBullet newBullet = Instantiate (serverProjectile, shotOrigin, Quaternion.identity).GetComponent<ServerBullet> ();
			newBullet.Initialize (shotDirection, shot, this, connectionToClient);
			//Create a similar visual object on other clients
			RpcOnFire (shotOrigin, shotDirection, shotIndex);
		}
		if (isLocalPlayer) {
			// Visual bullets should come from the gun 
			shotOrigin = activeHand + hand.transform.TransformVector (shot.muzzle);
			//Use the math to create a visual bullet (same as server side projectiles without the player collision
			ClientBullet newBullet = Instantiate (clientProjectile, shotOrigin, Quaternion.identity).GetComponent<ClientBullet> ();
			newBullet.Initialize (shotDirection, shot);
		}
	}
	#endregion SHOOTING

	#region RELOAD
	protected void Reload () {
		if (gunInventory [currentGun].mag == 0) {
			ReloadBehavior ();
		} else if (!gunInventory [currentGun].chambered) {
			ChamberBehavior ();
		} else if (gunInventory [currentGun].mag != gunInventory [currentGun].data.magSize) {
			ReloadBehavior ();
		}
	}

	private void ReloadBehavior () {
		//Reload the gun
		gunInventory [currentGun].reloading = gunInventory [currentGun].data.reloadTime;
		if (helmet != null) {
			helmet.LoadDelayInfo (-1f, gunInventory [currentGun].data.reloadTime);
		}
		//Set animations
		if (isLocalPlayer) {
			StartCoroutine (ReloadDelay ());
			ReloadAnimation ();
		}
		if (isServer) {
			RpcOnReload ();
		} else {
			CmdReload ();
		}
		PlaySound3D (gunInventory [currentGun].data.reloadSound);
	}

	private void ChamberBehavior () {
		//Chamber the gun
		gunInventory [currentGun].reloading = gunInventory [currentGun].data.chargeTime;
		if (helmet != null) {
			helmet.LoadDelayInfo (-1f, gunInventory [currentGun].data.chargeTime);
		}
		//Set animations
		if (isLocalPlayer) {
			StartCoroutine (ChamberDelay ());
			ChargeAnimation ();
		}
		if (isServer) {
			RpcOnChamber ();
		} else {
			CmdChamber ();
		}
		PlaySound3D (gunInventory [currentGun].data.chamberSound);
	}

	[Command]
	void CmdReload () {
		PlaySound3D (gunInventory [currentGun].data.reloadSound);
		RpcOnReload ();
	}
	[ClientRpc]
	void RpcOnReload () {
		if (!isLocalPlayer) {
			PlaySound3D (gunInventory [currentGun].data.reloadSound);
			ReloadAnimation ();
		}
	}
	IEnumerator ReloadDelay () {
		yield return new WaitForSeconds (gunInventory [currentGun].data.reloadTime);
		if (alive == false) {
			//throw new System.Exception ("You cannot reload while dead.");
			yield break;
		}
		if (gunInventory.Count == 0) {
			//throw new System.Exception ("You cannot reload if you do not have a gun.");
			yield break;
		}
		gunInventory [currentGun].mag = gunInventory [currentGun].data.magSize;
		if (!gunInventory [currentGun].chambered) {
			// Auto chamber the gun if it isn't already chambered
			ChamberBehavior ();
		}
		if (helmet != null) {
			helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gunInventory [currentGun].data.magSize);
		}
		CmdReloadDelay ();
	}
	[Command]
	void CmdReloadDelay () {
		gunInventory [currentGun].mag = gunInventory [currentGun].data.magSize;
	}
	[Command]
	void CmdChamber () {
		PlaySound3D (gunInventory [currentGun].data.reloadSound);
		RpcOnChamber ();
	}
	[ClientRpc]
	void RpcOnChamber () {
		if (!isServer && !isLocalPlayer) {
			PlaySound3D (gunInventory [currentGun].data.reloadSound);
			ChargeAnimation ();
		}
	}
	IEnumerator ChamberDelay () {
		yield return new WaitForSeconds (gunInventory [currentGun].data.chargeTime);
		gunInventory [currentGun].chambered = true;
		if (helmet != null) {
			helmet.LoadGunInfo (gunInventory [currentGun].chambered, gunInventory [currentGun].mag, gunInventory [currentGun].data.magSize);
		}
		CmdChamberDelay ();
	}
	[Command]
	void CmdChamberDelay () {
		gunInventory [currentGun].chambered = true;
	}
	#endregion RELOAD

	#region SHIELDS
	float timeSinceShieldDamage;
	float shieldChargeBuffer;
	void ShieldGlow () {
		if (shieldLoader.currentShieldData != null) {
			float intensity = Mathf.Clamp01 (1f - timeSinceShieldDamage / 3f);
			intensity += shieldSpinUp.Evaluate (Mathf.Clamp01 (timeSinceShieldDamage - shieldLoader.currentShieldData.chargeDelay));
			intensity *= 1f - ((float)shield / shieldLoader.currentShieldData.maxCharge);
			if (shield == 0 || intensity < 0.02f) {
				intensity = 0f;
			}
			shieldLoader.SetShieldStrength (intensity);
		} else {
			shieldLoader.SetShieldStrength (0f);
		}
	}
	void ShieldChargeStep () {
		ShieldData data = shieldLoader.currentShieldData;
		if (data != null) {
			if (timeSinceShieldDamage > data.chargeDelay) {
				//In the future this is where the shield charge types will be calculated as well
				shieldChargeBuffer += data.maxCharge * (1 / data.fullLinearChargeTime * Time.fixedDeltaTime);
				while (shieldChargeBuffer > 1f && shield < data.maxCharge) {
					shieldChargeBuffer--;
					shield++;
					RpcOnShieldChanged (shield);
					if (helmet != null) {
						helmet.LoadVitals (armor, health, shield);
					}
				}
			} else {
				shieldChargeBuffer = 0f;
			}
		}
	}
	void PopShield () {
		if (isServer) {
			ShieldPopBehavior ();
			RpcOnPopShield ();
		}
	}
	[ClientRpc]
	void RpcOnPopShield () {
		ShieldPopBehavior ();
	}
	void ShieldPopBehavior () {
		//Just play the sound and run the particles
		PlaySound3D (shieldPop);
		shieldParticles.Play ();
	}
	#endregion SHIELDS

	#region FLASHLIGHT
	protected void ToggleFlashlight () {
		bool enable = !flashlight.enabled;
		if (isLocalPlayer && !isServer) {
			ToggleBehavior (enable);
			CmdToggleFlashLight (enable);
		} else if (isLocalPlayer) {
			ToggleBehavior (enable);
			RpcOnToggleFlashLight (enable);
		}
	}
	[Command]
	void CmdToggleFlashLight (bool enable) {
		ToggleBehavior (enable);
		RpcOnToggleFlashLight (enable);
	}
	[ClientRpc]
	void RpcOnToggleFlashLight (bool enable) {
		if (!isServer && !isLocalPlayer) {
			ToggleBehavior (enable);
		}
	}
	void ToggleBehavior (bool enable) {
		//UnityEngine.Debug.Log ("FlashlightToggle");
		flashlight.enabled = enable;
	}
	#endregion FLASHLIGHT

	#region SETSTATE
	bool aiming = false;
	protected void SetAim (bool aiming) {
		if (isLocalPlayer && !isServer) {
			this.aiming = aiming;
			SetAimingAnimation (aiming);
			CmdSetAim (aiming);
		} else if (isLocalPlayer) {
			this.aiming = aiming;
			SetAimingAnimation (aiming);
			RpcOnSetAim (aiming);
		}
	}
	[Command]
	void CmdSetAim (bool aiming) {
		this.aiming = aiming;
		SetAimingAnimation (aiming);
		RpcOnSetAim (aiming);
	}
	[ClientRpc]
	void RpcOnSetAim (bool aiming) {
		if (!isServer && !isLocalPlayer) {
			this.aiming = aiming;
			SetAimingAnimation (aiming);
		}
	}
	protected void SetCrouch (bool crouching) {
		if (isLocalPlayer && !isServer) {
			SetCrouchingAnimation (crouching);
			sneaking = crouching;
			CmdSetCrouch (crouching);
		} else if (isLocalPlayer) {
			SetCrouchingAnimation (crouching);
			sneaking = crouching;
			RpcOnSetCrouch (crouching);
		}
	}
	[Command]
	void CmdSetCrouch (bool crouching) {
		SetCrouchingAnimation (crouching);
		sneaking = crouching;
		RpcOnSetCrouch (crouching);
	}
	[ClientRpc]
	void RpcOnSetCrouch (bool crouching) {
		if (!isServer && !isLocalPlayer) {
			SetCrouchingAnimation (crouching);
			sneaking = crouching;
		}
	}
	#endregion SETSTATE

	#region CUSTOMIZATION
	void SetPlayerData (PlayerData data) {
		if (isLocalPlayer && !isServer) {
			SetPlayerDataBehavior (data);
			CmdSetPlayerData (data);
		} else if  (isServer) {
			SetPlayerDataBehavior (data);
			RpcOnSetPlayerData (data);
		}
	}
	[Command]
	void CmdSetPlayerData (PlayerData data) {
		SetPlayerDataBehavior (data);
		RpcOnSetPlayerData (data);
	}
	[ClientRpc]
	void RpcOnSetPlayerData (PlayerData data) {
		if (!isLocalPlayer) {
			Debug.Log ("Recieved " + data.username + "'s Palette");
			SetPlayerDataBehavior (data);
		}
	}
	[TargetRpc]
	void RpcShowPlayerData (NetworkConnection conn, PlayerData data) {
		SetPlayerDataBehavior (data);
	}
	void SetPlayerDataBehavior (PlayerData data) {
		playerData = data;
		gameObject.name = data.username;
		SetMainColorBehavior (data);
		armorLoader.UpdateAllArmorColors (data, team);
	}
	private void SetMainColorBehavior (PlayerData data) {
		if (team == 0) {
			skin.material.SetColor ("_BaseColor", data.FFAMainColor);
		} else if (team == 1) {
			skin.material.SetColor ("_BaseColor", data.GoldTeamColor (playerData.GoldTeamMainColor));
		} else if (team == 2) {
			skin.material.SetColor ("_BaseColor", data.BlueTeamColor (playerData.BlueTeamMainColor));
		}
	}
	#endregion CUSTOMIZATION
}