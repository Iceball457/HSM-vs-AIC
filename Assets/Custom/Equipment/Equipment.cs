using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : ScriptableObject {
	public Texture visual;
	public abstract bool Use (Character owner, Vector3 position, Vector3 forward, Vector3 velocity, float force);
	public abstract void Drop (Vector3 position, Vector3 forward);
}