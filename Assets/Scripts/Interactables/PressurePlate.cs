// Written by Liam Bansal
// Date Created: 27/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

public class PressurePlate : MonoBehaviour, ITrigger {
	public bool Active {
		get { return active; }
		set { }
	}
	public ITrigger Trigger {
		get { return trigger; }
		set { }
	}

	private bool active = false;
	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private ITrigger trigger = null;

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
		ITrigger trigger = collider.GetComponent<ITrigger>();

		if (trigger == null) {
			return;
		}

		active = touchingCollider;

		if (touchingCollider) {
			this.trigger = trigger;
		} else {
			this.trigger = null;
		}
	}
}
