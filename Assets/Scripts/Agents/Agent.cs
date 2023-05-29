// Written by Liam Bansal
// Date Created: 26/5/2023

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Task;

public class Agent : MonoBehaviour, Interactable.IAssociatedAgentInfo {
	public Transform PickupPoint {
		get { return pickupPoint; }
		set { }
	}
	public Interactable CurrentInteractable {
		get { return currentInteractable; }
		protected set { currentInteractable = value; }
	}

	[SerializeField, Tooltip("The transform whose position will decide where " +
		"this agent hold's interactable game-objects.")]
	protected Transform pickupPoint = null;
	/// <summary>
	/// The interactable game-object the agent is currently holding onto.
	/// </summary>
	protected Interactable currentInteractable = null;
	protected List<Interactable> nearbyInteractables = new List<Interactable>();

	protected virtual void Awake() { }

	protected virtual void Start() { }

	protected virtual void Update() {
		MoveHeldObject();
	}

	protected virtual void FixedUpdate() {
		OrderNearbyInteractables();
	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Interactable")) {
			InteractableDetected(other.GetComponent<Interactable>(), true);
		}
	}

	protected virtual void OnTriggerStay(Collider other) {
		if (other.CompareTag("Interactable")) {
			InteractableDetected(other.GetComponent<Interactable>(), true);
		}
	}

	protected virtual void OnTriggerExit(Collider other) {
		if (other.CompareTag("Interactable")) {
			InteractableDetected(other.GetComponent<Interactable>(), false);
		}
	}

	/// <summary>
	/// Makes the agent pickup the specified interactable.
	/// </summary>
	/// <param name="objectToPickUp"> The interactable that the agent will pick up. </param>
	protected TaskState PickupObject(Interactable objectToPickUp) {
		if (!objectToPickUp) {
			return TaskState.Failed;
		}

		if (!currentInteractable && objectToPickUp.Pickup(this, true)) {
			currentInteractable = objectToPickUp;
			return TaskState.Succeeded;
		}

		return TaskState.Failed;
	}

	/// <summary>
	/// Makes the agent drop the interactable they're currently holding.
	/// </summary>
	protected TaskState DropObject() {
		if (!currentInteractable) {
			return TaskState.Succeeded;
		}

		if (currentInteractable.Pickup(this, false)) {
			currentInteractable = null;
			return TaskState.Succeeded;
		}

		return TaskState.Failed;
	}

	private void InteractableDetected(Interactable interactable, bool nearby) {
		if (nearby &&
			interactable.AssociatedAgent == null &&
			!nearbyInteractables.Contains(interactable)) {
			nearbyInteractables.Add(interactable);
		} else if (!nearby &&
			nearbyInteractables.Contains(interactable)) {
			nearbyInteractables.Remove(interactable);
		}
	}

	private void OrderNearbyInteractables() {
		nearbyInteractables.OrderBy(interactable => Vector3.Distance(transform.position, interactable.transform.position));
	}

	/// <summary>
	/// Moves the agent's held object to their pickup point.
	/// </summary>
	private void MoveHeldObject() {
		if (currentInteractable) {
			currentInteractable.transform.position = pickupPoint.position;
		}
	}
}
