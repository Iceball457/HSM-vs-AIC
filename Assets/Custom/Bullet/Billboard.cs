using UnityEngine;

public class Billboard : MonoBehaviour {
	private void OnWillRenderObject () {
		transform.rotation = Camera.current.transform.rotation;
	}
}