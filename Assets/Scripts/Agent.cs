// Written by Liam Bansal
// Date Created: 26/5/2023

using UnityEngine;
using static Task;

public class Agent : MonoBehaviour, Interactable.PickupOperator {
	public Transform PickupPoint {
		get { return pickupPoint; }
		set { }
	}
	
	[SerializeField, Tooltip("The transform whose position will decide where " +
		"this agent hold's interactable game-objects.")]
	protected Transform pickupPoint = null;
	/// <summary>
	/// The interactable game-object the agent is currently holding onto.
	/// </summary>
	protected Interactable currentInteractable = null;

	protected virtual void Awake() { }

	protected virtual void Start() { }

	protected virtual void Update() {
		MoveHeldObject();
	}

	/// <summary>
	/// Makes the agent pickup the specified interactable.
	/// </summary>
	/// <param name="objectToPickUp"> The interactable that the agent will pick up. </param>
	protected TaskState PickupObject(Interactable objectToPickUp) {
		if (objectToPickUp.Pickup(this, true)) {
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

	/// <summary>
	/// Moves the agent's held object to their pickup point.
	/// </summary>
	private void MoveHeldObject() {
		if (currentInteractable) {
			currentInteractable.transform.position = pickupPoint.position;
		}
	}
}
