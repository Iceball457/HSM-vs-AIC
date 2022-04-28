using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBullet : MonoBehaviour {
	public NetworkConnection owner;
	public Character ownerObject;
	public LayerMask mask;
	Vector3 velocity;

	int damage;
	float armorPenetration;
	float shieldPenetration;

	public void Initialize (Vector3 forward, ShotData shot, Character ownerObject = null, NetworkConnection conn = null) {
		this.ownerObject = ownerObject;
		owner = conn;
		velocity = forward * shot.shotSpeed;
		damage = shot.damagePerShot + Random.Range (0, shot.maxBonusDamagePerShot + 1);
		armorPenetration = shot.armorPenetration;
		shieldPenetration = shot.shieldPenetration;
		//Debug.Log ("Created Bullet with " + damage + " damage");
	}

	private void FixedUpdate () {
		RaycastHit hit;
		if (Physics.Raycast (new Ray (transform.position, velocity), out hit, velocity.magnitude * Time.fixedDeltaTime, mask)) {
			// We hit something
			transform.position = hit.point + hit.normal * 0.01f;
			Character hitCharacter = hit.collider.GetComponentInParent<Character> ();
			if (hitCharacter != ownerObject) {
				if (hitCharacter != null) {
					if (hitCharacter.aliveFlagForBullets) {
						bool kill = hitCharacter.RecieveDamage (damage, shieldPenetration, armorPenetration, hit.collider.tag == "Critical Hurtbox");
						if (kill) {
							ownerObject.RpcPlayKillConfirm (owner);
							GameMode.activeMode.OnKill (ownerObject, hitCharacter);
							hitCharacter.aliveFlagForBullets = false;
						} else {
							if (hit.collider.tag == "Critical Hurtbox") {
								ownerObject.RpcPlayCritConfirm (owner);
							} else {
								ownerObject.RpcPlayHitConfirm (owner);
							}
						}
					}
				}
			}
			StartCoroutine (WaitForHitSoundThenDestroy ());
		}
		transform.position += velocity * Time.fixedDeltaTime;
	}
	IEnumerator WaitForHitSoundThenDestroy () {
		velocity = new Vector3 ();
		yield return new WaitForSeconds (0.1f);
		Destroy (gameObject);
	}
}