using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownMode : DropdownSelector {
	public ArenaNetworkLink netLink;
	//public List<GameObject> gameModePrefabs;
	private void Start () {
		OnValueChanged (0);
	}
	protected override void OnValueChanged (int arg0) {
		//netLink.SetGamemode ();
	}
}