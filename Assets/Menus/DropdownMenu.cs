using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownMenu : DropdownSelector {
	public MainMenu dropdownControlledMenu;
	protected override void OnValueChanged (int arg0) {
		dropdownControlledMenu.ShowMenu (dropdownControlledMenu.menus [dropdown.value]);
	}
}