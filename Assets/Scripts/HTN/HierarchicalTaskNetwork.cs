// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections.Generic;
using System.Linq;
using static Task;

/// <summary>
/// The decision making process that will control what actions an AI agent 
/// performs to fulfill their order(s).
/// </summary>
public class HierarchicalTaskNetwork {
	private Task goal = null;
	/// <summary>
	/// A collection of tasks which the hierarchical task network can choose 
	/// from to set a goal to achieve.
	/// </summary>
	private Task[] goals = null;
	private Task currentTaskToExecute = null;
	private TaskState currentTaskState = TaskState.NotStarted;
	/// <summary>
	/// An array of tasks that can be used by the hierarchical task network.
	/// </summary>
	private Task[] accessibleTasks = null;
	private Stack<Task> plan = new Stack<Task>();
	/// <summary>
	/// Used for sharing conditions between different tasks.
	/// </summary>
	private List<Condition> conditionBlackboard = new List<Condition>();

	public HierarchicalTaskNetwork(Task[] goals, Task[] accessibleTasks) {
		this.goal = null;
		this.goals = goals;
		this.currentTaskToExecute = null;
		this.accessibleTasks = accessibleTasks;
		this.plan = new Stack<Task>();
	}

	public void SetGoal(Task goal) {
		if (goal == null) {
			// Return if no goal has been set.
			return;
		}

		this.goal = goal;
		CreatePlan();
	}

	public void Update() {
		FindGoal();
		ExecutePlan();
	}

	private void FindGoal() {
		if (goal != null) {
			return;
		}

		Task newGoal = null;
		// TODO: find the best goal to achieve here.

		if (goal != newGoal) {
			// Only set the goal if it's different to the current one.
			SetGoal(newGoal);
		}
	}

	#region Creating a HTN Plan
	private void CreatePlan() {
		if (goal == null) {
			return;
		}

		if (plan.Count > 0 || currentTaskToExecute != null) {
			CancelPlan();
		}

		plan = DecomposeTask(goal, accessibleTasks);
	}

	private void CancelPlan() {
		plan.Clear();
		currentTaskState = default;
		currentTaskToExecute = null;
	}

	/// <summary>
	/// Creates a stack of tasks to be executed in order to achieve the goal 
	/// task.
	/// </summary>
	/// <param name="taskToDecompose"> The task to create a plan for. </param>
	/// <returns> The stack of subtasks that acheive the goal. </returns>
	private Stack<Task> DecomposeTask(Task taskToDecompose, Task[] availableTasks) {
		OrderTasks(ref availableTasks, taskToDecompose);
		// Gets the tasks that only satisfy the preconditions of the task to decompose.
		Task[] validTasks = GetValidTasks(taskToDecompose, availableTasks);
		Stack<Task> plan = new Stack<Task>();
		UpdateAlternateTasks(ref taskToDecompose);

		if (!taskToDecompose.AllConditionsSatisfied(taskToDecompose.Preconditions)) {
			// Gets a set of tasks that solve the preconditions of the task to
			// decompose.
			for (int i = 0; i < validTasks.Length; ++i) {
				validTasks[i].UpdateGoal(validTasks[i].Goal.goalType, taskToDecompose.Goal.goalObject);
				UpdateAlternateTasks(ref validTasks[i]);
				plan.Push(validTasks[i]);
				
				if (!AreAllTasksAddressed(plan.Reverse().ToArray())) {
					plan.Pop();
					Stack<Task> subtaskStack = DecomposeTask(validTasks[i], accessibleTasks);
					CompoundTask compoundTask = new CompoundTask(subtaskStack,
						subtaskStack.Peek().Preconditions,
						validTasks[i].Postconditions,
						validTasks[i].Goal);
					plan.Push(compoundTask);
				}

				// Push the task to decompose prior to checking if all tasks
				// have been addressed.
				plan.Push(taskToDecompose);

				if (AreAllTasksAddressed(plan.Reverse().ToArray())) {
					// All the goal's preconditions have been addressed, break.
					break;
				} else {
					// TODO: remove other tasks that only share the same postconditions and have no more
					// Remove the final task since the plan isn't finished yet.
					plan.Pop();
					continue;
				}
			}
		}

		if (plan.Count == 0) {
			// Remeber to add the original task to execute.
			plan.Push(taskToDecompose);
		}

		bool areAllSubtsksAddressed = AreAllTasksAddressed(plan.Reverse().ToArray());

		if (areAllSubtsksAddressed) {
			Task[] planCopy = plan.ToArray();
			plan.Clear();

			for (int i = 0; i < planCopy.Length; ++i) {
				// Pushing each element from the array onto a stack reverses
				// the order of the collection.
				plan.Push(planCopy[i]);
			}

			return plan;
		}

		return new Stack<Task>();

		// Sets the data that's used as the argument for the tasks with parameters.
		void UpdateAlternateTasks(ref Task task) {
			if (task is PrimitiveVectorTask && task.Goal.goalObject) {
				(task as PrimitiveVectorTask).SetVector(task.Goal.goalObject.transform.position);
				task.ChangeCondition(ConditionLists.Preconditions,
					"Vector Set",
					"",
					true);
			} else if (task is PrimitiveInteractableTask && task.Goal.goalObject) {
				(task as PrimitiveInteractableTask).SetInteractable(task.Goal.goalObject.GetComponent<Interactable>());
			}
		}

		// Returns true if all the tasks in the plan have been satisfied or
		// addressed by another plan.
		bool AreAllTasksAddressed(Task[] plan) {
			Condition[] planPostconditions = GatherConditions(plan, ConditionLists.Postconditions);

			foreach (Task task in plan) {
				if (task is CompoundTask) {
					if (!AreAllTasksAddressed((task as CompoundTask).Subtasks.ToArray())) {
						return false;
					}

					continue;
				}

				foreach (Condition condition in task.Preconditions) {
					if (condition.satisfied || planPostconditions.Contains(condition)) {
						continue;
					}

					return false;
				}
			}

			return true;
		}
	}

