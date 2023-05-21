// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Task;

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

	/// <summary>
	/// Gets the class' required references for components attached to the 
	/// same game-object as this script, or any of it's children.
	/// </summary>
	private void GetComponents() {
		visualSensor = GetComponent<VisualSensor>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	/// <summary>
	/// Finds the class' required references for components that are attached 
	/// to other game-objects.
	/// </summary>
	private void FindComponents() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	/// <summary>
	/// Creates a hierarchical task network for the AI to use, if one has not 
	/// already been created.
	/// </summary>
	private void CreateNetwork() {
		if (hierarchicalTaskNetwork != null) {
			return;
		}

		Task FollowTask = new Task(null, null, Follow);
		Task MoveToTask = new Task(null, null, MoveTo);
		Task PickUpTask = new Task(null, null, PickUp);
		Task DropTask = new Task(null, null, Drop);
		Task StayTask = new Task(null, null, Stay);
		// TODO: create a goal that can be set by the player.
		Task FollowOrder = new Task(null, null, null);

		Task[] networkTasks = new Task[] {
			FollowTask,
			MoveToTask,
			PickUpTask,
			DropTask,
			StayTask
		};

		// TODO: create a list of goals that the HTN can complete.
		hierarchicalTaskNetwork = new HierarchicalTaskNetwork(null, null);
	}

	#region Actions
	/// <summary>
	/// Makes the AI agent follow the player.
	/// </summary>
	private TaskState Follow() {
		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent stand at the specified position.
	/// </summary>
	/// <param name="targetStandPosition"> The position where the AI agent will move to. </param>
	private TaskState MoveTo(Vector3 targetStandPosition) {
		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent pick up the specified interactable.
	/// </summary>
	/// <param name="objectToPickUp"> The interactable that the AI agent will pick up. </param>
	private TaskState PickUp(Interactable objectToPickUp) {
		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent drop the interactable it's currently holding.
	/// </summary>
	private TaskState Drop() {
		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent stay in place at it's current position.
	/// </summary>
	private TaskState Stay() {
		return TaskState.Executing;
	}
	#endregion
}
