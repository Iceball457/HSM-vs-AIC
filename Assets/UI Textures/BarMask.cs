using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMask : MonoBehaviour {
	RectTransform rect;
	public float elementWidth;
	private void Awake () {
		rect = GetComponent<RectTransform> ();
	}
	public void SetBarFill (float percent) {
		rect.sizeDelta = new Vector2 (elementWidth * percent, rect.sizeDelta.y);
	}
}