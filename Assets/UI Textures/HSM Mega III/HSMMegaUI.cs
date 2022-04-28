using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSMMegaUI : HelmetUI {
	public BarMask armor;
	public BarMask health;
	public BarMask shield;
	public BarMask charged;
	public BarMask ammo;
	public BarMask firingDelay;
	public BarMask manualDelay;
	float firingDelayStart = 1;
	float firingDelayCount;
	float manualDelayStart = 1;
	float manualDelayCount;
	private void Update () {
		firingDelayCount -= Time.deltaTime;
		manualDelayCount -= Time.deltaTime;
		//Debug.Log (firingDelayStart);
		firingDelay.SetBarFill (firingDelayCount / firingDelayStart);
		manualDelay.SetBarFill (manualDelayCount / manualDelayStart);
	}

	public override void LoadDelayInfo (float firingDelay, float otherDelay) {
		//Debug.Log (firingDelay);
		if (firingDelay > 0) {
			firingDelayStart = firingDelay;
			firingDelayCount = firingDelay;
		}
		if (otherDelay > 0) {
			manualDelayStart = otherDelay;
			manualDelayCount = otherDelay;
		}
	}
	public override void LoadGunInfo (bool charged, int mag, int magSize) {
		if (charged) {
			this.charged.SetBarFill (1f);
		} else {
			this.charged.SetBarFill (0f);
		}
		ammo.SetBarFill ((float)mag / magSize);
	}

	public override void LoadVitals (int armor, int health, int shield) {
		//Debug.Log (health);
		this.armor.SetBarFill (armor / 100f);
		this.health.SetBarFill (health / 100f);
		this.shield.SetBarFill (shield / 100f);
	}
}