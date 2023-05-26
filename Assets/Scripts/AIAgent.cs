// Written by Liam Bansal
// Date Created: 9/5/2023

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
		"AI agent when it's following them.")]
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

	private void Update() {
		UpdateHTN();
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

		// Define the pre/postconditions in the order which they must be met first.
		#region Follow Task
		Condition[] followPreconditions = new Condition[] {
			new Condition("See Object")
		};
		Condition[] followPostconditions = new Condition[] {
			new Condition("In Position")
		};
		PrimitiveTask followTask = new PrimitiveTask(Follow, followPreconditions, followPostconditions);
		#endregion
		#region Move To Task
		Condition[] moveToPreconditions = new Condition[] {
			new Condition("Has Destination")
		};
		Condition[] moveToPostconditions = new Condition[] {
			new Condition("In Position"),
			new Condition("In Range")
		};
		PrimitiveVectorTask moveToTask = new PrimitiveVectorTask(MoveTo, Vector3.zero, moveToPreconditions, moveToPostconditions);
		#endregion
		#region PickupTask
		Condition[] pickupPreconditions = new Condition[] {
			new Condition("Not Holding Object"),
			new Condition("See Object"),
			new Condition("In Range")
		};
		Condition[] pickupPostconditions = new Condition[] {
			new Condition("Holding Object")
		};
		PrimitiveInteractableTask pickUpTask = new PrimitiveInteractableTask(PickUp, pickupPreconditions, pickupPostconditions);
		#endregion
		#region Drop Task
		Condition[] dropPreconditions = new Condition[] {
			new Condition("Holding Object")
		};
		Condition[] dropPostconditions = new Condition[] {
			new Condition("Not Holding Object")
		};
		PrimitiveTask dropTask = new PrimitiveTask(Drop, dropPreconditions, dropPostconditions);
		#endregion
		#region Stay Task
		Condition[] stayPreconditions = new Condition[] {
			new Condition("Has Destination")
		};
		Condition[] stayPostconditions = new Condition[] {
			new Condition("In Position")
		};
		PrimitiveTask stayTask = new PrimitiveTask(Stay, stayPreconditions, stayPostconditions);
		#endregion
		#region Look Around Task
		Condition[] lookAroundPreconditions = new Condition[] {
			new Condition("In Position")
		};
		Condition[] lookAroundPostconditions = new Condition[] {
			new Condition("See Object")
		};
		PrimitiveTask lookAroundTask = new PrimitiveTask(LookAround, lookAroundPreconditions, lookAroundPostconditions);
		#endregion

		Task[] networkTasks = new Task[] {
			followTask,
			moveToTask,
			pickUpTask,
			dropTask,
			stayTask,
			lookAroundTask
		};

		#region Follow Goal
		Condition[] followGoalPreconditions = new Condition[] {
			new Condition("See Object")
		};
		Condition[] followGoalPostconditions = new Condition[] {
			new Condition("In Position")
		};
		PrimitiveTask followGoal = new PrimitiveTask(Follow, followGoalPreconditions, followGoalPostconditions);
		#endregion

		Task[] goals = new Task[] {
			followGoal
		};

		hierarchicalTaskNetwork = new HierarchicalTaskNetwork(goals, networkTasks);
		hierarchicalTaskNetwork.SetGoal(followGoal);
	}

	private void UpdateHTN() {
		if (hierarchicalTaskNetwork == null) {
			return;
		}

		hierarchicalTaskNetwork.Update();
	}

	#region Actions
	/// <summary>
	/// Makes the AI agent follow the player.
	/// </summary>
	private TaskState Follow() {
		return TaskState.Succeeded;
	}

	/// <summary>
	/// Makes the AI agent stand at the specified position.
	/// </summary>
	/// <param name="targetStandPosition"> The position where the AI agent will move to. </param>
	private TaskState MoveTo(Vector3 targetStandPosition) {
		return TaskState.Succeeded;
	}

	/// <summary>
	/// Makes the AI agent stay in place at it's current position.
	/// </summary>
	private TaskState Stay() {
		return TaskState.Succeeded;
	}

	private TaskState LookAround() {
		return TaskState.Succeeded;
	}

	/// <summary>
	/// Makes the AI agent pick up the specified interactable.
	/// </summary>
	/// <param name="objectToPickUp"> The interactable that the AI agent will pick up. </param>
	private TaskState PickUp(Interactable objectToPickUp) {
		return TaskState.Succeeded;
	}

	/// <summary>
	/// Makes the AI agent drop the interactable it's currently holding.
	/// </summary>
	private TaskState Drop() {
		return TaskState.Succeeded;
	}
	#endregion
}
