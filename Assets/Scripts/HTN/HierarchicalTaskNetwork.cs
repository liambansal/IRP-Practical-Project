// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
	private Stack<Task> plan = null;
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
		OrderTasks(ref availableTasks);
		// Gets the tasks that only satisfy the preconditions of the task to decompose.
		Task[] validTasks = GetValidTasks(taskToDecompose, availableTasks);
		Stack<Task> plan = new Stack<Task>();

		if (!taskToDecompose.AllConditionsSatisfied(taskToDecompose.Preconditions)) {
			// Get the task(s) that solve the goal tasks preconditions and add to a stack
			// loop over tasks that meet the goals preconditions
			foreach (Task task in validTasks) {
				task.UpdateGoal(task.Goal.goalType, taskToDecompose.Goal.goalObject);

				// Sets the data that's used as the argument for the tasks with parameters.
				if (task is PrimitiveVectorTask) {
					(task as PrimitiveVectorTask).SetVector(task.Goal.goalObject.transform.position);
				} else if (task is PrimitiveInteractableTask) {
					(task as PrimitiveInteractableTask).SetInteractable(task.Goal.goalObject.GetComponent<Interactable>());
				}

				plan.Push(task);
				Condition[] planPostconditions = GatherConditions(plan.ToArray(), ConditionLists.Postconditions);

				if (!MissingCondition(goal.Preconditions, planPostconditions)) {
					// Remeber to add the original task to execute.
					plan.Push(taskToDecompose);
					// All the goal's preconditions have been addressed, break.
					break;
				} else {
					// TODO: remove other tasks that only share the same postconditions and have no more
					continue;
				}
			}
		// Sets the data that's used as the argument for the tasks with parameters.
		} else {
			if (taskToDecompose is PrimitiveVectorTask) {
				(taskToDecompose as PrimitiveVectorTask).SetVector(taskToDecompose.Goal.goalObject.transform.position);
			} else if (taskToDecompose is PrimitiveInteractableTask) {
				(taskToDecompose as PrimitiveInteractableTask).SetInteractable(taskToDecompose.Goal.goalObject.GetComponent<Interactable>());
			}

			// Remeber to add the original task to execute.
			plan.Push(taskToDecompose);
		}

		// TODO: loop over the planned tasks to check all preconditions have been addressed

		// if all new tasks' preconditions are met
		if (true) {
			Task[] planCopy = plan.ToArray();
			plan.Clear();

			for (int i = 0; i < planCopy.Length; ++i) {
				// Pushing each element from the array onto a stack reverses
				// the order of the collection.
				plan.Push(planCopy[i]);
			}

			return plan;
		} else {
			// foreach task
				// if precons not met && no subtasks exist
					// repeat above process for all new tasks by CallingAboveMethod() and pass the subTask as a parameter
					// task.subtasks = AboveMethod();
		}

		return null;

		// NOTES:
		// How would you resolve this so the tasks are executed in the correct order?
		// (if the postcondition of one task, on the same level of depth, satisfies the precondition of another task,
		// then it must be executed first?)

		// goal: pickup cube
		// pickup cube (precon = seeCube && inRange)
			// find cube (compound) (postCon seeCube)
				// move to (precon = knowDestination, postcon = inPosition)
					// Must have an order to know the destination.
				// look around (precon = inPosition, postcon = seeCube)
			// move to cube (precon = seeCube, postcon = inRange)

		// TODO: add a bias for tasks that have their postconditions matching
		// the order of the goal tasks preconditions.
	}

	/// <summary>
	/// Orders the tasks by which one is the most optimal to execute first.
	/// </summary>
	private void OrderTasks(ref Task[] tasks) {
		// order the available tasks by which one's postconditions satisfies the goal tasks' preconditions best,
		// and set them in the same order as the goal tasks preconditions e.g. goal precons =
		// seeObject, inRange. Then subtasks should be in oder of (seeObject, inRange), (inRange, otherPostcon, seeObject)
		// seeObject, (otherPostCon, seeObject), inRange, (inRange, otherPostcon).
		tasks = tasks.OrderByDescending(task => task.Goal.goalType == goal.Goal.goalType).ThenByDescending(task => MatchingConditions(task.Postconditions,
			goal.Preconditions)).ToArray();
		// TODO: if two tasks are on par then order them by something else...
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
		if (goal == null||
			// Has the HTN's goal been achieved?
			(this.currentTaskState == TaskState.Succeeded && plan.Count == 0) ||
			// Is there no plan?
			plan.Count == 0) {
			return;
		}

		if (currentTaskToExecute == null) {
			currentTaskToExecute = plan.Pop();
		}

		// Checks the task is executable.
		//if (!currentTaskToExecute.AllConditionsSatisfied(currentTaskToExecute.Preconditions)) {
		//	// TODO: append more tasks to satisfy the current tasks preconditions.
		//	return;
		//}

		TaskState currentTaskState = TaskState.NotStarted;

		if (currentTaskToExecute is PrimitiveTask &&
			!(currentTaskToExecute is PrimitiveVectorTask) &&
			!(currentTaskToExecute is PrimitiveInteractableTask)) {
			currentTaskState = (currentTaskToExecute as PrimitiveTask).Task();
		} else if (currentTaskToExecute is PrimitiveVectorTask) {
			currentTaskState = (currentTaskToExecute as PrimitiveVectorTask).Task((currentTaskToExecute as PrimitiveVectorTask).Vector);
		} else if (currentTaskToExecute is PrimitiveInteractableTask) {
			currentTaskState = (currentTaskToExecute as PrimitiveInteractableTask).Task((currentTaskToExecute as PrimitiveInteractableTask).Interactable);
		} else if (currentTaskToExecute is CompoundTask) {
			// TODO: call method belonging to CompoundTask that executes
			// the subtasks one by one.
		}

		switch (currentTaskState) {
			case TaskState.NotStarted: {
				break;
			}
			case TaskState.Started: {
				break;
			}
			case TaskState.Executing: {
				break;
			}
			// Enables all of the current task's postconditions.
			case TaskState.Succeeded: {
				for (int i = 0; i < currentTaskToExecute.Postconditions.Length; ++i) {
					currentTaskToExecute.ChangeCondition(ConditionLists.Postconditions,
						currentTaskToExecute.Postconditions[i].name,
						"",
						true);
				}

				currentTaskToExecute = null;
				break;
			}
			case TaskState.Failed: {
				// TODO: check plan is still valid and try the task again.
				break;
			}
			case TaskState.Cancelled: {
				// TODO: check plan is still valid and try the task again.
				break;
			}
			default: {
				break;
			}
		}
	}
}
