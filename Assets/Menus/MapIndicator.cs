using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapIndicator : MonoBehaviour {
	public List<int> scenes;
	public List<GameObject> indicators;
	private void Start () {
		ClearValue ();
	}
	public void ClearValue () {
		ChangeValue (0);
	}
	public void ChangeValue (int scene) {
		for (int i = 0; i < indicators.Count; i++) {
			indicators [i].SetActive (scenes [i] == scene);
		}
	}
}