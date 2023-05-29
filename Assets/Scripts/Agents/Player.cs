// Written by Liam Bansal
// Date Created: 9/5/2023

using StarterAssets;
using UnityEngine;

/// <summary>
/// Controls the player's response to all device inputs such as movement, 
/// interactability, gestures, and pings.
/// </summary>
public class Player : Agent {
	private ThirdPersonController characterController = null;
	private AIAgent aiAgent = null;
	private HierarchicalTaskNetwork taskNetwork = null;

	public void OrderAIToFollow() {
		PrimitiveTask followTask = aiAgent.FollowGoal;
		followTask.UpdateGoal(Task.GoalType.Follow, gameObject);
		taskNetwork.SetGoal(followTask);
	}

	public void OrderAIToStay() {
		PrimitiveTask stayTask = aiAgent.StayGoal;
		// Agent should prefer tasks that satisfy this goal type over others.
		stayTask.UpdateGoal(Task.GoalType.MoveTo, aiAgent.gameObject);
		taskNetwork.SetGoal(stayTask);
	}

	protected override void Awake() {
		base.Awake();
		GetComponents();
	}

	protected override void Start() {
		base.Start();
		FindComponents();
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

	private void FindComponents() {
		aiAgent = GameObject.FindGameObjectWithTag("AI Agent").GetComponent<AIAgent>();
		taskNetwork = aiAgent.HierarchicalTaskNetwork;
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

		// TODO: raycast for position to place ping
		// TODO: get type of marker to place
		// TODO: create ping marker
		// TODO: give new goal to AI based on type of marker
		// if object is hit, pick it up
		// if ground was hit, move to
		// if pressure plate was hit and ai is holding object, drop object there
	}
}
