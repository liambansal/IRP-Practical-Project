// Written by Liam Bansal
// Date Created: 27/5/2023

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	private bool componentsFound = false;
	[SerializeField, Tooltip("The object that triggers this " +
		"interactable to become active.")]
	private IIsTrigger trigger = null;
	[SerializeField]
	private GameObject triggerGameObject = null;
	private Animator animator = null;

	private void Awake() {
		GetComponents();
	}

	private void Update() {
		FindComponents();
		IsTriggered();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") || other.CompareTag("AI Agent")) {
			LoadNextScene();
		}
	}

	private void GetComponents() {
		animator = GetComponent<Animator>();
	}

	private void FindComponents() {
		if (componentsFound) {
			return;
		}

		trigger = triggerGameObject.GetComponent<IIsTrigger>();
		componentsFound = true;
	}

	private void IsTriggered() {
		if (trigger == null || !trigger.Active) {
			animator.SetBool("Active", false);
			return;
		}

		animator.SetBool("Active", true);
	}

	private void LoadNextScene() {
		if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1) {
			SceneManager.LoadScene(0);
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