	// TODO: add a bias for tasks that have their postconditions matching
	// the order of the goal tasks preconditions.
	/// <summary>
	/// Orders the tasks by which one is the most optimal to execute first.
	/// </summary>
	/// <param name="tasks"> The collection of tasks to change the order of. </param>
	/// <param name="goalTask"> Where the conditions for ordering the tasks is 
	/// obtained from. </param>
	private void OrderTasks(ref Task[] tasks, Task goalTask) {
		// TODO: Order the available tasks by which one's postconditions satisfies
		// the goal tasks' preconditions best, and set them in the same order
		// as the goal tasks preconditions e.g. goal precons = seeObject,
		// inRange. Then subtasks should be in oder of (seeObject, inRange),
		// (inRange, otherPostcon, seeObject) seeObject, (otherPostCon,
		// seeObject), inRange, (inRange, otherPostcon).
		tasks = tasks.OrderByDescending(task => task.Goal.goalType == goalTask.Goal.goalType).ThenByDescending(task => MatchingConditions(task.Postconditions,
			goalTask.Preconditions)).ToArray();
	}

	// TODO: add a task even if it's condition only partially matches.
	/// <summary>
	/// Returns an array of tasks that completely or partially satisfy the 
	/// parameter task's preconditions.
	/// </summary>
	/// <returns></returns>
	private Task[] GetValidTasks(Task goalTask, Task[] availableTasks) {
		List<Task> validTasks = new List<Task>();

		foreach (Task task in availableTasks) {
			foreach (Condition postCondition in task.Postconditions) {
				// If a postcondition and precondition match then the
				// available task satisfies the goal task.
				if (goalTask.Preconditions.Contains(postCondition)) {
					validTasks.Add(task);
				}
			}
		}

		return validTasks.ToArray();
	}
	#endregion

	/// <summary>
	/// Executes the stack of tasks, one at a time, to complete the HTN's goal.
	/// </summary>
	private void ExecutePlan() {
		// Does the HTN have no goal?
		if (goal == null ||
			(plan != null &&
			// Has the HTN's goal been achieved?
			((this.currentTaskState == TaskState.Succeeded && plan.Count == 0) ||
			// Is there no plan?
			plan.Count == 0))) {
			return;
		}

		if (currentTaskToExecute == null) {
			currentTaskToExecute = plan.Pop();
			currentTaskState = TaskState.NotStarted;
		}

		if (!currentTaskToExecute.ExecuteTask(ref currentTaskToExecute, ref currentTaskState)) {
			CancelPlan();
		}
	}
}
