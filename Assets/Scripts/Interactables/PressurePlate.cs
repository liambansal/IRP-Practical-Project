// Written by Liam Bansal
// Date Created: 27/5/2023

using UnityEngine;
using static Interactable;

public class PressurePlate : MonoBehaviour,
	IHasTrigger,
	IIsTrigger,
	Ping.IPingInfo {
	public bool Active {
		get { return active; }
		set { }
	}
	public IIsTrigger Trigger {
		get { return trigger; }
		set { }
	}
	public GameObject TriggerGameObject {
		get { return triggerGameObject; }
		set { }
	}
	public string ObjectName {
		get { return objectName; }
		set { }
	}
	public Vector3 WorldPosition {
		get;
		set;
	}

	private bool active = false;
	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private IIsTrigger trigger = null;
	[SerializeField]
	private string objectName = "";
	[SerializeField, Tooltip("The game-object that triggers this " +
		"interactable to become active.")]
	private GameObject triggerGameObject = null;

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

		if (trigger == null &&
			!collider.CompareTag("Player") &&
			!collider.CompareTag("AI Agent")) {
			return;
		}

		if (touchingCollider) {
			active = true;

			if (trigger != null) {
				this.trigger = trigger;
				TriggerGameObject = collider.gameObject;
			}
		} else if ((trigger != null && !collider.CompareTag("Player") && !collider.CompareTag("AI Agent")) ||
			(trigger == null && (collider.CompareTag("Player") || collider.CompareTag("AI Agent")))) {
			active = false;
			this.trigger = trigger;
			TriggerGameObject = collider.gameObject;
		}
	}
}
