// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An autonomous agent that follows orders given by the player, but also 
/// makes their own decisions when no order are given.
/// </summary>
public class AIAgent : MonoBehaviour {
	#region Sensor Variables
	private VisualSensor visualSensor = null;
	#endregion

	#region Movement
	private bool moveDestinationSet = false;
	[SerializeField, Tooltip("The desired distance between the player and " +
		"AI agent when following them.")]
	private int followDistance = 3;
	private Vector3 followDestination = Vector3.zero;
	/// <summary>
	/// The position where the AI will aim to move to next.
	/// It isn't constanly the same as the nav mesh agent's destination.
	/// </summary>
	private Vector3 nextMoveDestination = Vector3.zero;
	private NavMeshAgent navMeshAgent = null;
	#endregion

	private HierarchicalTaskNetwork hierarchicalTaskNetwork = null;

	private Player player = null;

	/// <summary>
	/// The interactable game-object the AI agent is currently holding onto.
	/// </summary>
	private Interactable currentInteractable = null;

	private void Awake() {
		GetComponents();
		CreateNetwork();
	}

	private void Start() {
		FindComponents();
	}

	private void GetComponents() {
		visualSensor = GetComponent<VisualSensor>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	private void FindComponents() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void CreateNetwork() {
		if (hierarchicalTaskNetwork != null) {
			return;
		}

		hierarchicalTaskNetwork = new HierarchicalTaskNetwork();
	}

	#region Actions
	private void Follow() { }

	private void MoveTo() { }

	private void PickUp() { }

	private void Drop() { }

	private void Stay() { }
	#endregion
}
