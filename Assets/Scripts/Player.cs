// Written by Liam Bansal
// Date Created: 9/5/2023

using UnityEngine;

/// <summary>
/// Controls the player's response to all device inputs such as movement, 
/// interactability, gestures, and pings.
/// </summary>
public class Player : Agent {
	protected override void Awake() {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	protected override void OnTriggerEnter(Collider other) {
		base.OnTriggerEnter(other);
	}

	protected override void OnTriggerStay(Collider other) {
		base.OnTriggerStay(other);
	}

	protected override void OnTriggerExit(Collider other) {
		base.OnTriggerExit(other);
	}
}
