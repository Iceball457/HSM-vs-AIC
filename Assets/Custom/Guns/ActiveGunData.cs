using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGunData {
	public GunData data;
	public int mag; //If less than GunData.maxMag, player can reload. Firing with no bullets causes a dryfire.
	public float reloading; //If greater than 0, cannot fire or swap guns
	public float firingCooldown; //If greater than 0, cannot fire
	public bool chambered; //If false, firing will cause a manual chamber
	public float chambering; //If greater than 0, cannot fire or swap guns
	public ActiveGunData () {
		
	}
	public ActiveGunData (GunData gun) {
		data = gun;
		mag = gun.magSize;
		chambered = true;
	}
	public ActiveGunData (int startingAmmo, bool startsChambered) {
		mag = startingAmmo;
		chambered = startsChambered;
	}
}