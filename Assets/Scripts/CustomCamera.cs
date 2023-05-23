// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the camera follow the player game-object.
/// </summary>
public class CustomCamera : MonoBehaviour {
	private Player player = null;
	private Vector3 offset = Vector3.zero;

	private void Start() {
		FindComponents();
		offset = player.transform.position - transform.position;
	}

	private void Update() {
		FollowPlayer();
	}

	private void FindComponents() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void FollowPlayer() {
		if (!player) {
			return;
		}

		transform.position = player.transform.position - offset;
	}
}
