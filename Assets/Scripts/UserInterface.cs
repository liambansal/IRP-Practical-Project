// Written by Liam Bansal
// Date Created: 28/5/2023

using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
	private void Update() {
		ShowCursor();
	}

	private void ShowCursor() {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
