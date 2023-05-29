// Written by Liam Bansal
// Date Created: 27/5/2023

using UnityEngine;
using static Interactable;

public class PressurePlate : MonoBehaviour, IHasTrigger, IIsTrigger {
	public bool Active {
		get { return active; }
		set { }
	}
	public IIsTrigger Trigger {
		get { return trigger; }
		set { }
	}

	private bool active = false;
	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private IIsTrigger trigger = null;

	private void OnTriggerEnter(Collider other) {
		CheckForTrigger(other, true);
	}

	private void OnTriggerStay(Collider other) {
		CheckForTrigger(other, true);
	}

	private void OnTriggerExit(Collider other) {
		CheckForTrigger(other, false);
	}

	private void CheckForTrigger(Collider collider, bool touchingCollider) {
		IIsTrigger trigger = collider.GetComponent<IIsTrigger>();

		if (trigger == null) {
			return;
		}

		if (trigger.Active && touchingCollider) {
			active = true;
			this.trigger = trigger;
		} else {
			active = false;
			this.trigger = null;
		}
	}
}
