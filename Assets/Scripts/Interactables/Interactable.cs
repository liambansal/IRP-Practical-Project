// Written by Liam Bansal
// Date Created: 9/5/2023

using UnityEngine;
using static Ping;

/// <summary>
/// The base class for all interactable game-objects.
/// Provides methods and variables that will be used by all, if not most, 
/// interactable game-objects.
/// </summary>
public abstract class Interactable : MonoBehaviour, IPingInfo {
	/// <summary>
	/// To be implemented by any agent's class that can pickup interactable objects.
	/// </summary>
	public interface IAssociatedAgentInfo {
		public Transform PickupPoint {
			get;
			set;
		}
	}

	/// <summary>
	/// Implement in interactable item classes that only work when multiple 
	/// interactables are used together.
	/// Can be implemented by classes that don't inherit from the
	/// interactable base class.
	/// </summary>
	public interface IHasTrigger {
		/// <summary>
		/// The object that triggers this interactable to become active.		
		/// </summary>
		public IIsTrigger Trigger {
			get;
			set;
		}
		public GameObject TriggerGameObject {
			get;
			set;
		}
	}

	/// <summary>
	/// To be implemented by interactable item that can act as a trigger for 
	/// others to activate.
	/// </summary>
	public interface IIsTrigger {
		public bool Active {
			get;
			set;
		}
	}

	public string ObjectName {
		get { return objectName; }
		set { }
	}
	public Vector3 WorldPosition {
		get { return transform.position; }
		set { }
	}
	public IAssociatedAgentInfo AssociatedAgent {
		get;
		private set;
	}

	protected bool pickedUp = false;
	[SerializeField]
	protected string objectName = "";
	protected Rigidbody rigidBody = null;

	private Transform pickupPoint = null;

	/// <summary>
	/// Causes the object to be picked up or dropped by an agent.
	/// </summary>
	/// <param name="associatedAgent"> The agent interacting with this interactable. </param>
	/// <param name="pickedUp"> True if the agent is picking up the 
	/// interactable, false if they're dropping it. </param>
	/// <returns> True if the item was manipulated successfully. </returns>
	public virtual bool Pickup(IAssociatedAgentInfo associatedAgent, bool pickedUp) {
		// Check if an agent who isn't holding this object is trying to
		// interact with it.
		if (AssociatedAgent != null && AssociatedAgent != associatedAgent) {
			return false;
		}

		this.pickedUp = pickedUp;
		rigidBody.useGravity = !pickedUp;
		rigidBody.isKinematic = pickedUp;
		rigidBody.freezeRotation = pickedUp;

		if (pickedUp) {
			AssociatedAgent = associatedAgent;
			pickupPoint = associatedAgent.PickupPoint;
		} else {
			AssociatedAgent = null;
			pickupPoint = null;
		}

		return true;
	}

	protected virtual void Awake() {
		rigidBody = GetComponent<Rigidbody>();
	}

	protected virtual void Update() {
		HoldObject();
	}

	private void HoldObject() {
		if (!pickedUp || !pickupPoint) {
			return;
		}

		transform.position = pickupPoint.position;
	}
}
