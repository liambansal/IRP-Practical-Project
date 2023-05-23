// Written by Liam Bansal
// Date Created: 9/5/2023

using UnityEngine;
using StarterAssets;

/// <summary>
/// Makes the camera follow the player game-object.
/// </summary>
public class CustomCamera : MonoBehaviour {
	private ThirdPersonController player = null;
	private Vector3 offset = Vector3.zero;

	private void Start() {
		FindComponents();
		offset = player.transform.position - transform.position;
	}

	private void Update() {
		FollowPlayer();
	}

	private void FindComponents() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
	}

	private void FollowPlayer() {
		if (!player) {
			return;
		}

		transform.position = player.transform.position - offset;
	}
}
