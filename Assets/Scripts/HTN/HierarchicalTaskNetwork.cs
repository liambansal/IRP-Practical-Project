// Written by Liam Bansal
// Date Created: 9/5/2023

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
	private List<Task> plan = null;

	public HierarchicalTaskNetwork(Task[] goals, Task[] availableTasks) {
		this.goal = null;
		this.goals = goals;
		this.task = null;
		this.availableTasks = availableTasks;
		this.executableTasks = new Task[availableTasks.Length];
	}

	public void SetGoal(Task goal) {
		this.goal = goal;
		CreatePlan();
	}

	private void Update() {
		if (!goal) {
			FindGoal();
		}

		if (task) {
			task.taskToExecute();
		}
	}

	// TODO: find the best goal to achieve if none was set by the player.
	private void FindGoal() {

	}

	// TODO: create plan at run-time.
	// TODO: create a method to solve which tasks should be executed to achieve
	// the HTN's goal.
	private void CreatePlan() {
		if (plan.Count > 0) {
			plan.Clear();
		}
	}
}
