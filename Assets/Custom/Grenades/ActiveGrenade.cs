using Mirror;
using Mirror.Experimental;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (Rigidbody), typeof (NetworkLerpRigidbody))]
public class ActiveGrenade : NetworkBehaviour {
	public float strongRange;
	public int strongDamage;
	public float weakRange;
	public int weakDamage;
	public float armorPenetration;
	public float shieldPenetration;
	public LayerMask mask;
	public float timer;
	bool pinOut;
	bool exploded;
	float cTimer;
	Rigidbody rb;
	ParticleSystem particles;
	AudioSource audio;
	Character ownerObject;
	// Start is called before the first frame update
	void Start () {
		rb = GetComponent<Rigidbody> ();
		particles = GetComponent<ParticleSystem> ();
		audio = GetComponent<AudioSource> ();
		rb.angularVelocity = new Vector3 (Random.Range (-6f, 6f), Random.Range (-6f, 6f), Random.Range (-6f, 6f));
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isServer) {
			if (pinOut) {
				cTimer += Time.fixedDeltaTime;
			}
			if (cTimer >= timer && !exploded) {
				//Explode
				Debug.Log ("Before ");
				Explode ();
				exploded = true;
			}
		}
	}

	private void Explode () {
		foreach (Character character in Character.allCharacters.Distinct ()) {
			//Raycast to each player
			//If the raycast hits, deal damage based on distance
			Vector3 characterPosition = character.transform.position + Vector3.up;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, characterPosition - transform.position, out hit, weakRange, mask)) {
				Character hitCharacter = hit.collider.gameObject.GetComponentInParent<Character> ();
				if (hitCharacter == character) {
					float distanceToCharacter = Vector3.Distance (transform.position, characterPosition);
					int damageToDeal = (int)Mathf.Lerp (strongDamage, weakDamage, Mathf.Clamp01 ((distanceToCharacter - strongRange) / (weakRange - strongRange)));
					Debug.Log ("Dealt " + damageToDeal + " damage to " + character.name + " at " + distanceToCharacter + " meters.");
					bool kill = hitCharacter.RecieveDamage (damageToDeal, shieldPenetration, armorPenetration, false);
					if (kill) {
						GameMode.activeMode.OnKill (ownerObject, hitCharacter);
						hitCharacter.aliveFlagForBullets = false;
					}
				}
			}
		}
		//Freeze position and wait for animations to finish
		StartCoroutine (WaitForVisualsThenDestroy ());
		RpcOnExplode ();
	}

	public void SetOwner (Character owner) {
		ownerObject = owner;
	}

	private void OnCollisionEnter (Collision collision) {
		Debug.Log ("Collided with something.");
		if (isServer) {
			Debug.Log ("Collided with something on an active server.");
			pinOut = true;
		}
	}

	IEnumerator WaitForVisualsThenDestroy () {
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}
	[ClientRpc]
	void RpcOnExplode () {
		//Play animation
		rb.constraints = RigidbodyConstraints.FreezeAll;
		particles.Play ();
		audio.Play ();
	}
}