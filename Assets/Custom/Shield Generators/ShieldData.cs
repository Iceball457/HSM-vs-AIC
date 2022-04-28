using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "New Shield")]
public class ShieldData : ScriptableObject {
	public int maxCharge;
	public float fullLinearChargeTime;
	public float chargeDelay;
	[ColorUsage (false, true)]
	public Color shieldColor;
	public enum ChargeType {
		Battery,
		Capacitor,
		Flywheel
	}
	public ChargeType chargeType;
}