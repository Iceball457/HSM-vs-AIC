using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DropdownSelector : MonoBehaviour {
	public TMP_Dropdown dropdown;

	private void Awake () {
		dropdown.onValueChanged.AddListener (OnValueChanged);
	}
	protected abstract void OnValueChanged (int arg0);
}