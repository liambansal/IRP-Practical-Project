// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Task;

/// <summary>
/// The decision making process that will control what actions an AI agent 
/// performs to fulfill their order(s).
/// </summary>
public class HierarchicalTaskNetwork : MonoBehaviour {
	private Task goal = null;
	private Task[] goals = null;
	private Task currentTaskToExecute = null;
	private TaskState currentTaskState = TaskState.NotStarted;
	private Task[] availableTasks = null;
	private Task[] executableTasks = null;
	private Stack<Task> plan = null;

	public HierarchicalTaskNetwork(Task[] goals, Task[] availableTasks) {
		this.goal = null;
		this.goals = goals;
		this.currentTaskToExecute = null;
		this.availableTasks = availableTasks;
		this.executableTasks = new Task[availableTasks.Length];
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

	private void Update() {
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

	private void FindExecuteableActions() {
		// TODO: check if any of the available tasks' preconditions are all
		// met and add any valid tasks to an array.  
	}

	private void CreatePlan() {
		if (goal == null) {
			return;
		}

		if (plan.Count > 0) {
			plan.Clear();
		}

		OrderTasks(GetValidTasks(goal, executableTasks));
		SetPlan();
	}

	/// <summary>
	/// Returns an array of tasks that completely or partiall satisfy the 
	/// parameter goal task's preconditions.
	/// </summary>
	/// <returns></returns>
	private Task[] GetValidTasks(Task goalTask, Task[] availableTasks) {
		List<Task> validTasks = new List<Task>();

		foreach (Task task in availableTasks) {
			foreach (Task.Condition postCondition in task.Postconditions) {
				// If a postcondition and precondition match then the
				// available task satisfies the goal task.
				if (goal.Preconditions.Contains(postCondition)) {
					validTasks.Add(task);
				}
			}
		}

		return validTasks.ToArray();
	}

	/// <summary>
	/// Orders the tasks by which one is the most optimal to execute first.
	/// </summary>
	private void OrderTasks(Task[] tasks) {
		List<Task> orderedTasks = new List<Task>();

		// TODO: order tasks by which one is the most effective to execute
		//orderedTasks = tasks.OrderBy(task => task.Postconditions);
		// TODO: if two tasks are on par then order them by something else...
	}

	/// <summary>
	/// Creates a stack of tasks to be executed in order to achieve the goal 
	/// task.
	/// </summary>
	/// <param name="goalTask"> The task to create a plan for. </param>
	/// <returns> The stack of subtasks that acheive the goal. </returns>
	private Task SetPlan(Task goalTask) {
		// order the available tasks by which one's postconditions satisfies the goal tasks' preconditions best,
		// and set them in the same order as the goal tasks preconditions e.g. goal precons =
		// seeObject, inRange. Then subtasks should be in oder of (seeObject, inRange), (inRange, otherPostcon, seeObject)
		// seeObject, (otherPostCon, seeObject), inRange, (inRange, otherPostcon).

		// Remove all tasks that don't satisfy the goal tasks preconditions
		foreach (Condition precondition in goalTask.Preconditions) {
			foreach (Task task in availableTasks) {
				if (!task.Postconditions.Contains(precondition)) {
					// remove task here.
				}
			}
		}

		// Get the task(s) that solve the goal tasks preconditions and add to a stack
			// loop over tasks that meet the goals preconditions
				// add current task

				// if goal tasks preconditions are all met, break.
				// else 
				// remove other tasks that only share the same postconditions and have no more
				// continue

		// loop over tasks to check all preconditions are met

		// if all new tasks' preconditions are met
			// add them as a plan
		// else 
		// foreach task
			// if precons not met && no subtasks exist
				// repeat above process for all new tasks by CallingAboveMethod() and pass the subTask as a parameter
				// task.subtasks = AboveMethod();

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

		return null;

		// ! OLD COMMENT - Can remove once method works !
		//// TODO: go through ordered tasks until a set that satisfies all preconditions is found.
		//// (whichever set of tasks complete the preconditions in the fewest steps).
		//	// TODO: if a task doesn't satisfy the goal's preconditions by itself
		//	// then look for other tasks that satisfy the other conditions. May need
		//	// to call GetTasks again, but remvove the tasks that have already been
		//	// evaluated here.
		//	// TODO: if a task is added to the set, remove all other ordered tasks
		//	// that only satisfy preconditions already found in the set.
		//// TODO: push all required tasks onto the plan stack.
	}

	/// <summary>
	/// Executes the stack of tasks, one at a time, to complete the HTN's goal.
	/// </summary>
	private void ExecutePlan() {
		// Does the HTN have no goal?
		if (goal == null||
			// Has the HTN's goal been achieved?
			(currentTaskState == TaskState.Finished && plan.Count == 0) ||
			// Is there no plan?
			plan.Count == 0) {
			return;
		}

		if (currentTaskToExecute == null) {
			currentTaskToExecute = plan.Pop();
		}

		// TODO: check tasks pre-conditions are met before executing it.
		if (currentTaskToExecute != null) {
			// TODO: start executing action...
			if (currentTaskToExecute is PrimitiveTask) {
				(currentTaskToExecute as PrimitiveTask).Task();
			} else if (currentTaskToExecute is PrimitiveTask) {
				(currentTaskToExecute as PrimitiveVectorTask).Task((currentTaskToExecute as PrimitiveVectorTask).Vector);
			} else if (currentTaskToExecute is PrimitiveTask) {
				(currentTaskToExecute as PrimitiveTaskInteractable).Task((currentTaskToExecute as PrimitiveTaskInteractable).Interactable);
			} else if (currentTaskToExecute is PrimitiveTask) {
				//(currentTaskToExecute as CompoundTaskClass).Task();
			}

			// continue executing action...
			// if completed go to next action...
			// if cancelled check plan is still valid...
		}
	}
}
