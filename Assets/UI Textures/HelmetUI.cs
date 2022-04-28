using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HelmetUI : MonoBehaviour {
	public abstract void LoadVitals (int armor, int health, int shields);
	public abstract void LoadGunInfo (bool charged, int mag, int magSize);
	public abstract void LoadDelayInfo (float firingDelay, float otherDelay);
}