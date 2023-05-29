// Written by Liam Bansal
// Date Created: 9/5/2023

using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using static Interactable;
using static Ping;

/// <summary>
/// Controls the player's response to all device inputs such as movement, 
/// interactability, gestures, and pings.
/// </summary>
public class Player : Agent {
	private ThirdPersonController characterController = null;
	private AIAgent aiAgent = null;
	private HierarchicalTaskNetwork taskNetwork = null;
	[SerializeField, Tooltip("The UI element used for pinging an object.")]
	private GameObject pingPrefab = null;

	public void OrderAIToFollow() {
		PrimitiveTask followTask = aiAgent.FollowTask;
		followTask.UpdateGoal(Task.GoalType.Follow, gameObject);
		taskNetwork.SetGoal(followTask);
	}

	public void OrderAIToMove(GameObject moveDestination) {
		PrimitiveVectorTask moveTask = aiAgent.MoveToTask;
		moveTask.UpdateGoal(Task.GoalType.MoveTo, moveDestination);
		taskNetwork.SetGoal(moveTask);
	}

	public void OrderAIToStay() {
		PrimitiveTask stayTask = aiAgent.StayTask;
		// Agent should prefer tasks that satisfy this goal type over others.
		stayTask.UpdateGoal(Task.GoalType.MoveTo, aiAgent.gameObject);
		taskNetwork.SetGoal(stayTask);
	}

	public void OrderAIToPickupObject(Interactable interactable) {
		PrimitiveInteractableTask pickupTask = aiAgent.PickupTask;
		pickupTask.UpdateGoal(Task.GoalType.Pickup, interactable.gameObject);
		taskNetwork.SetGoal(pickupTask);
	}

	public void OrderAIToDropObject() {
		PrimitiveTask dropTask = aiAgent.DropTask;
		dropTask.UpdateGoal(Task.GoalType.Drop, null);
		taskNetwork.SetGoal(dropTask);
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
		ProcessPlayerInput();
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

	private void ProcessPlayerInput() {
		Interact();
		Ping();
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

		characterController.Input.ping = false;
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		const int raycastDistance = 20;
		Physics.Raycast(ray,
			out RaycastHit raycastHit,
			raycastDistance);
		Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow, Mathf.Infinity);

		if (!raycastHit.collider) {
			return;
		}

		const int pingYOffset = 1;
		Vector3 pingPosition = raycastHit.point + Vector3.up * pingYOffset;
		Ping instantiatedPing = Instantiate(pingPrefab, pingPosition, Quaternion.identity, null).GetComponentInChildren<Ping>();
		Interactable interactableScript = raycastHit.collider.GetComponent<Interactable>();
		PressurePlate pressurePlate = raycastHit.collider.GetComponent<PressurePlate>();

		// Case for picking up an interactable item.
		if (interactableScript &&
			interactableScript is IIsTrigger &&
			!(interactableScript is IHasTrigger)) {
			OrderAIToPickupObject(interactableScript);
			instantiatedPing.SetPing(interactableScript);
			// Case for dropping an interactable item to trigger something.
		} else if (pressurePlate && aiAgent) {
			OrderAIToDropObject();
			instantiatedPing.SetPing(pressurePlate);
			// Case for activating an interactable item.
		} else if (true == false) {
			// TODO: create plan to activate the interactable.
		} else {
			OrderAIToMove(instantiatedPing.gameObject);
			instantiatedPing.SetPing("Move here!");
		}
	}
}
