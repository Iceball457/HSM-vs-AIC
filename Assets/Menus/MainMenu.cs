using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public List<GameObject> menus;
	private void Start () {
		ShowMenu (menus [0]);
	}
	public void ShowMenu (GameObject menu) {
		foreach (GameObject cMenu in menus) {
			cMenu.SetActive (menu == cMenu);
		}
	}
}