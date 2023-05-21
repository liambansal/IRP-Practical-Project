// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The decision making process that will control what actions an AI agent 
/// performs to fulfill their order(s).
/// </summary>
public class HierarchicalTaskNetwork : MonoBehaviour {
	private Task goal = null;
	private Task[] goals = null;
	private Task nextTaskToExecute = null;
	private Task[] availableTasks = null;
	private Task[] executableTasks = null;
	private Stack<Task> plan = null;

	public HierarchicalTaskNetwork(Task[] goals, Task[] availableTasks) {
		this.goal = null;
		this.goals = goals;
		this.nextTaskToExecute = null;
		this.availableTasks = availableTasks;
		this.executableTasks = new Task[availableTasks.Length];
		this.plan = new Stack<Task>();
	}

	public void SetGoal(Task goal) {
		if (!goal) {
			// Return if no goal has been set.
			return;
		}

		this.goal = goal;
		CreatePlan();
	}

	private void Update() {
		if (!goal) {
			FindGoal();
		} else {
			ExecutePlan();
		}
	}

	// TODO: find the best goal to achieve if none was set by the player.
	private void FindGoal() {
		Task newGoal = null;
		// TODO: find the goal here.

		if (goal != newGoal) {
			// Only set the goal if it's different to the current one.
			SetGoal(newGoal);
		}
	}

	private void FindExecuteableActions() {

	}

	private void CreatePlan() {
		if (!goal) {
			return;
		}

		if (plan.Count > 0) {
			plan.Clear();
		}

		OrderTasks(GetValidTasks(goal, availableTasks));
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

	// TODO: find which task(s) should be executed to achieve the goal.
	private void SetPlan() {
		// TODO: go through ordered tasks until a set that satisfies all preconditions is found.

		// (whichever set of tasks complete the preconditions in the fewest steps).
			// TODO: if a task doesn't satisfy the goal's preconditions by itself
			// then look for other tasks that satisfy the other conditions. May need
			// to call GetTasks again, but remvove the tasks that have already been
			// evaluated here.
			// TODO: if a task is added to the set, remove all other ordered tasks
			// that only satisfy preconditions already found in the set.
		// TODO: push all required tasks onto the plan stack.
	}

	/// <summary>
	/// Executes the stack of tasks, one at a time, to complete the HTN's goal.
	/// </summary>
	private void ExecutePlan() {
		if (!nextTaskToExecute && plan.Count > 0) {
			nextTaskToExecute = plan.Pop();
		} else {
			// Return because there's no plan to execute.
			return;
		}

		// TODO: check tasks pre-conditions are met before executing it.
		if (nextTaskToExecute) {
			// TODO: start executing action...
			// continue executing action...
			// if completed go to next action...
			// if cancelled check plan is still valid...
			nextTaskToExecute.task();
		}
	}
}
