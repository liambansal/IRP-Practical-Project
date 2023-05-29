// Written by Liam Bansal
// Date Created: 27/5/2023

using UnityEngine;
using static Interactable;

public class Door : MonoBehaviour, IHasTrigger {
	public IIsTrigger Trigger {
		get { return trigger; }
		set { }
	}
	public GameObject TriggerGameObject {
		get { return triggerGameObject; }
		set { }
	}

	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private IIsTrigger trigger = null;
	[SerializeField]
	private GameObject triggerGameObject = null;

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
