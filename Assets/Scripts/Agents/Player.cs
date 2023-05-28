// Written by Liam Bansal
// Date Created: 9/5/2023

using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the player's response to all device inputs such as movement, 
/// interactability, gestures, and pings.
/// </summary>
public class Player : Agent {
	private ThirdPersonController characterController = null;

	protected override void Awake() {
		base.Awake();
		GetComponents();
	}

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();
		Interact();
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

	private void GetComponents() {
		characterController = GetComponent<ThirdPersonController>();
	}

	private void Interact() {
		if (!characterController.Input.interact) {
			return;
		} 

		if (!currentInteractable &&
			nearbyInteractables.Count > 0) {
			PickupObject(nearbyInteractables[0]);
			characterController.Input.interact = false;
		} else if (currentInteractable) {
			DropObject();
			characterController.Input.interact = false;
		}
	}

	private void Ping() {
		if (!characterController.Input.ping) {
			return;
		}

		// TODO: create ping marker
		// TODO: get type of marker
		// if object is hit, pick it up
		// if ground was hit, move to
		// if pressure plate was hit, drop object there
		// TODO: give new goal to AI based on type of marker
	}
}
