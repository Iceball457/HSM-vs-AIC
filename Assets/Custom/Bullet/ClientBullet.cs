using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBullet : MonoBehaviour {
	public Light light;
	public TrailRenderer tr;
	public LayerMask mask;
	Vector3 velocity;

	public void Initialize (Vector3 forward, ShotData shot) {
		velocity = forward * shot.shotSpeed;
		//Debug.Log (velocity.magnitude);
		light.color = shot.shotColor;
		Gradient gradient = new Gradient ();
		gradient.colorKeys = new GradientColorKey [] { new GradientColorKey (shot.shotColor, 0), new GradientColorKey (shot.shotColor, 1) };
		tr.colorGradient = gradient;
	}

	private void FixedUpdate () {
		RaycastHit hit;
		if (Physics.Raycast (new Ray (transform.position, velocity), out hit, velocity.magnitude * Time.fixedDeltaTime, mask)) {
			// We hit something
			transform.position = hit.point + hit.normal * 0.01f;
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