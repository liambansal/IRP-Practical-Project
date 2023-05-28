// Written by Liam Bansal
// Date Created: 9/5/2023

using UnityEngine;
using UnityEngine.AI;
using static Task;

/// <summary>
/// An autonomous agent that follows orders given by the player, but also 
/// makes their own decisions when no order are given.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : Agent {
	#region Sensor Variables
	private VisualSensor visualSensor = null;
	#endregion

	#region Movement
	private bool moveDestinationSet = false;
	[SerializeField, Tooltip("The desired distance between the player and " +
		"AI agent when it's following them.")]
	private float followDistance = 2.0f;
	/// <summary>
	/// The position where the AI will aim to move to next.
	/// It isn't constanly the same as the nav mesh agent's destination.
	/// </summary>
	private Vector3 nextMoveDestination = Vector3.zero;
	private NavMeshAgent navMeshAgent = null;
	#endregion

	private HierarchicalTaskNetwork hierarchicalTaskNetwork = null;

	private Player player = null;

	protected override void Awake() {
		base.Awake();
		GetComponents();
		CreateHierarchicalTaskNetwork();
	}

	protected override void Start() {
		base.Start();
		FindComponents();
	}

	protected override void Update() {
		base.Update();
		UpdateHTN();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	protected override void OnTriggerEnter(Collider other) {
		base.OnTriggerEnter(other);
	}

	protected override void OnTriggerStay(Collider other) {
		base.OnTriggerStay(other);
	}

	protected override void OnTriggerExit(Collider other) {
		base.OnTriggerExit(other);
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
	private void CreateHierarchicalTaskNetwork() {
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
		PrimitiveInteractableTask pickUpTask = new PrimitiveInteractableTask(PickupObject, pickupPreconditions, pickupPostconditions);
		#endregion
		#region Drop Task
		Condition[] dropPreconditions = new Condition[] {
			new Condition("Holding Object")
		};
		Condition[] dropPostconditions = new Condition[] {
			new Condition("Not Holding Object")
		};
		PrimitiveTask dropTask = new PrimitiveTask(DropObject, dropPreconditions, dropPostconditions);
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
		if (!navMeshAgent.isOnNavMesh) {
			navMeshAgent.ResetPath();
			moveDestinationSet = false;
			return TaskState.Failed;
		}

		if (!moveDestinationSet) {
			Vector3 targetDirection = (player.transform.position - transform.position).normalized;
			Vector3 targetDestination = player.transform.position - targetDirection * followDistance;
			moveDestinationSet = navMeshAgent.SetDestination(targetDestination);
		}

		if (ArrivedAtDestination()) {
			return TaskState.Succeeded;
		}

		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent stand at the specified position.
	/// </summary>
	/// <param name="targetStandPosition"> The position where the AI agent will move to. </param>
	private TaskState MoveTo(Vector3 targetStandPosition) {
		if (!navMeshAgent.isOnNavMesh) {
			navMeshAgent.ResetPath();
			moveDestinationSet = false;
			return TaskState.Failed;
		}

		if (!moveDestinationSet) {
			moveDestinationSet = navMeshAgent.SetDestination(targetStandPosition);
		}

		if (ArrivedAtDestination()) {
			return TaskState.Succeeded;
		}

		return TaskState.Executing;
	}

	/// <summary>
	/// Makes the AI agent stand in place at their current position indefinitely.
	/// </summary>
	private TaskState Stay() {
		if (moveDestinationSet ||
			navMeshAgent.hasPath ||
			navMeshAgent.pathPending) {
			navMeshAgent.ResetPath();
			moveDestinationSet = false;
		}

		return TaskState.Executing;
	}

	private TaskState LookAround() {
		// TODO: make the AI look around them by turning their head.
		return TaskState.Succeeded;
	}
	#endregion

	/// <summary>
	/// Checks if the AI agent has arrived at their nav mesh agent's 
	/// destination.
	/// </summary>
	/// <returns> True if the agent has arrived at their destination. </returns>
	private bool ArrivedAtDestination() {
		if (moveDestinationSet &&
			!navMeshAgent.hasPath &&
			!navMeshAgent.pathPending) {
			moveDestinationSet = false;
			return true;
		}

		return false;
	}
}