// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The decision making process that will control what actions an AI agent 
/// performs to fulfill their order(s).
/// </summary>
public class HierarchicalTaskNetwork : MonoBehaviour {
	private Task goal = null;
	private Task[] goals = null;
	private Task task = null;
	private Task[] availableTasks = null;
	private Task[] executableTasks = null;
	private Stack<Task> plan = null;

	public HierarchicalTaskNetwork(Task[] goals, Task[] availableTasks) {
		this.goal = null;
		this.goals = goals;
		this.task = null;
		this.availableTasks = availableTasks;
		this.executableTasks = new Task[availableTasks.Length];
		this.plan = new Stack<Task>();
	}

	public void SetGoal(Task goal) {
		this.goal = goal;
		CreatePlan();
	}

	private void Update() {
		if (!goal) {
			FindGoal();
		}

		
	}

	// TODO: find the best goal to achieve if none was set by the player.
	private void FindGoal() {

	}

	private void FindExecuteableActions() {

	}

	// TODO: create plan at run-time.
	// TODO: create a method to solve which tasks should be executed to achieve
	// the HTN's goal.
	private void CreatePlan() {
		if (plan.Count > 0) {
			plan.Clear();
		}

		GetTasks(goal, availableTasks);
		OrderTasks();
		// TODO: find which task(s) should be executed to achieve the goal.
		SetPlan();
	}

	/// <summary>
	/// Returns an array of tasks that completely or partiall satisfy the 
	/// parameter goal task's preconditions.
	/// </summary>
	/// <returns></returns>
	private Task[] GetTasks(Task goalTask, Task[] availableTasks) {
		// TODO: search tasks for any that satisfy the goal tasks preconditions
		return null;
	}

	/// <summary>
	/// Orders the tasks by which one is the most optimal to execute first.
	/// </summary>
	private void OrderTasks() {
		// TODO: order tasks by which one is the most effective to execute.
		// TODO: if two tasks are on par then order them by something else...
	}

	private void SetPlan() {
		// TODO: go through ordered tasks until a set that satisfies all preconditions is found.
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
		if (!task && plan.Count > 0) {
			task = plan.Pop();
		} else {
			// No plan to execute.
			return;
		}

		if (task) {
			// TODO: start executing action...
			// continue executing action...
			// if completed go to next action...
			// if cancelled check plan is still valid...
			task.taskToExecute();
		}
	}
}
