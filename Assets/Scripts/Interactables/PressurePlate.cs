// Written by Liam Bansal
// Date Created: 27/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

public class PressurePlate : MonoBehaviour, IHasTrigger, ICanTrigger {
	public bool Active {
		get { return active; }
		set { }
	}
	public ICanTrigger Trigger {
		get { return trigger; }
		set { }
	}

	private bool active = false;
	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private ICanTrigger trigger = null;

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
		ICanTrigger trigger = collider.GetComponent<ICanTrigger>();

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
