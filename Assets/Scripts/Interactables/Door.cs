// Written by Liam Bansal
// Date Created: 27/5/2023

using UnityEngine;
using static Interactable;

public class Door : MonoBehaviour, IHasTrigger {
	public ICanTrigger Trigger {
		get { return trigger; }
		set { }
	}

	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private ICanTrigger trigger = null;

	private void Update() {
		IsTriggered();
	}

	private void IsTriggered() {
		if (trigger == null || !trigger.Active) {
			return;
		}

		// TODO: play the animation?
	}
}
