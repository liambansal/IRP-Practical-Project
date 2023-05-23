// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ping;

/// <summary>
/// The base class for all interactable game-objects.
/// Provides methods and variables that will be used by all, if not most, 
/// interactable game-objects.
/// </summary>
public abstract class Interactable : MonoBehaviour, PingInfo {
	/// <summary>
	/// To be implemented by any agent's class that can pickup interactable objects.
	/// </summary>
	public interface PickupOperator {
		public Transform PickUpPoint {
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

	protected bool pickedUp = false;
	protected string objectName = "";
	protected Rigidbody rigidBody = null;

	private Transform pickupPoint = null;

	protected virtual void Awake() {
		rigidBody = GetComponent<Rigidbody>();
	}

	protected virtual void Update() {
		HoldObject();
	}

	protected virtual void Pickup(PickupOperator pickupOperator, bool pickedUp) {
		this.pickedUp = pickedUp;
		rigidBody.useGravity = !pickedUp;
		rigidBody.isKinematic = pickedUp;
		rigidBody.freezeRotation = pickedUp;

		if (pickedUp) {
			pickupPoint = pickupOperator.PickUpPoint;
		}
	}

	private void HoldObject() {
		if (!pickedUp || !pickupPoint) {
			return;
		}

		transform.position = pickupPoint.position;
	}
}
